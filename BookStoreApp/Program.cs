using System;
using BookStoreApp.Models;
using BookStoreApp.Repositories;
using BookStoreApp.Services;
using BookStoreApp.UI;
using BookStoreApp.Extensions;
using System.Threading.Tasks;

namespace BookStoreApp
{
    class Program
    {
        static IRepository<Book> bookRepo = new InMemoryRepository<Book>();
        static IRepository<Customer> customerRepo = new InMemoryRepository<Customer>();
        static IRepository<Purchase> purchaseRepo = new InMemoryRepository<Purchase>();

        static BookService bookService = new BookService(bookRepo);
        static OrderService orderService = new OrderService(purchaseRepo);
        static FileService fileService = new FileService();

        static async Task Main(string[] args)
        {
            var loadedData = await fileService.LoadDataAsync();
            foreach (var b in loadedData.Books) bookRepo.Add(b);
            foreach (var c in loadedData.Customers) customerRepo.Add(c);

            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("   📚 BOOKSTORE MANAGER 📚       ");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Add a new Book");
                Console.WriteLine("2. List all Books");
                Console.WriteLine("3. Register a Customer");
                Console.WriteLine("4. Make a Purchase");
                Console.WriteLine("5. View Store Stats (Revenue, Top Book/Customer)");
                Console.WriteLine("6. Remove a Book");
                Console.WriteLine("7. Search & Filter Books");
                Console.WriteLine("8. Apply a Discount");
                Console.WriteLine("0. Save & Exit"); // غيرنا الاسم هنا
                Console.WriteLine("=================================");

                int choice = ConsoleHelper.ReadInt("Enter your choice: ", 0);

                Console.Clear();
                try
                {
                    switch (choice)
                    {
                        case 1: AddBookMenu(); break;
                        case 2: ListBooksMenu(); break;
                        case 3: RegisterCustomerMenu(); break;
                        case 4: MakePurchaseMenu(); break;
                        case 5: ViewStatsMenu(); break;
                        case 6: RemoveBookMenu(); break;
                        case 7: FilterBooksMenu(); break;
                        case 8: ApplyDiscountMenu(); break;
                        case 0:
                            isRunning = false;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Saving your data safely to the disk...");

                            await fileService.SaveDataAsync(bookRepo.GetAll(), customerRepo.GetAll());

                            Console.WriteLine("Data saved! Goodbye!");
                            Console.ResetColor();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[SYSTEM ERROR] {ex.Message}");
                    Console.ResetColor();
                }

                if (isRunning)
                {
                    Console.WriteLine("\nPress any key to return to the main menu...");
                    Console.ReadKey();
                }
            }
        }
        static void AddBookMenu()
        {
            Console.WriteLine("--- Add New Book ---");
            string title = ConsoleHelper.ReadString("Enter Title: ");
            string author = ConsoleHelper.ReadString("Enter Author: ");
            string category = ConsoleHelper.ReadString("Enter Category: ");
            decimal price = ConsoleHelper.ReadDecimal("Enter Price: ");
            int stock = ConsoleHelper.ReadInt("Enter Initial Stock: ");

            var newBook = new PaperbackBook
            {
                Title = title,
                Author = author,
                Category = category,
                Price = price,
                Stock = stock,
                WeightInGrams = 200  
            };

            bookService.AddBook(newBook);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCCESS] Book added successfully!");
            Console.ResetColor();
        }

