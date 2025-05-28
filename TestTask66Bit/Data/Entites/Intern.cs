using TestTask66Bit.Enums;

namespace TestTask66Bit.Data.Entites
{
    public class Intern : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime BirthDate { get; set; }


        public int InternshipId { get; set; }
        public Internship Internship { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
