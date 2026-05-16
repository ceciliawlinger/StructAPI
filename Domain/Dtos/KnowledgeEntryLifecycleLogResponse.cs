using StructAPI.Domain.Enums;

namespace StructAPI.Domain.Dtos;

public class KnowledgeEntryLifecycleLogResponse
{
    public EntryStatus OldStatus { get; set; }

    public EntryStatus Status { get; set; }

    public string Reason { get; set; }

    public DateTimeOffset OccurredAt { get; set; }

    public string User { get; set; }
}
