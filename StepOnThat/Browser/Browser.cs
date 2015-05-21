using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace StepOnThat.Browser
{
    public class Browser
    {
        private static Browser browser;

        public static int DefailtWaitInSeconds = 5;

        private IWebDriver driver;

        public Browser()
        {
            driver = new ChromeDriver(new ChromeOptions());
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(DefailtWaitInSeconds));
        }

        public static Browser Current
        {
            get { return browser ?? (browser = new Browser()); }
        }

        public Browser GoTo(string url)
        {
            driver.Navigate().GoToUrl(url);
            return this;
        }

        public Browser Back()
        {
            driver.Navigate().Back();
            return this;
        }

        public Browser Forward()
        {
            driver.Navigate().Forward();
            return this;
        }

        public Browser Click(string cssOrXpathSelector)
        {
            var elem = driver.FindElement(GetSelector(cssOrXpathSelector));

            elem.Click();
            return this;
        }

        private static By GetSelector(string cssOrXpathSelector)
        {
            if (cssOrXpathSelector.Contains("/") || cssOrXpathSelector.Contains("::"))
                return By.XPath(cssOrXpathSelector);

            return By.CssSelector(cssOrXpathSelector);
        }

        public Browser Set(string cssOrXpathSelector, string value)
        {
            var elem = driver.FindElement(GetSelector(cssOrXpathSelector));

            elem.SendKeys(value);
            return this;
        }

        public string Get(string cssOrXpathSelector)
        {
            var elem = driver.FindElement(GetSelector(cssOrXpathSelector));

            if (elem.TagName.ToLowerInvariant() == "input" && elem.GetAttribute("type") == "text")
                return elem.GetAttribute("value");

            if (elem.TagName.ToLowerInvariant() == "select")
                return elem.GetAttribute("value");

            return elem.Text;
        }

        public string Title()
        {
            return driver.Title;
        }

        public Browser WaitFor(string cssOrXpathSelector)
        {
            WaitFor(cssOrXpathSelector, DefailtWaitInSeconds);
            return this;
        }

        public Browser WaitFor(string cssOrXpathSelector, int seconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            var elem = wait.Until(d => d.FindElement(GetSelector(cssOrXpathSelector)));
            return this;
        }

        public void Close()
        {
            driver.Close();
            driver.Dispose();
            driver = null;
        }

    }
}