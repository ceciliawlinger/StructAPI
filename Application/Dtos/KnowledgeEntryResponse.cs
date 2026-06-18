using StructAPI.Domain.Entities;
using StructAPI.Domain.Enums;

namespace StructAPI.Application.Dtos
{
    public class KnowledgeEntryResponse
    {
        public Guid Id { get; }

        public string Content { get; }

        public EntryStatus Status { get; }

        public DateTimeOffset CreatedAt { get; }

        public KnowledgeEntryResponse(KnowledgeEntry entry)
        {
            this.Id = entry.Id;
            this.Content = entry.Content;
            this.CreatedAt = entry.CreatedAt;
            this.Status = entry.Status;
        }
    }
}
