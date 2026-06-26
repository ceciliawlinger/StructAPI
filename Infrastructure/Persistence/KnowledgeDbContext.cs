using Microsoft.EntityFrameworkCore;
using StructAPI.Domain.Entities;

namespace StructAPI.Infrastructure.Persistence
{
    public class KnowledgeDbContext : DbContext
    {
        public KnowledgeDbContext(DbContextOptions<KnowledgeDbContext> options)
            : base(options)
        {
        }

        public DbSet<KnowledgeEntry> KnowledgeEntries => Set<KnowledgeEntry>();

        public DbSet<KnowledgeEntryLifecycleLog> KnowledgeEntryLifecycleLogs
        => Set<KnowledgeEntryLifecycleLog>();

        public DbSet<KnowledgeRelation> KnowledgeRelations => Set<KnowledgeRelation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(KnowledgeDbContext).Assembly);
        }
    }
}
