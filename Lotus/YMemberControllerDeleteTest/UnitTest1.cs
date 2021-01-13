using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace YMemberControllerDeleteTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<MembersController> logger;

        [TestMethod]
        public void Delete()
        {
            IEnumerable<MembersModel> Members;

            string testid = "test";
            int firstcheck = 0;
            int secondcheck = 0;

            MembersController testcontroller = new MembersController(logger);

            Members = testcontroller.Get();

            using (var sequenceEnum = Members.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.Delete(testid);

            Members = testcontroller.Get();

            using (var sequenceEnum = Members.GetEnumerator())
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
