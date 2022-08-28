using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models
{
    public partial class BooksBorrowing
    {
        public int Id { get; set; }
        public DateTime BorrowedDateBegining { get; set; }
        public DateTime? BorrowedDateFinish { get; set; }
        public DateTime? ReturnedBookDate { get; set; }
        public int BookId { get; set; }
        public BooksBorrowing(int id , int bookId){
            Id = id;
            BorrowedDateBegining = DateTime.Now;
            BorrowedDateFinish = DateTime.Now.AddDays(7);
            ReturnedBookDate = null;
            BookId = bookId;
            
            
        }
    }
    
}