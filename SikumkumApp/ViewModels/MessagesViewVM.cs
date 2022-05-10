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
    class MessagesViewVM : BaseVM
    {
        #region Variables 
        public SikumFile ChosenFile { get; set; }
        private const string STAR_COLOR_FILLED = "starTranspColor.png";
        private const string STAR_COLOR_EMPTY = "StarTrans.png";
        public double RatingGiven { get; set; }
        #region StarImages
        //Sets all star colors recursiouvly (sorta), cool beans.
        private string starImageOne { get; set; } //User's new password to set.
        public string StarImageOne
        {
            get => starImageOne;
            set
            {
                starImageOne = value;
                OnPropertyChanged("StarImageOne");
            }
        }
        private string starImageTwo { get; set; }
        public string StarImageTwo
        {
            get => starImageTwo;
            set
            {
                starImageTwo = value;
                OnPropertyChanged("StarImageTwo");
            }
        }
        private string starImageThree { get; set; }
        public string StarImageThree
        {
            get => starImageThree;
            set
            {
                StarImageTwo = value;
                starImageThree = value;
                OnPropertyChanged("StarImageThree");
            }
        }
        private string starImageFour { get; set; }
        public string StarImageFour
        {
            get => starImageFour;
            set
            {
                StarImageThree = value;
                starImageFour = value;
                OnPropertyChanged("StarImageFour");
            }
        }
        private string starImageFive { get; set; }
        public string StarImageFive
        {
            get => starImageFour;
            set
            {
                StarImageFour = value;
                starImageFive = value;
                OnPropertyChanged("StarImageFive");
            }
        }
        public int StarOne { get; set; }
        public int StarTwo { get; set; }
        public int StarThree { get; set; }
        public int StarFour { get; set; }
        public int StarFive { get; set; }

        private string ratingSuccess { get; set; }
        public string RatingSuccess
        {
            get => ratingSuccess;
            set
            {
                ratingSuccess = value;
                OnPropertyChanged("RatingSuccess");
            }
        }
        #endregion

        #endregion

        #region Constructor
        public MessagesViewVM(SikumFile chosen)
        {
            this.ChosenFile = chosen;
            this.RatingGiven = 1; //Minumum value is always one.
            this.StarImageOne = STAR_COLOR_FILLED; //Star one is always colored.
            this.StarImageFive = STAR_COLOR_EMPTY; //Sets all of the rest to be empty. 
            this.RatingSuccess = "";

            //Star numbers for the function.
            this.StarOne = 1;
            this.StarTwo = 2;
            this.StarThree = 3;
            this.StarFour = 4;
            this.StarFive = 5;

        }
        #endregion

        #region Commands
        public Command RateCommand => new Command(RateSikum); //Work in progress.
        private async void RateSikum()
        {
            Rating newRating = new Rating(this.ChosenFile.FileId, this.currentApp.CurrentUser.UserID, this.RatingGiven);
            double totalRating = await API.RateSikum(newRating);
            if (totalRating == -1) 
            {  //Rating failed.
                return;
            }
            this.currentApp.CurrentFile.FileRating = totalRating;
             //Make new rating appear in SikumFile Page after you open it. Work in progress.
        }
        public Command ClickedOnStarCommand => new Command<int>(ClickedOnStar); 
        private void ClickedOnStar(int starNumber) //This code is ugly. I am aware.
        {
            //Reset all star values to default.
            this.StarImageFive = STAR_COLOR_EMPTY;
            this.RatingGiven = starNumber;

            if (starNumber == 1)
                return;

            if (starNumber == 2)
                this.StarImageTwo = STAR_COLOR_FILLED;

            if (starNumber == 3)
                this.StarImageThree = STAR_COLOR_FILLED;

            if (starNumber == 4)

                this.StarImageFour = STAR_COLOR_FILLED;

            if (starNumber == 5)
                this.StarImageFive = STAR_COLOR_FILLED;
        }

        #endregion

        #region Validations

        #endregion

    }
}
