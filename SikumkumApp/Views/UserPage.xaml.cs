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
    public partial class UserPage : ContentPage
    {
        UserPageVM upVM;
        public UserPage()
        {
            upVM = new UserPageVM();
            this.BindingContext = upVM;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            App currentApp = (App)App.Current;
            if (currentApp.CurrentUser.IsAdmin)
            {
                upVM.IsAdmin = true;
            }
        }
    }
}