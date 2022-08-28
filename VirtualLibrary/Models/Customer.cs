using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models
{
    public partial class Customer
    {
       

        public int Id { get; set; }
        public string? Fname { get; set; }
        public string? Lname { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? Bdate { get; set; }
        public bool? Gender { get; set; }
        public string? Password { get; set; }



    }
}