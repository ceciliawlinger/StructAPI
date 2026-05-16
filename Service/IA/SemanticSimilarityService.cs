using OpenAI;
using static System.Net.Mime.MediaTypeNames;

namespace StructAPI.Service.IA
{
    public class SemanticSimilarityService : ISemanticSimilarityService 
    {
        private readonly string _apiKey;
        private const string EmbeddingModel = "text-embedding-3-small";

        public SemanticSimilarityService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"]
                        ?? throw new InvalidOperationException(
                            "OpenAI API key not configured.");
        }

        public Task<double> CalculateSimilarityAsync(string source, string target)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException("target");

            var sourceEmbedding = GenerateEmbeddingAsync(source);
            var targetEmbedding = GenerateEmbeddingAsync(target);
            return CalculateSemanticSimilarity(
                sourceEmbedding.Result,
                targetEmbedding.Result);
        }

        private async Task<double> CalculateSemanticSimilarity(float[] sourceVector, float[] targetVector)
        {
            double product = 0.0;
            double sourceMagnitude = 0.0;
            double targetMagnitude = 0.0; 

            for (int i = 0; i < sourceVector.Length; i++)
            {
                product += sourceVector[i] * targetVector[i];
                sourceMagnitude += Math.Pow(sourceVector[i], 2);
                targetMagnitude += Math.Pow(targetVector[i], 2);
            }

            sourceMagnitude = Math.Sqrt(sourceMagnitude);
            targetMagnitude = Math.Sqrt(targetMagnitude);

            if (sourceMagnitude == 0 || targetMagnitude == 0)
                return 0.0;

            return product / (sourceMagnitude * targetMagnitude);
        }

        private async Task<float[]> GenerateEmbeddingAsync(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            var client = new OpenAIClient(_apiKey);
            var embeddingClient = client.GetEmbeddingClient(EmbeddingModel);
            var response = await embeddingClient
                            .GenerateEmbeddingAsync(content);

            return response.Value.ToFloats().ToArray();
        }
    }
}
