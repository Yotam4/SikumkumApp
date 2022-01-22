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

        private bool isSummary { get; set; }
        public bool IsSummary
        {
            get { return this.isSummary; }
            set
            {
                this.isSummary = value;
                this.OnPropertyChanged("IsSummary");
            }
        }

        private bool isPractice { get; set; }
        public bool IsPractice
        {
            get { return this.isPractice; }
            set
            {
                this.isPractice = value;
                this.OnPropertyChanged("IsPractice");
            }
        }

        private bool isEssay { get; set; }
        public bool IsEssay
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
            this.isSummary = true; //Only lookup summaries when page is opened.
            this.isEssay = false;
            this.isPractice = false;
        }

        #endregion

        #region Commands

        #endregion
    }

}
