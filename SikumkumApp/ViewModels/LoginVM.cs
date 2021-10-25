using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SikumkumApp.Models;

namespace SikumkumApp.ViewModels
{
    class LoginVM : INotifyPropertyChanged
    {
        
        public LoginVM()
        {

        }

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Variables

        private string username { get; set; }
        public string Username
        {
            get { return this.username; }
            set
            {
                this.username = value;
                this.OnPropertyChanged("Username");
            }
        }

        private string password { get; set; }
        public string Password
        {
            get { return this.password; }
            set
            {
                this.password = value;
                OnPropertyChanged("Password");
            }
        }

        #endregion

        #region Commands

        public ICommand LoginCommand => new Command(LoginFunction);
        private void LoginFunction()
        {
            
        }

        #endregion
    }
}
