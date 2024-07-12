using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Core.Models.MainPageSections;
public sealed class WebsiteProductsCarouselSection : MainPageSection
{
    public ProductsCarouselSectionType ProductsCarouselSectionType { get; private set; } = default!;

    public WebsiteProductsCarouselSection(
        ProductsCarouselSectionType productsCarouselSectionType,
        MainPageSectionPosition position
        ) : base(
            MainPageSectionType.WebsiteProductsCarouselSection,
            position
            )
    {
        ProductsCarouselSectionType = productsCarouselSectionType;
    }

    private WebsiteProductsCarouselSection() { }
}
