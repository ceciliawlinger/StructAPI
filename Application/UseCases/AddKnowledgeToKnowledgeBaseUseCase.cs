using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Service.IA;
using StructAPI.Service.Suggestions.Analysis;
using System.Linq;

namespace StructAPI.Application.UseCases
{
    public class AddKnowledgeToKnowledgeBaseUseCase
    {
        private readonly IKnowledgeEntryRepository _repository;
        private readonly IEmbeddingService _embeddingService;
        private readonly SemanticMatchService _semanticMatchService;
        private readonly IKnowledgeRelationRepository _knowledgeRelationRepository;

        public AddKnowledgeToKnowledgeBaseUseCase(
            IKnowledgeEntryRepository repository,
            IEmbeddingService embeddingService,
            SemanticMatchService semanticMatchService,
            IKnowledgeRelationRepository knowledgeRelationRepository)
        {
            _repository = repository;
            _embeddingService = embeddingService;
            _semanticMatchService = semanticMatchService;
            _knowledgeRelationRepository = knowledgeRelationRepository;
        }

        public async Task AddKnowledgeAsync(string content, string user)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));
            ArgumentException.ThrowIfNullOrWhiteSpace(user, nameof(user));

            var embedding = await _embeddingService.GenerateEmbeddingAsync(content);
            if (embedding == null) 
                throw new InvalidOperationException("Failed to generate embedding for the provided content.");

            var matches = await _semanticMatchService.FindSemanticMatchesAsync(embedding);
            var entry = new KnowledgeEntry(content, user, embedding);
            await _repository.CreateAsync(entry);

            if (matches.Count == 0)
                return;

            foreach (var match in matches)
            {
                var relation = new KnowledgeRelation(entry.Id, match.Entry.Id, match.Analysis.RelationType, match.Analysis.SimilarityScore);
                await _knowledgeRelationRepository.CreateAsync(relation);
            }
        }
    }
}
