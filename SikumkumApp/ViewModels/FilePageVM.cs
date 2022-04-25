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

        public SikumFile ChosenFile { get; set; }
        public string SikumBy { get; set; }
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

        #endregion

        #region Constructor
        public FilePageVM(SikumFile chosen)
        {
            this.ChosenFile = chosen; 
            this.Headline = chosen.Headline;
            this.Username = chosen.Username;
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
                //Work in progress.
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
        public Command UploadedUserCommand => new Command(UploadedUser);
        private async void UploadedUser()
        {
            //Work in progress.
        }
        
        public Command DeleteCommand => new Command(DeleteSikum);
        private async void DeleteSikum()
        {
            try
            {
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

        public Command DisplayImagesCommand => new Command(DisplayImages);
        private async void DisplayImages()
        {
            DisplayImages displayImgs = new DisplayImages(this.Sources);
            displayImgs.WidthRequest = 300;
            displayImgs.HeightRequest = 300;
            await this.currentApp.MainPage.Navigation.PushModalAsync(displayImgs);
        }
        #endregion

        #region Validations

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
}
