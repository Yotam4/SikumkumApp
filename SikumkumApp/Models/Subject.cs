using System;
using System.Collections.Generic;


namespace SikumkumApp.Models
{
    public partial class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public Subject() { }
        public Subject(string name, int id)
        {
            this.SubjectName = name;
            this.SubjectId = id;
        }
    }
}
