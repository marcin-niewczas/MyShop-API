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
4. [Launch myShop Projects](#launch-myshop-projects)

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
> [!Important]
> For fully functionality, the **myShop API** project must have **[related projects](#related-projects)** running.
> 
> Full instruction for starting **myShop** projects is **[here](#launch-myshop-projects)**.
1. Clone repository
   ```sh
   git clone https://github.com/marcin-niewczas/MyShop-API.git  
   ```
2. Database
   - Windows
     - Nothing to do, but if you wanna run database via **Docker** go to `Mac OS/Linux` step
   - Mac OS/Linux
     - Go to `./src/MyShop.API/appsettings.json` and comment `WindowsConnectionString`, then uncomment `DockerConnectionString`
     - Run **Docker App**
     - In root directory of repository run
       ```sh
       docker-compose up -d
       ```
3. In root directory of repository run
   ```sh
   dotnet run --project ./src/MyShop.API/MyShop.API.csproj --launch-profile MyShop.HTTPS.Development
   ```
## Launch myShop Projects
### Clone repositories
   ```sh
   git clone https://github.com/marcin-niewczas/MyShop-API.git
   git clone https://github.com/marcin-niewczas/MyShop-Angular-Client.git
   git clone https://github.com/marcin-niewczas/MyShop-Pay.git
   ```

### myShop API
1. Go to root folder of **myShop API** repository
   ```sh
   cd MyShop-API
   ```
2. Database
   - Windows
     - Nothing to do, but if you wanna run database via **Docker** go to `Mac OS/Linux` step
   - Mac OS/Linux
     - Go to `./src/MyShop.API/appsettings.json` and comment `WindowsConnectionString`, then uncomment `DockerConnectionString`
     - Run **Docker App**
     - In root directory of repository run
       ```sh
       docker-compose up -d
       ```
3. In root directory of repository run
   ```sh
   dotnet run --project ./src/MyShop.API/MyShop.API.csproj --launch-profile MyShop.HTTPS.Development
   ```
### myShop Angular Client
1. Go to root folder of **myShop Angular Client** repository
   ```sh
   cd ../MyShop-Angular-Client
   ```
2. In root folder of repository install NPM packages
   ```sh
   npm install
   ```
3. Run application
   ```sh
   ng serve
   ```

### myShop Pay
1. Go to root folder of **myShop Pay** repository
   ```sh
   cd ../MyShop-Pay
   ```
3. Database
   - Windows
     - Nothing to do, but if you wanna run database via **Docker** go to `Mac OS/Linux` step
   - Mac OS/Linux
     - Go to `./MyShopPay/appsettings.json` and comment `WindowsConnectionString`, then uncomment `DockerConnectionString`     
4. In root directory of repository run
   ```sh
   dotnet run --project ./MyShopPay/MyShopPay.csproj --launch-profile https
   ```
