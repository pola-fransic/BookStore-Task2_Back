using System;
using System.Text.Json.Serialization; 

namespace BookStoreApp.Models
{
    [JsonDerivedType(typeof(PaperbackBook), typeDiscriminator: "Paperback")]
    [JsonDerivedType(typeof(EBook), typeDiscriminator: "EBook")]
    public abstract class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        private readonly object _stockLock = new object();

        public delegate void StockEmptyHandler(Book book);
        public event StockEmptyHandler? OnOutOfStock;

        public void DecreaseStock(int quantity)
        {
            lock (_stockLock)
            {
                if (quantity > Stock)
                    throw new InvalidOperationException("Not enough stock available.");

                Stock -= quantity;

                if (Stock == 0)
                {
                    OnOutOfStock?.Invoke(this);
                }
            }
        }
    }
}