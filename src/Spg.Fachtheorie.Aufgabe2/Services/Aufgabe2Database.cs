using Bogus;
using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe2.Model;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using static Bogus.DataSets.Name;

namespace Spg.Fachtheorie.Aufgabe2.Services
{
    public class Aufgabe2Database : DbContext
    {
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Grade> Grades => Set<Grade>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        public Aufgabe2Database()
        { }

        public Aufgabe2Database(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();

                entity.HasMany(e => e.Subjects)
                    .WithMany(e => e.Students);

                entity.HasMany(e => e.Grades)
                    .WithOne(e => e.Student)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Enrollments)
                    .WithOne(e => e.Student)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Seats).IsRequired();

                entity.HasMany(e => e.Grades)
                    .WithOne(e => e.Subject)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Enrollments)
                    .WithOne(e => e.Subject)
                    .HasForeignKey(e => e.SubjectName)
                    .HasPrincipalKey(e => e.Name)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Value).IsRequired();
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SubjectName).IsRequired();
                entity.Property(e => e.Rank).IsRequired();
            });
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(1832);

            var students = new Faker<Student>("de")
                .CustomInstantiator(f => new Student(
                    f.Name.FirstName(),
                    f.Name.LastName()))
                .Generate(20)
                .ToList();
            Students.AddRange(students);
            SaveChanges();

            var subjectNames = new[] { "AM", "POS", "DBI", "E", "D", "PRE", "BWM", "WMC", "TINF", "BAP", "IOT", "SOS" };
            var f = new Faker();
            var subjects = subjectNames.Select(name => new Subject(name, f.Random.Int(1, 20))).ToList();
            Subjects.AddRange(subjects);
            SaveChanges();

            var grades = new List<Grade>();
            foreach (var student in students)
            {
                var numGrades = f.Random.Int(3, 5);
                var studentSubjects = f.Random.ListItems(subjects, numGrades).ToList();

                foreach (var subject in studentSubjects)
                {
                    grades.Add(new Grade(f.Random.Int(1, 5), student, subject));
                }
            }
            Grades.AddRange(grades);
            SaveChanges();
        }
    }
}