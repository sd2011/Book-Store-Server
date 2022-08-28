using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualLibrary.Models
{
    
    public class ReviewAndCustomerName
    {
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public int Rate {get; set;}
        public int BookId { get; set; }
        public string UserName {get; set;}

    public ReviewAndCustomerName(
        int id,
         DateTime reviewDate, 
         string title, 
         string reviewText ,
         int rate, 
         int bookId,
         string? userName
         )
         {
               Id=id;
             
            ReviewDate = reviewDate;

            Title = title;

            ReviewText = reviewText;

            Rate = rate;

            BookId = bookId;

            UserName = userName;
         }

    }
}