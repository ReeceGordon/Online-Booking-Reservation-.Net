using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace YServiceControllerDeleteTest
{
    [TestClass]
    public class UnitTest1
    {

        private static readonly ILogger<ServiceController> logger;

        [TestMethod]
        public void DeleteServiceByCategory()
        {
            IEnumerable<ServiceModel> services;

            string testid = "testid";
            int firstcheck = 0;
            int secondcheck = 0;

            ServiceController testcontroller = new ServiceController(logger);

            services = testcontroller.Get(testid);

            using (var sequenceEnum = services.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.DeletewithCategory(testid);

            services = testcontroller.Get(testid);

            using (var sequenceEnum = services.GetEnumerator())
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
