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
    class UserPageVM
    {
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Variables
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
                NewConfirm = value;
                OnPropertyChanged("OldPassword");
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

        private string passwordChanged { get; set; }
        public string PasswordChanged
        {
            get => passwordChanged;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordChanged");
            }
        }

        #endregion
        #region Constructor
        public UserPageVM()
        {
            this.ShowPasswordChanged = false;
            this.ShowPasswordError = false;
        }
        #endregion
        #region Commands
        public Command GoToChangePassCommand => new Command(ChangePassword);
        private async void ChangePassword() //Should be Async??
        {
            try
            {
                App currentApp = (App)App.Current;

                if (!ValidatePassword(currentApp)) //Stops function if input was incorrect.           
                    return;                

                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();                
                if (await API.TryChangePassword(currentApp.CurrentUser, this.NewPassword))
                {
                    currentApp.CurrentUser.Password = this.NewPassword;

                    //Reset Values so they won't appear on screen anymore.
                    this.OldPassword = ""; 
                    this.NewPassword = "";
                    this.NewConfirm = "";

                    this.ShowPasswordChanged = true;
                    this.PasswordChanged = "הסיסמה שונתה בהצלחה.";
                }
                else
                    throw new Exception("שינוי סיסמה לא הצליח");
            }
            catch(Exception e)
            {

            }
        }

        private bool ValidatePassword(App currentApp) //Returns false if input was incorrect, and displays errors accordingly.
        {
            this.ShowPasswordError = false;
            this.ShowPasswordChanged = false;
            if (this.OldPassword != currentApp.CurrentUser.Password) //Check that user inputted his real old password.
            {
                this.PasswordError = "הסיסמה הישנה לא נכונה.";
                this.ShowPasswordError = true;
                return false;
            }

            if (this.NewPassword != this.NewConfirm) //Check that the new passwords match.
            {
                this.PasswordError = "הסיסמאות לא מתאימות.";
                this.ShowPasswordError = true;
                return false;
            }

            return true; //Input is correct.
        }
        #endregion
    }



}
