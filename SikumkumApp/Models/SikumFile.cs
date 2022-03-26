using System;
using System.Collections.Generic;
using SikumkumApp.Models;

namespace SikumkumApp.Models
{
    public partial class SikumFile
    {
        public string Username { get; set; }
        public int UserID { get; set; }
        public int TypeID { get; set; }
        public int YearID { get; set; }
        public int SubjectID { get; set; }
        public string Headline { get; set; }
        public string TextDesc { get; set; }
        public string Url { get; set; }

        public SikumFile() { }

        public SikumFile(int userID, string username, string headline, string url, int yearID, int typeID, int subjectID, string textDesc)
        {
            this.UserID = userID;
            this.Username = username;
            this.Headline = headline;
            this.TextDesc = textDesc;
            this.Url = url;
            this.YearID = yearID;
            this.TypeID = typeID;
            this.SubjectID = subjectID;

        }

    }
}
