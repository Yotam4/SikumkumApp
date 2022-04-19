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


        private ObservableCollection<ImgSrc> sources { get; set; }
        public ObservableCollection<ImgSrc> Sources
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

            if(this.currentApp.CurrentUser != null && this.currentApp.CurrentUser.IsAdmin && !chosen.Approved) //If sikumfile needs approval, only admin can approve.
            {
                this.NeedApproval = true;
            }


            this.Sources = new ObservableCollection<ImgSrc>();
            for (int i = 1; i <= chosen.NumOfFiles; i++) //Adds all imageSources urls to list.
            {
                string source = $"{API.basePhotosUri}{chosen.Url}{i}.jpg"; //Current image source.
                ImgSrc imgsrc = new ImgSrc(source);
                this.Sources.Add(imgsrc);
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
