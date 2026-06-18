using StructAPI.Domain.Enums;

namespace StructAPI.Application.Dtos
{
    public class SemanticAnalysis
    {
        public double SimilarityScore { get; set; }

        public bool HasContradiction = false; //TODO: LLM logic => ISemanticReasoningService

        public bool CorrectsPrevious = false; //TODO: LLM logic => ISemanticReasoningService

        public bool ReplacesPrevious => SimilarityScore >= 0.70 && SimilarityScore < 0.90;

        public bool AddsInformation => SimilarityScore >= 0.50 &&SimilarityScore < 0.70;

        public bool IsRedundant => SimilarityScore >= 0.90;

        public SemanticAnalysis(double similarityScore)
        {
            this.SimilarityScore = similarityScore;
        }

        public KnowledgeRelationType RelationType
        {
            get
            {
                if (HasContradiction)
                    return KnowledgeRelationType.Contradicts;

                if (CorrectsPrevious)
                    return KnowledgeRelationType.CorrectsPrevious;

                if (IsRedundant)
                    return KnowledgeRelationType.Redundant;

                if (ReplacesPrevious)
                    return KnowledgeRelationType.ReplacesPrevious;

                if (AddsInformation)
                    return KnowledgeRelationType.AddsInformation;

                return KnowledgeRelationType.Related;
            }
        }
    }
}
