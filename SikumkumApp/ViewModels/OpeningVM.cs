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
    class OpeningVM : INotifyPropertyChanged
    {
        public ObservableCollection<Subject> subjectsCollec { get;}
        public OpeningVM(List<Subject> subjectsL)
        {
            this.subjectsCollec = new ObservableCollection<Subject>(subjectsL); 
        }
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


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
            UploadFile SignupPage = new UploadFile();
            App.Current.MainPage.Navigation.PushAsync(SignupPage);
        }

        public Command ClickedOnSubject => new Command<Subject>(OpenSubjectPage);
        private void OpenSubjectPage(Subject chosen)
        {
            SubjectPage subjectPage = new SubjectPage(chosen);
            App.Current.MainPage.Navigation.PushAsync(subjectPage); 
        }

        #endregion



    }
}
