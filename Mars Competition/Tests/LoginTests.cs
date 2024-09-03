using Mars_Competition.Helpers;
using Mars_Competition.Models;
using Mars_Competition.Pages;
using Mars_Competition.Utilities;
using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mars_Competition.Tests
{
        [TestFixture]
        public class LoginTests : Driverfactory
        {
            private LoginPage loginpage;
            private LoginData logindata;

            [SetUp]
            public void SetUp()
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                //  Path to Login JSON files
                string validLoginPath = Path.Combine(basePath, "TestData", "LoginScenarios", "ValidLogin.json");
                string invalidLoginPath = Path.Combine(basePath, "TestData", "LoginScenarios", "InvalidLogin.json");

                // Load the login data from the JSON files
                logindata = new LoginData
                {
                    ValidLogin = JSONHelper.LoadData<LoginInfo>(validLoginPath),
                    InvalidLogin = JSONHelper.LoadData<LoginInfo>(invalidLoginPath)
                };

                // Initialize the page object
                loginpage = new LoginPage(driver);
            }

            [Test]
            public void ValidLoginTest()
            {
                // Retrieve valid login credentials
                var validLogin = logindata.ValidLogin;

                // Perform login action
                loginpage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            string Actualtext=loginpage.GetWelcomeText();
            string Expectedtext = "Hi Sai";
            Assert.That(Actualtext, Is.EqualTo(Expectedtext));
        }

            [TearDown]
            public void TearDown()
            {
                Dispose();
            }
        }
    }
