using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Spg.Fachtheorie.Aufgabe2.Model
{
    public class Student
    {
        public int Id { get; private set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<Subject> Subjects { get; set; } = new();
        public List<Grade> Grades { get; set; } = new();
        public List<Enrollment> Enrollments { get; set; } = new();

        [NotMapped]
        public double AverageGrade
        {
            get
            {
                return Grades.Any() ? Grades.Average(g => g.Value) : 0;
            }
        }

        public double GetAverageGradeForSubject(string subjectName)
        {
            var subjectGrades = Grades.Where(g => g.Subject.Name == subjectName);
            return subjectGrades.Any() ? subjectGrades.Average(g => g.Value) : 0;
        }

        public Student()
        {
        }

        public Student(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public void AddGrade(Grade grade)
        {
            if (grade.Value < 1 || grade.Value > 5)
            {
                throw new ArgumentException("Grade value must be between 1 and 5.");
            }
            Grades.Add(grade);
        }
    }
}
