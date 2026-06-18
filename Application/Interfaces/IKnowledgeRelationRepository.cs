using StructAPI.Domain.Entities;

namespace StructAPI.Application.Interfaces
{
    public interface IKnowledgeRelationRepository
    {
        Task CreateAsync(KnowledgeRelation relation);
    }
}
