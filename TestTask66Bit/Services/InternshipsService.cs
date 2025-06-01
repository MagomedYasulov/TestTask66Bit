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
    public class InternshipsService : IInternshipsService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;

        public InternshipsService(
            IMapper mapper,
            ApplicationContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<InternshipPartialDto> Create(CreateInternshipDto model)
        {
            await CreateInternshipValidation(model);

            var internship = _mapper.Map<Internship>(model);

            await _dbContext.Internships.AddAsync(internship);
            foreach(var internId in model.Interns!)
            {
                var intern = new Intern() { Id = internId, InternshipId = internship.Id };
                _dbContext.Interns.Attach(intern);
                _dbContext.Entry(intern).Property(i => i.InternshipId).IsModified = true;
            }
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InternshipPartialDto>(internship);
        }

        public async Task<InternshipDto> Get(int id)
        {
            var internship = await _dbContext.Internships.AsNoTracking().Include(p => p.Interns).FirstOrDefaultAsync(p => p.Id == id);
            if (internship == null)
                throw new ServiceException("Internship Not Found", $"Internship with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<InternshipDto>(internship);
        }

        public async Task<InternshipDto[]> Get()
        {
            var internships = await _dbContext.Internships.AsNoTracking().Include(p => p.Interns).ToArrayAsync();
            return _mapper.Map<InternshipDto[]>(internships);
        }

        /// <summary>
        /// При обновлении стажировки не получится сразу менять массив стажеров, 
        /// так как не понятно какой id стажировки присваивать стажерам которых убрали из массива при обновлении 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<InternshipPartialDto> Update(int id, UpdateInternshipDto model)
        {
            var internship = await _dbContext.Internships.FirstOrDefaultAsync(p => p.Id == id);
            if (internship == null)
                throw new ServiceException("Internship Not Found", $"Internship with id {id} not found", StatusCodes.Status404NotFound);

            internship.Name = model.Name;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InternshipPartialDto>(internship);
        }

        public async Task Delete(int internshipId)
        {
            if (await _dbContext.Interns.AnyAsync(i => i.InternshipId == internshipId))
                throw new ServiceException("Can`t delete internship", $"Can`t delete internship with id {internshipId}, while there are interns with this internship", StatusCodes.Status409Conflict);

            var internship = await _dbContext.Internships.FirstOrDefaultAsync(p => p.Id == internshipId);
            if (internship == null)
                throw new ServiceException("Internship Not Found", $"Internship with id {internshipId} not found", StatusCodes.Status404NotFound);

            _dbContext.Internships.Remove(internship);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateInternshipValidation(CreateInternshipDto model)
        {
            var internsId = await _dbContext.Interns.Select(i => i.Id).ToArrayAsync();
            var notExistInterns = model.Interns!.Where(internId => !internsId.Contains(internId)).ToArray();
            if (notExistInterns.Length > 0)
                throw new ServiceException("Interns Not Found", $"Interns with id {string.Join(",", notExistInterns)} not found", StatusCodes.Status404NotFound);
        }
    }
}
