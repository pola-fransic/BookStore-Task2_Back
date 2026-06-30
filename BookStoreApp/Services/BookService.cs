using System;
using System.Collections.Generic;
using System.Linq;
using BookStoreApp.Models;
using BookStoreApp.Repositories;

namespace BookStoreApp.Services
{
    public class BookService
    {
        private readonly IRepository<Book> _bookRepo;

        public BookService(IRepository<Book> bookRepo)
        {
            _bookRepo = bookRepo;
        }

        public void AddBook(Book book)
        {
            book.OnOutOfStock += (b) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ALERT] The book '{b.Title}' is now out of stock!");
                Console.ResetColor();
            };

            _bookRepo.Add(book);
        }

        public IEnumerable<Book> FilterBooks(string? category = null, string? author = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            return _bookRepo.Find(b =>
                (string.IsNullOrEmpty(category) || b.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(author) || b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)) &&
                (!minPrice.HasValue || b.Price >= minPrice.Value) &&
                (!maxPrice.HasValue || b.Price <= maxPrice.Value)
            );
        }

        public void ApplyCustomRule(Action<Book> rule)
        {
            foreach (var book in _bookRepo.GetAll())
            {
                rule(book);
            }
        }
    }
}