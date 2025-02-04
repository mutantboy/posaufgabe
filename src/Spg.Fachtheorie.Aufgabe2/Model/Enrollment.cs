using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe2.Model
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required]
        public string SubjectName { get; set; } = string.Empty;

        [Required]
        [Range(1, 3)]
        public int Rank { get; set; } // 1 = Erstwahl, 2 = Zweitwahl, 3 = Drittwahl

        [JsonIgnore]
        public Student Student { get; set; } = default!;

        [JsonIgnore]
        public Subject Subject { get; set; } = default!;

        public Enrollment()
        {
        }

        public Enrollment(int studentId, string subjectName, int rank)
        {
            StudentId = studentId;
            SubjectName = subjectName;
            Rank = rank;
        }
    }
}


