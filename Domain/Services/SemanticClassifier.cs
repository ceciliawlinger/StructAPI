using StructAPI.Domain.Enums;

namespace StructAPI.Domain.Services
{
    public class SemanticClassifier
    {
        // TODO: Extend classification with semantic reasoning
        // (e.g., contradiction and correction) when LLM-based reasoning is introduced.
        //public bool HasContradiction = false; //TODO: LLM logic => ISemanticReasoningService
        //public bool CorrectsPrevious = false; //TODO: LLM logic => ISemanticReasoningService
        public KnowledgeRelationType Classify(double similarity)
        {
            if (similarity >= 0.90)
                return KnowledgeRelationType.Redundant;

            if (similarity >= 0.70)
                return KnowledgeRelationType.ReplacesPrevious;

            if (similarity >= 0.50)
                return KnowledgeRelationType.AddsInformation;

            return KnowledgeRelationType.Related;
        }
    }
}
}
