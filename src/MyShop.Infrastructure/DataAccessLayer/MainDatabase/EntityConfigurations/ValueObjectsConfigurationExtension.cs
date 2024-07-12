using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.ValueObjects.Categories;
using MyShop.Core.ValueObjects.MainPageSections;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Core.ValueObjects.Photos;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Core.ValueObjects.ProductReviews;
using MyShop.Core.ValueObjects.Products;
using MyShop.Core.ValueObjects.Shared;
using MyShop.Core.ValueObjects.ShoppingCarts;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations;
internal static class ValueObjectsConfigurationExtension
{
    #region Shared

    public static PropertyBuilder<FirstName> HasFirstNameConfiguration(this PropertyBuilder<FirstName> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new FirstName(v))
            .HasMaxLength(FirstName.MaxLength)
            .IsRequired();

    public static PropertyBuilder<LastName> HasLastNameConfiguration(this PropertyBuilder<LastName> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new LastName(v))
            .HasMaxLength(LastName.MaxLength)
            .IsRequired();

    public static PropertyBuilder<Email> HasEmailConfiguration(this PropertyBuilder<Email> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new Email(v))
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

    public static PropertyBuilder<StreetName> HasStreetNameConfiguration(this PropertyBuilder<StreetName> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new StreetName(v))
            .HasMaxLength(StreetName.MaxLength)
            .IsRequired();

    public static PropertyBuilder<BuildingNumber> HasBuildingNumberConfiguration(this PropertyBuilder<BuildingNumber> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new BuildingNumber(v))
            .HasMaxLength(BuildingNumber.MaxLength)
            .IsRequired();

    public static PropertyBuilder<ApartmentNumber> HasApartmentNumberConfiguration(this PropertyBuilder<ApartmentNumber> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new ApartmentNumber(v))
            .HasMaxLength(ApartmentNumber.MaxLength)
            .IsRequired(false);

    public static PropertyBuilder<ZipCode> HasZipCodeConfiguration(this PropertyBuilder<ZipCode> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new ZipCode(v))
            .HasMaxLength(ZipCode.MaxLength)
            .IsRequired();

    public static PropertyBuilder<City> HasCityConfiguration(this PropertyBuilder<City> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new City(v))
            .HasMaxLength(City.MaxLength)
            .IsRequired();

    public static PropertyBuilder<Country> HasCountryConfiguration(this PropertyBuilder<Country> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new Country(v))
            .HasMaxLength(Country.MaxLength)
            .IsRequired();

    #endregion

    #region Users

    public static PropertyBuilder<UserRole> HasUserRoleConfiguration(this PropertyBuilder<UserRole> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new UserRole(v))
            .HasAllowedValuesStringMaxLength()
            .IsRequired();

    public static PropertyBuilder<EmployeeRole> HasEmployeeRoleConfiguration(this PropertyBuilder<EmployeeRole> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new EmployeeRole(v))
            .HasAllowedValuesStringMaxLength()
            .IsRequired();

    public static PropertyBuilder<Gender> HasGenderConfiguration(this PropertyBuilder<Gender> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new Gender(v))
            .HasAllowedValuesStringMaxLength()
            .IsRequired();

    public static PropertyBuilder<UserPhoneNumber> HasUserPhoneNumberConfiguration(this PropertyBuilder<UserPhoneNumber> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new UserPhoneNumber(v))
            .HasMaxLength(UserPhoneNumber.MaxLength)
            .IsRequired(false);

    public static PropertyBuilder<UserAddressName> HasUserAddressNameConfiguration(this PropertyBuilder<UserAddressName> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new UserAddressName(v))
            .HasMaxLength(UserAddressName.MaxLength)
            .IsRequired();

    public static PropertyBuilder<DateOfBirth> HasDateOfBirthConfiguration(this PropertyBuilder<DateOfBirth> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.Value, v => new DateOfBirth(v))
            .IsRequired();

    #endregion

    #region ShoppingCartItems

    public static PropertyBuilder<ShoppingCartItemQuantity> HasShoppingCartItemQuantityConfiguration(
        this PropertyBuilder<ShoppingCartItemQuantity> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new ShoppingCartItemQuantity(v))
                .IsRequired();

    #endregion

    #region Products

    public static PropertyBuilder<ProductVariantQuantity> HasProductVariantItemQuantityConfiguration(
        this PropertyBuilder<ProductVariantQuantity> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new ProductVariantQuantity(v))
                .IsRequired();

