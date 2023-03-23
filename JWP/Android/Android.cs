using System;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using PercyIO.Appium;
using OpenQA.Selenium;

namespace csharp_appium__jwp_first_android_test_browserstack
{
  class Android
  {
    static void Main(string[] args)
    {
      AppiumOptions capabilities = new AppiumOptions();
      // Set your BrowserStack access credentials
      capabilities.AddAdditionalCapability("browserstack.user", "<USER>");
      capabilities.AddAdditionalCapability("browserstack.key", "<USER_AUTH_KEY>");
      // App url we get post uploading in response
      capabilities.AddAdditionalCapability("app", "<APP_URL>");
      // Specify device and os_version
      capabilities.AddAdditionalCapability("device", "Google Pixel 3");
      capabilities.AddAdditionalCapability("os_version", "9.0");
      // Specify the platform name
      capabilities.PlatformName = "Android";
      // Percy Options
      capabilities.AddAdditionalCapability("percy.enabled", "True");
      capabilities.AddAdditionalCapability("percy.ignoreErrors", "True");
      // Set other BrowserStack capabilities
      capabilities.AddAdditionalCapability("project", "First CSharp JWP Project");
      capabilities.AddAdditionalCapability("build", "CSharp Android");
      capabilities.AddAdditionalCapability("name", "first_test");
      // Initialize the remote Webdriver using BrowserStack remote URL
      // and desired capabilities defined above
      AndroidDriver<AndroidElement> driver = new AndroidDriver<AndroidElement>(
              new Uri("http://hub-cloud.browserstack.com/wd/hub"), capabilities);

      // Initialize AppPercy
      AppPercy appPercy = new AppPercy(driver);
      try
      {
        Thread.Sleep(5000);
      }
      catch (ThreadInterruptedException e)
      {
        Console.WriteLine(e.Message);
      }
      // Taking first screenshot
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

      // Second screenshot
      appPercy.Screenshot("Second Screenshot");
      // Invoke driver.quit() after the test is done to indicate that the test is completed.
      driver.Quit();
    }
  }
}
