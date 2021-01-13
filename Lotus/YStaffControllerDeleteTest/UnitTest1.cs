using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace YStaffControllerDeleteTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<StaffController> logger;

        [TestMethod]
        public void Delete()
        {
            IEnumerable<StaffModel> staffs;

            string testid = "test";
            int firstcheck = 0;
            int secondcheck = 0;

            StaffController testcontroller = new StaffController(logger);

            staffs = testcontroller.Get();

            using (var sequenceEnum = staffs.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.Delete(testid);

            staffs = testcontroller.Get();

            using (var sequenceEnum = staffs.GetEnumerator())
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
