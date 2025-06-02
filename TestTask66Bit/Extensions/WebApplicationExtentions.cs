using TestTask66Bit.Data;
using TestTask66Bit.Data.Entites;

namespace TestTask66Bit.Extensions
{
    public static class WebApplicationExtentions
    {
        public static void SeedData(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            using var dbContex = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            dbContex.Internships.Add(new Internship() { Name = "Стажировка 1",  });
            dbContex.Internships.Add(new Internship() { Name = "Стажировка 2", });
            dbContex.Internships.Add(new Internship() { Name = "Стажировка 3", });

            dbContex.Projects.Add(new Project() { Name = "Проект 1",  });
            dbContex.Projects.Add(new Project() { Name = "Проект 2", });
            dbContex.Projects.Add(new Project() { Name = "Проект 3", });

            dbContex.SaveChanges();

            dbContex.Interns.Add(new Intern() 
            { 
                InternshipId = 1, 
                Name = "Имя 1", 
                Surname = "Фамилия 1",
                Email = "example1@gmail.com",
                Gender = Enums.Gender.Male,
                BirthDate = DateTime.UtcNow, 
                ProjectId = 1,
            });

            dbContex.Interns.Add(new Intern()
            {
                InternshipId = 1,
                Name = "Имя 2",
                Surname = "Фамилия 2",
                Email = "example2@gmail.com",
                Gender = Enums.Gender.Male,
                BirthDate = DateTime.UtcNow,
                ProjectId = 1,
            });

            dbContex.Interns.Add(new Intern()
            {
                InternshipId = 1,
                Name = "Имя 3",
                Surname = "Фамилия 3",
                Email = "example3@gmail.com",
                Gender = Enums.Gender.Male,
                BirthDate = DateTime.UtcNow,
                ProjectId = 1,
            });

            dbContex.SaveChanges();
        }
    }
}
