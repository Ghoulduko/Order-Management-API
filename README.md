Order Management API (Practice Project)

This project is a backend API built to practice and apply SOLID principles and common design patterns in a real-world scenario.

🚀 Overview

The API simulates a simple order management system that includes:

Users
Products
Cart
Orders
Payments
It focuses on structuring a clean, maintainable backend rather than building a full production-ready system.

🧠 Purpose

The main goal of this project is to:

Practice SOLID principles (especially SRP and separation of concerns)
Apply patterns like Repository Pattern, Service Layer, and basic Unit of Work behavior
Understand how to organize business logic vs data access
Work with Entity Framework Core and relational data
⚙️ Tech Stack

ASP.NET Core Web API
Entity Framework Core
SQL Server
AutoMapper
🔑 Important Notes

❌ No authentication or authorization is implemented
A static userId is used from appsettings.json for simplicity
This project is not production-ready — it’s focused purely on learning and structure
🧩 Features

Add/remove/update items in cart
Validate stock before adding items
Create orders from cart
Handle payments linked to orders
Clear cart after ordering
Proper entity relationships (Orders, OrderItems, Payments, Products)
🎯 What I Learned

Designing clean architecture with clear responsibility boundaries
Avoiding business logic inside repositories
Managing EF Core relationships and query composition (Include, IQueryable)
Handling common backend issues (circular references, FK constraints, etc.)
⚠️ Limitations

No authentication (JWT, sessions, etc.)
No concurrency handling
Basic error handling (no advanced domain-level exceptions)
Not optimized for scalability
📌 Summary

This project is a learning-focused backend system built to improve architectural thinking and backend fundamentals, not to serve as a finished product.
