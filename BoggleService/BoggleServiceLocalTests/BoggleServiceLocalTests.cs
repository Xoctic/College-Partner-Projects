using BoggleService.Controllers;
using BoggleService.Models;
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
        public void PostJoinGameTest()
        {
            BoggleController controller = new BoggleController();
            string result = controller.PostRegister("Billy");
            JoinGameInfo joinGame = new JoinGameInfo();

            //testing when userToken is invalid
            joinGame.timeLimit = 80;
            joinGame.userToken = "";

            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

            //testing when timeLimit < 5
            joinGame.userToken = Guid.NewGuid().ToString();
            joinGame.timeLimit = 0;

            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }


            //testing when timeLimis > 120
            joinGame.userToken = Guid.NewGuid().ToString();
            joinGame.timeLimit = 121;

            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

        }

        /// <summary>
        /// Testing joining a game with the same name
        /// </summary>
        [TestMethod]
        public void PostJoinGameTest2()
        {
            BoggleController controller = new BoggleController();
            JoinGameInfo joinGame = new JoinGameInfo();

            joinGame.userToken = controller.PostRegister("Billy");
            joinGame.timeLimit = 10;

            controller.PostJoinGame(joinGame);

            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Conflict, e.Response.StatusCode);
            }
        }

        /// <summary>
        /// Tests when usertoken passed in is invalid (null or not 36 characters long)
        /// </summary>
        [TestMethod]
        public void CancelJoinRequestTest()
        {
            BoggleController controller = new BoggleController();
            JoinGameInfo joinGame = new JoinGameInfo();

            joinGame.userToken = controller.PostRegister("Billy");
            joinGame.timeLimit = 10;

            controller.PostJoinGame(joinGame);

            try
            {
                controller.PutCancelJoin(null);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

            try
            {
                controller.PutCancelJoin("");
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

        }

        [TestMethod]
        public void PlayWordTest()
        {

        }



        


    }
}
