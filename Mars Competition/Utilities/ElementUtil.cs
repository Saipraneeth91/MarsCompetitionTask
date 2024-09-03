using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Competition.Utilities
{
    public class ElementUtil
    {
        private IWebDriver driver;

        public ElementUtil(IWebDriver driver)
        {

            this.driver = driver;
        }
        public void doSendKeys(By locator, string value)
        {
            getElement(locator).SendKeys(value);
        }
        public IWebElement getElement(By locator)
        {
            return driver.FindElement(locator);
        }
        public void doClick(By locator)
        {
            getElement(locator).Click();
        }
        public string getText(By locator)
        {
            return getElement(locator).Text;
        }
        public void doClear(By locator)
        {
            getElement(locator).Clear();
        }

        // ***************Select drop Down Utils***************//

        private SelectElement createSelect(By locator)
        {
            SelectElement select = new SelectElement(getElement(locator));
            return select;
        }

        public void doSelectDropDownByIndex(By locator, int index)
        {
            createSelect(locator).SelectByIndex(index);
        }

        public void doSelectDropDownByVisibleText(By locator, String visibleText)
        {
            createSelect(locator).SelectByText(visibleText);
        }

        public void doSelectDropDownByValue(By locator, String value)
        {
            createSelect(locator).SelectByValue(value);
        }

        public int getDropDownOptionsCount(By locator)
        {
            return createSelect(locator).Options.Count;
        }

        public List<String> getDropDownOptions(By locator)
        {
            var optionsList = createSelect(locator).Options;
            return optionsList.Select(e => e.Text).ToList(); 
        }

        public List<string> GetDropDownOptions(By locator)
        {
            var optionsList = createSelect(locator).Options;
            return optionsList.Select(e => e.Text).ToList();
        }
        public List<IWebElement> GetElements(By locator)
        {
            return driver.FindElements(locator).ToList();
        }

        public void selectDropDownOption(By locator, String dropDownValue)
        {

            var optionsList = createSelect(locator).Options;

            foreach (var option in optionsList)
            {
                string text = option.Text;
                if (text.Equals(dropDownValue))
                {
                    option.Click();
                    break;
                }
            }
        }

            public void SelectDropDownValue(By locator, string value)
            {
                var optionsList = GetElements(locator);

                foreach (var e in optionsList)
                {
                    string text = e.Text;
                    if (text.Equals(value))
                    {
                        e.Click();
                        break;
                    }
                }
            }

    }
}
