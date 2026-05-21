// PER-8195 Phase 2 — appium-dotnet advanced example.
// Each [Fact] exercises one row of the App Percy / Appium Native matrix.
// See ../matrix.yml for the canonical mapping.
//
// Run against the BrowserStack App Automate hub. Requires AA_USERNAME,
// AA_ACCESS_KEY, APP env vars. See ../README.md.

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
            { "projectName", Environment.GetEnvironmentVariable("PERCY_PROJECT") ?? "Percy Appium .NET Advanced" },
            { "buildName", Environment.GetEnvironmentVariable("PERCY_BUILD") ?? "Advanced Dotnet Appium" },
        });
        caps.AddAdditionalCapability("appium:percyOptions", new Dictionary<string, string>
        {
            { "enabled", "true" }, { "ignoreErrors", "true" },
        });
        caps.AddAdditionalCapability("appium:deviceName", Environment.GetEnvironmentVariable("DEVICE") ?? "Google Pixel 6");
        caps.AddAdditionalCapability("os_version", Environment.GetEnvironmentVariable("OS_VERSION") ?? "12.0");
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
        var opts = new Dictionary<string, object>
        {
            { "device_name", Environment.GetEnvironmentVariable("DEVICE") ?? "Google Pixel 6" },
            { "orientation", "landscape" },
        };
        _percy.Screenshot("Wikipedia Home — landscape", opts);
    }

    [Fact]
    public void ExercisesFullscreenAndBars()
    {
        var opts = new Dictionary<string, object>
        {
            { "fullscreen", true },
            { "status_bar_height", 24 },
            { "nav_bar_height", 0 },
        };
        _percy.Screenshot("Wikipedia Home — fullscreen", opts);
    }

    [Fact]
    public void ExercisesIgnoreRegionsViaXpath()
    {
        var opts = new Dictionary<string, object>
        {
            { "ignore_regions_xpaths", new[] { "//android.widget.TextView[@text=\"Search Wikipedia\"]" } },
        };
        _percy.Screenshot("Wikipedia Home — ignore via xpath", opts);
    }

    [Fact]
    public void ExercisesIgnoreRegionsViaAppiumElement()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        var el = wait.Until(d => d.FindElement(MobileBy.AccessibilityId("Search Wikipedia")));
        var opts = new Dictionary<string, object>
        {
            { "ignore_region_appium_elements", new[] { el } },
        };
        _percy.Screenshot("Wikipedia Home — ignore via appium element", opts);
    }

    [Fact]
    public void ExercisesCustomIgnoreRegions()
    {
        var region = new Dictionary<string, object>
        {
            { "top", 0 }, { "bottom", 100 }, { "left", 0 }, { "right", 300 },
        };
        var opts = new Dictionary<string, object>
        {
            { "custom_ignore_regions", new[] { region } },
        };
        _percy.Screenshot("Wikipedia Home — custom ignore region", opts);
    }

    [Fact]
    public void ExercisesConsiderRegionsViaXpath()
    {
        var opts = new Dictionary<string, object>
        {
            { "consider_regions_xpaths", new[] { "//android.widget.TextView[@text=\"Search Wikipedia\"]" } },
        };
        _percy.Screenshot("Wikipedia Home — consider via xpath", opts);
    }

    [Fact]
    public void ExercisesSyncMode()
    {
        var opts = new Dictionary<string, object> { { "sync", true } };
        _percy.Screenshot("Wikipedia Home — sync", opts);
    }

    [Fact]
    public void ExercisesTestCaseAndLabels()
    {
        var opts = new Dictionary<string, object>
        {
            { "test_case", "home-smoke" }, { "labels", "smoke,appium-dotnet" },
        };
        _percy.Screenshot("Wikipedia Home — test_case + labels", opts);
    }
}
