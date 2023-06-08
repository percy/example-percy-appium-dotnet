using System;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using PercyIO.Appium;
using OpenQA.Selenium.Support.UI;

namespace csharp_appium__w3c_v5_first_android_test_browserstack
{
  class Android
  {
    static void Main(string[] args)
    {
      AppiumOptions capabilities = new AppiumOptions();
      capabilities.App = "<APP_URL>";
      Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
      // Set your BrowserStack access credentials
      browserstackOptions.Add("userName", "<USER>");
      browserstackOptions.Add("accessKey", "<USER_AUTH_KEY>");
      // Percy Options
      Dictionary<string, string> percyOtions = new Dictionary<string, string>();
      percyOtions.Add("ignoreErrors", "true");
      percyOtions.Add("enabled", "true");
      // Adding capabilities
      capabilities.AddAdditionalAppiumOption("bstack:options", browserstackOptions);
      capabilities.AddAdditionalAppiumOption("appium:percyOptions", percyOtions);
      // Specify device and os_version
      capabilities.DeviceName = "Samsung Galaxy M52";
      capabilities.PlatformVersion = "11.0";
      // Specify the platform name
      capabilities.PlatformName = "Android";
      // Percy Options
      capabilities.AddAdditionalAppiumOption("project", "First CSharp JWP Project");
      capabilities.AddAdditionalAppiumOption("build", "CSharp Android");
      capabilities.AddAdditionalAppiumOption("name", "first_test");

      AppiumDriver driver = new AndroidDriver(new Uri("https://hub-cloud.browserstack.com/wd/hub"), capabilities);
      AppPercy appPercy = new AppPercy(driver);

      try
      {
        Thread.Sleep(5000);
      }
      catch (ThreadInterruptedException e)
      {
        Console.WriteLine(e.Message);
      }
      appPercy.Screenshot("First Screenshot");

      var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
      var searchElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(MobileBy.AccessibilityId("Search Wikipedia")));
      searchElement.Click();

      var textInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(MobileBy.Id("org.wikipedia.alpha:id/search_src_text")));
      textInput.SendKeys("Browserstack\n");

      try
      {
        Thread.Sleep(5000);
      }
      catch (ThreadInterruptedException e)
      {
        Console.WriteLine(e.Message);
      }

      appPercy.Screenshot("Second Screenshot");
      driver.Quit();
    }
  }
}
