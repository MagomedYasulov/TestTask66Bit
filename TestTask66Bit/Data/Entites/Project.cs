namespace TestTask66Bit.Data.Entites
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public List<Intern> Interns { get; set; } = [];
    }
}
