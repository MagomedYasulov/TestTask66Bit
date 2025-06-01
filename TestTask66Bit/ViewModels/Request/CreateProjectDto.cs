using TestTask66Bit.Data.Entites;

namespace TestTask66Bit.ViewModels.Request
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public int[]? Interns { get; set; }
    }
}
