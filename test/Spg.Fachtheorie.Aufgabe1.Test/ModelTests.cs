using Spg.Fachtheorie.Aufgabe1.Infrastructure;
using Spg.Fachtheorie.Aufgabe1.Test.Helpers;
using Spg.Fachtheorie.Aufgabe1.Model;
using Microsoft.EntityFrameworkCore;

namespace Spg.Fachtheorie.Aufgabe1.Test
{
    [Collection("Sequential")]
    public class ModelTests
    {
        private Aufgabe1Database GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=exams.db")
                .Options;

            var db = new Aufgabe1Database(options);
            db.Database.EnsureDeleted();   
            db.Database.EnsureCreated();
            return db;
        }

        private Aufgabe1Database GetDbContext()
        {
            var options = new DbContextOptionsBuilder<Aufgabe1Database>()
                .UseInMemoryDatabase(databaseName: "examsDB")
                .Options;

            var db = new Aufgabe1Database(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        [Fact]
        public void Should_CreateDatabaseAndSeedWithTestData()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var students = db.Students.ToList();

            Assert.NotEmpty(students);
        }


        #region CRUD
        [Fact]
        public void Should_AddStudent()
        {
            using (var db = DatabaseUtilities.GetDbInMemory())
            {
                // Arrange
                var student = new Student("S123", "John", "Doe", DateTime.Parse("2000-01-01"));

                // Act
                db.Students.Add(student);
                db.SaveChanges();

                // Assert
                var fetchedStudent = db.Students.FirstOrDefault(s => s.StudentIdentifier == "S123");
                Assert.NotNull(fetchedStudent);
                Assert.Equal("John", fetchedStudent.FirstName);
            }
        }

        [Fact]
        public void Should_UpdateStudent()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);
            var student = db.Students.First();
            student.LastName = "Smith";

            db.Students.Update(student);
            db.SaveChanges();

