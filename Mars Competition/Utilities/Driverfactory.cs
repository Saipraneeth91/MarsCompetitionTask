using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using Mars_Competition.Pages;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.Configuration;
using System.IO;

public class Driverfactory : IDisposable
{
    public ExtentReports extent;
    public ExtentTest test;
    public IWebDriver? driver { get; private set; }

    [OneTimeSetUp]
    public void Setup()
    {
        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

        // Ensure the "Reports" folder exists
        string reportsFolder = Path.Combine(projectDirectory, "Reports");
        if (!Directory.Exists(reportsFolder))
        {
            Directory.CreateDirectory(reportsFolder);
        }

        // Path to save the report file
        string reportPath = Path.Combine(reportsFolder, "TestReport.html");
        var htmlReporter = new ExtentHtmlReporter(reportPath);

        extent = new ExtentReports();
        extent.AttachReporter(htmlReporter);
        extent.AddSystemInfo("Host Name", "Local host");
        extent.AddSystemInfo("Username", "Sai Praneeth");

        // Start browser and delete all existing data before the first test run
        StartBrowser();
        DeleteAllExistingData(); // Deletes all the data before the test run
    }

    [SetUp]
    public void InitializeTest()
    {
        // Initialize the test with the current test name
        string testName = TestContext.CurrentContext.Test.Name;
        test = extent.CreateTest(testName);

        // Log the test name in the report
        test.Log(Status.Info, $"Starting test: {testName}");
    }

    public void StartBrowser()
    {
        // Read data from AppConfig
        string browser = ConfigurationManager.AppSettings.Get("browser");
        string url = ConfigurationManager.AppSettings.Get("url");

        InitBrowser(browser);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        driver.Manage().Window.Maximize();
        driver.Navigate().GoToUrl(url);
    }

    public void InitBrowser(string browser)
    {
        switch (browser)
        {
            case "Chrome":
                driver = new ChromeDriver();
                break;
            case "Firefox":
                driver = new FirefoxDriver();
                break;
            default:
                throw new ArgumentException("Browser not supported: " + browser);
        }
    }

    [TearDown]
    public void Cleanup()
    {
        var testResult = TestContext.CurrentContext.Result;
        var status = testResult.Outcome.Status;
        var stackTrace = testResult.StackTrace;
        DateTime time = DateTime.Now;
        string fileName = "Screenshot_" + time.ToString("yyyyMMdd_HHmmss") + ".png";
        string screenshotsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

        // Ensure the Screenshots folder exists
        if (!Directory.Exists(screenshotsFolder))
        {
            Directory.CreateDirectory(screenshotsFolder);
        }

        string filePath = Path.Combine(screenshotsFolder, fileName);

        // Save screenshot to file and attach to ExtentReports
        if (status == NUnit.Framework.Interfaces.TestStatus.Failed || status == NUnit.Framework.Interfaces.TestStatus.Passed)
        {
            try
            {
                CaptureScreenshot(filePath);
                var screenshotMedia = MediaEntityBuilder.CreateScreenCaptureFromPath(filePath).Build();
                test.Log(status == NUnit.Framework.Interfaces.TestStatus.Failed ? Status.Fail : Status.Pass,
                         "Test " + (status == NUnit.Framework.Interfaces.TestStatus.Failed ? "failed" : "passed"),
                         screenshotMedia);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to capture screenshot: " + ex.Message);
            }
        }

        driver?.Quit();
        driver?.Dispose();
    }

    private void CaptureScreenshot(string filePath)
    {
        if (driver != null)
        {
            try
            {
                ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to capture screenshot: " + ex.Message);
            }
        }
    }

    private void DeleteAllExistingData()
    {
        // Call the delete methods from the respective page classes
        var loginPage = new LoginPage(driver);
        var educationPage = new EducationPage(driver);
        var certificationPage = new CertificationPage(driver);
        loginPage.LoginActions("saipraneethg.91@gmail.com", "Praneeth@1");
        educationPage.ClearEducation();
        certificationPage.ClearCertification();
    }

    [OneTimeTearDown]
    public void EndReport()
    {
        extent.Flush();
    }

    public void Dispose()
    {
        EndReport();
    }
}
