using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe1.Model;

namespace Spg.Fachtheorie.Aufgabe1.Infrastructure
{
    public class Aufgabe1Database : DbContext
    {
        public DbSet<FinalExamination> FinalExaminations { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<OralExam> OralExams { get; set; }
        public DbSet<WrittenExam> WrittenExams { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public Aufgabe1Database(DbContextOptions<Aufgabe1Database> options)
            : base(options)
        { }

        public Aufgabe1Database(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinalExamination>().HasIndex(f => f.Code).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.StudentIdentifier).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(t => t.TeacherIdentifier).IsUnique();
            modelBuilder.Entity<Subject>().HasIndex(s => s.Code).IsUnique();


            modelBuilder.Entity<FinalExamination>()
                .HasMany(f => f.Exams)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Student)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Examiner)
                .WithMany(e => e.ExamExaminer)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Assessor)
                .WithMany(e => e.ExamAssessor)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>()
                .HasDiscriminator<string>("ExamType")
                .HasValue<Exam>("Exam")
                .HasValue<OralExam>("OralExam")
                .HasValue<WrittenExam>("WrittenExam");

            base.OnModelCreating(modelBuilder);
        }
    }
}