            var updatedStudent = db.Students.FirstOrDefault(s => s.Id == student.Id);
            Assert.Equal("Smith", updatedStudent.LastName);
        }


        [Fact]
        public void Should_DeleteStudent()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);
            var student = db.Students.First();

            db.Students.Remove(student);
            db.SaveChanges();

            var deletedStudent = db.Students.FirstOrDefault(s => s.Id == student.Id);
            Assert.Null(deletedStudent);
        }

        [Fact]
        public void Should_AddTeacher()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            var teacher = new Teacher("T123", "Jane", "Doe");

            db.Teachers.Add(teacher);
            db.SaveChanges();

            var fetchedTeacher = db.Teachers.FirstOrDefault(t => t.TeacherIdentifier == "T123");
            Assert.NotNull(fetchedTeacher);
            Assert.Equal("Jane", fetchedTeacher.FirstName);
        }

        [Fact]
        public void Should_UpdateTeacher()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);
            var teacher = db.Teachers.First();
            teacher.LastName = "Johnson";

            db.Teachers.Update(teacher);
            db.SaveChanges();

            var updatedTeacher = db.Teachers.FirstOrDefault(t => t.Id == teacher.Id);
            Assert.Equal("Johnson", updatedTeacher.LastName);
        }

        [Fact]
        public void Should_DeleteTeacher()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);
            var teacher = db.Teachers.First();

            db.Teachers.Remove(teacher);
            db.SaveChanges();

            var deletedTeacher = db.Teachers.FirstOrDefault(t => t.Id == teacher.Id);
            Assert.Null(deletedTeacher);
        }

        [Fact]
        public void Should_AddSubject()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            var subject = new Subject("CH", "Chemistry", "Chem stuff");

            db.Subjects.Add(subject);
            db.SaveChanges();

            var fetchedSubject = db.Subjects.FirstOrDefault(s => s.Name == "Chemistry");
            Assert.NotNull(fetchedSubject);
            Assert.Equal("Chemistry", fetchedSubject.Name);
        }

        [Fact]
        public void Should_UpdateSubject()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);
            var subject = db.Subjects.First();
            subject.Name = "Biology";

            db.Subjects.Update(subject);
            db.SaveChanges();

            var updatedSubject = db.Subjects.FirstOrDefault(s => s.Id == subject.Id);
            Assert.Equal("Biology", updatedSubject.Name);
        }

        [Fact]
        public void Should_DeleteSubject()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);
            var subject = db.Subjects.First();

            db.Subjects.Remove(subject);
            db.SaveChanges();

            var deletedSubject = db.Subjects.FirstOrDefault(s => s.Id == subject.Id);
            Assert.Null(deletedSubject);
        }

        [Fact]
        public void Should_AddOralExam()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var student = db.Students.First();
            var examiner = db.Teachers.First();
            var assessor = db.Teachers.Skip(1).First();
            var subject = db.Subjects.First();

            var exam = new OralExam(
                examStart: DateTime.Parse("2023-06-16"),
                examEnd: DateTime.Parse("2023-06-16 10:30"),
                subject: subject,
                student: student,
                examiner: examiner,
                assessor: assessor,
                preparationStart: DateTime.Parse("2023-06-16 09:30")
            );

            db.OralExams.Add(exam);
            db.SaveChanges();

            var fetchedExam = db.OralExams.Skip(1).FirstOrDefault(e => e.Student.Id == student.Id && e.Examiner.Id == examiner.Id);
            Assert.NotNull(fetchedExam);
            Assert.Equal(DateTime.Parse("2023-06-16 09:30"), fetchedExam.PreparationStart);
        }

        [Fact]
        //update with tracking
        public void Should_UpdateOralExam()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var exam = db.OralExams
                         .Include(e => e.Student)
                         .Include(e => e.Subject)
                         .Include(e => e.Examiner)
                         .Include(e => e.Assessor)
                         .First();

            exam.PreparationStart = DateTime.Parse("2023-06-16 09:00");

            db.SaveChanges();  

            var updatedExam = db.OralExams
                                .AsNoTracking() 
                                .FirstOrDefault(e => e.Id == exam.Id);

            Assert.Equal(DateTime.Parse("2023-06-16 09:00"), updatedExam.PreparationStart);
        }

        [Fact]
        public void Should_DeleteOralExam()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var exam = db.OralExams.First();

            db.OralExams.Remove(exam);
            db.SaveChanges();

            var deletedExam = db.OralExams.FirstOrDefault(e => e.Id == exam.Id);
            Assert.Null(deletedExam);
        }

        [Fact]
        public void Should_AddWrittenExam()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var student = db.Students.First();
            var examiner = db.Teachers.First();
            var assessor = db.Teachers.Skip(1).First();
            var subject = db.Subjects.First();

            var exam = new WrittenExam(
                examStart: DateTime.Parse("2023-06-15"),
                examEnd: DateTime.Parse("2023-06-15 11:30"),
                subject: subject,
                student: student,
                examiner: examiner,
                assessor: assessor
            );

            db.WrittenExams.Add(exam);
            db.SaveChanges();

            var fetchedExam = db.WrittenExams.FirstOrDefault(e => e.Student.Id == student.Id && e.Examiner.Id == examiner.Id);
            Assert.NotNull(fetchedExam);
            Assert.Equal(DateTime.Parse("2023-06-15 11:30"), fetchedExam.ExamEnd);
        }

        [Fact]
        public void Should_UpdateWrittenExam()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var exam = db.WrittenExams.First();
            exam.ExamEnd = DateTime.Parse("2023-06-15 12:00");

            db.WrittenExams.Update(exam);
            db.SaveChanges();

            var updatedExam = db.WrittenExams.FirstOrDefault(e => e.Id == exam.Id);
            Assert.Equal(DateTime.Parse("2023-06-15 12:00"), updatedExam.ExamEnd);
        }

        [Fact]
        public void Should_DeleteWrittenExam()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var exam = db.WrittenExams.First();

            db.WrittenExams.Remove(exam);
            db.SaveChanges();

            var deletedExam = db.WrittenExams.FirstOrDefault(e => e.Id == exam.Id);
            Assert.Null(deletedExam);
        }
        #endregion CRUD

        #region Queries
        [Fact]
        public void Should_GetExamsForStudent()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var student = db.Students.FirstOrDefault();
            Assert.NotNull(student); 

            var examsForStudent = db.WrittenExams
                                    .Where(e => e.Student.Id == student.Id)
                                    .Include(e => e.Subject)
                                    .Include(e => e.Examiner)
                                    .Include(e => e.Assessor)
                                    .ToList();

            Assert.NotEmpty(examsForStudent);
            Assert.All(examsForStudent, exam =>
            {
                Assert.Equal(student.Id, exam.Student.Id);
                Assert.NotNull(exam.Subject);
                Assert.NotNull(exam.Examiner);
            });
        }

        [Fact]
        public void Should_GetExamsForTeacher()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var teacher = db.Teachers.First();

            var examsForTeacher = db.WrittenExams
                                    .Where(e => e.Examiner.Id == teacher.Id || e.Assessor.Id == teacher.Id)
                                    .Include(e => e.Subject)
                                    .Include(e => e.Student)
                                    .ToList();

            Assert.NotEmpty(examsForTeacher);
            Assert.All(examsForTeacher, exam =>
            {
                Assert.True(exam.Examiner.Id == teacher.Id || exam.Assessor.Id == teacher.Id);
                Assert.NotNull(exam.Student);
                Assert.NotNull(exam.Subject);
            });
        }

        [Fact]
        public void Should_GetExamsForSubject()
        {
            using var db = DatabaseUtilities.GetDbInMemory();
            DatabaseUtilities.SeedDatabase(db);

            var subject = db.Subjects.FirstOrDefault();
            Assert.NotNull(subject); 

            var examsForSubject = db.WrittenExams
                                    .Where(e =>e.Subject.Id == subject.Id)
                                    .Include(e => e.Student)
                                    .Include(e => e.Examiner)
                                    .Include(e => e.Assessor)
                                    .ToList();

            Assert.NotEmpty(examsForSubject);
            Assert.All(examsForSubject, exam =>
            {
                Assert.Equal(subject.Id, exam.Subject.Id);
                Assert.NotNull(exam.Student);
                Assert.NotNull(exam.Examiner);
            });
        }
        #endregion Queries
    }
}

