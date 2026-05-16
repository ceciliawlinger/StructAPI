using StructAPI.Domain;

namespace StructAPI.Repository
{
    public interface IKnowledgeEntryLifecycleLogRepository
    {
        Task CreateAsync(KnowledgeEntryLifecycleLog log);
    }
}
