using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;

namespace CategoriesControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<CategoriesController> logger;

        [TestMethod]
        public void AddCategory()
        {
            bool expectedresult = true;
            bool actualresult = false;

            string testid = "testid";
            string testname = "testname";
            string testdescription = "testdescription";

            CategoriesModel categoriestest = new CategoriesModel
            {
                Id = testid,
                Name = testname,
                Description = testdescription
            };

            CategoriesController testcontroller = new CategoriesController(logger);

            testcontroller.Post(categoriestest);

            IEnumerable<CategoriesModel> checkdatabase = testcontroller.Get();

            using (var iterate = checkdatabase.GetEnumerator())
            {
                while (iterate.MoveNext())
                {
                    if (iterate.Current.Name.Contains(testname))
                    {
                        actualresult = true;
                        break;
                    }
                }
            }

            Assert.AreEqual(expectedresult, actualresult);

        }

        [TestMethod]
        public void GetCategory()
        {
            IEnumerable<CategoriesModel> categories;

            CategoriesController testcontroller = new CategoriesController(logger);

            categories = testcontroller.Get();

            Assert.IsNotNull(categories);
           
        }

        [TestMethod]
        public void UpdateCategory()
        {
            IEnumerable<CategoriesModel> categories;
            bool expectedresult = true;
            bool actualresult = false;
            string testid = "testid";
            string testname = "testname2";
            string testdescription = "testdescription2";

            CategoriesModel categoriestest = new CategoriesModel
            {
                Id = testid,
                Name = testname,
                Description = testdescription
            };

            CategoriesController testcontroller = new CategoriesController(logger);

            testcontroller.Update(categoriestest);

            categories = testcontroller.Get();

            using (var iterate = categories.GetEnumerator())
            {
                while (iterate.MoveNext())
                {
                    if (iterate.Current.Id.Contains(testid))
                    {
                        if (iterate.Current.Name.Contains(testname))
                        {
                            actualresult = true;
                            break;
                        }
                    }
                }
            }

            Assert.AreEqual(expectedresult, actualresult);
        }

      
    }
}
