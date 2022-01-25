using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.Models;

namespace SikumkumApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilePage : ContentPage
    {
        SikumFile sf;
        public FilePage(SikumFile sf)
        {
            this.sf = sf;
            InitializeComponent();
        }
    }
}