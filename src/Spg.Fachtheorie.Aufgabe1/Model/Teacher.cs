using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe1.Model
{
    public class Teacher
    {
        public Teacher(string teacherIdentifier, string firstName, string lastName)
        {
            TeacherIdentifier = teacherIdentifier;
            FirstName = firstName;
            LastName = lastName;
        }

        protected Teacher() { }

        public int Id { get; private set; } = default!;

        public string TeacherIdentifier { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<Exam> ExamAssessor = new();
        public List<Exam> ExamExaminer = new();



    }
}
