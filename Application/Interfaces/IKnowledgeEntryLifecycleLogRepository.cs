using StructAPI.Domain.Entities;

namespace StructAPI.Application.Interfaces
{
    public interface IKnowledgeEntryLifecycleLogRepository
    {
        Task CreateAsync(KnowledgeEntryLifecycleLog log);
    }
}
