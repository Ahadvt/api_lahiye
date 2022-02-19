using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data.Entetiy
{
    public class Book:Base
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubilisDate { get; set; }
        public bool InStock { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public int AuthorId { get; set; }
        public int JanrId { get; set; }
        public Janr Janr { get; set; }
        public Author Author { get; set; }

    }
}
