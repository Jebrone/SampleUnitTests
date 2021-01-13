using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleUnitTests
{
    public class UnitTests
    {
        // Driver is normally created for Page models and initialized in a setup
        private IWebDriver Driver;

        // Base URLs are created as a variable because they may be used more and so they're easy to track and is often static
        private static Uri BASE_URL = new Uri("https://dotnetfiddle.net/");

        // Locators are held in the page objects and are often hidden so that only actions can be used on them
        // These are examples of different ways to locate an object
        private By RunButton = By.Id("run-button");
        private By OutputBox = By.XPath("//div[@id='output']");
        private By LoaderOverlay = By.CssSelector("div#stats-loader");
        private By NugetSearchBar = By.CssSelector(".nuget-panel input");
        private By NugetPackageList = By.CssSelector(".nuget-packages .package-name");
        private By MenuContainer = By.Id("menu");
        
        [SetUp]
        public void Setup()
        {
            // Driver is Initialized and can be managed here for initial setup
            // Multiple browsers can be instantiated here
            Driver = new ChromeDriver();
            //Driver.Manage().Window.FullScreen();

            // The driver often is passed into a page object model then the page can navigate to the URL
            Driver.Navigate().GoToUrl(BASE_URL);
        }

        [TearDown]
        public void TearDown()
        {
            // The Driver needs to be disposed of after every use or there will be drivers left in memory
            // If a driver is not properly disposed of, failures may occur such as not being able to launch a driver
            Driver.Dispose();
        }

        /// <summary>
        /// Test 1:
        /// Click “Run” button
        /// Check Output window(“Hello World” text is expected)
        /// </summary>
        [Test]
        [Description("Test 1: Test Output")]
        public void Test1()
        {
            Driver.FindElement(RunButton).Click();

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(LoaderOverlay));

            String output = Driver.FindElement(OutputBox).Text;

            Assert.AreEqual(output, "Hello World");
        }

        /// <summary>
        /// Test 2 (If your first name starts with letter “A-B-C-D-E”):
        /// Select NuGet Packages: nUnit(3.12.0)
        /// Check that nUnit package is added
        /// </summary>
        [TestCase("NUnit", "3.12.0")]
        [Description("Test 2: Check Nuget Selection")]
        public void Test2(String package, String version)
        {
            Driver.FindElement(NugetSearchBar).SendKeys(package);

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

            By NUnitMenuItem = By.XPath($"//a[text()='{package}']");
            wait.Until(ExpectedConditions.ElementIsVisible(NUnitMenuItem)).Click();

            By NUnitMenuItemList = By.XPath($"//a[text()='{package}']/../ul/li/a[contains(@version-name, '{version}')]");
            wait.Until(ExpectedConditions.ElementIsVisible(NUnitMenuItemList)).Click();

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(MenuContainer));

            // There is no functionality inside the website that allows you to verify the Nuget package version
            List<String> nugetList = new List<String>(Driver.FindElements(NugetPackageList).Select(element => element.Text));

            Assert.IsTrue(nugetList.Contains(package));
        }
    }
}