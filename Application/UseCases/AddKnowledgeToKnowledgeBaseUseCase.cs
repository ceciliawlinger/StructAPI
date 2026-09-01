using Pgvector;
using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Domain.Services;

namespace StructAPI.Application.UseCases
{
    public class AddKnowledgeToKnowledgeBaseUseCase
    {
        private readonly IKnowledgeEntryRepository _repository;
        private readonly IEmbeddingService _embeddingService;
        private readonly SemanticClassifier _classifier;
        private readonly IKnowledgeRelationRepository _knowledgeRelationRepository;

        public AddKnowledgeToKnowledgeBaseUseCase(
            IKnowledgeEntryRepository repository,
            IEmbeddingService embeddingService,
            SemanticClassifier classifier,
            IKnowledgeRelationRepository knowledgeRelationRepository)
        {
            _repository = repository;
            _embeddingService = embeddingService;
            _classifier = classifier;
            _knowledgeRelationRepository = knowledgeRelationRepository;
        }

        public async Task AddKnowledgeAsync(string content, string user)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));
            ArgumentException.ThrowIfNullOrWhiteSpace(user, nameof(user));

            var embedding = await _embeddingService.GenerateEmbeddingAsync(content);
            if (embedding == null) 
                throw new InvalidOperationException("Failed to generate embedding for the provided content.");
            var matches = await _repository.FindSimilarAsync(embedding, 5);
            var entry = new KnowledgeEntry(content, user, embedding);
            await _repository.CreateAsync(entry);

            if (matches.Count == 0)
                return;

            foreach (var match in matches)
            {
                var relationType = _classifier.Classify(match.Similarity);
                var relation = new KnowledgeRelation(entry.Id, match.Entry.Id, relationType, match.Similarity);
                await _knowledgeRelationRepository.CreateAsync(relation);
            }
        }
    }
}
