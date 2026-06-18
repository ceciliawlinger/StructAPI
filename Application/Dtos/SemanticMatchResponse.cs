using StructAPI.Domain.Entities;

namespace StructAPI.Application.Dtos
{
    public class SemanticMatchResponse
    {
        public KnowledgeEntryResponse Entry { get; }
        public SemanticAnalysis Analysis { get; }
        public SemanticMatchResponse(
            KnowledgeEntry entry,
            SemanticAnalysis analysis)
        {
            Entry = new KnowledgeEntryResponse(entry);

            Analysis = analysis;
        }
    }
}