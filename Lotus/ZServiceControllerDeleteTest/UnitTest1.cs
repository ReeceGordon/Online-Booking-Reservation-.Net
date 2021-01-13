using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;


namespace ZServiceControllerDeleteTest
{

    [TestClass]
    public class UnitTest1
    {

        private static readonly ILogger<ServiceController> logger;

        [TestMethod]
        public void DeleteServiceByID()
        {
            IEnumerable<ServiceModel> services;

            int firstcheck = 0;
            int secondcheck = 0;

            int id = 0;
            string testid = "testid";
            string testname = "testname";
            int testduration = 30;
            double testprice = 100.00;
            List<ServiceModel> serviceslist = new List<ServiceModel>();
            ServiceController testcontroller = new ServiceController(logger);
          

            ServiceModel servicetest = new ServiceModel
            {
                Category_Id = testid,
                Name = testname,
                Duration = testduration,
                Price = testprice
            };



            testcontroller.Post(servicetest);

            serviceslist = testcontroller.Get(testid).ToList<ServiceModel>();

            foreach (var service in serviceslist)
            {
                Console.WriteLine(service.Id);
                if (service.Name == testname)
                {
                    id = service.Id;
                    break;
                }
            }

            services = testcontroller.Get(testid);


            using (var sequenceEnum = services.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.DeletewithService(id.ToString());
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
