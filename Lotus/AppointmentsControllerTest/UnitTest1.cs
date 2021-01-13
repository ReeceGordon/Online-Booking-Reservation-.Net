using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;
using Google.Type;
using System.Globalization;

namespace AppointmentsControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<AppointmentsController> logger;

        [TestMethod]
        public void AddAppointment()
        {
            string testmemberid = "test";
            string teststaffid = "test";
            string type = "test";
            string fullname = "test";
            string start = "test";
            int duration = 30;
            string end = "0";
            DateTime appdate = DateTime.ParseExact("25/05/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string status = "Waiting";

            AppointmentsModel appointmenttest = new AppointmentsModel
            {
                Member_Id = testmemberid,
                Staff_Id = teststaffid,
                Type = type,
                Full_Name = fullname,
                Start = start,
                Duration = duration,
                End = end,
                App_Date = appdate,
                Status = status
            };

            AppointmentsController testcontroller = new AppointmentsController(logger);

            testcontroller.Add(appointmenttest);

            IEnumerable<AppointmentsModel> checkdatabase = testcontroller.GetAllByMemberID(testmemberid);

            Assert.IsNotNull(checkdatabase);

        }

        [TestMethod]
        public void GetbyMemberID()
        {
            IEnumerable<AppointmentsModel> appointments;

            string testmemberid = "test";

            AppointmentsController testcontroller = new AppointmentsController(logger);

            appointments = testcontroller.GetAllByMemberID(testmemberid);

            Assert.IsNotNull(appointments);

        }

        [TestMethod]
        public void Getby2dates()
        {
            IEnumerable<AppointmentsModel> appointments;

            string teststaffid = "test";

            string startdate = "2025-05-24";
            string enddate = "2025-05-26";

            AppointmentsController testcontroller = new AppointmentsController(logger);

            appointments = testcontroller.Get(startdate, enddate, teststaffid);

            Assert.IsNotNull(appointments);

        }


        [TestMethod]
        public void GetbyDate()
        {
            IEnumerable<AppointmentsModel> appointments;

            string teststaffid = "test";

            AppointmentsController testcontroller = new AppointmentsController(logger);

            appointments = testcontroller.GetByDate(DateTime.Now.ToString(), teststaffid);

            Assert.IsNotNull(appointments);

        }

        [TestMethod]
        public void GetbyReview()
        {
            IEnumerable<AppointmentsModel> appointments;
            AppointmentsController testcontroller = new AppointmentsController(logger);
            string testmemberid = "test";
            int id = 0;
            List<AppointmentsModel> list = new List<AppointmentsModel>();

            list = testcontroller.GetAllByMemberID(testmemberid).ToList<AppointmentsModel>();

            foreach (var a in list)
            {
                if (a.Member_Id == testmemberid)
                {
                    id = a.Id;
                    break;
                }
            }

            appointments = testcontroller.GetReview(id.ToString());

            Assert.IsNotNull(appointments);

        }

        [TestMethod]
        public void GetWaitingbyStaffID()
        {
            IEnumerable<AppointmentsModel> appointments;

            string teststaffid = "test";

            AppointmentsController testcontroller = new AppointmentsController(logger);

            appointments = testcontroller.GetWaiting(teststaffid);

            Assert.IsNotNull(appointments);

        }

        [TestMethod]
        public void Update()
        {
            bool expectedresult = true;
            bool actualresult = false;

            string testmemberid = "test";

            AppointmentsController testcontroller = new AppointmentsController(logger);
            List<AppointmentsModel> list = new List<AppointmentsModel>();

            int id = 0;

            list = testcontroller.GetAllByMemberID(testmemberid).ToList<AppointmentsModel>();

            foreach (var a in list)
            {
                if (a.Member_Id == testmemberid)
                {
                    id = a.Id;
                    break;
                }
            }

            AppointmentsModel appointmenttest = new AppointmentsModel
            {
                Id = id,
            };

            testcontroller.Update(appointmenttest);

            list = testcontroller.GetAllByMemberID(testmemberid).ToList<AppointmentsModel>();

            foreach (var appointments in list)
            {
                if (appointments.Status == "Confirmed")
                {
                    actualresult = true;
                    break;
                }
            }

            Assert.AreEqual(expectedresult, actualresult);
        }

        [TestMethod]
        public void UpdateTime()
        {
            bool expectedresult = true;
            bool actualresult = false;

            string testmemberid = "test";
            string start = "test2";
            string end = "0";

            AppointmentsController testcontroller = new AppointmentsController(logger);
            List<AppointmentsModel> list = new List<AppointmentsModel>();

            int id = 0;

            list = testcontroller.GetAllByMemberID(testmemberid).ToList<AppointmentsModel>();

            foreach (var a in list)
            {
                if (a.Member_Id == testmemberid)
                {
                    id = a.Id;
                    break;
                }
            }

            AppointmentsModel appointmenttest = new AppointmentsModel
            {
                Id = id,
                Start = start,
                End = end,
                App_Date = DateTime.Now,
            };

            testcontroller.UpdateTimeSlot(appointmenttest);

            list = testcontroller.GetAllByMemberID(testmemberid).ToList<AppointmentsModel>(); 

            foreach (var appointments in list)
            {
                if (appointments.Start == start)
                {
                    actualresult = true;
                    break;
                }
            }

            Assert.AreEqual(expectedresult, actualresult);
        }

        
    }
}
