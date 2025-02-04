using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe1.Model
{
    public class FinalExamination 
    {
        public FinalExamination(string code, string department, DateTime from, DateTime to)
        {
            Code = code;
            Department = department;
            From = from;
            To = to;
        }

        protected FinalExamination() { }

        public int Id { get; private set; } = default!;
        public string Code { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        protected List<Exam> _exams { get; set; } = new();
        public IReadOnlyList<Exam> Exams => _exams;

        
    }
}
