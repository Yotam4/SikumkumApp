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
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region Variables

        public SikumFile ChosenFile { get; set; }
        public List<string> s  { get; set; } 

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

            this.Sources = new ObservableCollection<ImgSrc>();
            for (int i = 1; i <= chosen.NumOfFiles; i++) //Adds all photo urls to thingy.
            {
                string source = $"{API.basePhotosUri}{chosen.Url}{i}.jpg"; //Current image source.
                ImgSrc imgsrc = new ImgSrc(source);
                this.Sources.Add(imgsrc);
            }

        }
        #endregion

        #region Commands
        private void GetFilesOfSikum()
        {

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
