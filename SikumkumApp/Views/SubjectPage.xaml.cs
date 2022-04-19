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
    public partial class SubjectPage : ContentPage
    {
        private Subject chosenSubject;
        private SubjectVM subjectvm;
        public SubjectPage(Subject chosenSub)
        {
            this.chosenSubject = chosenSub;
            this.subjectvm = new SubjectVM(chosenSub);
            this.BindingContext = subjectvm;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.subjectvm.GetSikumFiles();
        }
    }
}