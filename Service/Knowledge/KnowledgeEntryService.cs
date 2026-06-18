using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using StructAPI.Application.Dtos;
using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;
using StructAPI.Service.IA;
using StructAPI.Service.Suggestions.Analysis;

namespace StructAPI.Service.Knowledge
{
    public class KnowledgeEntryService
    {
        private readonly IKnowledgeEntryRepository _repository;
        private readonly ISemanticAnalyzer _analyzer;
        private readonly SemanticMatchService _matchService;
        private readonly ISemanticSimilarityService _similarityService;

        public KnowledgeEntryService(IKnowledgeEntryRepository repository, ISemanticAnalyzer analyzer, SemanticMatchService matchService, ISemanticSimilarityService similarityService)
        {
            if (repository == null) throw new ArgumentNullException("Repository cannot be null");
            _repository = repository;

            if (analyzer == null) throw new ArgumentNullException("Analyzer cannot be null");
            _analyzer = analyzer;

            if (matchService == null) throw new ArgumentNullException("Match service cannot be null");
            _matchService = matchService;

            if (similarityService == null) throw new ArgumentNullException("Similarity service cannot be null");
            _similarityService = similarityService;

        }
        public async Task<KnowledgeEntryResponse> CreateKnowledgeEntry(CreateKnowledgeEntryRequest request) 
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new DomainException("Content cannot be empty.");

            var embedding = await _similarityService.GenerateEmbeddingAsync(request.Content);
            KnowledgeEntry entry = new KnowledgeEntry(request.Content, request.User, embedding);
            var matches = await _matchService.FindMatchesAsync(request.Content);

            if (matches.Any(x => x.Analysis.IsRedundant))
                throw new DuplicateKnowledgeEntryException();

            await _repository.CreateAsync(entry);

            return new KnowledgeEntryResponse(entry);
        }

        public async Task<KnowledgeEntryResponse> ReplaceKnowledgeEntry(ReplaceKnowledgeEntryRequest request) 
        {
           var existingEntry = await _repository.GetByIdAsync(request.OldEntryID);
            if (existingEntry == null)
                throw new DomainException("Entry to be replaced not found.");

            var matches = await _matchService.FindMatchesAsync(request.Content);

            if (matches.Any(x => x.Analysis.IsRedundant))
                throw new DuplicateKnowledgeEntryException();

            matches = matches.Where(x => x.Entry.Id != request.OldEntryID).ToList();
            
            var embedding = await _similarityService.GenerateEmbeddingAsync(request.Content);

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
            
        public async Task DeleteKnowledgeEntry(Guid entryId) 
        {
            await _repository.DeleteAsync(entryId);
        }
    }
}
