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
        private UserPageVM upVM;
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

        private async void ChangePassword()
        {



        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        async void ChangePassword(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("שים לב", "האם אתה בטוח שאתה רוצה לשנות את הסיסמה?", "כן", "לא");
            if (answer)
            {
                this.upVM.ChangePassCommand.Execute(null); //Executes the change password command in the VM.
            }
        }
    }
}