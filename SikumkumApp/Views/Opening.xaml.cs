using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.ViewModels;
using SikumkumApp.Services;
using SikumkumApp.Models;
using System.Collections.ObjectModel;

namespace SikumkumApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Opening : ContentPage
    {
        OpeningVM oV;
        public Opening()
        {
            oV = new OpeningVM();

            this.BindingContext = oV;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            App currentApp = (App)App.Current;
            if (currentApp.CurrentUser != null) //Changes UI if user is logged in.
            {
                oV.CanLogIn = false; //No longer shcws signup and log in.
                oV.IsLoggedIn = true; //Shows user page button. 
            }
            
        }
    }
}