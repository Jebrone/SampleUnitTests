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

        DotNetFiddle MainPage;
        
        [SetUp]
        public void Setup()
        {
            // Driver is Initialized and can be managed here for initial setup
            // Multiple browsers can be instantiated here
            Driver = new ChromeDriver();
            Driver.Manage().Window.FullScreen();

            MainPage = new DotNetFiddle(Driver);
            MainPage.NavigateTo();
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
            MainPage.ClickRunButton();

            Assert.AreEqual(MainPage.GetOutputText(), "Hello World");
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
            MainPage.AddNewNugetPackage(package, version);

            Assert.IsTrue(MainPage.GetListOfNugetPackages().Contains(package));
        }
    }
}