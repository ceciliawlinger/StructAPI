namespace StructAPI.Domain.Dtos
{
    public class SemanticMatch
    {
        public KnowledgeEntry Entry { get; }
        public SemanticAnalysis Analysis { get; }

        public SemanticMatch(KnowledgeEntry entry, SemanticAnalysis analysis)
        {
            Entry = entry;
            Analysis = analysis;
        }
    }
}
