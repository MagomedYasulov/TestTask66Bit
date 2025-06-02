using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask66Bit.Abstractions;
using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InternsController : BaseController
    {
        private readonly IInternsService _internsService;

        public InternsController(IInternsService internsService)
        {
            _internsService = internsService;
        }

        [HttpGet("{internId}")]
        public async Task<ActionResult<InternDto>> Get(int internId)
        {
            var internDto = await _internsService.Get(internId);
            return Ok(internDto);
        }

        [HttpGet]
        public async Task<ActionResult<InternDto[]>> Get(InternsFilter filter)
        {
            var internDtos = await _internsService.Get(filter);
            return Ok(internDtos);
        }

        [HttpPost]
        public async Task<ActionResult<InternDto[]>> Create(CreateInternDto model)
        {
            var internDto = await _internsService.Create(model);
            return Ok(internDto);
        }

        [HttpPut("{internId}")]
        public async Task<ActionResult<InternDto[]>> Update(int internId, UpdateInternDto model)
        {
            var internDto = await _internsService.Update(internId, model);
            return Ok(internDto);
        }

        [HttpDelete("{internId}")]
        public async Task<ActionResult> Delete(int internId)
        {
            await _internsService.Delete(internId);
            return Ok();
        }
    }
}
