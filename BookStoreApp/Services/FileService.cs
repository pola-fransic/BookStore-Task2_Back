using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using BookStoreApp.Models;

namespace BookStoreApp.Services
{
    public class FileService
    {
        private readonly string _filePath = "bookstore_data.json";

        public async Task SaveDataAsync(IEnumerable<Book> books, IEnumerable<Customer> customers)
        {
            var dataToSave = new
            {
                Books = books,
                Customers = customers
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(dataToSave, options);

            await File.WriteAllTextAsync(_filePath, jsonString);
        }

        public async Task<(List<Book> Books, List<Customer> Customers)> LoadDataAsync()
        {
            if (!File.Exists(_filePath))
            {
                return (new List<Book>(), new List<Customer>());
            }

            string jsonString = await File.ReadAllTextAsync(_filePath);

            try
            {
                var loadedData = JsonSerializer.Deserialize<StoreData>(jsonString);
                return (loadedData?.Books ?? new List<Book>(), loadedData?.Customers ?? new List<Customer>());
            }
            catch
            {
                return (new List<Book>(), new List<Customer>());
            }
        }

        private class StoreData
        {
            public List<Book> Books { get; set; } = new List<Book>();
            public List<Customer> Customers { get; set; } = new List<Customer>();
        }
    }
}