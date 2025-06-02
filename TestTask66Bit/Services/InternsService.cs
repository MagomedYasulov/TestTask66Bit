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
    public class InternsService : IInternsService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;

        public InternsService(
            IMapper mapper,
            ApplicationContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<InternDto> Create(CreateInternDto model)
        {
            await CreateInternValidations(model);

            var intern = _mapper.Map<Intern>(model);

            await _dbContext.Interns.AddAsync(intern);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InternDto>(intern);
        }

        public async Task<InternDto> Get(int id)
        {
            var intern = await _dbContext.Interns.AsNoTracking().Include(p => p.Project).Include(i => i.Internship).FirstOrDefaultAsync(p => p.Id == id);
            if (intern == null)
                throw new ServiceException("Intern Not Found", $"Intern with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<InternDto>(intern);
        }

        public async Task<InternDto[]> Get()
        {
            var interns = await _dbContext.Interns.AsNoTracking().Include(p => p.Project).Include(i => i.Internship).OrderBy(i => i.CreatedAt).ToArrayAsync();
            return _mapper.Map<InternDto[]>(interns);
        }

        public async Task<InternDto> Update(int id, UpdateInternDto model)
        {
            await UpdateInternValidations(id, model);

            var intern = await _dbContext.Interns.Include(p => p.Project).Include(i => i.Internship).FirstOrDefaultAsync(p => p.Id == id);
            if (intern == null)
                throw new ServiceException("Intern Not Found", $"Intern with id {id} not found", StatusCodes.Status404NotFound);

            intern.Name = model.Name;
            intern.Surname = model.Surname;
            intern.BirthDate = model.BirthDate!.Value;
            intern.Phone = model.Phone;
            intern.Email = model.Email;
            intern.Gender = model.Gender;
            intern.ProjectId = model.ProjectId;
            intern.InternshipId = model.InternshipId;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InternDto>(intern);
        }

        public async Task Delete(int internId)
        {
            var intern = await _dbContext.Interns.FirstOrDefaultAsync(p => p.Id == internId);
            if (intern == null)
                throw new ServiceException("Intern Not Found", $"Intern with id {internId} not found", StatusCodes.Status404NotFound);

            _dbContext.Interns.Remove(intern);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateInternValidations(CreateInternDto model)
        {
            var isNotFreeEmail = _dbContext.Interns.AnyAsync(i => i.Email == model.Email);
            var isNotFreePhone = model.Phone == null ? Task.FromResult(false) : _dbContext.Interns.AnyAsync(i => i.Phone == model.Phone);
            var isValidIds = ValidateIds(model.ProjectId, model.InternshipId);

            if (await isNotFreeEmail)
                throw new ServiceException("Not Free Email", $"Email {model.Email} already in use", StatusCodes.Status409Conflict);

            if (await isNotFreePhone)
                throw new ServiceException("Not Free Phone", $"Phone {model.Phone} already in use", StatusCodes.Status409Conflict);

            await isValidIds;
        }

        private async Task UpdateInternValidations(int internId, UpdateInternDto model)
        {
            var isNotFreeEmail = _dbContext.Interns.AnyAsync(i => i.Id != internId && i.Email == model.Email);
            var isNotFreePhone = model.Phone == null ? Task.FromResult(false) : _dbContext.Interns.AnyAsync(i => i.Id != internId && i.Phone == model.Phone);
            var isValidIds = ValidateIds(model.ProjectId, model.InternshipId);

            if (await isNotFreeEmail)
                throw new ServiceException("Not Free Email", $"Email {model.Email} already in use", StatusCodes.Status409Conflict);

            if (await isNotFreePhone)
                throw new ServiceException("Not Free Phone", $"Phone {model.Email} already in use", StatusCodes.Status409Conflict);

            await isValidIds;
        }

        private async Task ValidateIds(int projectId, int internShipId)
        {
            var projectIdTask = _dbContext.Projects.AnyAsync(p => p.Id == projectId);
            var internshipIdTask = _dbContext.Internships.AnyAsync(i => i.Id == internShipId);

            if(!await projectIdTask)
                throw new ServiceException("Project Not Found", $"Project with id {projectId} not found", StatusCodes.Status404NotFound);

            if(!await internshipIdTask)
                throw new ServiceException("Internship Not Found", $"Internship with id {internShipId} not found", StatusCodes.Status404NotFound);
        }
    }
}
