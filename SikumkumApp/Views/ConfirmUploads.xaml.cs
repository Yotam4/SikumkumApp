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
    public partial class ConfirmUploads : ContentPage
    {
        private ConfirmUploadsVM cuVM;
        public ConfirmUploads()
        {
            this.cuVM = new ConfirmUploadsVM();
            this.BindingContext = cuVM;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.cuVM.GetPendingFiles();
        }
    }
}