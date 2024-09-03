using Mars_Competition.Helpers;
using Mars_Competition.Models;
using Mars_Competition.Pages;
using Mars_Competition.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mars_Competition.Tests
{
  
    public class EducationTests : Driverfactory
    {
        private LoginPage loginPage;
        private EducationPage educationPage;
        private LoginData loginData;
        private EducationData educationData;
        private List<string> dataUsedInTest;


        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            // Path to JSON files
            string validLoginPath = Path.Combine(basePath, "TestData", "LoginScenarios", "ValidLogin.json");
            // Load the login data from the JSON file
            loginData = new LoginData
            {
                ValidLogin = JSONHelper.LoadData<LoginInfo>(validLoginPath)
            };
            // Initialize the page objects
            loginPage = new LoginPage(driver);
            educationPage = new EducationPage(driver);
            dataUsedInTest = new List<string>();
        }
        [Test]
        public void AddValidEducationrecord()
        {
            string validEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "ValidEducationDetails.json");
            educationData = JSONHelper.LoadData<EducationData>(validEducationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var educationRecord in educationData.EducationRecords)
            {
                educationPage.AddEducationdetails(educationRecord.University, educationRecord.Country, educationRecord.Title, educationRecord.Degree, educationRecord.GraduationYear);
                dataUsedInTest.Add(educationRecord.Degree);
                string actualTitle = educationPage.Getlastrecordtitle();
                string expectedTitle = educationRecord.Title;
                if (actualTitle == expectedTitle)
                {
                    Assert.Pass("Record Added Succesfully");
                }

            }
        }
        [Test]
        public void AddSpecialCharactersEducation()
        {
            string specialEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "AddEducationWithSpecialCharacters.json");
            educationData = JSONHelper.LoadData<EducationData>(specialEducationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(loginData.ValidLogin.Emailaddress, loginData.ValidLogin.Password);
            foreach (var educationRecord in educationData.EducationRecords)
            {
                educationPage.AddEducationdetails(educationRecord.University, educationRecord.Country, educationRecord.Title, educationRecord.Degree, educationRecord.GraduationYear);
                dataUsedInTest.Add(educationRecord.Degree);
                string actualNotification = educationPage.Getnotificationtext();
                string expectedNotification = "Specialcharacters are not allowed";
                Assert.That(actualNotification, Is.EqualTo(expectedNotification));
                //currently system is accepting special character records, its a defect
            }
        }
        [Test]
        public void AddCaseSensitiveEducation()
        {
            string caseSensitivePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "AddEducationWithCaseSensitive.json");
            educationData = JSONHelper.LoadData<EducationData>(caseSensitivePath);
            var validLogin = loginData.ValidLogin;
            // Log in and add the first record
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var educationRecord in educationData.EducationRecords)
            {
                educationPage.AddEducationdetails(educationRecord.University, educationRecord.Country, educationRecord.Title, educationRecord.Degree, educationRecord.GraduationYear);
                dataUsedInTest.Add(educationRecord.Degree);
            }
            string expectedText = "This information is already exist.";
             string actualText = educationPage.Getnotificationtext();
             Assert.That(actualText, Is.EqualTo(expectedText));
            //currently system is accepting case sensitive records, its a defect
        }

        [Test]
        public void AddDuplicateEducationDetails()
        {
            string duplicateEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "DuplicateEducationDetails.json");
            educationData = JSONHelper.LoadData<EducationData>(duplicateEducationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            // Add first record
            var firstRecord = educationData.EducationRecords[0];
            educationPage.AddEducationdetails(firstRecord.University, firstRecord.Country, firstRecord.Title, firstRecord.Degree, firstRecord.GraduationYear);
            dataUsedInTest.Add(firstRecord.Degree);
            //Add duplicate record
            var duplicateRecord = educationData.EducationRecords[1];
            educationPage.AddEducationdetails(duplicateRecord.University, duplicateRecord.Country, duplicateRecord.Title, duplicateRecord.Degree, duplicateRecord.GraduationYear);
            string expectedText = "This information is already exist.";
            string actualText = educationPage.Getnotificationtext();
            Assert.That(actualText, Is.EqualTo(expectedText));
        }
        [Test]
        public void AddBlankEducationDetails()
        {
            string blankEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "BlankEducationDetails.json");
            educationData = JSONHelper.LoadData<EducationData>(blankEducationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var educationRecord in educationData.EducationRecords)
            {
                educationPage.AddEducationdetails(educationRecord.University, educationRecord.Country, educationRecord.Title, educationRecord.Degree, educationRecord.GraduationYear);
                string expectedText = "Please enter all the fields";
                string actualText = educationPage.Getnotificationtext();
                Assert.That(actualText, Is.EqualTo(expectedText));
            }
        }
        [Test]
        public void AddDestructiveEducationDetails()
        {
            string destructiveEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "DestructiveEducationDetails.json");
            educationData = JSONHelper.LoadData<EducationData>(destructiveEducationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var educationRecord in educationData.EducationRecords)
            {
                educationPage.AddEducationdetails(educationRecord.University, educationRecord.Country, educationRecord.Title, educationRecord.Degree, educationRecord.GraduationYear);
                dataUsedInTest.Add(educationRecord.Degree);
                string expectedText = "Length of fields exceed limit defined";
                string actualText = educationPage.Getnotificationtext();
                Assert.That(actualText, Is.EqualTo(expectedText));
                //currently system is accepting destructive lengthy records also, its a defect
            }
        }
        [Test]
        public void EditEducationDetails()
        {
            string EditEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "EditEducationDetails.json");
            educationData = JSONHelper.LoadData<EducationData>(EditEducationPath);
            var validLogin = loginData.ValidLogin;
            var addeducation = educationData.EducationRecords[0];
            var editeducation = educationData.EducationRecords[1];
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            educationPage.AddEducationdetails(addeducation.University, addeducation.Country, addeducation.Title, addeducation.Degree, addeducation.GraduationYear);
            educationPage.Editeducationdetails(editeducation.University, editeducation.Country, editeducation.Title, editeducation.Degree, editeducation.GraduationYear);
            dataUsedInTest.Add(editeducation.Degree);
            string actualCountry = educationPage.Getlastrecordcountry();
            string actualUniversity = educationPage.Getlastrecorduniversity();
            string actualTitle = educationPage.Getlastrecordtitle();
            string actualDegree = educationPage.GetlastrecordDegree();
            string actualYear = educationPage.GetlastrecordYear();
            string expectedCountry = editeducation.Country;
            string expectedUniversity = editeducation.University;
            string expectedTitle = editeducation.Title;
            string expectedDegree = editeducation.Degree;
            string expectedYear = editeducation.GraduationYear;
            //Assert Edited record details
            Assert.That(actualCountry, Is.EqualTo(expectedCountry));
            Assert.That(actualUniversity, Is.EqualTo(expectedUniversity));
            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
            Assert.That(actualDegree, Is.EqualTo(expectedDegree));
            Assert.That(actualYear, Is.EqualTo(expectedYear));
        }
        [Test]
        public void EditEducationWithsamedata()     
        {
            string EditEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "EditEducationDetails.json");
            educationData = JSONHelper.LoadData<EducationData>(EditEducationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            // Add first record
            var firstRecord = educationData.EducationRecords[0];
            educationPage.AddEducationdetails(firstRecord.University, firstRecord.Country, firstRecord.Title, firstRecord.Degree, firstRecord.GraduationYear);
            dataUsedInTest.Add(firstRecord.Degree);
            var duplicateRecord = educationData.EducationRecords[1];
            educationPage.Editeducationwithoutchange();
            string actualNotification = educationPage.Getnotificationtext();
            string expectedNotification = "This information is already exist.";
            Assert.That(actualNotification, Is.EqualTo(expectedNotification));
            }
        [Test]
        public void DeleteEducation()
        {
            string validEducationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "EducationScenarios", "ValidEducationDetails.json");
            educationData = JSONHelper.LoadData<EducationData>(validEducationPath);
            var validLogin = loginData.ValidLogin;
            var validEducation = educationData.EducationRecords[0];
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            educationPage.AddEducationdetails(validEducation.University, validEducation.Country, validEducation.Title, validEducation.Degree, validEducation.GraduationYear);
            educationPage.Deleteeducationdetails();
            string expectedNotification = "Education entry successfully removed";
            string actualNotification = educationPage.Getnotificationtext();
            Assert.That(actualNotification, Is.EqualTo(expectedNotification));
        }
        [TearDown]
        public void TearDown()
        {
            // Delete all education records created in the current test
            foreach (var degree in dataUsedInTest)
            {
               educationPage.Deleteeducation(degree);
            }
            //Clear the datalist for the next test
            dataUsedInTest.Clear();
            Dispose();
        }

    }

}










































































































