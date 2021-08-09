using NUnit.Framework;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace GitlabSample
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            IWebDriver driver;
            String username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            String accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            String browserstackLocal = Environment.GetEnvironmentVariable("BROWSERSTACK_LOCAL");
            String browserstackIdentifier = Environment.GetEnvironmentVariable("BROWSERSTACK_LOCAL_IDENTIFIER");
            String buidName = Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME");

            OpenQA.Selenium.Chrome.ChromeOptions capability = new OpenQA.Selenium.Chrome.ChromeOptions();
            capability.AddAdditionalCapability("os_version", "10", true);
            capability.AddAdditionalCapability("resolution", "1920x1080", true);
            capability.AddAdditionalCapability("browser", "Chrome", true);
            capability.AddAdditionalCapability("browser_version", "91.0", true);
            capability.AddAdditionalCapability("os", "Windows", true);
            capability.AddAdditionalCapability("name", "BStack-[C_sharp] Sample Test", true); // test name
            capability.AddAdditionalCapability("build", buidName, true); // CI/CD job or build name
            capability.AddAdditionalCapability("browserstack.user", username, true);
            capability.AddAdditionalCapability("browserstack.key", accessKey, true);
            capability.AddAdditionalCapability("browserstack.local", browserstackLocal, true);
            driver = new RemoteWebDriver(
              new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capability
            );
            driver.Navigate().GoToUrl("http://localhost:8888");
            Thread.Sleep(2000);
            driver.Navigate().GoToUrl("https://www.google.com");
            Console.WriteLine(driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("BrowserStack");
            query.Submit();
            Console.WriteLine(driver.Title);
            // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
            if (string.Equals(driver.Title.Substring(0, 12), "BrowserStack"))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Title not matched \"}}");
            }
            driver.Quit();
        }
    }
}