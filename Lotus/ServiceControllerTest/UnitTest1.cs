using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace ServiceControllerTest
{
    [TestClass]
    public class UnitTest1
    {

        private static readonly ILogger<ServiceController> logger;

        [TestMethod]
        public void AddService()
        {
            bool expectedresult = true;
            bool actualresult = false;
            string testid = "testid";
            string testname = "testname";
            int testduration = 30;
            double testprice = 100.00;

            ServiceModel servicetest = new ServiceModel
            {
                Category_Id = testid,
                Name = testname,
                Duration = testduration,
                Price = testprice
            };

            ServiceController testcontroller = new ServiceController(logger);

            testcontroller.Post(servicetest);

            IEnumerable<ServiceModel> checkdatabase = testcontroller.Get(testid);

            using (var iterate = checkdatabase.GetEnumerator())
            {
                while (iterate.MoveNext())
                {
                    if (iterate.Current.Category_Id.Contains(testid))
                    {
                        actualresult = true;
                        break;
                    }
                }
            }

            Assert.AreEqual(expectedresult, actualresult);

        }

        [TestMethod]
        public void GetService()
        {
            IEnumerable<ServiceModel> services;
           
            string testid = "testid";

            ServiceController testcontroller = new ServiceController(logger);

            services = testcontroller.Get(testid);

            Assert.IsNotNull(services);
            
        }


        [TestMethod]
        public void UpdateService()
        {
            bool expectedresult = true;
            bool actualresult = false;
            int id = 0;
            string testid = "testid";
            string testname = "testname";
            string testname2 = "testname2";
            int testduration = 60;
            double testprice = 50.00;
            ServiceController testcontroller = new ServiceController(logger);
            List<ServiceModel> serviceslist = new List<ServiceModel>();

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
            
            if (id == 0)
            {
                Assert.IsFalse(true);
            }

            ServiceModel servicetest = new ServiceModel
            {
                Id = id,
                Name = testname2,
                Duration = testduration,
                Price = testprice
            };

            testcontroller.Update(servicetest);

            serviceslist = testcontroller.Get(testid).ToList();

            foreach (var service in serviceslist)
            {
                if (service.Name == testname2)
                {
                    actualresult = true;
                    break;
                }
            }
            
            Assert.AreEqual(expectedresult, actualresult);
        }

        
    }
}
