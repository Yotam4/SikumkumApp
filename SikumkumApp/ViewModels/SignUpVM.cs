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
using System.Text.RegularExpressions;
using System.Linq;

namespace SikumkumApp.ViewModels
{
    class SignUpVM: INotifyPropertyChanged
    {
        public SignUpVM()
        {
            this.ShowNameError = false;
            this.ShowEmailError = false;
            this.ShowPasswordError = false;

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


        private string email { get; set; }
        public string Email
        {
            get { return this.email; }
            set
            {
                this.email = value;
                OnPropertyChanged("Email");
            }
        }

        private bool showEmailError { get; set; }
        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string emailError { get; set; }
        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        #endregion

        #region Commands
        private void ValidateEmail()
        {
            this.ShowEmailError = string.IsNullOrEmpty(Email);
            if (this.ShowEmailError)            
                this.EmailError = "זהו שדה חובה.";
            if (!Regex.IsMatch(this.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                this.ShowEmailError = true;
                this.EmailError = ERROR_MESSAGES.BAD_EMAIL;
            }

        }
        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(this.Username); //Checks that password is not null.
            if (this.ShowNameError)
                this.NameError = "שם משתמש לא יכול להיות ריק.";
            if(this.Username.Length > 16)
            {
                this.ShowNameError = true;
                this.NameError = "שם משתמש יכול להיות עד 16 תווים";
            }
        }
        public void ValidatePassword() //Also Exists in change password. DO NOT FORGOT, IF CHANGED HERE.
        {
            this.ShowPasswordError = string.IsNullOrEmpty(Password);
            if (this.ShowPasswordError)
                this.PasswordError = "סיסמה לא יכולה להיות ריקה. נא הכנס סיסמה בת 8-16 תווים.";
            if(this.Password.Length > 0 && (this.Password.Length < 8 || this.Password.Length > 16)) //If user's password is not 8-16 chars or empty.
            {
                this.ShowPasswordError = true;
                this.PasswordError = "נא הכנס סיסמה בת 8-16 תווים.";
                return;
            }

            Regex rgx = new Regex("[^A-Za-z0-9]"); //Checks that it has special chars. If i ever make this App public, remove this part. it's annoying for the user. Although its cool technical wise, just wanted to see how regex works.
            bool hasSpecialChar = rgx.IsMatch(this.Password);
            if (!hasSpecialChar)
            {
                this.ShowPasswordError = true;
                this.PasswordError = "הסיסמה צריכה לכלול אותיות מיוחדות .";
            }
            bool hasNumbers = this.Password.Any(char.IsDigit); //Check that passwords has numbers. Also might be a little annoying for user, but not as special chars.
            if (!hasNumbers)
            {
                this.ShowPasswordError = true;
                this.PasswordError = "הסיסמה צריכה לכלול מספרים .";
            }
        }

        private bool ValidateSignUp()
        {
            ValidateEmail();
            ValidateName();
            ValidatePassword();

            if (!ShowNameError && !ShowEmailError && !showPasswordError) //If none of the warnings are displayed, return true, validation complete.
                return true;
            return false;
        }
        public Command SignUpCommand => new Command(SignUpAsync);
        private async void SignUpAsync()
        {
            try
            {
                if (!ValidateSignUp()) //If validation was not successful.
                    return;

                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
                User signingUp = new User(this.Username, this.Email, this.Password);

                bool isSigned = await API.SignUpAsync(signingUp);

                if (isSigned) 
                {
                    await App.Current.MainPage.DisplayAlert("ההרשמה עברה בהצלחה", "חזרה למסך הבית", "סגור" );
                    await App.Current.MainPage.Navigation.PopAsync(); //Maybe add screen that says successfully signed up. Work in progress.

                }
                else
                {
                    throw new Exception("Could not sign up. Please try again");
                }
            }

            catch (Exception e)
            {
                return;
            }
        }

        #endregion
    }
}
