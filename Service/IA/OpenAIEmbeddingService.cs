using OpenAI;
using OpenAI.Embeddings;
using Pgvector;
using StructAPI.Application.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace StructAPI.Service.IA
{
    public class OpenAIEmbeddingService : IEmbeddingService
    {
        private readonly string? _apiKey;
        private readonly string? _embeddingModel;
        private readonly EmbeddingClient _embeddingClient;

        public OpenAIEmbeddingService(IConfiguration configuration)
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(_apiKey))
                    throw new InvalidOperationException("OpenAI API key is not set in environment variables.");

            _embeddingModel = configuration["OpenAI:EmbeddingModel"];
            if (string.IsNullOrEmpty(_embeddingModel))
                throw new InvalidOperationException("Embedding model is not set in configuration.");

            var client = new OpenAIClient(_apiKey);
            _embeddingClient = client.GetEmbeddingClient(_embeddingModel);
        }

        public async Task<float[]> GenerateEmbeddingAsync(string content)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content);

            var response = await _embeddingClient.GenerateEmbeddingAsync(content);
            return response.Value.ToFloats().ToArray();
        }
    }
}
