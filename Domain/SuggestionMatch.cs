using StructAPI.Domain.Dtos;
using StructAPI.Domain.Enums;

namespace StructAPI.Domain
{
    public class SuggestionMatch
    {
        public KnowledgeEntry Entry { get; }

        public SemanticAnalysis Analysis { get; }

        public SuggestionMatch(
            KnowledgeEntry entry,
            SemanticAnalysis analysis)
        {
            Entry = entry;
            Analysis = analysis;
        }
    }
}
