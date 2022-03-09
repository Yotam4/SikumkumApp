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
        private string newPassword { get; set; }
        public string NewPassword
        {
            get => newPassword;
            set
            {
                newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        #endregion
        #region Constructor
        public UserPageVM()
        {

        }
        #endregion
        #region Commands
        public Command GoToChangePassCommand => new Command(ChangePassword);
        private async void ChangePassword() //Should be Async??
        {
            try
            {
                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
                App currentApp = (App)App.Current;
                if (await API.TryChangePassword(currentApp.CurrentUser, this.NewPassword))
                {
                    currentApp.CurrentUser.Password = this.NewPassword;
                }
                else
                    throw new Exception("Could not change password.");
            }
            catch(Exception e)
            {

            }
        }
        #endregion
    }



}
