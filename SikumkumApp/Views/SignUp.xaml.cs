using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.ViewModels;

namespace SikumkumApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        SignUpVM su;
        public SignUp()
        {
            su = new SignUpVM();
            this.BindingContext = su;

            InitializeComponent();
        }
    }
}