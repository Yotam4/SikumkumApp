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
    class OpeningVM : INotifyPropertyChanged
    {
        private ObservableCollection<Subject> subjectsCollec;
        public OpeningVM()
        {
            SikumkumAPIProxy API = SikumkumAPIProxy.CreateProxy();
            Task<List<Subject>> subjectsList = API.GetSubjects();
            this.subjectsCollec = new ObservableCollection<Subject>(subjectsList.Result);
        }
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
