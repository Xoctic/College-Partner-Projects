using BoggleService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BoggleServiceLocalTests
{
    [TestClass]
    public class BoggleServiceLocalTests
    {
        [TestMethod]
        public void PostRegisterTest()
        {
            BoggleController controller = new BoggleController();

            string result = controller.PostRegister("Billy");

            Assert.IsNotNull(result);
            Assert.AreEqual(36, result.Length);

            try
            {
                controller.PostRegister("");
                Assert.Fail("Exception not found");
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

            try
            {
                controller.PostRegister(null);
                Assert.Fail("Exception not found");

            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);

            }
            
        }


        [TestMethod]
        public void GetTest1()
        {
            BoggleController controller = new BoggleController();
            string result = controller.PostRegister("Billy");

            
        }



        


    }
}
