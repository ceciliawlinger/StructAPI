using StructAPI.Domain.Enums;

namespace StructAPI.Application.Dtos;

public class KnowledgeEntryLifecycleLogResponse
{
    public EntryStatus OldStatus { get; set; }

    public EntryStatus Status { get; set; }

    public string Reason { get; set; }

    public DateTimeOffset OccurredAt { get; set; }

    public string User { get; set; }
}
