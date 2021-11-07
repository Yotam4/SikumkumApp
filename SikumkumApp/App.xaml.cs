using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.Views;

namespace SikumkumApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
