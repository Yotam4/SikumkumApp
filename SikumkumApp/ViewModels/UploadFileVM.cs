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
using System.Linq;

namespace SikumkumApp.ViewModels
{

    public static class ERROR_MESSAGES
    {
        public const string REQUIRED_FIELD = "זהו שדה חובה";
        public const string BAD_EMAIL = "מייל לא תקין";
    }

    public static class FileTypeNames //File types user chose, or null.
    {
        public const string NULL_TYPE = "null";
        public const string PDF_TYPE = "pdf";
        public const string IMAGE_TYPE = "image";
    }
    class UploadFileVM : INotifyPropertyChanged
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
        public List<string> subjectNamesList { get; set; }

        private App currentApp;

        private string contentType; //To allow the server to know which file is being uploaded.

        private bool clickedOnPDF; //No use of it so far. might be needed for UI purposes.
        private bool clickdOnImages;

        private bool hasImage { get; set; }
        private bool hasPdf { get; set; }


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

        private int subjectChosen { get; set; }
        public int SubjectChosen
        {
            get { return this.subjectChosen; }
            set
            {
                this.subjectChosen = value;
                this.OnPropertyChanged("SubjectChosen");
            }
        }

        private int typeChosen { get; set; }
        public int TypeChosen
        {
            get { return this.typeChosen; }
            set
            {
                this.typeChosen = value;
                this.OnPropertyChanged("TypeChosen");
            }
        }

