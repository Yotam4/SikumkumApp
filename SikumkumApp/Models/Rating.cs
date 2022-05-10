using System;
using System.Collections.Generic;


namespace SikumkumApp.Models
{
    public partial class Rating
    {
        public int RatingId { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public double RatingGiven { get; set; }

        public Rating() { }
        public Rating(int fileId, int userId, double ratingGiven)
        {
           this.RatingId = -1; //Dumb value to preset.
           this.FileId = fileId;
           this.UserId = userId;
           this.RatingGiven = ratingGiven;            
        }
    }
}
