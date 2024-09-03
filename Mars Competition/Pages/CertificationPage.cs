using Mars_Competition.Tests;
using Mars_Competition.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mars_Competition.Pages
{
    public class CertificationPage
    {
        private readonly IWebDriver driver;
        private readonly ElementUtil eleUtil;
        public CertificationPage(IWebDriver driver)
        {
            this.driver = driver;
            eleUtil = new ElementUtil(driver);
        }
        //By Locators
        private readonly By certificationtab = By.XPath("//a[text()='Certifications']");
        private readonly By addnewbutton = By.XPath("//div[@class='ui bottom attached tab segment tooltip-target active']//div[contains(@class,'ui teal button')][normalize-space()='Add New']");
        private readonly By certificate = By.Name("certificationName");
        private readonly By certifiedfrom = By.Name("certificationFrom");
        private readonly By year = By.XPath("//select[@name = 'certificationYear']");
        private readonly By add = By.XPath("//input[@value='Add']");
        private readonly By editicon = By.XPath("//tbody[last()]/tr[last()]/td[4]/span[1]/i[1]");
        private readonly By updatebutton = By.XPath("//input[@value='Update']");
        private readonly By deleteicon = By.XPath("//tbody[last()]/tr[last()]/td[4]/span[2]/i[1]");
        private readonly By cancelbutton = By.XPath("//input[@value='Cancel']");
        private readonly By notificationtext = By.XPath("//div[@class='ns-box-inner']");
        private readonly By lastrecordcertificate = By.XPath("//th[text()='Certificate']/ancestor::thead//following-sibling::tbody[last()]/tr/td[1]");
        private readonly By lastrecordcertificatefrom = By.XPath("//th[text()='From']/ancestor::thead//following-sibling::tbody[last()]/tr/td[2]");
        private readonly By lastrecordcertificateyear = By.XPath("//th[text()='Year']/ancestor::thead//following-sibling::tbody[last()]/tr/td[3]");
        private readonly By logoutbutton = By.XPath("//button[normalize-space()='Sign Out']");
        public ReadOnlyCollection<IWebElement> Rows => driver.FindElements(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody"));
        public void Addcertification(string certificatename, string certificationfrom, string certificationyear)
        {
            eleUtil.doClick(certificationtab);
            eleUtil.doClick(addnewbutton);
            eleUtil.doSendKeys(certificate, certificatename);
            eleUtil.doSendKeys(certifiedfrom, certificationfrom);
            eleUtil.doSendKeys(year, certificationyear);
            eleUtil.doClick(add);
            Thread.Sleep(4000);
        }
        public void Editcertification(string certificatename, string certificationfrom, string certificationyear)
        {
            eleUtil.doClick(certificationtab);
            eleUtil.doClick(editicon);
            eleUtil.doClear(certificate);
            eleUtil.doSendKeys(certificate, certificatename);
            eleUtil.doClear(certifiedfrom);
            eleUtil.doSendKeys(certifiedfrom, certificationfrom);
            eleUtil.doSendKeys(year, certificationyear);
            eleUtil.doClick(updatebutton);
            Thread.Sleep(2000);
        }
        public void Editcertificationwithoutchange()
        {
            eleUtil.doClick(certificationtab);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            Wait.WaitToBeVisible(driver, editicon, Wait.LONG_DEFAULT_WAIT);
            eleUtil.doClick(editicon);
            eleUtil.doClick(updatebutton);
            eleUtil.doClick(cancelbutton);

        }
        public void Deletecertificationdetails()
        {
            eleUtil.doClick(certificationtab);
            eleUtil.doClick(deleteicon);
            Thread.Sleep(2000);
        }
        public string Getnotificationtext()
        {
            return eleUtil.getText(notificationtext);
        }
        public string Getlastrecordcertificate()
        {
            return eleUtil.getText(lastrecordcertificate);
        }
        public string Getlastrecordfrom()
        {
            return eleUtil.getText(lastrecordcertificatefrom);
        }
        public string GetlastrecordYear()
        {
            return eleUtil.getText(lastrecordcertificateyear);
        }
        public void ClearCertification()
        {
            Wait.WaitToBeClickable(driver, certificationtab, Wait.LONG_DEFAULT_WAIT);
            eleUtil.doClick(certificationtab);
            int totalrows = Rows.Count;
            Console.WriteLine(totalrows);

            for (int i = 0; i < totalrows; i = i + 1)
            {
                Wait.WaitToBeClickable(driver, certificationtab, Wait.LONG_DEFAULT_WAIT);
                // click on delete icon
                eleUtil.doClick(deleteicon);
                Thread.Sleep(2000);
            }
            eleUtil.doClick(logoutbutton);
        }
        public void Logout()
        {
            eleUtil.doClick(logoutbutton);

        }
        public void Deletecertification(string certification)
        {
            // click to navigate to skill section 
            eleUtil.doClick(certificationtab);
            // delete skill by skill name passed
            By deletebycertification = By.XPath("//td[text()='" + certification + "']/following-sibling::td/span[@class='button'][2]");
            Wait.WaitToBeClickable(driver, deletebycertification, Wait.MEDIUM_DEFAULT_WAIT);
            eleUtil.doClick(deletebycertification);
            //Thread.Sleep(1000); 

        }


    }
}