        static void ListBooksMenu()
        {
            Console.WriteLine("--- Book Inventory ---");
            var books = bookRepo.GetAll();

            foreach (var book in books)
            {
                Console.WriteLine($"- {book.Title} by {book.Author} | Category: {book.Category} | Price: {book.Price.ToCurrency()} | Stock: {book.Stock}");
            }
        }
        static void RegisterCustomerMenu()
        {
            Console.WriteLine("--- Register New Customer ---");
            string name = ConsoleHelper.ReadString("Enter Customer Name: ");
            string email = ConsoleHelper.ReadString("Enter Customer Email: ");

            string phone = ConsoleHelper.ReadPhoneNumber("Enter Customer Phone (11 digits, starts with 01): ");

            var newCustomer = new Customer
            {
                Name = name,
                Email = email,
                Phone = phone 
            };

            customerRepo.Add(newCustomer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCCESS] Customer registered successfully!");
            Console.ResetColor();
        }
        static void MakePurchaseMenu()
        {
            Console.WriteLine("--- Make a Purchase ---");

            var customers = customerRepo.GetAll().ToList();
            if (!customers.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No customers registered yet. Please register a customer first.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("\nSelect a Customer:");
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {customers[i].Name} ({customers[i].Phone})");
            }

            int custIndex;
            while (true)
            {
                custIndex = ConsoleHelper.ReadInt("Enter customer number: ", 1) - 1;

                if (custIndex >= 0 && custIndex < customers.Count)
                {
                    break; 
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Invalid selection! Please enter a number between 1 and {customers.Count}.");
                Console.ResetColor();
            }
            var selectedCustomer = customers[custIndex];

            var availableBooks = bookRepo.GetAll().Where(b => b.Stock > 0).ToList();
            if (!availableBooks.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No books available in stock right now.");
                Console.ResetColor();
                return;
            }

            List<Book> cart = new List<Book>();

            while (true)
            {
                Console.WriteLine("\n--- Available Books ---");
                for (int i = 0; i < availableBooks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableBooks[i].Title} | Price: {availableBooks[i].Price.ToCurrency()} | Stock: {availableBooks[i].Stock}");
                }

                int bookInput = ConsoleHelper.ReadInt("\nEnter book number to add to cart (or type '0' to complete purchase): ", 0);

                if (bookInput == 0)
                {
                    break; 
                }

                int bookIndex = bookInput - 1;

                if (bookIndex >= 0 && bookIndex < availableBooks.Count)
                {
                    cart.Add(availableBooks[bookIndex]);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n=> Great! '{availableBooks[bookIndex].Title}' was added to your cart.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[Error] Invalid book! Please enter a number between 1 and {availableBooks.Count}.");
                    Console.ResetColor();
                }
            }

            if (cart.Any())
            {
                orderService.CreatePurchase(selectedCustomer, cart);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[SUCCESS] Purchase completed successfully for {selectedCustomer.Name}!");
                Console.WriteLine($"Total Paid: {cart.Sum(b => b.Price).ToCurrency()}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nCart is empty. Purchase cancelled.");
                Console.ResetColor();
            }
        }

        static void ViewStatsMenu()
        {
            Console.WriteLine("--- Store Statistics ---");

            decimal totalRevenue = orderService.GetTotalRevenue();
            Console.WriteLine($"Total Revenue: {totalRevenue.ToCurrency()}");

            var topCustomer = orderService.GetTopCustomer();
            Console.WriteLine($"Top Customer: {(topCustomer != null ? topCustomer.Name : "None yet")}");

            var bestBook = orderService.GetBestSellingBook();
            Console.WriteLine($"Best-Selling Book: {(bestBook != null ? bestBook.Title : "None yet")}");
        }
        static void RemoveBookMenu()
        {
            Console.WriteLine("--- Remove a Book ---");
            var books = bookRepo.GetAll().ToList();

            if (!books.Any())
            {
                Console.WriteLine("No books available to remove.");
                return;
            }

            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {books[i].Title} by {books[i].Author}");
            }

            int bookIndex = ConsoleHelper.ReadInt("\nEnter book number to remove (or 0 to cancel): ", 0) - 1;

            if (bookIndex == -1) return;

            if (bookIndex >= 0 && bookIndex < books.Count)
            {
                var bookToRemove = books[bookIndex];
                bookRepo.Remove(bookToRemove.Id);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[SUCCESS] '{bookToRemove.Title}' was removed from the store.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        static void FilterBooksMenu()
        {
            Console.WriteLine("--- Search & Filter Books ---");
            Console.WriteLine("Press Enter to skip any filter.");

            Console.Write("Enter Category: ");
            string? category = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(category)) category = null;

            Console.Write("Enter Author: ");
            string? author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author)) author = null;

            Console.Write("Enter Minimum Price: ");
            string? minPriceInput = Console.ReadLine();
            decimal? minPrice = decimal.TryParse(minPriceInput, out decimal min) ? min : null;

            Console.Write("Enter Maximum Price: ");
            string? maxPriceInput = Console.ReadLine();
            decimal? maxPrice = decimal.TryParse(maxPriceInput, out decimal max) ? max : null;

            var filteredBooks = bookService.FilterBooks(category, author, minPrice, maxPrice).ToList();

            Console.WriteLine("\n--- Search Results ---");
            if (!filteredBooks.Any())
            {
                Console.WriteLine("No books match your criteria.");
            }
            else
            {
                foreach (var book in filteredBooks)
                {
                    Console.WriteLine($"- {book.Title} by {book.Author} | Category: {book.Category} | Price: {book.Price.ToCurrency()}");
                }
            }
        }

        static void ApplyDiscountMenu()
        {
            Console.WriteLine("--- Apply a Discount to All Books ---");
            decimal discountPercentage = ConsoleHelper.ReadDecimal("Enter discount percentage (e.g., 10 for 10%): ", 0);

            if (discountPercentage > 100)
            {
                Console.WriteLine("Cannot apply a discount greater than 100%.");
                return;
            }

            bookService.ApplyCustomRule(book =>
            {
                decimal discountAmount = book.Price * (discountPercentage / 100);
                book.Price -= discountAmount;
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[SUCCESS] A {discountPercentage}% discount has been applied to all books!");
            Console.ResetColor();
        }
    }

}