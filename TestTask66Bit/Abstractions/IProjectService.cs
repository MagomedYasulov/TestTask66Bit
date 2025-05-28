using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Abstractions
{
    public interface IProjectService
    {
        public Task<ProjectDto> Get(int id);
        public Task<ProjectDto[]> Get();
        public Task<ProjectDto> Create(CreateProjectDto model);
        public Task<ProjectDto> Update(int id, UpdateProjectDto model);

        public Task Delete(int projectId);
    }
}
