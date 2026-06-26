using Pgvector;

namespace StructAPI.Service.IA
{
    public interface IEmbeddingService
    {
        Task<Vector> GenerateEmbeddingAsync(string content);
    }
}
