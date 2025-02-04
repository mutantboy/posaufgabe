using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe2.Model;
using Spg.Fachtheorie.Aufgabe2.Services;

namespace Spg.Fachtheorie.Aufgabe2.Test.Helpers
{
    public class DatabaseUtilities
    {
        public static Aufgabe2Database GetDbFile()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite("Data Source = C:\\Scratch\\Aufgabe2_Tests.db")
                .Options;

            Aufgabe2Database db = new Aufgabe2Database(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }
        public static Aufgabe2Database GetDbInMemory()
        {
            SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite(connection)
                .Options;

            Aufgabe2Database db = new Aufgabe2Database(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        // TODO: Seed Database for Unit Tests
        public static void SeedDatabase(Aufgabe2Database db)
        {
            // Create some test students
            var students = new List<Student>
            {
                new Student { FirstName = "John", LastName = "Doe" },
                new Student { FirstName = "Jane", LastName = "Smith" }
            };
            db.Students.AddRange(students);

            // Create some test subjects
            var subjects = new List<Subject>
            {
                new Subject { Name = "BAP", Seats = 5 },
                new Subject { Name = "IOT", Seats = 5 },
                new Subject { Name = "SOS", Seats = 3 }
            };
            db.Subjects.AddRange(subjects);

            // Create some test grades
            var grades = new List<Grade>();
            foreach (var student in students)
            {
                foreach (var subject in subjects)
                {
                    grades.Add(new Grade
                    {
                        Value = new Random().Next(1, 6),
                        Student = student,
                        Subject = subject
                    });
                }
            }
            db.Grades.AddRange(grades);

            db.SaveChanges();
        }
    }
}
