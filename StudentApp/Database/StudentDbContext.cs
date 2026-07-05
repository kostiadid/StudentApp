using Microsoft.EntityFrameworkCore;
using StudentApp.Entities;

namespace StudentApp.Database
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
           : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Petition> Petitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentDbContext).Assembly);
        }
    }
}
