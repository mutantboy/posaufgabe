using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.Fachtheorie.Aufgabe1.Model
{
    public class Subject
    {
        public Subject(string code, string name, string description)
        {
            Code = code;
            Name = name;
            Description = description;
        }

        protected Subject() { }

        public int Id { get; private set; } = default!;

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        
    }
}
