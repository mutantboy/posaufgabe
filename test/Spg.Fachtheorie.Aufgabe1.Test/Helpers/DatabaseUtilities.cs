using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe1.Infrastructure;
using Spg.Fachtheorie.Aufgabe1.Model;
using System.Net.Mail;

namespace Spg.Fachtheorie.Aufgabe1.Test.Helpers
{
    public class DatabaseUtilities
    {
        public static Aufgabe1Database GetDbFile()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite("Data Source = C:\\Scratch\\Aufgabe1_Tests.db")
                .Options;

            Aufgabe1Database db = new Aufgabe1Database(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }
        public static Aufgabe1Database GetDbInMemory()
        {
            SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite(connection)
                .Options;

            Aufgabe1Database db = new Aufgabe1Database(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        // TODO: Seed Database for Unit Tests
        public static void SeedDatabase(Aufgabe1Database db)
        {
            // Adding Students
            var students = new List<Student>
                {
                    new Student("S001", "Alice", "Smith", DateTime.Parse("2001-01-01")),
                    new Student("S002", "Bob", "Jones", DateTime.Parse("2002-02-02"))
                };
            db.Students.AddRange(students);

            // Adding Teachers
            var teachers = new List<Teacher>
                {
                    new Teacher("T001", "Charles", "Brown"),
                    new Teacher("T002", "Diana", "Evans")
                };
            db.Teachers.AddRange(teachers);

            // Adding Subjects
            var subjects = new List<Subject>
                {
                    new Subject("MA", "Mathematics", "Doing maths n stuff"),
                    new Subject("PH", "Physics", "Physic stuff")
                };
            db.Subjects.AddRange(subjects);

            var oralExam = new OralExam(DateTime.Parse("2023-06-15 09:00"), DateTime.Parse("2023-06-15 10:00"), subjects[0], students[0], teachers[0], teachers[1], DateTime.Parse("2023-06-15 08:30"));
            var writtenExam = new WrittenExam(DateTime.Parse("2023-06-16 09:00"), DateTime.Parse("2023-06-16 11:00"), subjects[0], students[0], teachers[1], teachers[0]);

            db.OralExams.Add(oralExam);
            db.WrittenExams.Add(writtenExam);

            db.SaveChanges();
        }
    }
}
