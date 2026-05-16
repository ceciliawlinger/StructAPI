using System.ComponentModel.DataAnnotations;

namespace StructAPI.Domain.Dtos
{
    public class CreateKnowledgeEntryRequest
    {
        [Required]
        [MinLength(10)]
        public string Content { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
    }
}
