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
      percyOtions.Add("ignoreErrors", "true");
      percyOtions.Add("enabled", "true");
      // Adding capabilities
      capabilities.AddAdditionalAppiumOption("bstack:options", browserstackOptions);
      capabilities.AddAdditionalAppiumOption("appium:percyOptions", percyOtions);
      // Adding Device
      capabilities.App = "<APP_URL>";
      capabilities.AddAdditionalAppiumOption("bstack:options", browserstackOptions);
      capabilities.AddAdditionalAppiumOption("appium:percyOptions", percyOtions);
      // Adding Device
      capabilities.DeviceName = "iPhone 14";
      capabilities.PlatformVersion = "16";
      capabilities.AddAdditionalAppiumOption("project", "First CSharp W3C Project");
      capabilities.AddAdditionalAppiumOption("build", "CSharp IOS");
      capabilities.AddAdditionalAppiumOption("name", "first_test");

      // Initialize the remote Webdriver using BrowserStack remote URL
      // and desired capabilities defined above
      AppiumDriver driver = new IOSDriver(
              new Uri("https://hub-cloud.browserstack.com/wd/hub"), capabilities);

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
