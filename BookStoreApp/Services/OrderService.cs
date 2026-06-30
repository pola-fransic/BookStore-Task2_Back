using System;
using System.Collections.Generic;
using System.Linq;
using BookStoreApp.Models;
using BookStoreApp.Repositories;

namespace BookStoreApp.Services
{
    public class OrderService
    {
        private readonly IRepository<Purchase> _purchaseRepo;

        public OrderService(IRepository<Purchase> purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;
        }

        public void CreatePurchase(Customer customer, List<Book> booksToBuy)
        {
            foreach (var book in booksToBuy)
            {
                if (book.Stock <= 0)
                {
                    throw new InvalidOperationException($"Cannot sell '{book.Title}'. It is out of stock.");
                }
            }

            foreach (var book in booksToBuy)
            {
                book.DecreaseStock(1);
            }

            var purchase = new Purchase
            {
                Buyer = customer,
                BooksBought = booksToBuy
            };

            _purchaseRepo.Add(purchase);
        }

        public decimal GetTotalRevenue()
        {
            return _purchaseRepo.GetAll().Sum(p => p.TotalAmount);
        }

        public Customer? GetTopCustomer()
        {
            return _purchaseRepo.GetAll()
                .Where(p => p.Buyer != null)
                .GroupBy(p => p.Buyer)
                .OrderByDescending(g => g.Sum(p => p.TotalAmount))
                .FirstOrDefault()?.Key;
        }

        public Book? GetBestSellingBook()
        {
            return _purchaseRepo.GetAll()
                .SelectMany(p => p.BooksBought)
                .GroupBy(b => b)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;
        }
    }
}