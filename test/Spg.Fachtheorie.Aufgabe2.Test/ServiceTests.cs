using Spg.Fachtheorie.Aufgabe2.Exceptions;
using Spg.Fachtheorie.Aufgabe2.Model;
using Spg.Fachtheorie.Aufgabe2.Services;
using Spg.Fachtheorie.Aufgabe2.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe2.Test
{
    public class ServiceTests
    {
        [Fact]
        public void Should_CreateDatabaseAndSeedWithTestData()
        {
            using (Aufgabe2Database db = DatabaseUtilities.GetDbInMemory())
            {
                DatabaseUtilities.SeedDatabase(db);

                Assert.True(db.Students.Any());
                Assert.True(db.Subjects.Any());
                Assert.True(db.Grades.Any());
            }
        }

        [Fact]
        public void Should_ThrowServiceExceptionWhenStudentIdIsInvalid()
        {
            using (Aufgabe2Database db = DatabaseUtilities.GetDbInMemory())
            {
                DatabaseUtilities.SeedDatabase(db);
                var service = new ApplicationService(db);

                Assert.Throws<ServiceException>(() =>
                    service.TryAddEnrollment(-1, "BAP", "IOT", "SOS"));
            }
        }

        [Fact]
        public void Should_ThrowServiceExceptionWhenSubjectIsInvalid()
        {
            using (Aufgabe2Database db = DatabaseUtilities.GetDbInMemory())
            {
                DatabaseUtilities.SeedDatabase(db);
                var service = new ApplicationService(db);
                var student = db.Students.First();

                Assert.Throws<ServiceException>(() =>
                    service.TryAddEnrollment(student.Id, "INVALID", "IOT", "SOS"));
            }
        }

        [Fact]
        public void Should_ReturnFalseWhenSubjectIsFull()
        {
            using (Aufgabe2Database db = DatabaseUtilities.GetDbInMemory())
            {
                DatabaseUtilities.SeedDatabase(db);
                var service = new ApplicationService(db);

                var subject = db.Subjects.First();
                subject.Seats = 0;
                db.SaveChanges();

                var student = db.Students.First();

                var result = service.TryAddEnrollment(
                    student.Id,
                    subject.Name,
                    db.Subjects.Skip(1).First().Name,
                    db.Subjects.Skip(2).First().Name
                );

                Assert.False(result);
            }
        }

        [Fact]
        public void Should_SuccessfullyAddEnrollment()
        {
            using (Aufgabe2Database db = DatabaseUtilities.GetDbInMemory())
            {
                DatabaseUtilities.SeedDatabase(db);
                var service = new ApplicationService(db);

                var student = db.Students.First();
                var subjects = db.Subjects.Take(3).ToList();

                var result = service.TryAddEnrollment(
                    student.Id,
                    subjects[0].Name,
                    subjects[1].Name,
                    subjects[2].Name
                );

                Assert.True(result);
                var enrollments = db.Enrollments
                    .Where(e => e.StudentId == student.Id)
                    .ToList();
                Assert.Equal(3, enrollments.Count);
            }
        }

        [Fact]
        public void Should_RankStudentsByAverageGrade()
        {
            using (Aufgabe2Database db = DatabaseUtilities.GetDbInMemory())
            {
                DatabaseUtilities.SeedDatabase(db);
                var service = new ApplicationService(db);

                var subject = db.Subjects.First();
                var students = db.Students.Take(2).ToList();

                students[0].Grades = new List<Grade>
                {
                    new Grade { Value = 1, Student = students[0], Subject = subject },
                    new Grade { Value = 1, Student = students[0], Subject = subject },
                    new Grade { Value = 1, Student = students[0], Subject = subject }
                };
                students[1].Grades = new List<Grade>
                {
                    new Grade { Value = 2, Student = students[1], Subject = subject },
                    new Grade { Value = 2, Student = students[1], Subject = subject },
                    new Grade { Value = 2, Student = students[1], Subject = subject }
                };
                db.SaveChanges();

                service.TryAddEnrollment(students[0].Id, subject.Name, "IOT", "SOS");
                service.TryAddEnrollment(students[1].Id, subject.Name, "IOT", "SOS");

                var rankedStudents = service.RankStudentByAverageGrade(subject.Name, 1);

                Assert.Single(rankedStudents);
                Assert.Equal(students[0].Id, rankedStudents.First().Id);
            }
        }

        [Fact]
        public void Should_HandleNoAvailableSeats_Correctly()
        {
            using (Aufgabe2Database db = DatabaseUtilities.GetDbInMemory())
            {
                DatabaseUtilities.SeedDatabase(db);
                var service = new ApplicationService(db);

                var subject = db.Subjects.First();
                subject.Seats = 0;
                db.SaveChanges();

                var student = db.Students.First();

                var result = service.TryAddEnrollment(
                    student.Id,
                    subject.Name,
                    "IOT",
                    "SOS"
                );

                Assert.False(result);
            }
        }
    }
}

