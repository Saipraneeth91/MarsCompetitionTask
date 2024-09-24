
using Mars_Competition.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Competition.Pages
{
    public class LoginPage
    {
            private readonly IWebDriver driver;
            private readonly ElementUtil eleUtil;
            public LoginPage(IWebDriver driver)
            {
                this.driver = driver;
                eleUtil = new ElementUtil(driver);
            }
            //By Locators
            private readonly By signin = By.XPath("//a[contains(text(),'Sign In')]");
            private readonly By emailaddress = By.XPath("//input[@placeholder='Email address']");
            private readonly By password = By.XPath("//input[@placeholder='Password']");
            private readonly By login = By.XPath("//button[contains(text(),'Login')]");
            private readonly By welcometext = By.XPath("//div/div[1]/div[2]/div/span");

            public void LoginActions(string email, string pwd ) // Login Method
            {

            Wait.WaitToBeVisible(driver, signin, Wait.LONG_DEFAULT_WAIT);
                eleUtil.doClick(signin);
                //enter email address
                eleUtil.doSendKeys(emailaddress, email);
                // enter password
                eleUtil.doSendKeys(password, pwd);
                // click login
                eleUtil.doClick(login);
            }
            public string GetWelcomeText() // Method to return welcome text
            {
                return eleUtil.getText(welcometext);
            }

        }
    }


