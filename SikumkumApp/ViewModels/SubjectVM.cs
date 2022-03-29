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
        public List<SikumFile> listOfFiles { get; set; }
        public List<string> StudyYearList {get; set; }

        private ObservableCollection<SikumFile> files { get; set; }
        public ObservableCollection<SikumFile> Files
        {
            get { return this.files; }
            set
            {
                this.files = value;
                this.OnPropertyChanged("Files");
            }
        }

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

        private int studyYear { get; set; }
        public int StudyYear
        {
            get { return this.studyYear; }
            set
            {
                this.studyYear = value;
                this.OnPropertyChanged("StudyYear");
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
            App currentApp = (App)App.Current;
            this.IsEmpty = false;

            this.currentSubject = chosen;
            this.GetSummary = true; //Only lookup summaries when page gets opened.
            this.GetEssay = false;
            this.GetPractice = false;

            this.StudyYearList = new List<string>();
            foreach(StudyYear year in currentApp.OpeningObj.StudyYearList)
            {
                this.StudyYearList.Add(year.YearName);
            }
            this.StudyYear = 2;

            GetSikumFiles();
        }

        #endregion

        #region Commands
        public Command ClickedOnFile => new Command<SikumFile>(FileClicked);
        private async void FileClicked(SikumFile sf)
        {
            FilePage fp = new FilePage(sf);
            await App.Current.MainPage.Navigation.PushAsync(fp); //SHOULD THAT BE AWAITABLE?
        }

        public Command SearchCommand => new Command(GetSikumFiles);
        private async void GetSikumFiles() 
        {
            try
            {
                if (!this.GetSummary && !this.GetPractice && !this.GetEssay)
                { //If user checked no boxes, lookup nothing.
                    return;
                }

                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
                this.listOfFiles = await API.GetSikumFiles(this.GetSummary, this.GetEssay, this.GetPractice, this.currentSubject.SubjectName, (this.StudyYear + 1));
                if (listOfFiles != null)
                {
                    this.Files = new ObservableCollection<SikumFile>(listOfFiles); //Creates new list.
                    this.IsEmpty = false;
                }

                if(listOfFiles == null) //If search found nothing.
                {
                    this.Files = new ObservableCollection<SikumFile>();
                    this.IsEmpty = true;
                    this.ErrorEmpty = "אין קבצים בקומקום מהסוג הזה כרגע..";
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
