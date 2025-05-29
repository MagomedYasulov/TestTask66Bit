using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask66Bit.Abstractions;
using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectsController : BaseController
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectDto>> Get(int projectId)
        {
            var projectDto = await _projectsService.Get(projectId);
            return Ok(projectDto);
        }

        [HttpGet]
        public async Task<ActionResult<ProjectDto[]>> Get()
        {
            var projectDtos = await _projectsService.Get();
            return Ok(projectDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto[]>> Create(CreateProjectDto model)
        {
            var projectDto = await _projectsService.Create(model);
            return Ok(projectDto);
        }

        [HttpPut("{projectId}")]
        public async Task<ActionResult<ProjectDto[]>> Update(int projectId, UpdateProjectDto model)
        {
            var projectDto = await _projectsService.Update(projectId, model);
            return Ok(projectDto);
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> Delete(int projectId)
        {
            await _projectsService.Delete(projectId);
            return Ok();
        }
    }
}
