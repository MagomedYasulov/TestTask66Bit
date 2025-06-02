using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestTask66Bit.Abstractions;
using TestTask66Bit.Data;
using TestTask66Bit.Data.Entites;
using TestTask66Bit.Exceptions;
using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;

        public ProjectsService(
            IMapper mapper, 
            ApplicationContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ProjectPartialDto> Create(CreateProjectDto model)
        {
            await ValidateInternsId(model.Interns!);

            var project = _mapper.Map<Project>(model);

            await _dbContext.Projects.AddAsync(project);
            foreach (var internId in model.Interns!)
            {
                var intern = new Intern() { Id = internId, ProjectId = project.Id };
                _dbContext.Interns.Attach(intern);
                _dbContext.Entry(intern).Property(i => i.ProjectId).IsModified = true;
            }
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProjectPartialDto>(project);
        }



        public async Task<ProjectDto> Get(int id)
        {
            var project = await _dbContext.Projects.AsNoTracking().Include(p => p.Interns).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                throw new ServiceException("Project Not Found", $"Project with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto[]> Get()
        {
            var projects = await _dbContext.Projects.AsNoTracking().Include(p => p.Interns).ToArrayAsync();
            return _mapper.Map<ProjectDto[]>(projects);
        }

        public async Task<ProjectPartialDto> Update(int id, UpdateProjectDto model)
        {
            await ValidateInternsId(model.Interns);

            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                throw new ServiceException("Project Not Found", $"Project with id {id} not found", StatusCodes.Status404NotFound);

            project.Name = model.Name;
            foreach (var internId in model.Interns!)
            {
                var intern = new Intern() { Id = internId, ProjectId = project.Id };
                _dbContext.Interns.Attach(intern);
                _dbContext.Entry(intern).Property(i => i.ProjectId).IsModified = true;
            }
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProjectPartialDto>(project);
        }

        public async Task Delete(int projectId)
        {
            if (await _dbContext.Interns.AnyAsync(i => i.ProjectId == projectId))
                throw new ServiceException("Can`t delete project", $"Can`t delete project with id {projectId}, while there are interns with this project", StatusCodes.Status409Conflict);

            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if(project == null)
                throw new ServiceException("Project Not Found", $"Project with id {projectId} not found", StatusCodes.Status404NotFound);
            
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }


        private async Task ValidateInternsId(int[] newInternsId)
        {
            if (newInternsId.Length == 0)
                return;

            var internsId = await _dbContext.Interns.Select(i => i.Id).ToArrayAsync();
            var notExistInterns = newInternsId!.Where(internId => !internsId.Contains(internId)).ToArray();
            if (notExistInterns.Length > 0)
                throw new ServiceException("Interns Not Found", $"Interns with id {string.Join(",", notExistInterns)} not found", StatusCodes.Status404NotFound);
        }
    }
}
