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

        private List<Message> messages;
        public List<Message> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged("Messages");
            }
        }
        private string newMessage;
        public string NewMessage
        {
            get => newMessage;
            set
            {
                newMessage = value;
                OnPropertyChanged("NewMessage");
            }
        }

        private string errorRating;
        public string ErrorRating //Displays when there are issue with displaying Messages.
        {
            get => errorRating;
            set
            {
                errorRating = value;
                OnPropertyChanged("ErrorRating");
            }
        }
        private bool showErrorRating;
        public bool ShowErrorRating
        {
            get => showErrorRating;
            set
            {
                showErrorRating = value;
                OnPropertyChanged("ShowErrorRating");
            }
        }

        private string errorMessages;
        public string ErrorMessages //Displays when there are issue with displaying Messages.
        {
            get => errorMessages;
            set
            {
                errorMessages = value;
                OnPropertyChanged("ErrorMessages");
            }
        }
        private bool showErrorMessages;
        public bool ShowErrorMessages
        {
            get => showErrorMessages;
            set
            {
                showErrorMessages = value;
                OnPropertyChanged("ShowErrorMessages");
            }
        }

                private string errorUploadMessage;
        public string ErrorUploadMessage //Displays when there are issue with displaying Messages.
        {
            get => errorUploadMessage;
            set
            {
                errorUploadMessage = value;
                OnPropertyChanged("ErrorUploadMessage");
            }
        }
        private bool showErrorUpload;
        public bool ShowErrorUpload
        {
            get => showErrorMessages;
            set
            {
                showErrorUpload = value;
                OnPropertyChanged("ShowErrorUpload");
            }
        }

        #region StarImages
        //Sets all star colors recursiouvly (sorta), cool beans.
        private string starImageOne { get; set; }
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

            //Setting errors.
            this.ShowErrorMessages = false;
            this.ErrorMessages = "אין הודעות עדיין";

            this.ShowErrorRating = false;
            this.ErrorRating = "";

            this.ShowErrorUpload = false;
            this.ErrorUploadMessage = "";

            //Star numbers for the function.
            this.StarOne = 1;
            this.StarTwo = 2;
            this.StarThree = 3;
            this.StarFour = 4;
            this.StarFive = 5;
            
            GetMessages(); //Sets messages in the list.

        }
        #endregion

        #region Commands
        public Command RateCommand => new Command(RateSikum); //Work in progress.
        private async void RateSikum()
        {
            if (ValidateRating() == false)
                return;

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
        private void ClickedOnStar(int starNumber) //This code is full of ifs. eef. (haha).
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

        private async void GetMessages()
        {
            try
            {
                this.Messages = await API.GetMessages(this.ChosenFile.FileId);
                if(this.Messages == null || this.Messages.Count == 0) //If Messages was null or empty.
                {
                    this.ShowErrorMessages = true;
                    this.ErrorMessages = "אין הודעות עדיין";
                    this.Messages = new List<Message>(); //Sets an empty list to prevent errors.
                }
            }
            catch //Something went wrong with settings the messages.
            {
                this.ShowErrorMessages = true;
                this.ErrorMessages = "היה בעיה בהצגת ההודעות, אנא נסה מאוחר יותר";
            }
        }

        public Command AddMessageComand => new Command(AddMessage); //Work in progress.
        private async void AddMessage()
        {
            if (ValidateTheMessage() == false) //If Validation was incorrect.
                return;

            Message newMessage = new Message(this.ChosenFile.FileId, this.currentApp.CurrentUser.UserID, this.currentApp.CurrentUser.Username, this.NewMessage); //Creates new message
            bool messageAdded = await API.AddMessage(newMessage);

            if(messageAdded == false) //Message wasn't added.
            {
                this.ShowErrorMessages = true;
                this.ErrorUploadMessage = "שגיאה בהעלאת הודעה, אנא נסה שנית";
            }
            else //Message was added
            {
                this.Messages.Add(newMessage);
                this.Messages = this.Messages;
                //Add Success confirmation. Work in progress.
            }
        }
        #endregion

        #region Validations
        private bool ValidateTheMessage()
        {
            if (String.IsNullOrEmpty(this.NewMessage))
            {
                this.ShowErrorUpload = true;
                this.ErrorUploadMessage = "הודעה לא יכולה להיות ריקה";
                return false;
            }
            if(this.currentApp.CurrentUser == null)
            {
                this.ShowErrorMessages = true;
                this.ErrorUploadMessage = "רק משתמשים מחוברים יכולים לכתוב הודעות";
                return false;
            }

            //If it ran until here,input is validated.
            this.ShowErrorMessages = false;
            return true;

        }

        private bool ValidateRating()
        {

            if (this.currentApp.CurrentUser == null)
            {
                this.ShowErrorRating = true;
                this.ErrorRating = "רק משתמשים מחוברים יכולים לדרג";
                return false;
            }

            //If it ran until here,input is validated.
            this.ShowErrorRating = false;
            return true;

        }
        #endregion

    }
}
