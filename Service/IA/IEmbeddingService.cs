using Pgvector;

namespace StructAPI.Service.IA
{
    public interface IEmbeddingService
    {
        Task<float[]> GenerateEmbeddingAsync(string content);
    }
}
