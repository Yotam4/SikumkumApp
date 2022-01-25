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

namespace SikumkumApp.ViewModels
{
    class SubjectVM : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Variables
        private Subject currentSubject;
        public ObservableCollection<SikumFile> files { get; set; }

        private bool getSummary { get; set; }
        public bool GetSummary
        {
            get { return this.getSummary; }
            set
            {
                this.getSummary = value;
                this.OnPropertyChanged("GetSummary");
            }
        }

        private bool getPractice { get; set; }
        public bool GetPractice
        {
            get { return this.getPractice; }
            set
            {
                this.getPractice = value;
                this.OnPropertyChanged("GetPractice");
            }
        }

        private bool getEssay { get; set; }
        public bool GetEssay
        {
            get { return this.getEssay; }
            set
            {
                this.getEssay = value;
                this.OnPropertyChanged("GetEssay");
            }
        }

        //Errors
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

        public SubjectVM(Subject chosen)
        {
            this.IsEmpty = false;
            GetSikumFiles();

            this.currentSubject = chosen;
            this.GetSummary = true; //Only lookup summaries when page gets opened.
            this.GetEssay = false;
            this.GetPractice = false;
        }

        #endregion

        #region Commands
        public Command ClickedOnFile => new Command<SikumFile>(FileClicked);
        private async void FileClicked(SikumFile sf)
        {
            FilePage fp = new FilePage(sf);
            await App.Current.MainPage.Navigation.PushAsync(fp); //SHOULD THgetBE AWAITABLE?
        }

        private async void GetSikumFiles()
        {
            try
            {
                if (!this.getSummary && !this.getPractice && !this.getEssay) //If user checked no boxes.
                    return;

                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
                List<SikumFile> listFiles = await API.GetSikumFiles(this.getSummary, this.getPractice, this.getEssay);
                this.files = new ObservableCollection<SikumFile>(listFiles); //Creates new list, if listFiles is null, it will create an empty one.

                if(files.Count == 0) //If search found nothing.
                {
                    this.IsEmpty = false;
                    this.ErrorEmpty = "There are no matchs currently";
                    return;
                }                    
            }

            catch
            {

            }
        }
        #endregion
    }

}
