using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe2.Model
{
    public class OptionalTopic
    {
        public string Name { get; set; } = string.Empty;
        public int Seats { get; set; }

        public List<Student> EnrolledStudents { get; set; } = new();

        public OptionalTopic(string name, int seats)
        {
            Name = name;
            Seats = seats;
        }
    }
}
