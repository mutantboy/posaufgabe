using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spg.Fachtheorie.Aufgabe2.Model
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Seats { get; set; }

        [JsonIgnore]
        public List<Grade> Grades { get; set; } = new();

        [JsonIgnore]
        public List<Student> Students { get; set; } = new();

        [JsonIgnore]
        public List<Enrollment> Enrollments { get; set; } = new();

        public Subject()
        {
        }

        public Subject(string name, int seats)
        {
            Name = name;
            Seats = seats;
        }
    }

}
