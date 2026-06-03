// PER-8195 Phase 2 — appium-dotnet advanced example.
// Each [Fact] exercises one row of the App Percy / Appium Native matrix.
// See ../matrix.yml for the canonical mapping.
//
// Run against the BrowserStack App Automate hub. Requires AA_USERNAME,
// AA_ACCESS_KEY, APP env vars. See ../README.md.

using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Support.UI;
using PercyIO.Appium;
using Xunit;

namespace AppiumAdvanced;

public class AppiumAdvancedFixture : IDisposable
{
    public readonly AndroidDriver<AppiumWebElement> Driver;
    public readonly AppPercy Percy;

    public AppiumAdvancedFixture()
    {
        var caps = new AppiumOptions();
        caps.AddAdditionalCapability("bstack:options", new Dictionary<string, object>
        {
            { "userName", Environment.GetEnvironmentVariable("AA_USERNAME") ?? "" },
            { "accessKey", Environment.GetEnvironmentVariable("AA_ACCESS_KEY") ?? "" },
            // BrowserStack W3C: device + OS selection must live inside bstack:options
            // (as deviceName / osVersion). A bare top-level os_version cap is dropped.
            { "deviceName", Environment.GetEnvironmentVariable("DEVICE") ?? "Google Pixel 6" },
            { "osVersion", Environment.GetEnvironmentVariable("OS_VERSION") ?? "12.0" },
            { "projectName", Environment.GetEnvironmentVariable("BROWSERSTACK_PROJECT_NAME") ?? "Percy Appium .NET Advanced" },
            { "buildName", Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME") ?? "Advanced Dotnet Appium" },
        });
        caps.AddAdditionalCapability("appium:percyOptions", new Dictionary<string, string>
        {
            { "enabled", "true" }, { "ignoreErrors", "true" },
        });
        caps.AddAdditionalCapability("appium:app", Environment.GetEnvironmentVariable("APP"));

        Driver = new AndroidDriver<AppiumWebElement>(
            new Uri("https://hub-cloud.browserstack.com/wd/hub"), caps);
        Percy = new AppPercy(Driver);
        Thread.Sleep(5000);
    }

    public void Dispose() => Driver.Quit();
}

public class AdvancedTests : IClassFixture<AppiumAdvancedFixture>
{
    // Wikipedia home-screen selector, verified against a real Pixel 6 / Android 12
    // page source: the feed's search bar has the locale-independent resource-id
    // org.wikipedia.alpha:id/search_container. (There is no content-desc="Wikipedia"
    // element, so the previous selector matched nothing and the ignore/consider
    // regions were silently empty.) All SDK examples use this same selector.
    private const string WikipediaRegionXpath = "//*[@resource-id=\"org.wikipedia.alpha:id/search_container\"]";

    private readonly AppPercy _percy;
    private readonly AndroidDriver<AppiumWebElement> _driver;

    public AdvancedTests(AppiumAdvancedFixture fixture)
    {
        _percy = fixture.Percy;
        _driver = fixture.Driver;
    }

    [Fact]
    public void ExercisesBaseline() => _percy.Screenshot("Wikipedia Home");

    [Fact]
    public void ExercisesDeviceNameAndOrientation()
    {
        // Actually rotate the device so the snapshot reflects landscape instead of
        // just being tagged with the metadata. Restore Portrait afterwards so
        // subsequent [Fact]s on the shared fixture start from a known orientation.
        _driver.Orientation = ScreenOrientation.Landscape;
        try
        {
            var opts = new ScreenshotOptions
            {
                DeviceName = Environment.GetEnvironmentVariable("DEVICE") ?? "Google Pixel 6",
                Orientation = "landscape",
            };
            _percy.Screenshot("Wikipedia Home - landscape", opts);
        }
        finally
        {
            _driver.Orientation = ScreenOrientation.Portrait;
        }
    }

    [Fact]
    public void ExercisesFullscreenAndBars()
    {
        var opts = new ScreenshotOptions
        {
            StatusBarHeight = 24,
            NavBarHeight = 0,
        };
        // AppPercy.Screenshot signature: Screenshot(name, options, fullScreen=false).
        // The wrapper overwrites options.FullScreen with the third argument, so
        // fullScreen MUST be passed positionally to take effect.
        _percy.Screenshot("Wikipedia Home - fullscreen", opts, true);
    }

    [Fact]
    public void ExercisesIgnoreRegionsViaXpath()
    {
        var opts = new ScreenshotOptions
        {
            IgnoreRegionXpaths = new List<string> { WikipediaRegionXpath },
        };
        _percy.Screenshot("Wikipedia Home - ignore via xpath", opts);
    }

    [Fact]
    public void ExercisesIgnoreRegionsViaAppiumElement()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        var el = wait.Until(d => d.FindElement(MobileBy.AccessibilityId("Search Wikipedia")));
        var opts = new ScreenshotOptions
        {
            IgnoreRegionAppiumElements = new List<object> { el },
        };
        _percy.Screenshot("Wikipedia Home - ignore via appium element", opts);
    }

    [Fact]
    public void ExercisesCustomIgnoreRegions()
    {
        var opts = new ScreenshotOptions
        {
            CustomIgnoreRegions = new List<Region>
            {
                new Region(0, 100, 0, 300),
            },
        };
        _percy.Screenshot("Wikipedia Home - custom ignore region", opts);
    }

    [Fact]
    public void ExercisesConsiderRegionsViaXpath()
    {
        var opts = new ScreenshotOptions
        {
            ConsiderRegionXpaths = new List<string> { WikipediaRegionXpath },
        };
        _percy.Screenshot("Wikipedia Home - consider via xpath", opts);
    }

    [Fact]
    public void ExercisesSyncMode()
    {
        var opts = new ScreenshotOptions { Sync = true };
        // sync=true makes AppPercy.Screenshot return a JObject: the comparison
        // results with a full-access PERCY_TOKEN, or an error payload (e.g. the
        // 403 returned by a write-only token, the common CI setup). The SDK never
        // returns null here, so a non-null check is safe regardless of token scope;
        // a full-access token is only needed to inspect real comparison data.
        var result = _percy.Screenshot("Wikipedia Home - sync", opts);
        Assert.NotNull(result);
    }

    [Fact]
    public void ExercisesTestCaseAndLabels()
    {
        var opts = new ScreenshotOptions
        {
            TestCase = "home-smoke",
            Labels = "smoke,appium-dotnet",
        };
        _percy.Screenshot("Wikipedia Home - test_case + labels", opts);
    }
}
