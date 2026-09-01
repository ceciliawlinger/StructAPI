using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Pgvector;
using StructAPI.Application.Dtos;
using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;
using StructAPI.Domain.Services;

namespace StructAPI.Service.Knowledge
{
    public class KnowledgeEntryService
    {
        private readonly IKnowledgeEntryRepository _repository;
        private readonly IEmbeddingService _embeddingService;
        private readonly SemanticClassifier _classifier;

        public KnowledgeEntryService(IKnowledgeEntryRepository repository, IEmbeddingService embeddingService, SemanticClassifier classifier)
        {
            if (repository == null) throw new ArgumentNullException("Repository cannot be null");
            _repository = repository;

            if (embeddingService == null) throw new ArgumentNullException("Embedding service cannot be null");
            _embeddingService = embeddingService;

            if (classifier == null) throw new ArgumentNullException("Classifier cannot be null");
            _classifier = classifier;

        }
        public async Task<KnowledgeEntryResponse> CreateKnowledgeEntry(CreateKnowledgeEntryRequest request) 
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new DomainException("Content cannot be empty.");

            var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Content);
            if (embedding == null)
                throw new DomainException("Failed to generate embedding for the content.");
            KnowledgeEntry entry = new KnowledgeEntry(request.Content, request.User, embedding);

            if (await IsDuplicity(embedding))
                throw new DuplicateKnowledgeEntryException();

            await _repository.CreateAsync(entry);

            return new KnowledgeEntryResponse(entry);
        }

        public async Task<KnowledgeEntryResponse> ReplaceKnowledgeEntry(ReplaceKnowledgeEntryRequest request) 
        {
           var existingEntry = await _repository.GetByIdAsync(request.OldEntryID);
            if (existingEntry == null)
                throw new DomainException("Entry to be replaced not found.");

            var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Content);
            if (embedding == null)
                throw new DomainException("Failed to generate embedding for the content.");

            var matches = await _repository.FindSimilarAsync(embedding, 5);
            if (await IsDuplicity(embedding, matches))
                throw new DuplicateKnowledgeEntryException();
            matches = matches.Where(x => x.Entry.Id != request.OldEntryID).ToList();

            var newEntry = KnowledgeEntry.CreateReplacement( // Always create a new one and deprecate the old one.
                 request.Content,
                 request.User,
                 request.OldEntryID,
                 embedding
             );

            var log = existingEntry.Deprecate("Replaced by new entry", request.User);
            await _repository.CreateAsync(newEntry);
            await _repository.UpdateStatusAsync(existingEntry);
            return new KnowledgeEntryResponse(newEntry);
        }

        private async Task<bool> IsDuplicity(float[] embedding, List<SemanticMatch> matches = null)
        {
            if (matches == null)
                matches = await _repository.FindSimilarAsync(embedding, 5);
            return matches.Any(x => _classifier.Classify(x.Similarity) == KnowledgeRelationType.Redundant);
        }
            
        public async Task DeleteKnowledgeEntry(Guid entryId) 
        {
            await _repository.DeleteAsync(entryId);
        }
    }
}
