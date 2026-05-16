using StructAPI.Domain;
using StructAPI.Domain.Dtos;
using StructAPI.Domain.Enums;

namespace StructAPI.Repository
{
    public interface IKnowledgeEntryRepository
    {
        Task<KnowledgeEntry> CreateAsync(KnowledgeEntry entry);
        Task<KnowledgeEntry> ReplaceAsync(int entryId, string newContent, string user);
        Task<KnowledgeEntry> UpdateAsync(KnowledgeEntry entry);
        Task DeleteAsync(KnowledgeEntry entry);
        Task<KnowledgeEntry?> GetByIdAsync(int id);
        Task<List<KnowledgeEntry>> SearchAsync(string content, int top);
        Task<List<KnowledgeEntry>> GetAllAsync();
        Task<List<KnowledgeEntry>> GetByStatusAsync(EntryStatus status);
    }
}
