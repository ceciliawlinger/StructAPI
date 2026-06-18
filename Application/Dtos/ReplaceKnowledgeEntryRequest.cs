namespace StructAPI.Application.Dtos
{
    public class ReplaceKnowledgeEntryRequest
    {
        public Guid OldEntryID { get; set; }
        public string Content { get; set; }
        public string User { get; set; }
    }
}
