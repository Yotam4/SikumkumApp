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
    class ConfirmUploadsVM : BaseVM
    {
        #region Variables 
        private ObservableCollection<SikumFile> pendingFiles { get; set; }
        public ObservableCollection<SikumFile> PendingFiles
        {
            get { return this.pendingFiles; }
            set
            {
                this.pendingFiles = value;
                this.OnPropertyChanged("PendingFiles");
            }
        }

        private bool isEmpty { get; set; }
        public bool IsEmpty
        {
            get { return this.isEmpty; }
            set
            {
                this.isEmpty = value;
                this.OnPropertyChanged("IsEmpty");
            }
        }
        private string errorEmpty { get; set; }
        public string ErrorEmpty
        {
            get { return this.errorEmpty; }
            set
            {
                this.errorEmpty = value;
                this.OnPropertyChanged("ErrorEmpty");
            }
        }

        #endregion

        #region Constructor
        public ConfirmUploadsVM()
        {
            GetPendingFiles();
        }
        #endregion

        #region Commands
        public async void GetPendingFiles()
        {
            try
            {

                List<SikumFile> files = await API.GetPendingFiles();
                this.PendingFiles = new ObservableCollection<SikumFile>(files);

                if (PendingFiles == null) //If empty.
                {
                    this.PendingFiles = new ObservableCollection<SikumFile>(); //Empty collection to prevent error.
                    this.IsEmpty = true;
                    this.ErrorEmpty = "אין קבצים שמחכים לאישור";
                }

            }

            catch
            {

            }
        }

        public Command ClickedOnFile => new Command<SikumFile>(FileClicked);
        private async void FileClicked(SikumFile sf)
        {
            FilePage fp = new FilePage(sf);
            await App.Current.MainPage.Navigation.PushAsync(fp); //SHOULD THAT BE AWAITABLE?
        }
        #endregion
    }
}
