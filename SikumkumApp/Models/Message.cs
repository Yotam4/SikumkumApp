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
        public string TheMessage { get; set; }
        public DateTime Date { get; set; }

        public Message() { }

        public Message(int fileID, int userId, string username, string theMessage)
        {
            this.Date = DateTime.Now;
            this.MessageId = -1; //non-existent value to preset for server.
            this.FileId = fileID;
            this.UserId = userId;
            this.Username = username;
            this.TheMessage = theMessage;
        }
    }
}
