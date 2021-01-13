using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Lotus.Server.Controllers;
using Lotus.Shared;
using System.Linq;

namespace OpeningControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILogger<OpeningController> logger;

        [TestMethod]
        public void GetOpening()
        {
            IEnumerable<OpeningModel> openings;

            OpeningController testcontroller = new OpeningController(logger);

            openings = testcontroller.Get();

            Assert.IsNotNull(openings);

        }


    }
}
