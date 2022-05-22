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
    class FilePageVM : BaseVM
    {


        #region Variables


        public string SikumBy { get; set; }
        private string username { get; set; }
        private SikumFile chosenFile { get; set; }
        public bool UploadRejected { get; set; }

        public SikumFile ChosenFile
        {
            get { return this.chosenFile; }
            set
            {
                this.chosenFile = value;
                this.OnPropertyChanged("ChosenFile");
            }
        }
        public string Username
        {
            get { return this.username; }
            set
            {
                this.username = value;
                this.OnPropertyChanged("Username");
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
        private double fileRating { get; set; }
        public double FileRating
        {
            get { return this.fileRating; }
            set
            {
                this.fileRating = value;
                this.OnPropertyChanged("FileRating");
            }
        }
        private bool needApproval { get; set; }
        public bool NeedApproval
        {
            get { return this.needApproval; }
            set
            {
                this.needApproval = value;
                this.OnPropertyChanged("NeedApproval");
            }
        }

        private bool isOwner { get; set; }
        public bool IsOwner
        {
            get { return this.isOwner; }
            set
            {
                this.isOwner = value;
                this.OnPropertyChanged("IsOwner");
            }
        }
        private string deleteSrc { get; set; }
        public string DeleteSrc
        {
            get { return this.deleteSrc; }
            set
            {
                this.deleteSrc = value;
                this.OnPropertyChanged("DeleteSrc");
            }
        }
        

        private List<ImgSrc> sources { get; set; }
        public List<ImgSrc> Sources
        {
            get { return this.sources; }
            set
            {
                this.sources = value;
                this.OnPropertyChanged("Sources");
            }
        }

        private PdfSrc pdfFile { get; set; } //List of pdf files, with name and URL.
        public PdfSrc PdfFile
        {
            get { return this.pdfFile; }
            set
            {
                this.pdfFile = value;
                this.OnPropertyChanged("PdfFile");
            }
        }

        #endregion

        #region Constructor
        public FilePageVM(SikumFile chosen)
        {
            this.ChosenFile = chosen;
            this.currentApp.CurrentFile = chosen;

            this.Headline = chosen.Headline;
            this.Username = chosen.Username;

            string fileRatingStr = GetRatingStr(chosen.FileRating.ToString()); 

            this.FileRating = double.Parse(fileRatingStr);

            this.NeedApproval = false; //Set approval initially to false.
            this.SikumBy = "העלאה של " + chosen.Username;

            if(this.currentApp.CurrentUser != null && this.currentApp.CurrentUser.IsAdmin && !chosen.Approved) //If sikumfile needs approval, only admin can approve.
            {
                this.NeedApproval = true;
            }

            this.Sources = new List<ImgSrc>();
            if (this.ChosenFile.HasImage) //If sikum contains images, add them to the collection.
            {
                for (int i = 1; i <= chosen.NumOfFiles; i++) //Adds all imageSources urls to list.
                {
                    string source = $"{API.basePhotosUri}{chosen.Url}{i}.jpg"; //Current image source.
                    ImgSrc imgsrc = new ImgSrc(source);
                    this.Sources.Add(imgsrc);
                }
            }

            if (this.ChosenFile.HasPdf) //If sikum contain pdf files, work with them.
            {
                string source = $"{API.basePdfsUri}{chosen.Url}{"1"}.pdf"; //Current pdf source. always has 1.
                PdfSrc pdfSrc = new PdfSrc(source, chosen.Url + "1"); //Source = url to photo. url = the name. It will be ugly so maybe change URL name.
                this.PdfFile = pdfSrc;
            }

            this.DeleteSrc = ""; //Sets it to base value to prevent errors.
            this.IsOwner = false; //Isn't owner, unless it enters the if statement.
            if(this.currentApp.CurrentUser != null && this.ChosenFile.UserID == this.currentApp.CurrentUser.UserID || this.currentApp.CurrentUser != null && this.currentApp.CurrentUser.IsAdmin) //If it is the owner's file or if it's an admin.
            {
                this.DeleteSrc = "DeleteIcon.pdf";
                this.IsOwner = true;
            }

            if(this.ChosenFile.Disapproved == true) //If file is rejected.
            {
                this.UploadRejected = true; //Sets rejected to true.
                this.NeedApproval = false; //Doesn't need to be rejected\accepted anymore.
            }
        }
        #endregion

        #region Commands
        public Command ConfirmUploadCommand => new Command(ConfirmUpload);
        private async void ConfirmUpload()
        {
            try
            {                
                bool uploadAccepted = await API.TryAcceptUpload(this.ChosenFile);
                if (uploadAccepted)
                {
                    this.NeedApproval = false;
                    await App.Current.MainPage.Navigation.PopAsync();
                    //Add Validation message. Work in progress.
                }
                else
                {
                    //Add error message.
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Command RejectUploadCommand => new Command(RejectUpload);
        private async void RejectUpload()
        {
            try
            {
                
                bool uploadRejected = await API.TryRejectUpload(this.ChosenFile);
                if (uploadRejected)
                {
                    this.NeedApproval = false;
                    //Maybe send admin to add a message of what is wrong with sikum, before the screen pops. Work in progress.
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    //Add error message.
                }
            }
            catch (Exception ex)
            {

            }
        }
        public Command UploadedUserCommand => new Command(UploadedUser);
        private async void UploadedUser()
        {
            MessagesView messagesView = new MessagesView(this.ChosenFile);
            App.Current.MainPage.Navigation.PushAsync(messagesView);
            //Work in progress.
        }
        public Command ClickedOnPdfCommand => new Command(ClickedOnPdf);
        private async void ClickedOnPdf()
        {
            var filePath = await API.DownloadPdfFileAsync(this.PdfFile.Url, this.PdfFile.PdfName); //Gets local pdf file path.
            try
            {
                if (filePath != null)
                {
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(filePath)
                    });
                }
            }

            catch(Exception ex)
            {
                int a = 4;
            }
        }
        public Command DeleteCommand => new Command(DeleteSikum);
        private async void DeleteSikum()
        {
            try
            {
                bool areYouSure = await App.Current.MainPage.DisplayAlert("האם אתה בטוח?", "כל הנתונים ימחקו וגם דירוגך ישתנה בהתאם", "מחק", "בטל");
                if (areYouSure == false) //If user doesnt want to delete, stop the function.
                    return;

                bool sikumDeleted = await API.TryDeleteSikum(this.ChosenFile);
                if (sikumDeleted)
                {
                    await this.currentApp.MainPage.Navigation.PopAsync();
                    //Add Validation message. Work in progress.
                }
                else
                {
                    //Add error message.
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Validations
        private string GetRatingStr(string ratingStr) //Sets the rating, to prevent from errors and also getting a number such as 3.6667, etc.
        {
            int couter = 0;
            string returnString = "";
            while(couter < ratingStr.Length && couter < 3) //Sets the number to be either (for example) 3 or 3.5
            {
                returnString += ratingStr[couter++];
            }
            return returnString;
        }
        #endregion

    }

    public class ImgSrc //class for storing sources, needed for collectionview, it doesn't let me just do "Binding,"
    {
        public string source { get; set; }
        public ImgSrc()
        {

        }
        public ImgSrc(string source)
        {
            this.source = source; 
        }
    }
    public class PdfSrc //class for storing sources, needed for collectionview, it doesn't let me just do "Binding,"
    {
        public string Url { get; set; }
        public string PdfName { get; set; }

        public PdfSrc()
        {

        }
        public PdfSrc(string url, string pdfName)
        {
            this.Url = url;
            this.PdfName = pdfName;
        }
    }
}
