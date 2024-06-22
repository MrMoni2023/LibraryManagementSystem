using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagementSystem.Models
{
    public class BookMV
    {
        public int BookID { get; set; }
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public int BookTypeID { get; set; }
        public string BookTitle { get; set; }
        public string ShortDescription { get; set; }
        public string Author { get; set; }
        public string BookName { get; set; }
        public double Edition { get; set; }
        public int TotalCopies { get; set; }
        public System.DateTime RegDate { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
}