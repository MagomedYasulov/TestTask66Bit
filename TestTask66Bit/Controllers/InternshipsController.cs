using Microsoft.AspNetCore.Mvc;
using TestTask66Bit.Abstractions;
using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InternshipsController : BaseController
    {
        private readonly IInternshipsService _internshipsService;

        public InternshipsController(IInternshipsService internshipsService)
        {
            _internshipsService = internshipsService;
        }

        [HttpGet("{internshipId}")]
        public async Task<ActionResult<InternshipDto>> Get(int internshipId)
        {
            var internshipDto = await _internshipsService.Get(internshipId);
            return Ok(internshipDto);
        }

        [HttpGet]
        public async Task<ActionResult<InternshipDto[]>> Get()
        {
            var internshipDtos = await _internshipsService.Get();
            return Ok(internshipDtos);
        }

        [HttpPost]
        public async Task<ActionResult<InternshipPartialDto>> Create(CreateInternshipDto model)
        {
            var internshipDto = await _internshipsService.Create(model);
            return Ok(internshipDto);
        }

        [HttpPut("{internshipId}")]
        public async Task<ActionResult<InternshipPartialDto>> Update(int internshipId, UpdateInternshipDto model)
        {
            var internshipDto = await _internshipsService.Update(internshipId, model);
            return Ok(internshipDto);
        }

        [HttpDelete("{internshipId}")]
        public async Task<ActionResult> Delete(int internshipId)
        {
            await _internshipsService.Delete(internshipId);
            return Ok();
        }
    }
}
