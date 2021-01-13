using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace StaffAssignmentControllerTest
{
    [TestClass]
    public class UnitTest1
    {

        private static readonly ILogger<StaffAssignmentController> logger;


        [TestMethod]
        public void AddStaffAssignment()
        {
            bool expectedresult = true;
            bool actualresult = false;
            string teststaffid = "testid";
            string testcategoryid = "thetestid";
           

            StaffAssignmentModel staffassigntest = new StaffAssignmentModel
            {
                Category_Id = testcategoryid,
                Staff_Id = teststaffid
            };

            StaffAssignmentController testcontroller = new StaffAssignmentController(logger);

            testcontroller.Post(staffassigntest);

            IEnumerable<StaffAssignmentModel> checkdatabase = testcontroller.Get();

            using (var iterate = checkdatabase.GetEnumerator())
            {
                while (iterate.MoveNext())
                {
                    if (iterate.Current.Category_Id.Contains(testcategoryid))
                    {
                        actualresult = true;
                        break;
                    }
                }
            }

            Assert.AreEqual(expectedresult, actualresult);

        }

        [TestMethod]
        public void GetStaffAssign()
        {
            IEnumerable<StaffAssignmentModel> staffAssignments;

            StaffAssignmentController testcontroller = new StaffAssignmentController(logger);

            staffAssignments = testcontroller.Get();

            Assert.IsNotNull(staffAssignments);

        }

        [TestMethod]
        public void GetStaffID()
        {
            IEnumerable<StaffAssignmentModel> staffAssignments;

            string teststaffid = "testid";

            StaffAssignmentController testcontroller = new StaffAssignmentController(logger);

            staffAssignments = testcontroller.GetByStaffId(teststaffid);

            Assert.IsNotNull(staffAssignments);

        }

        [TestMethod]
        public void GetCategoryID()
        {
            IEnumerable<StaffAssignmentModel> staffAssignments;

            string testcategoryid = "thetestid";

            StaffAssignmentController testcontroller = new StaffAssignmentController(logger);

            staffAssignments = testcontroller.GetByCategoryId(testcategoryid);

            Assert.IsNotNull(staffAssignments);

        }

    }
}
