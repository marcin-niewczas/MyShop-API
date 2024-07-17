<div align="center"> 
  <h3>myShop API</h3>
  <h6>E-Commerce | Management Panel<h6>
</div>

## Table of Contents
1. [About This Project](#about-this-project)
    - [Features](#features)
    - [Built with](#built-with)
2. [Related Projects](#related-projects)
3. [Getting Started](#getting-started)

## About This Project
The main goal of **myShop API** project was to create a flexible E-Commerce API platform, that can be easily managed. The project has been built as Clean Architecture REST API and developed with CQRS (Command Query Responsibility Segregation), Unit of Work and Event Driven Architecture Patterns.

### Clients
- **[myShop Angular Client](https://github.com/marcin-niewczas/MyShop-Angular-Client)**

### Features
- **For the entire platform**
  - Real time notifications with SignalR,
  - User Roles
    - Guest
    - Customer
    - Employee
      - Seller
      - Manager
      - Admin
      - SuperAdmin
  - Message broker built with Channel
    - CRON Jobs,
    - Async Tasks (e.g. Payment Processing with **[myShop Pay](https://github.com/marcin-niewczas/MyShop-Pay)** )
- **E-Commerce**
  - Customized Home Page
  - Products List
    - Advanced Filtering (by Variant Options, Detail Options, Price)
    - Advanced Sorting (e.g. Top Rated, Bestsellers, Most Reviewed Products)
    - Group Products By All Variant Options or Main Variant Option
  - Product Detail (Single or Product Variants)
  - Product Reviews
  - Favorites
  - Quick Add Product
  - Shopping Basket
  - Ordering Wizard
  - Create Order by Guest
- **Management Panel**
  - Dashboard
  - E-Commerce Home Page Management
  - Categories Management
  - Product Options Management
  - Product, Product Reviews, Product Variants, Product Photos Management
  - Orders Management
- **Account**
  - Basic Account Information Management
  - User Addresses Management
  - User Orders Management
  - Notifications List
  - Favorite Products Management
  - Account Security Management
- **Authenticates**
  - JWT Authenticates with Refresh Tokens
  - Possibility of sessions on various devices
  - Auth
  - Sign In
  - Sign Up
### Built with
* .NET 8
* ASP.NET Core Web API
* ASP.NET Core Authentication JWT Bearer
* Entity Framework Core (MS SQL)
* ASP.NET Core SignalR
* Swagger
* Quartz.NET

## Related Projects
* **[myShop Angular Client](https://github.com/marcin-niewczas/MyShop-Angular-Client)**
* **[myShop Pay](https://github.com/marcin-niewczas/MyShop-Pay)**

## Getting Started
