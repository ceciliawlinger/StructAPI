using Pgvector;
using StructAPI.Application.Dtos;
using StructAPI.Application.Interfaces;
using StructAPI.Service.IA;

namespace StructAPI.Service.Suggestions.Analysis
{
    public class SemanticMatchService
    {
        private readonly IKnowledgeEntryRepository _repository;
        private readonly IEmbeddingService _embeddingService;
        private readonly IKnowledgeEntryRepository _knowledgeEntryRepository;

        public SemanticMatchService(IKnowledgeEntryRepository repository, IEmbeddingService embeddingService, IKnowledgeEntryRepository knowledgeEntryRepository)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
            _embeddingService = embeddingService
                ?? throw new ArgumentNullException(nameof(embeddingService));
            _knowledgeEntryRepository = knowledgeEntryRepository
                ?? throw new ArgumentNullException(nameof(knowledgeEntryRepository));
        }

        public async Task<List<SemanticMatch>> FindSemanticMatchesAsync(float[] embedding, int top = 5)
        {
            if (embedding == null)
                return new List<SemanticMatch>();

            return await _knowledgeEntryRepository.FindSimilarAsync(embedding, top);
        }
    }
}

