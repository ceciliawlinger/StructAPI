using Dapper;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using StructAPI.Application.Dtos;
using StructAPI.Application.Interfaces;
using StructAPI.Domain.Entities;
using StructAPI.Domain.Enums;
using StructAPI.Infrastructure.Persistence.Mappings;
using StructAPI.Service.IA;

namespace StructAPI.Infrastructure.Persistence.Repositories
{
    public class KnowledgeEntryRepository : IKnowledgeEntryRepository
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly KnowledgeDbContext _context;

        public KnowledgeEntryRepository(KnowledgeDbContext context, IEmbeddingService embeddingService)
        {
            _context = context;
            _embeddingService = embeddingService;
        }
        public async Task<List<SemanticMatch>> FindSimilarAsync(float[] embedding, int top)
        {
            var vector = new Vector(embedding);

            var matches = await _context.KnowledgeEntries
                .Where(x => x.Status == EntryStatus.Active)
                .Select(x => new
                {
                    Entry = x,
                    Distance = x.Embedding.CosineDistance(vector)
                })
                .OrderBy(x => x.Distance)
                .Take(top)
                .ToListAsync();

            return matches
                .Select(x => new SemanticMatch(
                    x.Entry,
                    1 - x.Distance))
                .ToList();
        }

        public async Task CreateAsync(KnowledgeEntry entry)
        {
            _context.KnowledgeEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entry = await _context.KnowledgeEntries.FindAsync(id);
            if (entry != null)
            {
                _context.KnowledgeEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<KnowledgeEntry>> GetActiveAsync()
        {
            return await _context.KnowledgeEntries
                .Where(x => x.Status == EntryStatus.Active)
                .ToListAsync();
        }

        public async Task<List<KnowledgeEntry>> GetAllAsync()
        {
            return await _context.KnowledgeEntries.ToListAsync();
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(Guid id)
        {
            return await _context.KnowledgeEntries.FindAsync(id);
        }

        public async Task<List<KnowledgeEntry>> GetByStatusAsync(EntryStatus status)
        {
            return await _context.KnowledgeEntries
                .Where(x => x.Status == status)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(KnowledgeEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            await _context.SaveChangesAsync(); // Assuming the entry is already tracked by the context
        }
    }
}
