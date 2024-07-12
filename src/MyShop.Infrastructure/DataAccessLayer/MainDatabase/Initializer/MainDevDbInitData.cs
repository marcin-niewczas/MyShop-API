using MyShop.Core.Abstractions;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Initializer;
internal static class MainDevDbInitData
{
    public static IReadOnlyCollection<Employee> GetEmployess(IPasswordManager passwordManager)
        => new Employee[]
        {
            new(
                "Example",
                EmployeeRole.SuperAdmin,
                "super-admin@myshop.com",
                Gender.Female,
                EmployeeRole.SuperAdmin,
                passwordManager.SecurePassword("myShopProject1#"),
                new DateOnly(1900, 1, 1),
                new UserPhoneNumber("+1111111111")
                ),
            new(
                "Example",
                EmployeeRole.Admin,
                "admin@myshop.com",
                Gender.Male,
                EmployeeRole.Admin,
                passwordManager.SecurePassword("myShopProject1#"),
                new DateOnly(1900, 1, 1),
                new UserPhoneNumber("+2222222222")
                ),
            new(
                "Example",
                EmployeeRole.Manager,
                "manager@myshop.com",
                Gender.Female,
                EmployeeRole.Manager,
                passwordManager.SecurePassword("myShopProject1#"),
                new DateOnly(1900, 1, 1),
                new UserPhoneNumber("+3333333333")
                ),
            new(
                "Example",
                EmployeeRole.Seller,
                "seller@myshop.com",
                Gender.Male,
                EmployeeRole.Seller,
                passwordManager.SecurePassword("myShopProject1#"),
                new DateOnly(1900, 1, 1),
                new UserPhoneNumber("+4444444444")
                )
        }.AsReadOnly();

    public static IReadOnlyCollection<Customer> GetCustomers(IPasswordManager passwordManager)
        => new Customer[]
        {
            new(
               "Example",
               nameof(Customer),
               "customer@myshop.com",
               Gender.Female,
               passwordManager.SecurePassword("myShopProject1#"),
               new DateOnly(1900, 1, 1),
               new UserPhoneNumber("+5555555555")
               ),

            new(
               "Example",
               $"{nameof(Customer)}2",
               "customer2@myshop.com",
               Gender.Male,
               passwordManager.SecurePassword("myShopProject1#"),
               new DateOnly(1900, 1, 1),
               new UserPhoneNumber("+6666666666")
               ),
        }.AsReadOnly();

    public static IReadOnlyCollection<Category> GetCategories()
    {
        var womenCategory = new Category("Women");
        var womensClothesCategory = new Category("Clothes", womenCategory);
        var womensTShirtsCategory = new Category("T-Shirts", womensClothesCategory);

        var menCategory = new Category("Men");
        var mensClothesCategory = new Category("Clothes", menCategory);
        var mensTShirtsCategory = new Category("T-Shirts", mensClothesCategory);

        var electronicCategory = new Category("Electronic");
        var electronicsPhonesCategory = new Category("Phones", electronicCategory);
        var electronicsSmartphonesCategory = new Category("Smartphones", electronicsPhonesCategory);
        var electronicsWearablesCategory = new Category("Wearables", electronicCategory);
        var electronicsSmartbandsCategory = new Category("Smartbands", electronicsWearablesCategory);
        var electronicsSmartwatchesCategory = new Category("Smartwatches", electronicsWearablesCategory);

        return new Category[]
        {
            womenCategory,
            womensClothesCategory,
            womensTShirtsCategory,
            menCategory,
            mensClothesCategory,
            mensTShirtsCategory,
            electronicCategory,
            electronicsPhonesCategory,
            electronicsSmartphonesCategory,
            electronicsWearablesCategory,
            electronicsSmartbandsCategory,
            electronicsSmartwatchesCategory
        }.AsReadOnly();
    }

    public static IReadOnlyCollection<ProductDetailOption> GetProductDetailOptions()
        => new ProductDetailOption[]
        {
            new(
                "Brand",
                ProductOptionSubtype.Main,
                ProductOptionSortType.Alphabetically,
                ["Adidas", "Nike", "Google", "Apple", "Samsung"]
                ),
            new(
                "Fit",
                ProductOptionSubtype.Additional,
                ProductOptionSortType.Custom,
                ["Slim Fit", "Muscle Fit", "Regular Fit", "Oversized Fit"]
                ),
            new(
                "Waterproof",
                ProductOptionSubtype.Additional,
                ProductOptionSortType.Custom,
                ["Yes", "No"]
                ),
            new(
                "Bluetooth",
                ProductOptionSubtype.Additional,
                ProductOptionSortType.Custom,
                [ "No", "3", "4", "4.1", "4.2", "5", "5.1", "5.2", "5.3"]
                ),
            new(
                "Release Date",
                ProductOptionSubtype.Additional,
                ProductOptionSortType.Custom,
                [ "Q1 2023", "Q2 2023", "Q3 2023", "Q4 2023", "Q1 2024", "Q2 2024" ]
                ),
        }.AsReadOnly();

    public static IReadOnlyCollection<ProductVariantOption> GetProductVariantOptions()
        => new ProductVariantOption[]
        {
            new(
                "Color",
                ProductOptionSubtype.Main,
                ProductOptionSortType.Alphabetically,
                ["Green", "Red", "Black", "Blue", "White", "Purple"]
                ),
            new(
                "Size",
                ProductOptionSubtype.Additional,
                ProductOptionSortType.Custom,
                ["XS", "S", "M", "L", "XL", "XXL"]
                ),
            new(
                "Data Storage",
                ProductOptionSubtype.Additional,
                ProductOptionSortType.Custom,
                ["16GB", "32GB", "64GB", "128GB", "256GB", "512GB", "1TB", "2TB"]
                ),
            new(
                "RAM",
                ProductOptionSubtype.Additional,
                ProductOptionSortType.Custom,
                ["256MB", "512MB", "1GB", "2GB", "4GB", "8GB", "16GB", "32GB", "64GB", "128GB"]
                ),

        }.AsReadOnly();
}
