using System;
using System.Collections.Generic;


namespace SikumkumApp.Models
{
    public partial class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User()
        {

        }

        public User(string username, string email, string password)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
        }

        public User(User u)
        {
            this.Username = u.Username;
            this.Email = u.Email;
            this.Password = u.Password;
        }


    }
}
