using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace YStaffAssignmentControllerDeleteTest
{

    [TestClass]
    public class UnitTest1
    {

        private static readonly ILogger<StaffAssignmentController> logger;


        [TestMethod]
        public void DeleteStaff()
        {
            IEnumerable<StaffAssignmentModel> staffAssignments;

            string teststaffid = "testid";
            string testcategoryid = "thetestid";
            int firstcheck = 0;
            int secondcheck = 0;

            StaffAssignmentController testcontroller = new StaffAssignmentController(logger);

            staffAssignments = testcontroller.Get();

            using (var sequenceEnum = staffAssignments.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.DeletewithStaff(teststaffid, testcategoryid);

            staffAssignments = testcontroller.Get();

            using (var sequenceEnum = staffAssignments.GetEnumerator())
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
