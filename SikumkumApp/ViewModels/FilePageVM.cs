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
        private string fileTest { get; set; }
        public string FileTest
        {
            get { return this.fileTest; }
            set
            {
                this.fileTest = value;
                this.OnPropertyChanged("FileTest");
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

        private List<string> fileBits { get; set; }
        public List<string> FileBits
        {
            get { return this.fileBits; }
            set
            {
                this.fileBits = value;
                this.OnPropertyChanged("FileBits");
            }
        }
        #endregion

        #region Constructor
        public FilePageVM(SikumFile chosen)
        {
            this.ChosenFile = chosen;
            this.Headline = chosen.Headline;
            this.Username = chosen.Username;
            this.FileTest = $"{API.baseUri}/imgs/{chosen.Url}1";
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
}
