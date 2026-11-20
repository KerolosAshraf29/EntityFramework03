using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework03
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.FullName).IsRequired().HasMaxLength(150);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(200);
            builder.Property(i => i.Bio).HasMaxLength(1000);
            builder.Property(i => i.JoinedDate).IsRequired();

            builder.HasData(
                new Instructor { Id = 1, FullName = "Dr. Ahmed Mansour", Email = "ahmed@gmail.com", Bio = "Senior Software Architect", JoinedDate = new DateTime(2023, 1, 15) },
                new Instructor { Id = 2, FullName = "Eng. Sarah Nabil", Email = "sarah@gmail.com", Bio = "Full-stack .NET Lead", JoinedDate = new DateTime(2023, 5, 20) },
                new Instructor { Id = 3, FullName = "Prof. Tarek Zaki", Email = "tarek@gmail.com", Bio = "Database & AI Specialist", JoinedDate = new DateTime(2024, 2, 10) }
            );
        }
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(p => p.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Seed 5 Categories across 3 levels:
            // Level 1: Programming (1)
            // Level 2: Backend (2), Frontend (3)
            // Level 3: ASP.NET Core (4), React (5)
            builder.HasData(
                new Category { Id = 1, Name = "Programming", Description = "Top level software category", ParentCategoryId = null },
                new Category { Id = 2, Name = "Backend", Description = "Server side development", ParentCategoryId = 1 },
                new Category { Id = 3, Name = "Frontend", Description = "Client side web development", ParentCategoryId = 1 },
                new Category { Id = 4, Name = "ASP.NET Core", Description = "C# and .NET web framework", ParentCategoryId = 2 },
                new Category { Id = 5, Name = "React", Description = "JavaScript UI library", ParentCategoryId = 3 }
            );
        }
    }

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Description).HasMaxLength(2000);
            builder.Property(c => c.Price).HasPrecision(18, 2);
            builder.Property(c => c.Level).IsRequired();
            builder.Property(c => c.IsPublished).IsRequired();

            builder.HasOne(c => c.Instructor)
                   .WithMany(i => i.Courses)
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Category)
                   .WithMany(cat => cat.Courses)
                   .HasForeignKey(c => c.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Course { Id = 1, Title = "C# Fundamentals", Description = "Basics of C#", Price = 50.00m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 1, 10), InstructorId = 1, CategoryId = 1 },
                new Course { Id = 2, Title = "ASP.NET Core Web API", Description = "Build REST APIs", Price = 120.00m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2024, 3, 15), InstructorId = 2, CategoryId = 4 },
                new Course { Id = 3, Title = "Advanced Microservices in .NET", Description = "Distributed architecture", Price = 250.00m, Level = CourseLevel.Advanced, IsPublished = true, PublishedDate = new DateTime(2024, 6, 1), InstructorId = 2, CategoryId = 4 },
                new Course { Id = 4, Title = "React & Redux Masterclass", Description = "Modern React", Price = 90.00m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2024, 2, 20), InstructorId = 1, CategoryId = 5 },
                new Course { Id = 5, Title = "Drafting SQL Performance", Description = "Optimizing SQL Queries", Price = 75.00m, Level = CourseLevel.Advanced, IsPublished = false, PublishedDate = null, InstructorId = 3, CategoryId = 2 },
                new Course { Id = 6, Title = "Upcoming Cloud Fundamentals", Description = "Cloud computing basics", Price = 40.00m, Level = CourseLevel.Beginner, IsPublished = false, PublishedDate = null, InstructorId = 3, CategoryId = 1 }
            );
        }
    }

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(150);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(200);
            builder.Property(s => s.DateOfBirth).IsRequired();
        }
    }

    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => new { e.StudentId, e.CourseId });

            builder.Property(e => e.Progress).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.FinalGrade).HasPrecision(5, 2);

            builder.HasOne(e => e.Student)
                   .WithMany(s => s.Enrollments)
                   .HasForeignKey(e => e.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Enrollments)
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
