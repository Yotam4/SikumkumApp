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

        private bool showNameError { get; set; }
        public bool ShowNameError
        {
            get => showNameError;
            set
            {
                showNameError = value;
                OnPropertyChanged("ShowNameError");
            }
        }

        private string nameError { get; set; }
        public string NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
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

        private bool showPasswordError { get; set; }
        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string passwordError { get; set; }
        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }

        private bool ShowUsernameError { get; set; }
        public string UsernameError { get; set; }

        #endregion

        #region Commands

        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(Username);
            if (this.ShowNameError)
                this.NameError = "שם משתמש לא יכול להיות ריק.";
        }
        private void ValidatePassword()
        {
            this.ShowPasswordError = string.IsNullOrEmpty(Password);
            if (this.ShowPasswordError)
                this.PasswordError = "סיסמה לא יכולה להיות ריקה.";
   
        }
        private bool ValidateLogin()
        {
            ValidateName();
            ValidatePassword();

            if (!ShowNameError || !showPasswordError)
                return false;
            return true;
        }
        public ICommand LoginCommand => new Command(LoginFunctionAsync);
        private async void LoginFunctionAsync()
        {
            try
            {
                if (ValidateLogin())
                    return;

                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();

                User loggingUser = new User();

                loggingUser = await API.LoginAsync(this.Username, this.Password);

                if (loggingUser != null) //User logged in.
                {
                    UserPage up = new UserPage();
                    up.BindingContext = loggingUser;  //Temporary to test if it works.
                    App.Current.MainPage.Navigation.PushAsync(up);

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
            App.Current.MainPage.Navigation.PushAsync(su);
        }

        #endregion
    }
}
