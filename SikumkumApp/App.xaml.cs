using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.Views;
using SikumkumApp.Services;
using SikumkumApp.Models;
using System.Collections.Generic;
[assembly: ExportFont("Rubik-Italic.ttf", Alias = "RubikItalic")]
[assembly: ExportFont("Rubik-Regular.ttf", Alias = "RubikRegular")]
[assembly: ExportFont("MiriamLibreR.ttf", Alias = "MiriamLibre")]

namespace SikumkumApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv { get; internal set; }
        public User CurrentUser { get; set; }
        public OpeningObject OpeningObj { get; set; }

        public SikumFile CurrentFile { get; set; }

        public App()
        {
            InitializeComponent();
            this.CurrentUser = null;
            MainPage = new LoadingPage(); //Change to new loading page.             
        }

        protected async override void OnStart()
        {
            SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();

            //this.SubjectsList = new List<Subject>(); //For testing purposes while server is down.
            //Subject s = new Subject("חשבון", 100);
            //Subject s1 = new Subject("מדעים", 100);
            //Subject s2 = new Subject("פיזיקה", 100);
            //Subject s3 = new Subject("לשון", 100);

            //this.SubjectsList.Add(s); this.SubjectsList.Add(s1); this.SubjectsList.Add(s2); this.SubjectsList.Add(s3 );

            try
            {
                this.OpeningObj = await API.GetOpeningObject();

                Opening openingPage = new Opening();
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
