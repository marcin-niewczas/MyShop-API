using MyShop.Core.HelperModels;

namespace MyShop.Application.Mappings;
internal static class UserTokenMappingExtension
{
    public static string GetFullName(this Browser browser)
        => browser switch
        {
            Browser.Unknown => "Unknown",
            Browser.Firefox => "Mozilla Firefox",
            Browser.Chrome => "Google Chrome",
            Browser.Edge => "Microsoft Edge",
            Browser.Safari => "Safari",
            Browser.Opera => "Opera",
            _ => throw new ArgumentException(browser.ToString(), nameof(browser)),
        };
}
