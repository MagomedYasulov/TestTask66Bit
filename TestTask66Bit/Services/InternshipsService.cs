using TestTask66Bit.Abstractions;
using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Services
{
    public class InternshipsService : IInternshipsService
    {
        public Task<InternshipDto> Create(CreateInternshipDto model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int internId)
        {
            throw new NotImplementedException();
        }

        public Task<InternshipDto> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<InternshipDto[]> Get()
        {
            throw new NotImplementedException();
        }

        public Task<InternshipDto> Update(int id, UpdateInternshipDto model)
        {
            throw new NotImplementedException();
        }
    }
}
