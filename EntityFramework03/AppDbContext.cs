using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework03
{
    public class AppDbContext : DbContext
    {
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=OnlineLearningPlatformDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
