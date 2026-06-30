# 📚 BookStore Console Application

A robust, Clean Code, C# console application built for managing a bookstore. This project demonstrates advanced Object-Oriented Programming (OOP) concepts, Generic Repositories, and SOLID principles.

## ✨ Features Implemented
* [cite_start]**Domain Modeling**: Abstract base classes and inheritance (`PaperbackBook`, `EBook`) to support future formats without modifying existing code (Open/Closed Principle)[cite: 11].
* [cite_start]**Generic Repository**: An in-memory data store using `IRepository<T>` to handle Books, Customers, and Purchases seamlessly[cite: 24].
* [cite_start]**Business Logic & Validation**: Strict input validation preventing crashes, and robust logic preventing the sale of out-of-stock books[cite: 10, 13, 19].
* [cite_start]**Delegates & Events**: Notifies the system automatically when a book runs completely out of stock[cite: 26].
* [cite_start]**Custom Rules (Func/Action)**: Ability to apply custom rules, such as store-wide discounts, across all books[cite: 25].
* [cite_start]**Bonus - Async File I/O**: Persists data to a `JSON` file safely upon exit and reloads it upon startup[cite: 28].
* [cite_start]**Bonus - Concurrency Handling**: Thread-safe stock reduction using `lock` to handle multiple simultaneous purchases[cite: 29].

## 🚀 How to Run the Application
1. [cite_start]Ensure you have the **.NET 8 SDK** installed.
2. Clone this repository to your local machine.
3. Open the solution in Visual Studio 2022 or open a terminal in the project folder.
4. Run the command: `dotnet run`
5. Follow the on-screen interactive menu.

## 📸 Screenshots

**1. Main Menu:**
<img width="1366" height="728" alt="menu" src="https://github.com/user-attachments/assets/364d51bb-ccb8-40dc-85e5-ffc1f01eefae" />

