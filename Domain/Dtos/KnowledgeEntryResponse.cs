using StructAPI.Domain.Enums;

namespace StructAPI.Domain.Dtos
{
    public class KnowledgeEntryResponse
    {
        public int Id { get; }

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
