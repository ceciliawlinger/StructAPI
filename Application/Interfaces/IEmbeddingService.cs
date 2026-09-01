using Pgvector;

namespace StructAPI.Application.Interfaces
{
    public interface IEmbeddingService
    {
        Task<float[]> GenerateEmbeddingAsync(string content);
    }
}
