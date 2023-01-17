using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Support.UI;
using PercyIO.Appium;

namespace csharp_appium__w3c_first_ios_test_browserstack
{
  public class Ios
  {
    static void Main(string[] args)
    {
      //w3c
      AppiumOptions capabilities = new AppiumOptions();
      // Bstack options
      Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
      browserstackOptions.Add("userName", "<USER>");
      browserstackOptions.Add("accessKey", "<USER_AUTH_KEY>");

      // Percy options
      Dictionary<string, string> percyOtions = new Dictionary<string, string>();
      percyOtions.Add("ignoreErrors", "false");
      percyOtions.Add("enabled", "false");
      // Adding capabilities
      capabilities.AddAdditionalCapability("bstack:options", browserstackOptions);
      capabilities.AddAdditionalCapability("appium:percyOptions", percyOtions);
      // Adding Device
      capabilities.AddAdditionalCapability("os_version", "16");
      capabilities.AddAdditionalCapability("appium:deviceName", "iPhone 14");
      // Adding app that was uploaded
      capabilities.AddAdditionalCapability("appium:app", "<APP_URL>");
      // Project details
      capabilities.AddAdditionalCapability("project", "First CSharp W3C Project");
      capabilities.AddAdditionalCapability("build", "CSharp IOS");
      capabilities.AddAdditionalCapability("name", "first_test");

      // Initialize the remote Webdriver using BrowserStack remote URL
      // and desired capabilities defined above
      IOSDriver<IOSElement> driver = new IOSDriver<IOSElement>(
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
