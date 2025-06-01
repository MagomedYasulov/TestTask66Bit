using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Abstractions
{
    public interface IInternshipsService
    {
        public Task<InternshipDto> Get(int id);
        public Task<InternshipDto[]> Get();
        public Task<InternshipPartialDto> Create(CreateInternshipDto model);
        public Task<InternshipPartialDto> Update(int id, UpdateInternshipDto model);

        public Task Delete(int internId);
    }
}
