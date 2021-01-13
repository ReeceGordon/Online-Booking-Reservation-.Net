using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;

namespace YCategoriesControllerDeleteTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<CategoriesController> logger;

        [TestMethod]
        public void DeleteCategory()
        {
            IEnumerable<CategoriesModel> categories;

            string testid = "testid";
            int firstcheck = 0;
            int secondcheck = 0;

            CategoriesController testcontroller = new CategoriesController(logger);

            categories = testcontroller.Get();

            using (var sequenceEnum = categories.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    firstcheck += 1;

                }
            }

            testcontroller.Delete(testid);
            categories = testcontroller.Get();

            using (var sequenceEnum = categories.GetEnumerator())
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
