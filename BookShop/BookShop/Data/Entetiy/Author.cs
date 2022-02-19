using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data.Entetiy
{
    public class Author:Base
    {
        public string FullName { get; set; }
        public string Image { get; set; }
        public bool DisplayStatus { get; set; }
        public List<Book> Books { get; set; }
    }

  
}
