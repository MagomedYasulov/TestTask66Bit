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
                Name = "Magomed", 
                Surname = "Yasulov",
                Email = "magomed@gmail.com",
                Gender = Enums.Gender.Male,
                BirthDate = DateTime.UtcNow, 
                ProjectId = 1,
            });

            dbContex.Interns.Add(new Intern()
            {
                InternshipId = 1,
                Name = "Magomed 2",
                Surname = "Yasulov 2",
                Email = "magomed2@gmail.com",
                Gender = Enums.Gender.Male,
                BirthDate = DateTime.UtcNow,
                ProjectId = 1,
            });

            dbContex.Interns.Add(new Intern()
            {
                InternshipId = 1,
                Name = "Magomed 3",
                Surname = "Yasulov 3",
                Email = "magomed3@gmail.com",
                Gender = Enums.Gender.Male,
                BirthDate = DateTime.UtcNow,
                ProjectId = 1,
            });

            dbContex.SaveChanges();
        }
    }
}
