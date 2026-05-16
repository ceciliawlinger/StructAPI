using StructAPI.Domain;
using StructAPI.Domain.Dtos;
using StructAPI.Service.IA;

namespace StructAPI.Service.Suggestions.Analysis
{
    public class SemanticAnalyzer : ISemanticAnalyzer
    {
        private readonly ISemanticSimilarityService _similarityService;
        public SemanticAnalyzer(
            ISemanticSimilarityService similarityService)
        {
            _similarityService = similarityService;
        }

        public async Task<SemanticAnalysis> AnalyzeAsync(string content, StructAPI.Domain.KnowledgeEntry target)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            if(target == null) throw new ArgumentNullException(nameof(target));

            var similarity = await _similarityService
            .CalculateSimilarityAsync(
                content,
                target.Content);

            return new SemanticAnalysis(similarity);
        }
    }
}
