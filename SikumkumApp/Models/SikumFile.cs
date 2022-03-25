using System;
using System.Collections.Generic;
using SikumkumApp.Models;

namespace SikumkumApp.Models
{
    public partial class SikumFile
    {
        public string Username { get; set; }
        public string TypeName { get; set; }
        public string YearName { get; set; }
        public string SubjectName { get; set; }
        public string Headline { get; set; }
        public string TextDesc { get; set; }
        public string Url { get; set; }

        public SikumFile(string username, string headline, string url, string yearName, string typeName, string subjectName, string textDesc)
        {
            this.Username = username;
            this.Headline = headline;
            this.Url = url;
            this.YearName = yearName;
            this.TypeName = typeName;
            this.TextDesc = textDesc;
            this.SubjectName = subjectName;

        }

    }
}
