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
    class SignUpVM: INotifyPropertyChanged
    {
        public SignUpVM()
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

        #endregion

        #region Commands

        public ICommand SignUpCommand => new Command(SignUpAsync);
        private async void SignUpAsync()
        {
            try
            {
                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
                User signedUp = new User();
                signedUp = await API.SignUpAsync(this.Username, this.Email, this.Password);

                if (signedUp != null) 
                {
                    UserPage up = new UserPage();
                    up.BindingContext = signedUp; //Temporary to test if it works.
                    App.Current.MainPage = up;
                }
                else
                {
                    throw new Exception("Could not sign up. Please try again");
                }
            }

            catch (Exception e)
            {
                return null;
            }
        }

        #endregion
    }
}
