using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SikumkumApp.Models;
using SikumkumApp.Services;
using System.Threading;
using System.Threading.Tasks;
using SikumkumApp.Views;


namespace SikumkumApp.ViewModels
{
    class ConfirmUploadsVM : BaseVM
    {
        #region Variables 
        public ObservableCollection<SikumFile> pendingFiles { get; set; }

        #endregion

        #region Constructor
        public ConfirmUploadsVM()
        {

        }
        #endregion

        #region Commands

        #endregion
    }
}
