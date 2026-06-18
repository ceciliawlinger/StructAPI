using Dapper;
using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Domain.Enums;
using StructAPI.Infrastructure.Persistence.Configurations;
using StructAPI.Service.IA;

namespace StructAPI.Infrastructure.Persistence.Repositories
{
    public class KnowledgeEntryRepository : IKnowledgeEntryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISemanticSimilarityService _semanticSimilarityService;

        public KnowledgeEntryRepository(IDbConnectionFactory connectionFactory, ISemanticSimilarityService semanticSimilarityService)
        {
            _connectionFactory = connectionFactory;
            _semanticSimilarityService = semanticSimilarityService;
        }
        public async Task<List<KnowledgeEntry>> FindSimilarAsync(float[] embedding, int top)
        {
            using var connection = _connectionFactory.CreateConnection();

            var entries = await GetActiveAsync();

            if (embedding == null || embedding.Length == 0)
                return new List<KnowledgeEntry>();

            var activeEntries = entries
                .Where(x => x.Status == EntryStatus.Active)
                .Where(x => x.Embedding != null)
                .Where(x => x.Embedding.Length == embedding.Length)
                .ToList();

            var relatedEntries = new List<(KnowledgeEntry Entry, double Score)>();
            foreach (var entry in activeEntries)
            {
                var similarity = _semanticSimilarityService.CalculateSimilarity(
                    embedding,
                    entry.Embedding);

                relatedEntries.Add((entry, similarity));
            }

            var topEntries = relatedEntries
                .OrderByDescending(x => x.Score)
                .Take(top)
                .Select(x => x.Entry)
                .ToList();

            return await Task.FromResult(topEntries);
        }

        public async Task CreateAsync(KnowledgeEntry entry)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                INSERT INTO KnowledgeEntries (Id, Content, [User], Status, CreatedAt)
                VALUES (@Id, @Content, @User, @Status, @CreatedAt)";

            await connection.ExecuteAsync(sql, new
            {
                entry.Id,
                entry.Content,
                entry.User,
                Status = (int)entry.Status,
                entry.CreatedAt
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                DELETE FROM KnowledgeEntries
                WHERE Id = @Id";
            await connection.ExecuteAsync(sql);
        }

        public async Task<List<KnowledgeEntry>> GetActiveAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    Id, 
                    Content,   
                    [User],
                    Status,
                    CreatedAt
                FROM KnowledgeEntries
                WHERE Status = @Status";

            var entries = await connection.QueryAsync<KnowledgeEntry>(sql, new { Status = (int)EntryStatus.Active });
            return entries.ToList();
        }

        public async Task<List<KnowledgeEntry>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    Id, 
                    Content,   
                    [User],
                    Status,
                    CreatedAt
                FROM KnowledgeEntries";

            var entries = await connection.QueryAsync<KnowledgeEntry>(sql);
            return entries.ToList();
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    Id, 
                    Content,   
                    [User],
                    Status,
                    CreatedAt
                FROM KnowledgeEntries
                WHERE Id = @Id";

            var entry = await connection.QuerySingleOrDefaultAsync<KnowledgeEntry>(sql, new { Id = id });
            return entry;
        }

        public async Task<List<KnowledgeEntry>> GetByStatusAsync(EntryStatus status)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    Id, 
                    Content,   
                    [User],
                    Status,
                    CreatedAt
                FROM KnowledgeEntries
                WHERE Status = @Status";

            var statusValue = (int)status;

            var entries = await connection.QueryAsync<KnowledgeEntry>(sql, new { Status = statusValue });
            return entries.ToList();
        }

        public async Task UpdateStatusAsync(KnowledgeEntry entry)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                UPDATE KnowledgeEntries
                SET 
                    Status = @Status
                WHERE Id = @Id";

            await connection.ExecuteAsync(sql, new
            {
                entry.Id,
                entry.Status
            });
        }
    }
}
