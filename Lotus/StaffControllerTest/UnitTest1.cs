using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace StaffControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<StaffController> logger;

        [TestMethod]
        public void AddService()
        {
            string testid = "test";
            string testname = "test";
            string testsurname = "test";
            string testdetails = "test";
            string testemail = "test";
            int phone = 0;
            string testimage = "test";

            StaffModel stafftest = new StaffModel
            {
                Staff_Id = testid,
                Name = testname,
                Surname = testsurname,
                Details = testdetails,
                Email = testemail,
                Phone_Number = phone,
                Image = testimage
            };

            StaffController testcontroller = new StaffController(logger);

            testcontroller.Post(stafftest);

            IEnumerable<StaffModel> checkdatabase = testcontroller.GetAStaff(testid);

            Assert.IsNotNull(checkdatabase);

        }

        [TestMethod]
        public void GetAll()
        {
            IEnumerable<StaffModel> openings;

            StaffController testcontroller = new StaffController(logger);

            openings = testcontroller.Get();

            Assert.IsNotNull(openings);
        }

        [TestMethod]
        public void GetbyID()
        {
            IEnumerable<StaffModel> openings;

            string testid = "test";

            StaffController testcontroller = new StaffController(logger);

            openings = testcontroller.GetAStaff(testid);

            Assert.IsNotNull(openings);

        }

        [TestMethod]
        public void GetbySearch()
        {
            IEnumerable<StaffModel> openings;

            string testname = "test";

            StaffController testcontroller = new StaffController(logger);

            openings = testcontroller.Search(testname);

            Assert.IsNotNull(openings);

        }

        [TestMethod]
        public void Update()
        {
            bool expectedresult = true;
            bool actualresult = false;

            string testid = "test";
            string testname = "test2";
            string testsurname = "test2";
            string testdetails = "test2";
            string testemail = "test2";
            int phone = 1;
            string testimage = "test2";

            StaffController testcontroller = new StaffController(logger);
            List<StaffModel> list = new List<StaffModel>();

         
            StaffModel stafftest = new StaffModel
            {
                Staff_Id = testid,
                Name = testname,
                Surname = testsurname,
                Details = testdetails,
                Email = testemail,
                Phone_Number = phone,
                Image = testimage
            };

            testcontroller.Update(stafftest);

            list = testcontroller.Get().ToList();

            foreach (var staff in list)
            {
                if (staff.Name == testname)
                {
                    actualresult = true;
                    break;
                }
            }

            Assert.AreEqual(expectedresult, actualresult);
        }

    }
}
