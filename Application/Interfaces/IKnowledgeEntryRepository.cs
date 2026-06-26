using Pgvector;
using StructAPI.Application.Dtos;
using StructAPI.Domain.Entities;
using StructAPI.Domain.Enums;

namespace StructAPI.Application.Interfaces
{
    public interface IKnowledgeEntryRepository
    {
        Task CreateAsync(KnowledgeEntry entry);
        Task UpdateStatusAsync(KnowledgeEntry entry);
        Task DeleteAsync(Guid id);
        Task<KnowledgeEntry?> GetByIdAsync(Guid id);
        Task<List<SemanticMatch>> FindSimilarAsync(Vector embedding, int top);
        Task<List<KnowledgeEntry>> GetAllAsync();
        Task<List<KnowledgeEntry>> GetByStatusAsync(EntryStatus status);
        Task<List<KnowledgeEntry>> GetActiveAsync();
    }
}
