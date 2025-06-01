using AutoMapper;
using TestTask66Bit.Data.Entites;
using TestTask66Bit.ViewModels.Request;
using TestTask66Bit.ViewModels.Response;

namespace TestTask66Bit.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProjectDto, Project>().ForMember(p => p.Interns, opt => opt.Ignore());
            CreateMap<CreateInternshipDto, Internship>().ForMember(i => i.Interns, opt => opt.Ignore());
            CreateMap<CreateInternDto, Intern>();

            CreateMap<Project, ProjectDto>();
            CreateMap<Internship, InternshipDto>();
            CreateMap<Intern, InternDto>();
            CreateMap<Project, ProjectPartialDto>();
            CreateMap<Internship, InternshipPartialDto>();
        }
    }
}
