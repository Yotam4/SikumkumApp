using System;
using System.Collections.Generic;


namespace SikumkumApp.Models
{
    public partial class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int UserID { get; set; }

        public User()
        {

        }

        public User(string username, string email, string password)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.IsAdmin = false;
            this.UserID = 0;
        }

        public User(string username, string password) //Overload for logging in
        {
            this.Username = username;
            this.Email = "";
            this.Password = password;
            this.IsAdmin = false;
            this.UserID = 0;
        }
        public User(User u)
        {
            this.Username = u.Username;
            this.Email = u.Email;
            this.Password = u.Password;
            this.IsAdmin = u.IsAdmin;
            this.UserID = u.UserID;
        }


    }
}
