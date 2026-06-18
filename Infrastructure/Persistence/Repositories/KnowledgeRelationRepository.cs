using Dapper;
using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Infrastructure.Persistence.Configurations;

namespace StructAPI.Infrastructure.Persistence.Repositories
{
    public class KnowledgeRelationRepository : IKnowledgeRelationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public KnowledgeRelationRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task CreateAsync(KnowledgeRelation relation)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                INSERT INTO KnowledgeRelation (SourceEntryId, TargetEntryId, RelationType, CreatedAt)
                VALUES (@SourceEntryId, @TargetEntryId, @RelationType, @CreatedAt)";
            
            await connection.ExecuteAsync(sql, new
            {
               relation.SourceEntryId,
               relation.RelatedEntryId,
               RelationType = (int)relation.RelationType,
               relation.CreatedAt
            });
        }
    }
}
