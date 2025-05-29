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

        public async Task<InternshipDto> Create(CreateInternshipDto model)
        {
            var internship = _mapper.Map<Internship>(model);

            await _dbContext.Internships.AddAsync(internship);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InternshipDto>(internship);
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

        public async Task<InternshipDto> Update(int id, UpdateInternshipDto model)
        {
            var internship = await _dbContext.Internships.Include(p => p.Interns).FirstOrDefaultAsync(p => p.Id == id);
            if (internship == null)
                throw new ServiceException("Internship Not Found", $"Internship with id {id} not found", StatusCodes.Status404NotFound);

            internship.Name = model.Name;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InternshipDto>(internship);
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
    }
}
