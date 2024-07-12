namespace MyShop.Core.HelperModels;
public enum OS
{
    Unknown = 0,
    Windows,
    Unix,
    Linux,
    MacOS,
    iOS,
    Android
}

public enum Browser
{
    Unknown = 0,
    Firefox,
    Chrome,
    Edge,
    Safari,
    Opera
}

public sealed record PlatformInfo
{
    public OS OperatingSystem { get; private set; } = default;
    public Browser Browser { get; private set; } = default;
    public string? BrowserVersion { get; private set; }
    public bool IsMobile { get; private set; }

    public PlatformInfo(string userAgent)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userAgent, nameof(userAgent));

        userAgent = userAgent.ToLower();

        var userAgentArray = userAgent
            .Replace(")", "")
            .Replace("(", "")
            .Replace(";", "")
            .Split(' ');

        SetBrowser(userAgentArray);
        SetOperatingSystem(userAgent);
        SetIsMobile(userAgent);

    }

    private void SetBrowser(string[] userAgent)
    {
        if (IsEdgeBrowser(userAgent))
        {
            return;
        }

        if (IsOperaBrowser(userAgent))
        {
            return;
        }

        if (IsFirefoxBrowser(userAgent))
        {
            return;
        }

        if (IsChromeBrowser(userAgent))
        {
            return;
        }

        if (IsSafariBrowser(userAgent))
        {
            return;
        }

    }

    private bool IsEdgeBrowser(string[] userAgentArray)
    {
        var browser = userAgentArray.FirstOrDefault(x => x.Contains("edg"));

        if (browser is not null)
        {
            Browser = Browser.Edge;

            var splittedString = browser.Split('/');

            if (splittedString.Length > 1)
            {
                var index = splittedString[1].IndexOf('.');

                if (index != -1)
                {
                    BrowserVersion = splittedString[1][..index];
                }
            }

            return true;
        }

        return false;
    }

    private bool IsOperaBrowser(string[] userAgentArray)
    {
        var browser = userAgentArray.FirstOrDefault(x => x.Contains("opr"));

        if (browser is not null)
        {
            Browser = Browser.Opera;

            var splittedString = browser.Split('/');

            if (splittedString.Length > 1)
            {
                var index = splittedString[1].IndexOf('.');

                if (index != -1)
                {
                    BrowserVersion = splittedString[1][..index];
                }
            }

            return true;
        }

        browser = userAgentArray.FirstOrDefault(x => x.Contains("presto"));

        if (browser is not null)
        {
            Browser = Browser.Opera;

            var browserVersion = userAgentArray.FirstOrDefault(x => x.Contains("opera"));

            if (browserVersion is not null)
            {
                var splittedString = browserVersion.Split('/');

                if (splittedString.Length > 1)
                {
                    var index = splittedString[1].IndexOf('.');

                    if (index != -1)
                    {
                        BrowserVersion = splittedString[1][..index];
                    }
                }
            }

            return true;
        }

        return false;
    }

    private bool IsFirefoxBrowser(string[] userAgentArray)
    {
        var browser = userAgentArray.FirstOrDefault(x => x.Contains("firefox"));

        if (browser is not null)
        {
            Browser = Browser.Firefox;

            var splittedString = browser.Split('/');

            if (splittedString.Length > 1)
            {
                var index = splittedString[1].IndexOf('.');

                if (index != -1)
                {
                    BrowserVersion = splittedString[1][..index];
                }
            }

            return true;
        }

        return false;
    }

    private bool IsSafariBrowser(string[] userAgentArray)
    {
        if (userAgentArray.Any(x => x.Contains("windows") || x.Contains("unix") || x.Contains("android")))
        {
            return false;
        }

        var browser = userAgentArray.FirstOrDefault(x => x.Contains("safari"));

        if (browser is not null)
        {
            Browser = Browser.Safari;

            var splittedString = browser.Split('/');

            if (splittedString.Length > 1)
            {
                var index = splittedString[1].IndexOf('.');

                if (index != -1)
                {
                    BrowserVersion = splittedString[1][..index];
                }
            }

            return true;
        }

        return false;
    }

    private bool IsChromeBrowser(string[] userAgentArray)
    {
        var browser = userAgentArray.FirstOrDefault(x => x.Contains("chrome"));

        if (browser is not null)
        {
            Browser = Browser.Chrome;

            var splittedString = browser.Split('/');

            if (splittedString.Length > 1)
            {
                var index = splittedString[1].IndexOf('.');

                if (index != -1)
                {
                    BrowserVersion = splittedString[1][..index];
                }
            }

            return true;
        }

        return false;
    }

    private void SetOperatingSystem(string userAgent)
    {
        if (userAgent.Contains("windows"))
        {
            OperatingSystem = OS.Windows;
            return;
        }

        if (userAgent.Contains("android"))
        {
            OperatingSystem = OS.Android;
            return;
        }

        if (userAgent.Contains("iphone") || userAgent.Contains("ipad"))
        {
            OperatingSystem = OS.iOS;
            return;
        }

        if (userAgent.Contains("mac os x") || userAgent.Contains("macintosh"))
        {
            OperatingSystem = OS.MacOS;
            return;
        }

        if (userAgent.Contains("linux"))
        {
            OperatingSystem = OS.Linux;
            return;
        }

        if (userAgent.Contains("unix"))
        {
            OperatingSystem = OS.Unix;
            return;
        }
    }

    private void SetIsMobile(string userAgent)
        => IsMobile = userAgent.Contains("mobile");

}
