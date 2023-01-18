using System;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using PercyIO.Appium;
using OpenQA.Selenium.Support.UI;

namespace csharp_appium__jwp_first_ios_test_browserstack
{
  class Ios
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
      capabilities.AddAdditionalCapability("os_version", "16");
      capabilities.AddAdditionalCapability("device", "iPhone 12 Pro");

      // Percy Options
      capabilities.AddAdditionalCapability("percy.enabled", "True");
      capabilities.AddAdditionalCapability("percy.ignoreErrors", "True");
      // Project details
      capabilities.AddAdditionalCapability("project", "First CSharp JWP project");
      capabilities.AddAdditionalCapability("build", "CSharp IOS");
      capabilities.AddAdditionalCapability("name", "first_test");
      // Initialize the remote Webdriver using BrowserStack remote URL
      // and desired capabilities defined above
      IOSDriver<IOSElement> driver = new IOSDriver<IOSElement>(
              new Uri("http://hub-cloud.browserstack.com/wd/hub"), capabilities);

      // // Initialize AppPercy
      AppPercy appPercy = new AppPercy(driver);
      try
      {
        Thread.Sleep(5000);
      }
      catch (ThreadInterruptedException e)
      {
        Console.WriteLine(e.Message);
      }
      //Taking first screenshot
      appPercy.Screenshot("First Screenshot");

      var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
      var searchElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(MobileBy.AccessibilityId("Text Button")));
      searchElement.Click();

      var textInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(MobileBy.Id("Text Input")));
      textInput.SendKeys("hello@percy.io\n");

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
