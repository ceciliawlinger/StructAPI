using System.Text.Json;
using OpenAI;
using StructAPI.Application.Dtos;
using StructAPI.Domain.Entities;
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

        public async Task<SemanticAnalysis> AnalyzeAsync(string content, KnowledgeEntry target)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            if (target == null) throw new ArgumentNullException(nameof(target));

            var sourceEmbedding = await _similarityService.GenerateEmbeddingAsync(content);

            var targetEmbedding = target.Embedding;

            var similarity = _similarityService
            .CalculateSimilarity(
                sourceEmbedding,
                targetEmbedding);

            return new SemanticAnalysis(similarity);
        }
    }
}
