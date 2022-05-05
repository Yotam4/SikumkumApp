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
    class UserFilesVM : BaseVM
    {
        #region Variables
        private ObservableCollection<SikumFile> userFiles { get; set; }
        public ObservableCollection<SikumFile> UserFiles
        {
            get => userFiles;
            set
            {
                userFiles = value;
                OnPropertyChanged("UserFiles");
            }
        }
        private int numApproved { get; set; }
        public int NumApproved
        {
            get => numApproved;
            set
            {
                numApproved = value;
                OnPropertyChanged("NumApproved");
            }
        }
        private string errorEmpty { get; set; }
        public string ErrorEmpty
        {
            get => errorEmpty;
            set
            {
                errorEmpty = value;
                OnPropertyChanged("ErrorEmpty");
            }
        }

        private bool showErrorEmpty { get; set; }
        public bool ShowErrorEmpty
        {
            get => showErrorEmpty;
            set
            {
                showErrorEmpty = value;
                OnPropertyChanged("ShowErrorEmpty");
            }
        }
        #endregion

        #region Constructor
        public UserFilesVM()
        {
            this.ShowErrorEmpty = false;

            this.UserFiles = new ObservableCollection<SikumFile>();
            this.NumApproved = 1; //Sets the opening's num to retrieve files that were approved.

            SetUserFiles();
        }
        #endregion

        #region Commands
        private async void SetUserFiles()
        {
            try 
            {
                List<SikumFile> sikumList = await BaseVM.API.GetUserSikumFiles(this.currentApp.CurrentUser, this.NumApproved);
                if (sikumList == null || sikumList.Count <= 0)
                {
                    this.ShowErrorEmpty = true;
                    this.ErrorEmpty = "עוד לא העלת פריטים לסיקומקום.";
                    return;
                }

                this.UserFiles = new ObservableCollection<SikumFile>(sikumList);
            }

            catch
            {

            }
        }

        public Command OpenSikumFilesCommand => new Command<SikumFile>(OpenSikumFile);
        private void OpenSikumFile(SikumFile sikum)
        {
            FilePage fp = new FilePage(sikum);
            App.Current.MainPage.Navigation.PushAsync(fp);
        }

        public Command DeleteSikumFilesCommand => new Command(DeleteSikumFile);
        private void DeleteSikumFile()
        {
            //Work in progress.
        }
        #endregion
    }
}
