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
        const string APPROVED_NAME = "הצג סיכומים מאושרים";
        const string DISAPPROVED_NAME = "הצג סיכומים לא מאושרים";
        const string APPROVED_DISPLAY = "סיכומים שאושרו";
        const string DISAPPROVED_DISPLAY = "סיכומים שטרם אושרו";
        const int NUM_APPROVED = 1;
        const int NUM_DISAPPROVED = 0;


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

        private ObservableCollection<SikumFile> rejectedFiles { get; set; }
        public ObservableCollection<SikumFile> RejectedFiles
        {
            get => rejectedFiles;
            set
            {
                rejectedFiles = value;
                OnPropertyChanged("RejectedFiles");
            }
        }

        private bool displayRejected { get; set; }
        public bool DisplayRejected
        {
            get => displayRejected;
            set
            {
                displayRejected = value;
                OnPropertyChanged("DisplayRejected");
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

        private string sikumGetName { get; set; }
        public string SikumGetName
        {
            get => sikumGetName;
            set
            {
                sikumGetName = value;
                OnPropertyChanged("SikumGetName");
            }
        }

        private string currentDisplayText { get; set; }
        public string CurrentDisplayText
        {
            get => currentDisplayText;
            set
            {
                currentDisplayText = value;
                OnPropertyChanged("CurrentDisplayText");
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
            //Setting booleans
            this.ShowErrorEmpty = false;
            this.DisplayRejected = false;

            //Setting strings
            this.CurrentDisplayText = APPROVED_DISPLAY;
            this.SikumGetName = DISAPPROVED_NAME;

            //Creating collections
            this.UserFiles = new ObservableCollection<SikumFile>();
            this.RejectedFiles = new ObservableCollection<SikumFile>();

            this.NumApproved = NUM_APPROVED; //Sets the opening's num to retrieve files that were approved.

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
                    this.ErrorEmpty = "אין לך פריטים מסוג זה.";
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

        public Command ChangeApprovedCommand => new Command<SikumFile>(ChangeApproved);
        private async void ChangeApproved(SikumFile sikum) //Changes type to be approved or non approved.
        {
            try
            {
                this.DisplayRejected = false; //Sets it to false until proven otherwise in function. 

                if (this.NumApproved == 0) //Changes to approved.
                {
                    this.NumApproved = NUM_APPROVED;
                    this.SikumGetName = DISAPPROVED_NAME; //Shows goto disaaproved items
                    this.CurrentDisplayText = APPROVED_DISPLAY;
                }
                else if (this.NumApproved == 1) //Changes to non-Approved
                {
                    this.NumApproved = NUM_DISAPPROVED;
                    this.SikumGetName = APPROVED_NAME;
                    this.CurrentDisplayText = DISAPPROVED_DISPLAY;

                }

                List<SikumFile> sikumList = await BaseVM.API.GetUserSikumFiles(this.currentApp.CurrentUser, this.NumApproved);

                if (sikumList == null || sikumList.Count <= 0)
                {
                    this.ShowErrorEmpty = true;
                    this.ErrorEmpty = "אין לך פריטים מסוג זה.";
                    this.UserFiles.Clear(); 
                    return;
                }
                else
                {
                    this.ShowErrorEmpty = false;
                }


                this.UserFiles = new ObservableCollection<SikumFile>(sikumList);

                if (this.NumApproved == 0) //Sets Rejected items in list to display.
                {
                    foreach (SikumFile sikumFile in sikumList)
                    {
                        if (sikumFile.Disapproved)
                        {
                            this.DisplayRejected = true;
                            this.RejectedFiles.Add(sikumFile);
                        }
                    }
                }
            }

            catch
            {

            }
        }

        public Command DeleteSikumFilesCommand => new Command(DeleteSikumFile);
        private void DeleteSikumFile()
        {
            //Work in progress.
        }
        #endregion

        #region Validations
        #endregion
    }
}
