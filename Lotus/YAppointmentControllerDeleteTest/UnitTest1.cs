using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;
using Google.Type;
using System.Globalization;

namespace YAppointmentControllerDeleteTest
{
    [TestClass]
    public class UnitTest1
    {

        private static readonly ILogger<AppointmentsController> logger;

        [TestMethod]
        public void Delete()
        {
            IEnumerable<AppointmentsModel> appointments;

            string testmemberid = "test";
            int firstcheck = 0;
            int secondcheck = 0;

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

            appointments = testcontroller.GetAllByMemberID(testmemberid);

            using (var sequenceEnum = appointments.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.Delete(id.ToString());

            appointments = testcontroller.GetAllByMemberID(testmemberid);

            using (var sequenceEnum = appointments.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    secondcheck += 1;
                }
            }

            Assert.AreNotEqual(firstcheck, secondcheck);
        }
    }
}
