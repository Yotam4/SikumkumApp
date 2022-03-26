using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SikumkumApp.Models;

namespace SikumkumApp.Models
{
    public class OpeningObject
    {
        public List<Subject> SubjectsList { get; set; }
        public List<FileType> FileTypeList { get; set; }
        public List<StudyYear> StudyYearList { get; set; }

        public OpeningObject() { }

        public OpeningObject(List<Subject> subjects, List<FileType> fileTypes, List<StudyYear> studyYears)
        {
            this.SubjectsList = subjects;
            this.FileTypeList = fileTypes;
            this.StudyYearList = studyYears;
        }
    }
}
