using System;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using OpenQA.Selenium.Appium.iOS;
using PercyIO.Appium;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OpenQA.Selenium;

namespace csharp_appium__w3c_first_android_test_browserstack
{
  class Android
  {
    static void Main(string[] args)
    {
      //w3c
      AppiumOptions capabilities = new AppiumOptions();
      // Bstack options
      Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
      string USERNAME = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
      string ACCESS_KEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
      string APP_URL = Environment.GetEnvironmentVariable("APP_URL");
      browserstackOptions.Add("userName", USERNAME);
      browserstackOptions.Add("accessKey", ACCESS_KEY);

      //percyOptions
      Dictionary<string, string> percyOtions = new Dictionary<string, string>();
      percyOtions.Add("ignoreErrors", "true");
      percyOtions.Add("enabled", "true");
      // Adding capabilities
      capabilities.AddAdditionalCapability("bstack:options", browserstackOptions);
      capabilities.AddAdditionalCapability("appium:percyOptions", percyOtions);
      // Adding Device
      capabilities.AddAdditionalCapability("platformName", "Android");
      capabilities.AddAdditionalCapability("platformVersion", "9.0");
      capabilities.AddAdditionalCapability("appium:deviceName", "Google Pixel 3");
      // Adding app that was uploaded
      capabilities.AddAdditionalCapability("appium:app", APP_URL);
      // Project details
      capabilities.AddAdditionalCapability("automationName", "Appium");
      capabilities.AddAdditionalCapability("project", "First CSharp W3C Project");
      capabilities.AddAdditionalCapability("build", "CSharp Android");
      capabilities.AddAdditionalCapability("name", "first_test");

      // Initialize the remote Webdriver using BrowserStack remote URL
      // and desired capabilities defined above
      AndroidDriver<AppiumWebElement> driver = new AndroidDriver<AppiumWebElement>(
              new Uri("https://hub-cloud.browserstack.com/wd/hub"), capabilities);

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
