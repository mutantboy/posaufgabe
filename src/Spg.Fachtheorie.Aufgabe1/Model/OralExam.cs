using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe1.Model
{
    public class OralExam : Exam
    {

        public OralExam(DateTime examStart, DateTime examEnd, Subject subject, Student student, Teacher examiner, Teacher assessor, DateTime preparationStart) : base(examStart, examEnd, subject, student, examiner, assessor)
        {
            PreparationStart = preparationStart;

        }

        protected OralExam() { }

        public DateTime PreparationStart { get; set; }

        
    }
}
