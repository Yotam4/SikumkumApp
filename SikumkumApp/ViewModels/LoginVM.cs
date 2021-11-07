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
using SikumkumApp.Services;
using System.Threading;
using System.Threading.Tasks;
using SikumkumApp.Views;

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

        public ICommand LoginCommand => new Command(LoginFunctionAsync);
        private async void LoginFunctionAsync()
        {
            try
            {
                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();

                User loggingUser = new User();

                loggingUser = await API.LoginAsync(this.Username, this.Password);

                if (loggingUser != null) //User logged in.
                {
                    UserPage up = new UserPage();
                    up.BindingContext = loggingUser;  //Temporary to test if it works.
                    App.Current.MainPage = up;

                }
                else
                {

                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public ICommand ToSignUp => new Command(ToSignUpFunction);
        private void ToSignUpFunction()
        {
            SignUp su = new SignUp();
            App.Current.MainPage = su;
        }

        #endregion
    }
}
