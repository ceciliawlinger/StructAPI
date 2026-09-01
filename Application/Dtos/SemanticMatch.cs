using StructAPI.Domain.Entities;

namespace StructAPI.Application.Dtos
{
    public class SemanticMatch
    {
        public KnowledgeEntry Entry { get; }
        public double Similarity { get; }

        public SemanticMatch(KnowledgeEntry entry, double similarity)
        {
            Entry = entry;
            Similarity = similarity;
        }
    }
}
