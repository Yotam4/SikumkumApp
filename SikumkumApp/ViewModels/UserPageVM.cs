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
using SikumkumApp.ViewModels;

namespace SikumkumApp.ViewModels
{
    class UserPageVM : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Variables
        public string Username { get; set; }
        private const string STAR_EMPTY  = "StarTransp.png";
        private const string STAR_COLOR = "starTranspColor.png";

        private App currentApp { get; set; }


        private string newPassword { get; set; } //User's new password to set.
        public string NewPassword
        {
            get => newPassword;
            set
            {
                newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }

        private string oldPassword { get; set; } //User's old password to validate
        public string OldPassword
        {
            get => oldPassword;
            set
            {
                oldPassword = value;
                OnPropertyChanged("OldPassword");
            }
        }

        private string newConfirm { get; set; } //The confirm to the new password.
        public string NewConfirm
        {
            get => newConfirm;
            set
            {
                newConfirm = value;
                OnPropertyChanged("NewConfirm");
            }
        }
        #region StarImages
        //Sets all star colors recursiouvly (sorta), cool beans.
        private string starImageOne { get; set; } //User's new password to set.
        public string StarImageOne
        {
            get => starImageOne;
            set
            {
                starImageOne = value;
                OnPropertyChanged("StarImageOne");
            }
        }
        private string starImageTwo { get; set; } //User's new password to set.
        public string StarImageTwo
        {
            get => starImageTwo;
            set
            {
                StarImageOne = value;
                starImageTwo = value;
                OnPropertyChanged("StarImageTwo");
            }
        }
        private string starImageThree { get; set; } //User's new password to set.
        public string StarImageThree
        {
            get => starImageThree;
            set
            {
                StarImageTwo = value;
                starImageThree = value;
                OnPropertyChanged("StarImageThree");
            }
        }
        private string starImageFour { get; set; } //User's new password to set.
        public string StarImageFour
        {
            get => starImageFour;
            set
            {
                StarImageThree = value;
                starImageFour = value;
                OnPropertyChanged("StarImageFour");
            }
        }
        private string starImageFive { get; set; } //User's new password to set.
        public string StarImageFive
        {
            get => starImageFour;
            set
            {
                StarImageFour = value;
                starImageFive = value;
                OnPropertyChanged("StarImageFive");
            }
        }

        #endregion
        private string passwordError { get; set; } //User's new password to set.
        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }

        private bool showPasswordError { get; set; } //User's new password to set.
        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string passwordChanged { get; set; }
        public string PasswordChanged
        {
            get => passwordChanged;
            set
            {
                passwordChanged = value;
                OnPropertyChanged("PasswordChanged");
            }
        }

        private bool showPasswordChanged { get; set; }
        public bool ShowPasswordChanged
        {
            get => showPasswordChanged;
            set
            {
                showPasswordChanged = value;
                OnPropertyChanged("ShowPasswordChanged");
            }
        }

        private bool isAdmin{ get; set; } //User's new password to set.
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }

        #endregion
        #region Constructor
        public UserPageVM()
        {
            this.currentApp = (App)App.Current;

            this.Username = this.currentApp.CurrentUser.Username;

            this.ShowPasswordChanged = false;
            this.ShowPasswordError = false;
            this.IsAdmin = false;


        }
        #endregion
        #region Commands
        public Command ChangePassCommand => new Command(ChangePassword);
        private async void ChangePassword() //Should be Async??
        {
            try
            {

                if (!ValidateInput() ) //Stops function if input was incorrect.           
                    return;


                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();                
                if (await API.TryChangePassword(this.currentApp.CurrentUser, this.NewPassword))
                {
                    this.currentApp.CurrentUser.Password = this.NewPassword;

                    //Reset Values so they won't appear on screen anymore.
                    this.OldPassword = ""; 
                    this.NewPassword = "";
                    this.NewConfirm = "";

                    this.ShowPasswordChanged = true; //Sets assurance values.
                    this.PasswordChanged = "הסיסמה שונתה בהצלחה.";
                }
                else
                    throw new Exception("שינוי סיסמה לא הצליח");
            }
            catch(Exception e)
            {
                this.ShowPasswordError = true;
                this.PasswordError = e.Message;
            }
        }

        public Command GoToUploadFileCommand => new Command(UploadFileFunc);
        private void UploadFileFunc()
        {
            UploadFile uf = new UploadFile();
            App.Current.MainPage.Navigation.PushAsync(uf);
        }

        public Command GoToConfirmUploadsCommand => new Command(GoToConfirmUploads);
        private void GoToConfirmUploads()
        {
            ConfirmUploads cu = new ConfirmUploads();
            App.Current.MainPage.Navigation.PushAsync(cu);
        }

        public Command RateCommand => new Command<string>(RateSikum); //Work in progress.
        private void RateSikum(string wgat)
        {

        }
        #endregion

        #region Validations
        private bool ValidateInput() //Returns false if input was incorrect, and displays errors accordingly.
        {
            this.ShowPasswordError = false;
            this.ShowPasswordChanged = false;

            if (this.NewPassword == this.currentApp.CurrentUser.Password) //User's new password matches his current one.
            {
                this.ShowPasswordError = true;
                this.PasswordError = "אנא הכנס סיסמה חדשה שונה.";
                return false;
            }

            if (this.OldPassword != this.currentApp.CurrentUser.Password) //Check that user inputted his real old password.
            {
                this.ShowPasswordError = true;
                this.PasswordError = "הסיסמה הישנה לא נכונה.";
                return false;
            }

            if (this.NewPassword != this.NewConfirm) //Check that the new passwords match.
            {
                this.ShowPasswordError = true;
                this.PasswordError = "הסיסמאות לא תואמות.";
                return false;
            }
            if (!ValidatePassword()) //Validates the characters in the password.
                return false;

            return true; //Input is correct.
        }

        private bool ValidatePassword() //Validates that the new password has correct values.
        {
            this.ShowPasswordError = string.IsNullOrEmpty(this.NewPassword); //Checks that password is not null.
            if (this.ShowPasswordError)
                this.PasswordError = "סיסמה לא יכולה להיות ריקה. נא הכנס סיסמה בת 8-16 תווים.";

            if (this.NewPassword.Length > 0 && (this.NewPassword.Length < 8 || this.NewPassword.Length > 16)) //If user's password is not 8-16 chars or empty.
            {
                this.ShowPasswordError = true;
                this.PasswordError = "נא הכנס סיסמה בת 8-16 תווים.";
                return false;
            }

            Regex rgx = new Regex("[^A-Za-z0-9]"); //Checks that it has special chars. If i ever make this App public, remove this part. it's annoying for the user. Although its cool technical wise, just wanted to see how regex works.
            bool hasSpecialChar = rgx.IsMatch(this.NewPassword);
            if (!hasSpecialChar)
            {
                this.ShowPasswordError = true;
                this.PasswordError = "הסיסמה צריכה לכלול אותיות מיוחדות .";
                return false;
            }
            bool hasNumbers = this.NewPassword.Any(char.IsDigit); //Check that passwords has numbers. Also might be a little annoying for user, but not as special chars.
            if (!hasNumbers)
            {
                this.ShowPasswordError = true;
                this.PasswordError = "הסיסמה צריכה לכלול מספרים .";
                return false;
            }

            return true;
        }

        #endregion
    }



}
