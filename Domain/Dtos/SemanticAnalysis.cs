namespace StructAPI.Domain.Dtos
{
    public class SemanticAnalysis
    {
        public double SimilarityScore { get; set; }

        public bool HasContradiction = false; //TODO: LLM logic => ISemanticReasoningService

        public bool CorrectsPrevious = false; //TODO: LLM logic => ISemanticReasoningService

        public bool ReplacesPrevious =>
        SimilarityScore >= 0.70;

        public bool AddsInformation =>
        SimilarityScore >= 0.50;

        public bool IsRedundant
        => SimilarityScore >= 0.90;

        public SemanticAnalysis(double similarityScore)
        {
            this.SimilarityScore = similarityScore;
        }
    }
}
