using System;
using System.Collections.Generic;
using System.Text;

namespace SikumkumApp.Models
{
    public class FileInfo
    {
        public long Length { get; set; }
        public string Name { get; set; }

        public FileInfo()
        {

        }

        public FileInfo(long length, string name)
        {
            this.Length = length;
            this.Name = name;
        }
    }
}
