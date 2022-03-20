﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SikumkumApp.ViewModels;
using SikumkumApp.Services;
using SikumkumApp.Models;
using System.Collections.ObjectModel;

namespace SikumkumApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Opening : ContentPage
    {
        OpeningVM oV;
        public Opening(List<Subject> subjects)
        {
            oV = new OpeningVM(subjects);

            this.BindingContext = oV;

            InitializeComponent();
        }
    }
}