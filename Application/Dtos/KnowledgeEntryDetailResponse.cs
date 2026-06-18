using StructAPI.Domain.Enums;

namespace StructAPI.Application.Dtos
{
    public class KnowledgeEntryDetailResponse
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public EntryStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public List<KnowledgeEntryLifecycleLogResponse> History { get; set; }

        public int? ReplacedById { get; set; }
    }
}
