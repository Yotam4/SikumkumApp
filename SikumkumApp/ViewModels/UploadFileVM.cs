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

    public static class ERROR_MESSAGES
    {
        public const string REQUIRED_FIELD = "זהו שדה חובה";
        public const string BAD_EMAIL = "מייל לא תקין";
    }

    class UploadFileVM
    {
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region Variables
        private SikumFile uploadSikumFile;

        public List<string> yearNamesList; //The given year and types the user can choose from in the picker.
        public List<string> typeNamesList;

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

        private string typeName { get; set; }
        public string TypeName
        {
            get { return this.typeName; }
            set
            {
                this.typeName = value;
                this.OnPropertyChanged("TypeName");
            }
        }

        private string yearName { get; set; }
        public string YearName
        {
            get { return this.yearName; }
            set
            {
                this.yearName = value;
                this.OnPropertyChanged("YearName");
            }
        }

        private string headline { get; set; }
        public string Headline
        {
            get { return this.headline; }
            set
            {
                this.headline = value;
                this.OnPropertyChanged("Headline");
            }
        }

        private string textDesc { get; set; }
        public string TextDesc
        {
            get { return this.textDesc; }
            set
            {
                this.textDesc = value;
                this.OnPropertyChanged("TextDesc");
            }
        }
        #region מקור התמונה
        private string sikumFileSrc;

        public string SikumFileSrc
        {
            get => sikumFileSrc;
            set
            {
                sikumFileSrc = value;
                OnPropertyChanged("SikumFileSrc" +
                    "");
            }
        }
        private const string DEFAULT_PHOTO_SRC = "defaultphoto.jpg";
        #endregion
        #endregion

        #region Constructor
        public UploadFileVM()
        {
            this.typeNamesList = new List<string>();
            this.typeNamesList.Add("סיכום"); this.typeNamesList.Add("מטלה"); this.typeNamesList.Add("תרגול"); //Adds the 3 type values in DB. If changed DB, must change it here!

            this.yearNamesList = new List<string>();
            this.yearNamesList.Add("יסודי"); this.typeNamesList.Add("חטיבה"); this.typeNamesList.Add("תיכון"); this.typeNamesList.Add("אוניברסיטה"); //Adds the 4 year values in DB. If changed DB, must change it here!


        }

        #endregion


        #region Commands
        public Command UploadSikumFileCommand => new Command(UploadSikumFile);
        private void UploadSikumFile()
        {
            App currentApp = (App)App.Current;
            this.uploadSikumFile = new SikumFile(currentApp.CurrentUser.Username, this.Headline, this.SikumFileSrc, this.YearName, this.TypeName, this.TextDesc); //Create new Sikum File to send to server.

        }
        #endregion
    }
}