    public static PropertyBuilder<ProductOptionType> HasProductOptionTypeConfiguration(
        this PropertyBuilder<ProductOptionType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new ProductOptionType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<ProductOptionSubtype> HasProductOptionSubtypeConfiguration(
        this PropertyBuilder<ProductOptionSubtype> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new ProductOptionSubtype(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<ProductOptionSortType> HasProductOptionSortTypeConfiguration(
        this PropertyBuilder<ProductOptionSortType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new ProductOptionSortType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<ProductReviewRate> HasProductReviewRateConfiguration(
        this PropertyBuilder<ProductReviewRate> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new ProductReviewRate(v))
                .IsRequired();

    public static PropertyBuilder<DisplayProductType> HasDisplayProductTypeConfiguration(
        this PropertyBuilder<DisplayProductType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new DisplayProductType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<ProductReviewText> HasProductReviewTextConfiguration(
        this PropertyBuilder<ProductReviewText> propertyBuilder
        ) => propertyBuilder
            .HasConversion(v => v.ToString(), v => new ProductReviewText(v))
            .HasMaxLength(ProductReviewText.MaxLength)
            .IsRequired(false);

    public static PropertyBuilder<ProductDescription> HasProductDescriptionConfiguration(
        this PropertyBuilder<ProductDescription> propertyBuilder
        ) => propertyBuilder
            .HasConversion(v => v.ToString(), v => new ProductDescription(v))
            .HasMaxLength(ProductDescription.MaxLength)
            .IsRequired(false);

    public static PropertyBuilder<ProductName> HasProductNameConfiguration(
       this PropertyBuilder<ProductName> propertyBuilder
       ) => propertyBuilder
               .HasConversion(v => v.ToString(), v => new ProductName(v))
               .HasMaxLength(ProductName.MaxLength)
               .IsRequired();

    public static PropertyBuilder<ProductOptionName> HasProductOptionNameConfiguration(
        this PropertyBuilder<ProductOptionName> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new ProductOptionName(v))
                .HasMaxLength(ProductOptionName.MaxLength)
                .IsRequired();

    public static PropertyBuilder<ProductOptionValue> HasProductOptionValueConfiguration(
        this PropertyBuilder<ProductOptionValue> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new ProductOptionValue(v))
                .HasMaxLength(ProductOptionValue.MaxLength)
                .IsRequired();

    public static PropertyBuilder<ProductOptionPosition> HasProductOptionPositionConfiguration(
        this PropertyBuilder<ProductOptionPosition> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new ProductOptionPosition(v))
                .IsRequired();

    public static PropertyBuilder<CategoryLevel> HasCategoryLevelConfiguration(
        this PropertyBuilder<CategoryLevel> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new CategoryLevel(v))
                .IsRequired();

    public static PropertyBuilder<CategoryName> HasCategoryNameConfiguration(
        this PropertyBuilder<CategoryName> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new CategoryName(v))
                .HasMaxLength(CategoryName.MaxLength)
                .IsRequired();

    public static PropertyBuilder<ProductVariantPrice> HasProductVariantPriceConfiguration(
        this PropertyBuilder<ProductVariantPrice> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new ProductVariantPrice(v))
                .HasPricePrecision()
                .IsRequired();

    public static PropertyBuilder<ProductVariantPhotoItemPosition> HasProductVariantPhotoItemPositionConfiguration(
        this PropertyBuilder<ProductVariantPhotoItemPosition> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new ProductVariantPhotoItemPosition(v))
                .IsRequired();

    #endregion

    #region Notifications

    public static PropertyBuilder<NotificationType> HasNotificationTypeConfiguration(
        this PropertyBuilder<NotificationType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new NotificationType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    #endregion

    #region Orders

    public static PropertyBuilder<OrderPhoneNumber> HasOrderPhoneNumberConfiguration(this PropertyBuilder<OrderPhoneNumber> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ToString(), v => new OrderPhoneNumber(v))
            .HasMaxLength(OrderPhoneNumber.MaxLength)
            .IsRequired();

    public static PropertyBuilder<OrderStatus> HasOrderStatusConfiguration(
        this PropertyBuilder<OrderStatus> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new OrderStatus(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<DeliveryMethod> HasDeliveryMethodConfiguration(
        this PropertyBuilder<DeliveryMethod> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new DeliveryMethod(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<PaymentMethod> HasPaymentMethodConfiguration(
        this PropertyBuilder<PaymentMethod> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new PaymentMethod(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<OrderProductPrice> HasOrderProductPriceConfiguration(
        this PropertyBuilder<OrderProductPrice> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new OrderProductPrice(v))
                .HasPricePrecision()
                .IsRequired();

    public static PropertyBuilder<OrderProductQuantity> HasOrderProductQuantityConfiguration(
        this PropertyBuilder<OrderProductQuantity> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.Value, v => new OrderProductQuantity(v))
                .IsRequired();

    #endregion

    #region Photos

    public static PropertyBuilder<PhotoType> HasPhotoTypeConfiguration(
        this PropertyBuilder<PhotoType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new PhotoType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<PhotoExtension> HasPhotoExtensionConfiguration(
        this PropertyBuilder<PhotoExtension> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new PhotoExtension(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<PhotoContentType> HasPhotoContentTypeConfiguration(
        this PropertyBuilder<PhotoContentType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new PhotoContentType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<PhotoSize> HasPhotoSizeConfiguration(this PropertyBuilder<PhotoSize> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.ValueinKilobytes, v => new PhotoSize(v))
            .HasPrecision(14, 2)
            .IsRequired();

    public static PropertyBuilder<PhotoAlt> HasPhotoAltConfiguration(
       this PropertyBuilder<PhotoAlt> propertyBuilder
       ) => propertyBuilder
           .HasConversion(v => v.ToString(), v => new PhotoAlt(v))
           .HasMaxLength(PhotoAlt.MaxLength)
           .IsRequired();

    public static PropertyBuilder<PhotoName> HasPhotoNameConfiguration(
       this PropertyBuilder<PhotoName> propertyBuilder
       ) => propertyBuilder
           .HasConversion(v => v.ToString(), v => new PhotoName(v))
           .HasMaxLength(PhotoName.MaxLength)
           .IsRequired();

    public static PropertyBuilder<PhotoFilePath> HasPhotoFilePathConfiguration(
       this PropertyBuilder<PhotoFilePath> propertyBuilder
       ) => propertyBuilder
           .HasConversion(v => v.ToString(), v => new PhotoFilePath(v))
           .HasMaxLength(PhotoFilePath.MaxLength)
           .IsRequired();

    #endregion

    #region MainPageSections

    public static PropertyBuilder<MainPageSectionType> HasMainPageSectionTypeConfiguration(
        this PropertyBuilder<MainPageSectionType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new MainPageSectionType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();
    public static PropertyBuilder<WebsiteHeroSectionDisplayType> HasWebsiteHeroSectionDisplayTypeConfiguration(
        this PropertyBuilder<WebsiteHeroSectionDisplayType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new WebsiteHeroSectionDisplayType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<ProductsCarouselSectionType> HasProductCarouselSectionTypeConfiguration(
        this PropertyBuilder<ProductsCarouselSectionType> propertyBuilder
        ) => propertyBuilder
                .HasConversion(v => v.ToString(), v => new ProductsCarouselSectionType(v))
                .HasAllowedValuesStringMaxLength()
                .IsRequired();

    public static PropertyBuilder<MainPageSectionPosition> HasMainPageSectionPositionConfiguration(this PropertyBuilder<MainPageSectionPosition> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.Value, v => new MainPageSectionPosition(v))
            .IsRequired();

    public static PropertyBuilder<WebsiteHeroSectionItemPosition> HasWebsiteHeroSectionItemPositionConfiguration(this PropertyBuilder<WebsiteHeroSectionItemPosition> propertyBuilder)
        => propertyBuilder
            .HasConversion(v => v.Value, v => new WebsiteHeroSectionItemPosition(v))
            .IsRequired(false);

    public static PropertyBuilder<WebsiteHeroSectionLabel> HasWebsiteHeroSectionLabelConfiguration(
       this PropertyBuilder<WebsiteHeroSectionLabel> propertyBuilder
       ) => propertyBuilder
           .HasConversion(v => v.ToString(), v => new WebsiteHeroSectionLabel(v))
           .HasMaxLength(WebsiteHeroSectionLabel.MaxLength)
           .IsRequired();

    public static PropertyBuilder<WebsiteHeroSectionItemTitle> HasWebsiteHeroSectionItemTitleConfiguration(
       this PropertyBuilder<WebsiteHeroSectionItemTitle> propertyBuilder
       ) => propertyBuilder
           .HasConversion(v => v.ToString(), v => new WebsiteHeroSectionItemTitle(v))
           .HasMaxLength(WebsiteHeroSectionItemTitle.MaxLength)
           .IsRequired(false);

    public static PropertyBuilder<WebsiteHeroSectionItemSubtitle> HasWebsiteHeroSectionItemSubtitleConfiguration(
       this PropertyBuilder<WebsiteHeroSectionItemSubtitle> propertyBuilder
       ) => propertyBuilder
           .HasConversion(v => v.ToString(), v => new WebsiteHeroSectionItemSubtitle(v))
           .HasMaxLength(WebsiteHeroSectionItemSubtitle.MaxLength)
           .IsRequired(false);

    public static PropertyBuilder<WebsiteHeroSectionItemRouterLink> HasWebsiteHeroSectionItemRouterLinkConfiguration(
       this PropertyBuilder<WebsiteHeroSectionItemRouterLink> propertyBuilder
       ) => propertyBuilder
           .HasConversion(v => v.ToString(), v => new WebsiteHeroSectionItemRouterLink(v))
           .HasMaxLength(WebsiteHeroSectionItemRouterLink.MaxLength)
           .IsRequired(false);

    #endregion
}
