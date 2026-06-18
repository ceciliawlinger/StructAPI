using StructAPI.Application.Dtos;
using StructAPI.Application.Interfaces;
using StructAPI.Domain;
using StructAPI.Domain.Enums;

namespace StructAPI.Service.Suggestions.Analysis
{
    public class SemanticMatchService
    {
        private readonly IKnowledgeEntryRepository _repository;
        private readonly ISemanticAnalyzer _analyzer;

        public SemanticMatchService(
            IKnowledgeEntryRepository repository,
            ISemanticAnalyzer analyzer)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));

            _analyzer = analyzer
                ?? throw new ArgumentNullException(nameof(analyzer));
        }

        public async Task<List<SemanticMatch>> FindMatchesAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return new List<SemanticMatch>();

            var candidates = await _repository.GetAllAsync();

            var results = new List<SemanticMatch>();
            foreach (var candidate in candidates
                .Where(x => x.Status == EntryStatus.Active)
                )
            {
                var analysis = await _analyzer
                    .AnalyzeAsync(content, candidate);

                results.Add(
                    new SemanticMatch(
                        candidate,
                        analysis));
            }

            return results;
        }
    }
}

