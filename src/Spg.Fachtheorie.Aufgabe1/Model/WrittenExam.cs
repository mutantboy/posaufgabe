using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe1.Model
{
    public class WrittenExam : Exam
    {
        public WrittenExam(DateTime examStart, DateTime examEnd, Subject subject, Student student, Teacher examiner, Teacher assessor) : base(examStart, examEnd, subject, student, examiner, assessor)
        {
        }

        protected WrittenExam() { }
        
    }
}
