using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreApp.Models
{
    public class Purchase : BaseEntity
    {
        public Customer? Buyer { get; set; }
        public List<Book> BooksBought { get; set; } = new List<Book>();
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public decimal TotalAmount => BooksBought.Sum(b => b.Price);
    }
}