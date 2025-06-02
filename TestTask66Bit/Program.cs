
using TestTask66Bit.Extensions;
using TestTask66Bit.Hubs;

namespace TestTask66Bit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.AddData();
            builder.AddControllers();
            builder.AddSignalR();
            builder.AddSwagger();
            builder.AddFluentValidation();
            builder.AddAutoMapper();

            builder.AddExceptionHandler();
            builder.AddAppServices();

            var app = builder.Build();

            app.SeedData();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseExceptionHandler();
            app.UseAuthorization();

            app.MapHub<InternsHub>("/interns");
            app.MapControllers();

            app.Run();
        }
    }
}
