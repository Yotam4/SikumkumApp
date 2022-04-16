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
        private List<SikumFile> userFiles { get; set; }
        public List<SikumFile> UserFiles
        {
            get => userFiles;
            set
            {
                userFiles = value;
                OnPropertyChanged("UserFiles");
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

            this.UserFiles = new List<SikumFile>();
        }
        #endregion

        #region Commands
        private async void SetUserFiles()
        {
            try 
            {
                this.UserFiles = await BaseVM.API.GetUserSikumFiles(this.currentApp.CurrentUser);

                if(this.UserFiles == null)
                {
                    this.ShowErrorEmpty = true;
                    this.ErrorEmpty = "עוד לא העלת פריטים לסיקומקום.";
                    return;
                }
            }

            catch
            {

            }
        }

        public Command OpenSikumFilesCommand => new Command(OpenSikumFile);
        private void OpenSikumFile()
        {
            //Work in progress.
        }

        public Command DeleteSikumFilesCommand => new Command(DeleteSikumFile);
        private void DeleteSikumFile()
        {
            //Work in progress.
        }
        #endregion
    }
}
