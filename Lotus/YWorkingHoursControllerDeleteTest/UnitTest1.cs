using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;
using Google.Type;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace YWorkingHoursControllerDeleteTest
{
    [TestClass]
    public class UnitTest1
    {

        private static readonly ILogger<WorkingHoursController> logger;

        [TestMethod]
        public void Delete()
        {
            IEnumerable<WorkingHoursModel> workingHours;

            string staffid = "test";
            int firstcheck = 0;
            int secondcheck = 0;

            WorkingHoursController testcontroller = new WorkingHoursController(logger);


            workingHours = testcontroller.Get(staffid);

            using (var sequenceEnum = workingHours.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.Delete(staffid);

            workingHours = testcontroller.Get(staffid);

            using (var sequenceEnum = workingHours.GetEnumerator())
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
