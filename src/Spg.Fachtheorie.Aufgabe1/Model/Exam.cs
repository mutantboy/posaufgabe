using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe1.Model
{
    public abstract class  Exam 
    {
        public Exam(DateTime examStart, DateTime examEnd, Subject subject, Student student, Teacher examiner, Teacher assessor)
        {
            ExamStart = examStart;
            ExamEnd = examEnd;
            Subject = subject;
            Student = student;
            Examiner = examiner;
            Assessor = assessor;
        }

        protected Exam() { }
        

        public int Id { get; private set; } = default!;

        public DateTime ExamStart { get; set; }
        public DateTime ExamEnd { get; set; }

        public Subject Subject { get; set; } = default!;
        public Student Student { get; set; } = default!;
        public Teacher Examiner { get; set; } = default!;
        public Teacher Assessor { get; set; } = default!;

        
    }
}
