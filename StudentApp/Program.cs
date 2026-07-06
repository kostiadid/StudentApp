using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StudentApp.Database;
using StudentApp.Repositories;
using StudentApp.Services;
using StudentApp.Validators;

namespace StudentApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<StudentDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddScoped<IStudentDbRepo, StudentDbRepo>();
            builder.Services.AddScoped<IPetitionService, PetitionService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddValidatorsFromAssembly(typeof(StudentCreateDtoValidator).Assembly);
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
