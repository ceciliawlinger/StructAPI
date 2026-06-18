using Microsoft.AspNetCore.Mvc;
using StructAPI.Application.Dtos;
using StructAPI.Service.Knowledge;

namespace StructAPI.Controllers
{
    [ApiController]
    [Route("api/knowledge")]
    public class KnowledgeController : ControllerBase
    {
        private readonly KnowledgeEntryService _service;

        public KnowledgeController(KnowledgeEntryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateKnowledgeEntryRequest request)
        {
            var response = await _service.CreateKnowledgeEntry(request);

            return Ok(response);
        }

        [HttpPost("replace")]
        public async Task<IActionResult> Replace(
            [FromBody] ReplaceKnowledgeEntryRequest request)
        {
            var response = await _service.ReplaceKnowledgeEntry(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteKnowledgeEntry(id);

            return NoContent();
        }
    }
}
