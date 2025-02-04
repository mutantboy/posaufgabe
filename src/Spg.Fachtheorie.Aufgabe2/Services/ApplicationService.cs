using Microsoft.EntityFrameworkCore;
using Spg.Fachtheorie.Aufgabe2.Exceptions;
using Spg.Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spg.Fachtheorie.Aufgabe2.Services
{
    public class ApplicationService
    {
        private readonly Aufgabe2Database _db;

        public ApplicationService(Aufgabe2Database db)
        {
            _db = db;
        }

        public bool TryAddEnrollment(int studentId, string subject1, string subject2, string subject3)
        {
            var student = _db.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                throw new ServiceException($"Student with ID {studentId} not found.");
            }

            var subjects = new[] { subject1, subject2, subject3 };
            var distinctSubjects = subjects.Distinct().ToList();
            if (distinctSubjects.Count != 3)
            {
                throw new ServiceException("All subjects must be different.");
            }

            var subjectEntities = _db.Subjects
                .Where(s => subjects.Contains(s.Name))
                .ToList();

            if (subjectEntities.Count != 3)
            {
                throw new ServiceException("One or more subjects not found.");
            }

            var firstChoiceSubject = subjectEntities.First(s => s.Name == subject1);
            var currentEnrollments = _db.Set<Enrollment>()
                .Count(e => e.SubjectName == subject1 && e.Rank == 1);

            if (currentEnrollments >= firstChoiceSubject.Seats)
            {
                return false;
            }

            var existingEnrollments = _db.Set<Enrollment>()
                .Where(e => e.StudentId == studentId)
                .ToList();
            _db.Set<Enrollment>().RemoveRange(existingEnrollments);

            var newEnrollments = new List<Enrollment>
            {
                new Enrollment(studentId, subject1, 1),
                new Enrollment(studentId, subject2, 2),
                new Enrollment(studentId, subject3, 3)
            };

            _db.Set<Enrollment>().AddRange(newEnrollments);
            _db.SaveChanges();

            return true;
        }

        public List<Student> RankStudentByAverageGrade(string subject, int numberOfSeats)
        {
            var studentsInSubject = _db.Students
                .Include(s => s.Grades)
                .Include(s => s.Enrollments)
                .Where(s => s.Enrollments.Any(e => e.SubjectName == subject && e.Rank == 1))
                .AsEnumerable() 
                .OrderBy(s => s.Grades.Any() ? s.Grades.Average(g => g.Value) : double.MaxValue)
                .Take(numberOfSeats)
                .ToList();

            return studentsInSubject;
        }

        public List<Student> StudentsPerElectiveSubject()
        {
            var enrolledStudents = _db.Students
                .Where(s => _db.Set<Enrollment>().Any(e => e.StudentId == s.Id))
                .OrderBy(s => s.AverageGrade)
                .ToList();

            return enrolledStudents;
        }
    }
}
