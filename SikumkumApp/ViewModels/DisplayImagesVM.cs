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
    class DisplayImagesVM : BaseVM
    {
        #region Variables 
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
        public DisplayImagesVM(List<ImgSrc> sourcesList)
        {
            if (sourcesList != null && sourcesList.Count > 0)
                this.Sources = new ObservableCollection<ImgSrc>(sourcesList);
            else
                this.Sources = new ObservableCollection<ImgSrc>(); //Empty
        }
        #endregion

        #region Commands
        public Command ExitPageCommand => new Command(ExitPageFunc);
        private async void ExitPageFunc()
        {
            await this.currentApp.MainPage.Navigation.PopModalAsync();
        }
        #endregion

        #region Validations

        #endregion
    }
}
