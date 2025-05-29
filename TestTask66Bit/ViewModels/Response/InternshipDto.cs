using TestTask66Bit.Data.Entites;

namespace TestTask66Bit.ViewModels.Response
{
    public class InternshipDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<InternDto>? Interns { get; set; } 
    }
}
