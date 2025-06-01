using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Abstractions
{
    public interface IProjectsService
    {
        public Task<ProjectDto> Get(int id);
        public Task<ProjectDto[]> Get();
        public Task<ProjectPartialDto> Create(CreateProjectDto model);
        public Task<ProjectPartialDto> Update(int id, UpdateProjectDto model);

        public Task Delete(int projectId);
    }
}
