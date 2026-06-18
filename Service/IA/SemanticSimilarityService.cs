using OpenAI;
using static System.Net.Mime.MediaTypeNames;

namespace StructAPI.Service.IA
{
    public class SemanticSimilarityService : ISemanticSimilarityService
    {
        private readonly string? _apiKey;
        private readonly string? _embeddingModel;

        public SemanticSimilarityService(IConfiguration configuration)
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(_apiKey))
                    throw new InvalidOperationException("OpenAI API key is not set in environment variables.");

            _embeddingModel = configuration["OpenAI:EmbeddingModel"];
            if (string.IsNullOrEmpty(_embeddingModel))
                throw new InvalidOperationException("Embedding model is not set in configuration.");
        }

        public double CalculateSimilarity(float[] source, float[] target)
        {
            if (source == null || source.Length == 0)
                throw new ArgumentNullException("source");

            if (target == null || target.Length == 0)
                throw new ArgumentNullException("target");

            return CalculateSemanticSimilarity(source, target);
        }

        private double CalculateSemanticSimilarity(float[] sourceVector, float[] targetVector)
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

        public async Task<float[]> GenerateEmbeddingAsync(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            var client = new OpenAIClient(_apiKey);
            var embeddingClient = client.GetEmbeddingClient(_embeddingModel);
            var response = await embeddingClient
                            .GenerateEmbeddingAsync(content);

            return response.Value.ToFloats().ToArray();
        }
    }
}
