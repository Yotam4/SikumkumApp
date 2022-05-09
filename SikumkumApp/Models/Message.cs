using System;
using System.Collections.Generic;


namespace SikumkumApp.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Message1 { get; set; }
        public DateTime Date { get; set; }

        public Message() { }

    }
}
