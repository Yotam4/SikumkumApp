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
    public partial class MessagesView : ContentPage
    {
        MessagesViewVM msgVM;
        SikumFile chosen;
        public MessagesView(SikumFile chosen)
        {
            this.chosen = chosen;
            this.msgVM = new MessagesViewVM(chosen);
            this.BindingContext = msgVM;
            InitializeComponent();
        }
    }
}