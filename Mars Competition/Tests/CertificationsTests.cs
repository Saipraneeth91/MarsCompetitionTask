using Mars_Competition.Helpers;
using Mars_Competition.Models;
using Mars_Competition.Pages;
using Mars_Competition.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mars_Competition.Tests
{
    [TestFixture]
    public class CertificationsTests : Driverfactory
    {
        private LoginPage loginPage;
        private CertificationPage certificationPage;
        private LoginData loginData;
        private CertificationData certificationData;
        private List<string> dataUsedInTest;

        [SetUp]
        public void Setup()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(basePath);

            //  Path to JSON files
            string validLoginPath = Path.Combine(basePath, "TestData", "LoginScenarios", "ValidLogin.json");
            // Load the login data from the JSON file
            loginData = new LoginData
            {
                ValidLogin = JSONHelper.LoadData<LoginInfo>(validLoginPath)
            };
            // Initialize the page objects
            loginPage = new LoginPage(driver);
            certificationPage = new CertificationPage(driver);
            dataUsedInTest = new List<string>();
        }
        [Test]
        public void Addvalidcertificationrecord()
        {
            string validCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "ValidCertificationDetails.json");
            certificationData = JSONHelper.LoadData<CertificationData>(validCertificationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var certificationRecord in certificationData.CertificationRecords)
            {
                certificationPage.Addcertification(certificationRecord.Certificate, certificationRecord.From, certificationRecord.Year);
                dataUsedInTest.Add(certificationRecord.Certificate);
                string actualCertificate = certificationPage.Getlastrecordcertificate();
                string expectedCertificate = certificationRecord.Certificate;
                Console.WriteLine(expectedCertificate);
                if (actualCertificate == expectedCertificate)
                {
                    Assert.Pass("Record Added Succesfully");
                }
            }
        }
        [Test]
        public void AddCertificationWithSpecialCharacters()
        {
            string specialCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "AddCertificationWithSpecialCharacters.json");
            certificationData = JSONHelper.LoadData<CertificationData>(specialCertificationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(loginData.ValidLogin.Emailaddress, loginData.ValidLogin.Password);
            foreach (var certificationRecord in certificationData.CertificationRecords)
            {
                certificationPage.Addcertification(certificationRecord.Certificate, certificationRecord.From, certificationRecord.Year);
                dataUsedInTest.Add(certificationRecord.Certificate);
                string actualnotification = certificationPage.Getnotificationtext();
                string expectednotification = "Specialcharacters are not allowed";
                Assert.That(actualnotification, Is.EqualTo(expectednotification));
                //currently system is accepting special characters, its a defect
            }
        }
        [Test]
        public void AddCaseSensitiveCertification()
        {
            string caseSensitivePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "AddCertificationWithCaseSensitive.json");
            certificationData = JSONHelper.LoadData<CertificationData>(caseSensitivePath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var certificationRecord in certificationData.CertificationRecords)
            {
                certificationPage.Addcertification(certificationRecord.Certificate, certificationRecord.From, certificationRecord.Year);
                dataUsedInTest.Add(certificationRecord.Certificate);
            }
                string expectedText = "This information is already exist.";
                string actualText = certificationPage.Getnotificationtext();
                Assert.That(actualText, Is.EqualTo(expectedText));
                //currently system is accepting case sensitive records, its a defect
            
        }
        [Test]
        public void AddDuplicateCertificationDetails()
        {
            string duplicateCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "DuplicateCertificationDetails.json");
            certificationData = JSONHelper.LoadData<CertificationData>(duplicateCertificationPath);
            var validLogin = loginData.ValidLogin;
            var firstCertificationRecord = certificationData.CertificationRecords[0];
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            certificationPage.Addcertification(firstCertificationRecord.Certificate, firstCertificationRecord.From, firstCertificationRecord.Year);
            dataUsedInTest.Add(firstCertificationRecord.Certificate);
            //Add duplicate record
            var duplicateCertificationRecord = certificationData.CertificationRecords[1];
            certificationPage.Addcertification(duplicateCertificationRecord.Certificate, duplicateCertificationRecord.From, duplicateCertificationRecord.Year);
            string expectedText = "This information is already exist.";
            string actualText = certificationPage.Getnotificationtext();
            Assert.That(actualText, Is.EqualTo(expectedText));
        }
        [Test]
        public void AddBlankCertificationDetails()
        {
            string blankCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "BlankCertificationDetails.json");
            certificationData = JSONHelper.LoadData<CertificationData>(blankCertificationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var certificationRecord in certificationData.CertificationRecords)
            {
                certificationPage.Addcertification(certificationRecord.Certificate, certificationRecord.From, certificationRecord.Year);
                string expectedText = "Please enter Certification Name, Certification From and Certification Year";
                string actualText = certificationPage.Getnotificationtext();
                Assert.That(actualText, Is.EqualTo(expectedText));
            }

        }
        [Test]
        public void AddDestructiveCertificationDetails()
        {
            string destructiveCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "DestructiveCertificationDetails.json");
            certificationData = JSONHelper.LoadData<CertificationData>(destructiveCertificationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            foreach (var certificationRecord in certificationData.CertificationRecords)
            {
                certificationPage.Addcertification(certificationRecord.Certificate, certificationRecord.From, certificationRecord.Year);
                dataUsedInTest.Add(certificationRecord.Certificate);
                string expectedText = "Length of fields exceed limit defined";
                string actualText = certificationPage.Getnotificationtext();
                Assert.That(actualText, Is.EqualTo(expectedText));
                //currently system is accepting destructive lengthy records also, its a defect
            }
        }
        [Test]
        public void EditCertificationDetails()
        {
            string EditCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "EditCertificationDetails.json");
            certificationData = JSONHelper.LoadData<CertificationData>(EditCertificationPath);
            var validLogin = loginData.ValidLogin;
            var addCertification = certificationData.CertificationRecords[0];
            var editCertification = certificationData.CertificationRecords[1];
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            certificationPage.Addcertification(addCertification.Certificate,addCertification.From,addCertification.Year);
            certificationPage.Editcertification(editCertification.Certificate,editCertification.From,editCertification.Year);
            dataUsedInTest.Add(editCertification.Certificate);
            string actualCertificate = certificationPage.Getlastrecordcertificate();
            string actualFrom = certificationPage.Getlastrecordfrom();
            string actualYear = certificationPage.GetlastrecordYear();
            string expectedCertificate = editCertification.Certificate;
            string expectedFrom = editCertification.From;
            string expectedYear = editCertification.Year;
            // Assert that the actual values match the expected values
            Assert.That(expectedCertificate, Is.EqualTo(actualCertificate));
            Assert.That(expectedFrom, Is.EqualTo(actualFrom));
            Assert.That(expectedYear, Is.EqualTo(actualYear));
        }
        [Test]
        public void EditCertificationDuplicateDetails()
        {
            string EditCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "EditCertificationDetails.json");
            certificationData = JSONHelper.LoadData<CertificationData>(EditCertificationPath);
            var validLogin = loginData.ValidLogin;
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            var addCertification = certificationData.CertificationRecords[0];
            certificationPage.Addcertification(addCertification.Certificate, addCertification.From, addCertification.Year);
            dataUsedInTest.Add(addCertification.Certificate);
            var editCertification = certificationData.CertificationRecords[1];
            certificationPage.Editcertificationwithoutchange();
            string actualNotification = certificationPage.Getnotificationtext();
            string expectedNotification = "This information is already exist.";
            Assert.That(actualNotification, Is.EqualTo(expectedNotification));

        }
        [Test]
        public void DeleteCertification()
        {
            string validCertificationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "CertificationScenarios", "ValidCertificationDetails.json");
            certificationData = JSONHelper.LoadData<CertificationData>(validCertificationPath);
            var validLogin = loginData.ValidLogin;
            var validCertification = certificationData.CertificationRecords[0];
            loginPage.LoginActions(validLogin.Emailaddress, validLogin.Password);
            certificationPage.Addcertification(validCertification.Certificate, validCertification.From, validCertification.Year);
            certificationPage.Deletecertificationdetails();
            string expectedNotification = validCertification.Certificate+" has been deleted from your certification";
            string actualNotification = certificationPage.Getnotificationtext();
            Assert.That(actualNotification, Is.EqualTo(expectedNotification));
        }
        [TearDown]
        public void TearDown()
        {
            foreach (var certificate in dataUsedInTest)
            {
                certificationPage.Deletecertification(certificate);
            }
            //Clear the datalist for the next test
            dataUsedInTest.Clear();
            Dispose();
        }
    }

}