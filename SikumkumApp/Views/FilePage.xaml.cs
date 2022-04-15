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
    public partial class FilePage : ContentPage
    {
        SikumFile sf;
        FilePageVM fpVM;
        public FilePage(SikumFile sf)
        {
            this.sf = sf;
            this.fpVM = new FilePageVM(sf);
            this.BindingContext = fpVM;

            InitializeComponent();
        }
    }
}