using EntityFramework03;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineLearningPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            using AppDbContext db = new AppDbContext();
            SeedData(db);
            AddInstructor(db);
            AddCourse(db, 1, 4);
            AddSeveralStudents(db);
            EnrollStudent(db, 4, 3);
            FindCourse(db, 2, "React & Redux Masterclass");
            FilterCourses(db, 40, 150);
            PrintSubCategories(db, 1);
            PrintCourseRoster(db, 1);
            ChangePrice(db, 1, 70);
            UpdateProgress(db, 1, 2, 100);
            PublishCourse(db, 5);
            Unenroll(db, 4, 3);
            DeleteCategoryUnsafe(db, 1);
            DeleteCategorySafe(db, 1);
            DeleteCourseWithEnrollments(db, 1);
            BulkPriceRaise(db, CourseLevel.Intermediate);
            BulkDeleteDropped(db);
        }

        static void SeedData(AppDbContext db)
        {
            if (!db.Students.Any())
            {
                db.Students.AddRange(
                    new Student { FullName = "Omar Khaled", Email = "omar@mail.com", DateOfBirth = new DateTime(2001, 3, 14) },
                    new Student { FullName = "Mona Adel", Email = "mona@mail.com", DateOfBirth = new DateTime(2002, 7, 22) },
                    new Student { FullName = "Youssef Nader", Email = "youssef@mail.com", DateOfBirth = new DateTime(1999, 11, 5) },
                    new Student { FullName = "Fatima Hassan", Email = "fatima@mail.com", DateOfBirth = new DateTime(2003, 1, 30) },
                    new Student { FullName = "Karim Said", Email = "karim@mail.com", DateOfBirth = new DateTime(2000, 9, 18) }
                );
                db.SaveChanges();
            }

            if (!db.Enrollments.Any())
            {
                db.Enrollments.AddRange(
                    new Enrollment { StudentId = 1, CourseId = 1, EnrollmentDate = DateTime.Now, Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 95 },
                    new Enrollment { StudentId = 1, CourseId = 2, EnrollmentDate = DateTime.Now, Progress = 40, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = 2, CourseId = 1, EnrollmentDate = DateTime.Now, Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 88 },
                    new Enrollment { StudentId = 2, CourseId = 3, EnrollmentDate = DateTime.Now, Progress = 20, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = 3, CourseId = 2, EnrollmentDate = DateTime.Now, Progress = 15, Status = EnrollmentStatus.Dropped },
                    new Enrollment { StudentId = 3, CourseId = 4, EnrollmentDate = DateTime.Now, Progress = 60, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = 4, CourseId = 1, EnrollmentDate = DateTime.Now, Progress = 10, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = 5, CourseId = 4, EnrollmentDate = DateTime.Now, Progress = 10, Status = EnrollmentStatus.Dropped }
                );
                db.SaveChanges();
            }
        }

        static void AddInstructor(AppDbContext db)
        {
            Instructor inst = new Instructor { FullName = "Hany Mostafa", Email = "hany@mail.com", JoinedDate = DateTime.Now };
            db.Instructors.Add(inst);
            db.SaveChanges();
            Console.WriteLine($"New Instructor Id: {inst.Id}");
        }

        static void AddCourse(AppDbContext db, int instId, int catId)
        {
            if (!db.Instructors.Any(i => i.Id == instId) || !db.Categories.Any(c => c.Id == catId)) { Console.WriteLine("Instructor or Category missing."); return; }
            Course c = new Course { Title = "EF Core Basics", Price = 100, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = DateTime.Now, InstructorId = instId, CategoryId = catId };
            db.Courses.Add(c);
            db.SaveChanges();
            Console.WriteLine($"Added Course Id: {c.Id}");
        }

        static void AddSeveralStudents(AppDbContext db)
        {
            List<Student> list = new List<Student>
            {
                new Student { FullName = "Kerolos ashraf", Email = "kerolos@mail.com", DateOfBirth = new DateTime(2002, 9, 10) },
                new Student { FullName = "amr ahmed", Email = "nouran@mail.com", DateOfBirth = new DateTime(2002, 8, 15) },
                new Student { FullName = "Ziad walid", Email = "ziad@mail.com", DateOfBirth = new DateTime(2000, 12, 1) }
            };
            db.Students.AddRange(list);
            db.SaveChanges();
            Console.WriteLine($"Added Students Ids: {string.Join(", ", list.Select(s => s.Id))}");
        }

        static void EnrollStudent(AppDbContext db, int sId, int cId)
        {
            Course? c = db.Courses.Find(cId);
            if (db.Students.Find(sId) == null || c == null) { Console.WriteLine("Refused: Student or Course not found."); return; }
            if (!c.IsPublished) { Console.WriteLine("Refused: Course not published."); return; }
            if (db.Enrollments.Any(e => e.StudentId == sId && e.CourseId == cId)) { Console.WriteLine("Refused: Already enrolled."); return; }
            db.Enrollments.Add(new Enrollment { StudentId = sId, CourseId = cId, EnrollmentDate = DateTime.Now, Progress = 0, Status = EnrollmentStatus.Active });
            db.SaveChanges();
            Console.WriteLine($"Enrolled Student {sId} in Course {cId}.");
        }

        static void FindCourse(AppDbContext db, int id, string title)
        {
            Console.WriteLine(db.Courses.Find(id)?.Title ?? "not found");
            Console.WriteLine(db.Courses.FirstOrDefault(c => c.Title == title)?.Title ?? "not found");
        }

        static void FilterCourses(AppDbContext db, decimal min, decimal max)
        {
            var list = db.Courses.Where(c => c.Price >= min && c.Price <= max).OrderByDescending(c => c.Price).Select(c => new { c.Title, c.Level, c.Price }).ToList();
            list.ForEach(c => Console.WriteLine($"{c.Title} - {c.Level} - {c.Price}"));
        }

        static void PrintSubCategories(AppDbContext db, int parentId)
        {
            Console.WriteLine("Subs: " + string.Join(", ", db.Categories.Where(c => c.ParentCategoryId == parentId).Select(c => c.Name)));
            Console.WriteLine("Top: " + string.Join(", ", db.Categories.Where(c => c.ParentCategoryId == null).Select(c => c.Name)));
        }

        static void PrintCourseRoster(AppDbContext db, int cId)
        {
            List<int> ids = db.Enrollments.Where(e => e.CourseId == cId).Select(e => e.StudentId).ToList();
            List<string> names = db.Students.Where(s => ids.Contains(s.Id)).Select(s => s.FullName).ToList();
            Console.WriteLine($"Count: {names.Count} | Students: {string.Join(", ", names)}");
        }

        static void ChangePrice(AppDbContext db, int cId, decimal price)
        {
            Course? c = db.Courses.Find(cId);
            if (c == null) { Console.WriteLine("Course not found."); return; }
            c.Price = price;
            db.SaveChanges();
        }

        static void UpdateProgress(AppDbContext db, int sId, int cId, int prog)
        {
            if (prog < 0 || prog > 100) { Console.WriteLine("Invalid progress."); return; }
            Enrollment? e = db.Enrollments.Find(sId, cId);
            if (e == null) { Console.WriteLine("Enrollment not found."); return; }
            e.Progress = prog;
            if (prog == 100) { e.Status = EnrollmentStatus.Completed; e.FinalGrade = 90; }
            db.SaveChanges();
        }

        static void PublishCourse(AppDbContext db, int cId)
        {
            Course? c = db.Courses.Find(cId);
            if (c == null) { Console.WriteLine("Course not found."); return; }
            c.IsPublished = true;
            c.PublishedDate = DateTime.Now;
            db.SaveChanges();
        }

        static void Unenroll(AppDbContext db, int sId, int cId)
        {
            Enrollment? e = db.Enrollments.Find(sId, cId);
            if (e == null) { Console.WriteLine("Not found."); return; }
            db.Enrollments.Remove(e);
            db.SaveChanges();
        }

        static void DeleteCategoryUnsafe(AppDbContext db, int id)
        {
            Category? cat = db.Categories.Find(id);
            if (cat == null) return;
            try { db.Categories.Remove(cat); db.SaveChanges(); }
            catch (DbUpdateException ex) { Console.WriteLine($"Caught Error: {ex.InnerException?.Message ?? ex.Message}"); db.Entry(cat).State = EntityState.Unchanged; }
        }

        static void DeleteCategorySafe(AppDbContext db, int id)
        {
            if (db.Categories.Any(c => c.ParentCategoryId == id)) { Console.WriteLine("Safe Check: Has sub-categories."); return; }
            Category? cat = db.Categories.Find(id);
            if (cat != null) { db.Categories.Remove(cat); db.SaveChanges(); }
        }

        static void DeleteCourseWithEnrollments(AppDbContext db, int cId)
        {
            Course? c = db.Courses.Find(cId);
            if (c == null) return;
            Console.WriteLine($"Before delete: {db.Enrollments.Count(e => e.CourseId == cId)}");
            db.Courses.Remove(c);
            db.SaveChanges();
            Console.WriteLine($"After delete: {db.Enrollments.Count(e => e.CourseId == cId)}");
        }

        static void BulkPriceRaise(AppDbContext db, CourseLevel lvl) => Console.WriteLine($"Updated: {db.Courses.Where(c => c.IsPublished && c.Level == lvl).ExecuteUpdate(s => s.SetProperty(c => c.Price, c => c.Price * 1.10m))}");
        static void BulkDeleteDropped(AppDbContext db) => Console.WriteLine($"Deleted: {db.Enrollments.Where(e => e.Status == EnrollmentStatus.Dropped).ExecuteDelete()}");
    }
}