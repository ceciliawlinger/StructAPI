using Dapper;
using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Infrastructure.Persistence.Configurations;

namespace StructAPI.Infrastructure.Persistence.Repositories
{
    public class KnowledgeRelationRepository : IKnowledgeRelationRepository
    {
        private readonly KnowledgeDbContext _context;

        public KnowledgeRelationRepository(KnowledgeDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(KnowledgeRelation relation)
        {
            ArgumentNullException.ThrowIfNull(relation);
            _context.KnowledgeRelations.Add(relation);
            await _context.SaveChangesAsync();
        }
    }
}
