using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe1.Model
{
    public class Student
    {
        public Student(string studentIdentifier, string firstName, string lastName, DateTime birthDate)
        {
            StudentIdentifier = studentIdentifier;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        protected Student() { }

        public int Id { get; private set; } = default!;
        public string StudentIdentifier { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }

        
    }
}
