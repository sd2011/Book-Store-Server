using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public int Rate {get; set;}
        public int BookId { get; set; }

        public Review(int id, string title, string reviewText ,int rate, int bookId){
            Id=id;
             
            ReviewDate = DateTime.Now;

            Title = title;

            ReviewText = reviewText;

            Rate = rate;

            BookId = bookId;
        }
       
    }
    
}