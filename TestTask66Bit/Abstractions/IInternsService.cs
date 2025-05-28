using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Abstractions
{
    public interface IInternsService
    {
        public Task<InternDto> Get(int id);
        public Task<InternDto[]> Get();
        public Task<InternDto> Create(CreateInternDto model);
        public Task<InternDto> Update(int id, UpdateInternDto model);

        public Task Delete(int internId);
    }
}
