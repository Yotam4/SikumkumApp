using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.Models;
using SikumkumApp.ViewModels;

namespace SikumkumApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserFiles : ContentPage
    {
        private UserFilesVM userFilesVM;
        public UserFiles()
        {
            this.userFilesVM = new UserFilesVM();
            this.BindingContext = userFilesVM;

            InitializeComponent();
        }

        //protected override void OnAppearing()
        //{
        //    this.userFilesVM.
        //}
    }
}