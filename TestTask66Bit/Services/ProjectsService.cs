using TestTask66Bit.Abstractions;
using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Services
{
    public class ProjectsService : IProjectsService
    {
        public Task<ProjectDto> Create(CreateProjectDto model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto[]> Get()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto> Update(int id, UpdateProjectDto model)
        {
            throw new NotImplementedException();
        }
    }
}