        private int yearChosen { get; set; }
        public int YearChosen
        {
            get { return this.yearChosen; }
            set
            {
                this.yearChosen = value;
                this.OnPropertyChanged("YearChosen");
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
        private string pdfFileName { get; set; }
        public string PdfFileName
        {
            get { return this.pdfFileName; }
            set
            {
                this.pdfFileName = value;
                this.OnPropertyChanged("PdfFileName");
            }
        }

        private string headlineError { get; set; }
        public string HeadlineError
        {
            get { return this.headlineError; }
            set
            {
                this.headlineError = value;
                this.OnPropertyChanged("HeadlineError");
            }
        }

        private bool showHeadlineError { get; set; }
        public bool ShowHeadlineError
        {
            get { return this.showHeadlineError; }
            set
            {
                this.showHeadlineError = value;
                this.OnPropertyChanged("ShowHeadlineError");
            }
        }

        private string descError { get; set; }
        public string DescError
        {
            get { return this.descError; }
            set
            {
                this.descError = value;
                this.OnPropertyChanged("DescError");
            }
        }
        private bool showDescError { get; set; }
        public bool ShowDescError
        {
            get { return this.showDescError; }
            set
            {
                this.showDescError = value;
                this.OnPropertyChanged("ShowDescError");
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

        private string uploadError { get; set; }
        public string UploadError
        {
            get { return this.uploadError; }
            set
            {
                this.uploadError = value;
                this.OnPropertyChanged("UploadError");
            }
        }

        private bool showUploadError { get; set; }
        public bool ShowUploadError
        {
            get { return this.showUploadError; }
            set
            {
                this.showUploadError = value;
                this.OnPropertyChanged("ShowUploadError");
            }
        }

        #region מקור התמונה
        private List<ImageSource> sikumListSrc;
        public List<ImageSource> SikumListSrc
        {
            get => sikumListSrc;
            set
            {
                sikumListSrc = value;
                OnPropertyChanged("SikumListSrc");
            }
        }
        private List<FileInfo> filesInfoList;
        private List<FileResult> fileResultsList;
        public List<FileResult> FileResultsList
        {
            get => fileResultsList;
            set
            {
                fileResultsList = value;
                OnPropertyChanged("FileResultsList");
            }
        }

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
            this.currentApp = (App)App.Current;
            this.Username = this.currentApp.CurrentUser.Username;

            this.typeNamesList = new List<string>(); //Adding file types to picker.
            foreach(FileType ft in this.currentApp.OpeningObj.FileTypeList)
            {
                this.typeNamesList.Add(ft.TypeName);
            }

            this.yearNamesList = new List<string>(); //Adding year names to picker.
            foreach(StudyYear studyYear in this.currentApp.OpeningObj.StudyYearList)
            {
                this.yearNamesList.Add(studyYear.YearName);
            }

            this.subjectNamesList = new List<string>(); //Adding subject names to picker.
            foreach(Subject s in this.currentApp.OpeningObj.SubjectsList)
            {
                this.subjectNamesList.Add(s.SubjectName);
            }

            //Booleans to set.
            this.clickdOnImages = false;
            this.clickedOnPDF = false;
            this.ShowUploadError = false;
            this.hasPdf = false;
            this.hasImage = false;
            this.ShowDescError = false;
            this.UploadError = "";
            this.PdfFileName = ""; //Empty until user uploads Pdf, then it recives the pdf file name.
            this.DescError = "";

            //Lists to set,
            this.FileResultsList = new List<FileResult>(); //Creates new lists.
            this.SikumListSrc = new List<ImageSource>();

            this.contentType = FileTypeNames.NULL_TYPE;
        }

        #endregion


        #region Commands
        public Command PickPDFCommand => new Command(ClickedOnPDF); //User chose to upload pdf files.
        private async void ClickedOnPDF() //Work in progress
        {
            try
            {
                var pickResult = await FilePicker.PickAsync(new PickOptions() //Maybe only let user pick one? WORK IN PROGRESS.
                {
                    FileTypes = FilePickerFileType.Pdf,
                    PickerTitle = "Pick PDF"
                }); ;

                if (pickResult != null)
                {
                    this.SikumListSrc.Clear(); //Delete old values.
                    this.FileResultsList.Clear();

                    var pdf = pickResult;
                    this.FileResultsList.Add(pdf);
                    var stream = await pdf.OpenReadAsync();
                    this.SikumListSrc.Add(ImageSource.FromStream(() => stream));

                    this.contentType = FileTypeNames.PDF_TYPE;
                    this.PdfFileName = pickResult.FileName; //Sets file name for later use.
                    this.hasPdf = true; //Sets sikum to have pdfs, and not images.
                    this.hasImage = false;
                }
            }

            catch //User opted out or something went wrong
            {

            }

        }
        public Command PickImageCommand => new Command(ClickedOnImages); //User chose to upload images
        private async void ClickedOnImages()//Work in progress.
        {
            try
            {
                var pickResult = await FilePicker.PickMultipleAsync(new PickOptions()
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Pick Images"
                }); ;

                if (pickResult != null)
                {
                    this.SikumListSrc.Clear(); //Delete old values.
                    this.FileResultsList.Clear();

                    foreach (var image in pickResult)
                    {
                        this.FileResultsList.Add(image);
                        var stream = await image.OpenReadAsync();
                        this.SikumListSrc.Add(ImageSource.FromStream(() => stream));
                    }

                    this.contentType = FileTypeNames.IMAGE_TYPE;

                    this.hasImage = true;
                    this.hasPdf = false;
                }
            }

            catch  
            {
                
            }
        }

        public Command UploadSikumFileCommand => new Command(UploadSikumFile);
        private async void UploadSikumFile()
        {
            try
            {

                if (ValidateForm() == false) //Form was not validated.
                    return;

                if (await TryUploadFiles() == false) //File didn't upload to server.
                    return;

                if (await TryUploadSikumFile() == false) //Sikumfile upload to database didn't work.
                    return;


                await App.Current.MainPage.Navigation.PopAsync();
            }

            catch (Exception e)
            {
                return;
            }
        }

        private async Task<bool> TryUploadSikumFile() //Work in progress.
        {
            try
            {
                int userId = currentApp.CurrentUser.UserID;
                //The Picker returns an integer of the person's choice, starting with 0. to get the correct ID for the chosen thing, I parsed the input and added +1. Neat.
                this.uploadSikumFile = new SikumFile(userId, this.Username, this.Headline, "", "", "", (this.YearChosen + 1), (this.TypeChosen + 1), (this.SubjectChosen + 1), this.TextDesc, this.FileResultsList.Count, this.hasPdf, this.hasImage, this.PdfFileName); //Create new Sikum File to send to server. Change SikumFileSrc WORK IN PROGRESS.

                
                if (this.uploadSikumFile == null) //File creation didn't work.
                    return false;

                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
                bool uploaded = await API.UploadSikumFile(this.uploadSikumFile);
                return uploaded;
            }

            catch (Exception e)
            {
                string message = e.Message;
                return false;
            }
        }

        private async Task<bool> TryUploadFiles() //Uploads files and return true if they were sucessfully uploaded to the server.
        {
            try
            {
                if (this.SikumListSrc.Count <= 0)
                    return false;
                
                SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
                List<FileInfo> filesInfoList = new List<FileInfo>();

                foreach (FileResult result in this.FileResultsList) //creates file info for each file in list.
                {
                    filesInfoList.Add(new FileInfo() { Name = result.FullPath });
                }
                FileInfo[] filesInfoArr = filesInfoList.ToArray(); //Function gets array, so conversion is needed.

                bool uploadedFiles = await API.UploadFiles(filesInfoArr, $"{this.Username}-{this.Headline}-", this.contentType);

                if (!uploadedFiles) //Something didn't work.
                {
                    this.ShowUploadError = true;
                    this.UploadError = "העלאת הקבצים לא עברה בהצלחה";
                    return false;
                    //Add error message
                }

                return true;
            }

            catch 
            {
                return false;
            }
        }
        #region Validations
        private bool ValidateForm() //Validates that all credentials are correct.
        {
            //Work in progress.
            this.ShowUploadError = false; //Resets the upload error.
            if(!this.hasPdf && !this.hasImage)
            {
                this.ShowUploadError = true;
                this.UploadError = "אנא בחר קובץ להעלאה.";
                return false;
            }

            if(this.TextDesc.Length == 0) //If Desc is empty.
            {
                this.ShowDescError = true;
                this.DescError = ERROR_MESSAGES.REQUIRED_FIELD;
                return false;
            }

            return (ValidateHeadline());

            return true;
            
        }
        private bool ValidateHeadline() //overload function for when used by submitting.
        {
            if (this.Headline.Length > 48)
            {
                this.ShowHeadlineError = true;
                this.HeadlineError = "אורך הכותרת עד 48 תווים";
                return false;
            }



            this.ShowHeadlineError = false;
            this.HeadlineError = "";
            return true;
        }
        private bool ValidateHeadline(string val) //Overload for function for when user inputs headline.
        {
            if (val.Length > 48)
            {
                this.ShowHeadlineError = true;
                this.HeadlineError = "אורך הכותרת עד 48 תווים";
                return false;                
            }



            this.ShowHeadlineError = false;
            this.HeadlineError = "";
            return true;
        }
        

        #endregion

        ///The following command handle the pick photo button
        /*       FileResult imageFileResult;
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
                   if (pickResults != null)
                   {
                       this.imageFileResult = result;

                       var stream = await result.OpenReadAsync();
                       ImageSource imgSource = ImageSource.FromStream(() => stream);
                       if (SetImageSourceEvent != null)
                           SetImageSourceEvent(imgSource);
                   }
               }*/

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
