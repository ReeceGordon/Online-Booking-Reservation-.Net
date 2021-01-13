using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace SettingControllerTest
{

    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<SettingsController> logger;

        [TestMethod]
        public void GetSettings()
        {
            IEnumerable<SettingsModel> settings;

            SettingsController testcontroller = new SettingsController(logger);

            settings = testcontroller.GetAllSettings();

            Assert.IsNotNull(settings);

        }

        [TestMethod]
        public void GetContacts()
        {
            IEnumerable<SettingsModel> settings;

            SettingsController testcontroller = new SettingsController(logger);

            settings = testcontroller.GetContactDetails();

            Assert.IsNotNull(settings);

        }

        [TestMethod]
        public void GetPassword()
        {
            IEnumerable<SettingsModel> settings;

            SettingsController testcontroller = new SettingsController(logger);

            settings = testcontroller.GetPassword();

            Assert.IsNotNull(settings);

        }

        [TestMethod]
        public void UpdateSettings()
        {
            bool expectedresult = true;
            bool actualresult = false;
            

            string testname = "testname";
            string email = "";
            string address = "";
            long phone = 0;
            string currency = "";
            string password = "";
            string sender = "";
            string server = "";
            string port = "";
            string emailpass = "";
            string ssl = "";
            string companyname = "";
            
   
            SettingsController testcontroller = new SettingsController(logger);
            List<SettingsModel> settingslist = new List<SettingsModel>();

            settingslist = testcontroller.GetAllSettings().ToList<SettingsModel>();

            foreach (var settings in settingslist)
            {
                email = settings.Email;
                address = settings.Address;
                phone = settings.Phone_Number;
                currency = settings.Currency;
                password = settings.Password;
                sender = settings.Email_Sender;
                server = settings.SmtpServer;
                port = settings.Port;
                emailpass = settings.Email_Pass;
                ssl = settings.SSL;
                companyname = settings.Company_Name;
            }

            SettingsModel settingstest = new SettingsModel
            {
                Email = email,
                Address = address,
                Phone_Number = phone,
                Currency = currency,
                Password = password,
                Email_Sender = sender,
                SmtpServer = server,
                Port = port,
                Email_Pass = emailpass,
                SSL = ssl,
                Company_Name = testname,
                Require_Aproval = true
            };

            testcontroller.Update(settingstest);

            settingslist = testcontroller.GetAllSettings().ToList<SettingsModel>();

            foreach (var settings in settingslist)
            {
                if (settings.Company_Name == testname)
                {
                    actualresult = true;
                    break;
                }
            }


            SettingsModel settingsreset = new SettingsModel
            {
                Email = email,
                Address = address,
                Phone_Number = phone,
                Currency = currency,
                Password = password,
                Email_Sender = sender,
                SmtpServer = server,
                Port = port,
                Email_Pass = emailpass,
                SSL = ssl,
                Company_Name = companyname,
                Require_Aproval = true
            };

            testcontroller.Update(settingsreset);


            Assert.AreEqual(expectedresult, actualresult);
        }

    }
}
