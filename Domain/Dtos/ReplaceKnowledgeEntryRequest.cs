namespace StructAPI.Domain.Dtos
{
    public class ReplaceKnowledgeEntryRequest
    {
        public int OldEntryID { get; set; }
        public string Content { get; set; }
        public string User { get; set; }
    }
}
