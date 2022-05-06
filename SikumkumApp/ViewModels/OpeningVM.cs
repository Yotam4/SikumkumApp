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
    class OpeningVM : BaseVM 
    {

#region Variables
        public ObservableCollection<Subject> subjectsCollec { get; set; }

        private bool canLogIn { get; set; }
        public bool CanLogIn
        {
            get => canLogIn;
            set
            {
                canLogIn = value;
                OnPropertyChanged("CanLogIn");
            }
        }

        private bool isLoggedIn { get; set; }
        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set
            {
                isLoggedIn = value;
                OnPropertyChanged("IsLoggedIn");
            }
        }
        #endregion
        public OpeningVM()
        {
            this.subjectsCollec = new ObservableCollection<Subject>(this.currentApp.OpeningObj.SubjectsList);
            this.CanLogIn = true;
            this.IsLoggedIn = false;
        }





        #region Commands
        

        public Command ClickedOnLogin => new Command(OpenLoginPage);
        private void OpenLoginPage()
        {
            Login loginPage = new Login();
            App.Current.MainPage.Navigation.PushAsync(loginPage);
        }

        public Command ClickedOnSignUp => new Command(OpenSignUpPage);
        private void OpenSignUpPage()
        {
            SignUp SignupPage = new SignUp();
            App.Current.MainPage.Navigation.PushAsync(SignupPage);
        }

        public Command ToUserPageCommand => new Command(ToUserPage);
        private void ToUserPage()
        {
            UserPage userPage = new UserPage();
            App.Current.MainPage.Navigation.PushAsync(userPage);
        }

        public Command ClickedOnSubject => new Command<Subject>(OpenSubjectPage);
        private void OpenSubjectPage(Subject chosen)
        {
            SubjectPage subjectPage = new SubjectPage(chosen);
            App.Current.MainPage.Navigation.PushAsync(subjectPage); 
        }

        public Command ClickedOnLogout => new Command(Logout);
        private async void Logout()
        {
            try
            {
                bool loggedOut = await API.LogoutAsync(this.currentApp.CurrentUser);
                if (!loggedOut) //User didn't log out.
                    return;

                this.currentApp.CurrentUser = null;
                this.CanLogIn = true;
                this.IsLoggedIn = false;
            }

            catch
            {

            }
        }

        #endregion



    }
}
