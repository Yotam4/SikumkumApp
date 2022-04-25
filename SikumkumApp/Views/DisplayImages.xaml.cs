using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.ViewModels;
using SikumkumApp.Models;
namespace SikumkumApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayImages : ContentPage
    {
        private List<ImgSrc> imageSrcs;
        private DisplayImagesVM displayImgVM;
        public DisplayImages(List<ImgSrc> imageSrcs)
        {
            this.imageSrcs = imageSrcs;
            this.displayImgVM = new DisplayImagesVM(imageSrcs);
            this.BindingContext = displayImgVM;

            InitializeComponent();
        }
    }
}