using Bogus.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spg.Fachtheorie.Aufgabe2.Model
{
    public class Grade
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Value { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }

        [JsonIgnore]
        public Student Student { get; set; } = default!;

        [JsonIgnore]
        public Subject Subject { get; set; } = default!;

        public Grade()
        {
        }

        public Grade(int value, Student student, Subject subject)
        {
            Value = value;
            Student = student;
            Subject = subject;
            StudentId = student.Id;
            SubjectId = subject.Id;
        }
    }
}
