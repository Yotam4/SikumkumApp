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

        public List<string> yearNamesList { get; set; } //The given year and types the user can choose from in the picker.
        public List<string> typeNamesList { get; set; }
        private bool clickedOnPDF;
        private bool clickdOnImages;

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
            this.yearNamesList.Add("יסודי"); this.yearNamesList.Add("חטיבה"); this.yearNamesList.Add("תיכון"); this.yearNamesList.Add("אוניברסיטה"); //Adds the 4 year values in DB. If changed DB, must change it here!

            this.clickdOnImages = false;
            this.clickedOnPDF = false;
        }

        #endregion


        #region Commands
        public Command UploadSikumFileCommandPDF => new Command(UploadSikumFile);
        private async void ClickedOnPDF() //Work in progress
        {
            var pickResult = await FilePicker.PickMultipleAsync(new PickOptions()
            {
                FileTypes = FilePickerFileType.Pdf,
                PickerTitle = "Pick PDF"
            }); ;

        }
        public Command UploadSikumFileCommandImages => new Command(UploadSikumFile);
        private async void ClickedOnImages()//Work in progress.
        {
            var pickResult = await FilePicker.PickMultipleAsync(new PickOptions()
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick Images" 
            }); ;

            if(pickResult != null)
            {
                var imageList = new List<ImageSource>();
                foreach(var image in pickResult)
                {
                    var stream =  await image.OpenReadAsync();
                    imageList.Add(ImageSource.FromStream(() => stream));
                }

                
            }
        }
        private void UploadSikumFile()
        {
            App currentApp = (App)App.Current;
            this.uploadSikumFile = new SikumFile(currentApp.CurrentUser.Username, this.Headline, this.SikumFileSrc, this.YearName, this.TypeName, this.TextDesc); //Create new Sikum File to send to server.

        }

        ///The following command handle the pick photo button
        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            if (this.clickdOnImages) //User chose to upload photos.
            {
                var pickResults = await FilePicker.PickMultipleAsync(new PickOptions()
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Pick Image"
                }); ;
            }

            if (this.clickedOnPDF) //User chose to upload PDFs
            {
                var pickResults = await FilePicker.PickMultipleAsync(new PickOptions()
                {
                    FileTypes = FilePickerFileType.Pdf,
                    PickerTitle = "Pick PDF"
                }); ;
            }
            if (result != null)
            {
                this.imageFileResult = result;
                
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }

/*        ///The following command handle the take photo button
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "צלם תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }*/
        #endregion
    }
}
