using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.Views;
using SikumkumApp.Services;
using SikumkumApp.Models;
using System.Collections.Generic;


namespace SikumkumApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new LoadingPage(); //Change to new loading page.
        }

        protected async override void OnStart()
        {
            SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
            try
            {
                List<Subject> subjects = await API.GetSubjects();
                Opening openingPage = new Opening(subjects);
                MainPage = new NavigationPage(openingPage);
            }
            catch (Exception e)
            {
                Application.Current.Quit();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
