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
            CreateMap<CreateProjectDto, Project>();
            CreateMap<CreateInternshipDto, Internship>();
            CreateMap<CreateInternDto, Intern>();

            CreateMap<Project, ProjectDto>();
            CreateMap<Internship, InternshipDto>();
            CreateMap<Intern, InternDto>();
        }
    }
}
