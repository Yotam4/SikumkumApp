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
using System.Text.RegularExpressions;
using System.Linq;

namespace SikumkumApp.ViewModels
{
    class SubjectVM : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Variables
        private Subject currentSubject;

        private string isSummary { get; set; }
        public string IsSummary
        {
            get { return this.isSummary; }
            set
            {
                this.isSummary = value;
                this.OnPropertyChanged("IsSummary");
            }
        }

        private string isPractice { get; set; }
        public string IsPractice
        {
            get { return this.isPractice; }
            set
            {
                this.isPractice = value;
                this.OnPropertyChanged("IsPractice");
            }
        }

        private string isEssay { get; set; }
        public string IsEssay
        {
            get { return this.isEssay; }
            set
            {
                this.isEssay = value;
                this.OnPropertyChanged("IsEssay");
            }
        }

        #endregion

        #region Constructor

        public SubjectVM(Subject chosen)
        {
            this.currentSubject = chosen;
        }

        #endregion

        #region Commands

        #endregion
    }

}
