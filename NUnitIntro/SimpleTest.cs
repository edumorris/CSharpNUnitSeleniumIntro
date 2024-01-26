using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace NUnitIntro
{
    
    public class SimpleApplicationRunner
    {
        private static IWebDriver driver;
        private const string SearchPhrase = "selenium";

        [OneTimeSetUp]
        public static void setUp()
        {
            
            new DriverManager().SetUpDriver(new ChromeConfig());

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--remote-allow-origins=*");
            
            driver = new ChromeDriver(chromeOptions);
            
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }
        
        [Test]
        public void GithubTest()
        {

            driver.Navigate().GoToUrl("https://github.com/"); // Open URL

            driver.FindElement(By.XPath("//span[(.)='Search or jump to...']")).Click();
            
            IWebElement searchInput = driver.FindElement(By.Id("query-builder-test"));
            searchInput.SendKeys(SearchPhrase);
            searchInput.SendKeys(Keys.Enter);

            Thread.Sleep(TimeSpan.FromSeconds(30));
        
            IList<string> actualItems = driver.FindElements(By.XPath("//a/span[contains(@class, 'search-match')]"))
                .Select(item => item.Text.ToLower())
                .ToList();
            
            // foreach (var actualItem in actualItems)
            // {
            //     Assert.That(actualItems.Contains("invalid search phrase"));
            // }
            
            Assert.That(actualItems.All(item => item.ToLower().Contains(SearchPhrase)));

        }

        [OneTimeTearDown]
        public static void tearDown()
        {
            
            driver.Quit();
            
        }
        
    }
    
}