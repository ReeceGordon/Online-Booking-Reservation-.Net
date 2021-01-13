using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;
using Google.Type;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WorkingHoursControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<WorkingHoursController> logger;

        [TestMethod]
        public void AddWorkingHours()
        {
            bool expectedresult = true;
            bool actualresult = false;

            string staffid = "test";
            string mondayhours = "test";
            bool mondayclosed = true;
            string tuesdayhours = "test";
            bool tuesdayclosed = true;
            string wednesdayhours = "test";
            bool wednesdayclosed = true;
            string thursdayhours = "test";
            bool thursdayclosed = true;
            string fridayhours = "test";
            bool fridayclosed = true;
            string saturdayhours = "test";
            bool saturdayclosed = true;
            string sundayhours = "test";
            bool sundayclosed = true;


            WorkingHoursModel working = new WorkingHoursModel
            {
                Staff_Id = staffid,
                MondayHours = mondayhours,
                MondayClosed = mondayclosed,
                TuesdayHours = tuesdayhours,
                TuesdayClosed = tuesdayclosed,
                WednesdayHours = wednesdayhours,
                WednesdayClosed = wednesdayclosed,
                ThursdayHours = thursdayhours,
                ThursdayClosed = thursdayclosed,
                FridayHours = fridayhours,
                FridayClosed = fridayclosed,
                SaturdayHours = saturdayhours,
                SaturdayClosed = saturdayclosed,
                SundayHours = sundayhours,
                SundayClosed = sundayclosed

            };

            WorkingHoursController testcontroller = new WorkingHoursController(logger);

            testcontroller.Add(working);

            List<WorkingHoursModel> list = new List<WorkingHoursModel>();

            list = testcontroller.Get(staffid).ToList<WorkingHoursModel>();

            foreach (var a in list)
            {
                if (a.Staff_Id == staffid)
                {
                    actualresult = true;
                    break;
                }
            }

            Assert.AreEqual(expectedresult, actualresult);

        }


        [TestMethod]
        public void GetbyID()
        {
            IEnumerable<WorkingHoursModel> workingHours;

            string staffid = "test";

            WorkingHoursController testcontroller = new WorkingHoursController(logger);

            workingHours = testcontroller.Get(staffid);

            Assert.IsNotNull(workingHours);

        }

        [TestMethod]
        public void Update()
        {
            bool expectedresult = true;
            bool actualresult = false;

            string staffid = "test";
            string mondayhours = "test2";
            bool mondayclosed = true;
            string tuesdayhours = "test2";
            bool tuesdayclosed = true;
            string wednesdayhours = "test2";
            bool wednesdayclosed = true;
            string thursdayhours = "test2";
            bool thursdayclosed = true;
            string fridayhours = "test2";
            bool fridayclosed = true;
            string saturdayhours = "test2";
            bool saturdayclosed = true;
            string sundayhours = "test2";
            bool sundayclosed = true;

            WorkingHoursController testcontroller = new WorkingHoursController(logger);
            List<WorkingHoursModel> list = new List<WorkingHoursModel>();

            WorkingHoursModel working = new WorkingHoursModel
            {
                Staff_Id = staffid,
                MondayHours = mondayhours,
                MondayClosed = mondayclosed,
                TuesdayHours = tuesdayhours,
                TuesdayClosed = tuesdayclosed,
                WednesdayHours = wednesdayhours,
                WednesdayClosed = wednesdayclosed,
                ThursdayHours = thursdayhours,
                ThursdayClosed = thursdayclosed,
                FridayHours = fridayhours,
                FridayClosed = fridayclosed,
                SaturdayHours = saturdayhours,
                SaturdayClosed = saturdayclosed,
                SundayHours = sundayhours,
                SundayClosed = sundayclosed

            };

            testcontroller.Update(working);

            list = testcontroller.Get(staffid).ToList<WorkingHoursModel>();

            foreach (var a in list)
            {
                if (a.MondayHours == mondayhours)
                {
                    actualresult = true;
                    break;
                }
            }

            Assert.AreEqual(expectedresult, actualresult);
        }


    }
}
