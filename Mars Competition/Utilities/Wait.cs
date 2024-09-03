using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;

namespace Mars_Competition.Utilities
{
    public class Wait
    {
        public static int SHORT_DEFAULT_WAIT = 5;
        public static int MEDIUM_DEFAULT_WAIT = 10;
        public static int LONG_DEFAULT_WAIT = 20;
        //private static WebDriver driver;

        public static IWebElement WaitToBeClickable(IWebDriver driver, By locator, int timeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            return element;
        }

        public static IWebElement WaitToBeVisible(IWebDriver driver, By locator, int timeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element;

        }

    }
}

