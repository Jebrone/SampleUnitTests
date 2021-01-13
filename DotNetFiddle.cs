using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleUnitTests
{
    class DotNetFiddle
    {
        private IWebDriver Driver;

        // Base URLs are created as a variable because they may be used more and so they're easy to track and is often static
        private static Uri BASE_URL = new Uri("https://dotnetfiddle.net/");

        // These are examples of different ways to locate an object
        private By RunButton = By.Id("run-button");
        private By OutputBox = By.XPath("//div[@id='output']");
        private By LoaderOverlay = By.CssSelector("div#stats-loader");
        private By NugetSearchBar = By.CssSelector(".nuget-panel input");
        private By NugetPackageList = By.CssSelector(".nuget-packages .package-name");
        private By MenuContainer = By.Id("menu");

        public DotNetFiddle(IWebDriver driver)
        {
            Driver = driver;
        }

        public DotNetFiddle NavigateTo()
        {
            Driver.Navigate().GoToUrl(BASE_URL);

            return this;
        }

        public DotNetFiddle ClickRunButton()
        {
            Driver.FindElement(RunButton).Click();

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(LoaderOverlay));

            return this;
        }

        public String GetOutputText()
        {
            return Driver.FindElement(OutputBox).Text;
        }

        public DotNetFiddle AddNewNugetPackage(String package, String version)
        {
            Driver.FindElement(NugetSearchBar).SendKeys(package);

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

            By NUnitMenuItem = By.XPath($"//a[text()='{package}']");
            wait.Until(ExpectedConditions.ElementIsVisible(NUnitMenuItem)).Click();

            By NUnitMenuItemList = By.XPath($"//a[text()='{package}']/../ul/li/a[contains(@version-name, '{version}')]");
            wait.Until(ExpectedConditions.ElementIsVisible(NUnitMenuItemList)).Click();

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(MenuContainer));

            return this;
        }

        public List<String> GetListOfNugetPackages()
        {
            // There is no functionality inside the website that allows you to verify the Nuget package version
            return new List<String>(Driver.FindElements(NugetPackageList).Select(element => element.Text));
        }
    }
}
