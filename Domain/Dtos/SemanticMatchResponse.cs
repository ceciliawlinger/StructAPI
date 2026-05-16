namespace StructAPI.Domain.Dtos
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