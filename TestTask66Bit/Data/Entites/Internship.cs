namespace TestTask66Bit.Data.Entites
{
    public class Internship : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public List<Intern> Interns { get; set; } = [];
    }
}
