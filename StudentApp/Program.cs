using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StudentApp.Database;
using StudentApp.Mapping;
using StudentApp.Middleware;
using StudentApp.Repositories;
using StudentApp.Services;
using StudentApp.Validators;
using System.Text;

namespace StudentApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudentDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


            var jwt = builder.Configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = key
                };
            });

            builder.Services.AddAuthorization();


            builder.Services.AddScoped<IStudentDbRepo, StudentDbRepo>();
            builder.Services.AddScoped<IPetitionService, PetitionService>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddValidatorsFromAssembly(typeof(StudentCreateDtoValidator).Assembly);
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
               c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
               {
                                Name = "Authorization",
                                Type = SecuritySchemeType.Http,
                                Scheme = "bearer",
                                BearerFormat = "JWT",
                                In = ParameterLocation.Header
               });

               c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
            var app = builder.Build();
            app.UseSerilogRequestLogging();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}