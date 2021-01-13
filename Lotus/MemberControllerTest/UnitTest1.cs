using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace MemberControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<MembersController> logger;

        [TestMethod]
        public void AddService()
        {
            string testid = "test";
            string testname = "test";
            string testsurname = "test";
            string testnotes = "test";
            string testemail = "test";
            int phone = 0;

            MembersModel Memberstest = new MembersModel
            {
                Member_Id = testid,
                Name = testname,
                Surname = testsurname,
                Notes = testnotes,
                Email = testemail,
                Phone_Number = phone
            };

            MembersController testcontroller = new MembersController(logger);

            testcontroller.Post(Memberstest);

            IEnumerable<MembersModel> checkdatabase = testcontroller.GetAMember(testid);

            Assert.IsNotNull(checkdatabase);

        }

        [TestMethod]
        public void GetAll()
        {
            IEnumerable<MembersModel> Members;

            MembersController testcontroller = new MembersController(logger);

            Members = testcontroller.Get();

            Assert.IsNotNull(Members);
        }

        [TestMethod]
        public void GetbyID()
        {
            IEnumerable<MembersModel> Members;

            string testid = "test";

            MembersController testcontroller = new MembersController(logger);

            Members = testcontroller.GetAMember(testid);

            Assert.IsNotNull(Members);

        }

        [TestMethod]
        public void GetbySearch()
        {
            IEnumerable<MembersModel> Members;

            string testname = "test";

            MembersController testcontroller = new MembersController(logger);

            Members = testcontroller.Search(testname);

            Assert.IsNotNull(Members);

        }

        [TestMethod]
        public void Update()
        {
            bool expectedresult = true;
            bool actualresult = false;

            string testid = "test";
            string testname = "test2";
            string testsurname = "test2";
            string testnotes = "test2";
            string testemail = "test2";
            int phone = 1;

            MembersController testcontroller = new MembersController(logger);
            List<MembersModel> list = new List<MembersModel>();


            MembersModel Memberstest = new MembersModel
            {
                Member_Id = testid,
                Name = testname,
                Surname = testsurname,
                Notes = testnotes,
                Email = testemail,
                Phone_Number = phone
            };

            testcontroller.Update(Memberstest);

            list = testcontroller.Get().ToList();

            foreach (var Members in list)
            {
                if (Members.Name == testname)
                {
                    actualresult = true;
                    break;
                }
            }

            Assert.AreEqual(expectedresult, actualresult);
        }

      
    }
}
