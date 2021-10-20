using System;
using System.Collections.Generic;


namespace SikumkumServerBL.Models
{
    public partial class Chat
    {
        public Chat()
        {
            //SikumFiles = new HashSet<SikumFile>();
        }

        public int ChatBoxId { get; set; }
        public string ChatTitle { get; set; }
        public string ChatDesc { get; set; }

    }
}
