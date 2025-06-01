using TestTask66Bit.Data.Entites;

namespace TestTask66Bit.ViewModels.Response
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public InternDto[] Interns { get; set; } = [];
    }
}
