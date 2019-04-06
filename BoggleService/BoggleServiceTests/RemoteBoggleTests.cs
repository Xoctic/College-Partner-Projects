using System;
using System.Text;
 using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using static System.Net.HttpStatusCode;
using static System.Net.Http.HttpMethod;
using System.Diagnostics;
using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using static System.Net.HttpStatusCode;
using static System.Net.Http.HttpMethod;
using System.Diagnostics;
using BoggleService.Models;

namespace BoggleTests
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// 
    /// Starting the service this way allows code coverage statistics for the service
    /// to be collected.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        // Full path to IIS_EXPRESS executable
        private const string IIS_EXECUTABLE = @"C:\Program Files (x86)\IIS Express\iisexpress.exe";

        // Command line arguments to IIS_EXPRESS
        private const string ARGUMENTS = @"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""";

        

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start()
        {
            if (process == null)
            {               
                ProcessStartInfo info = new ProcessStartInfo(IIS_EXECUTABLE, ARGUMENTS);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }

    [TestClass]
    public class ToDoTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server.
        /// </summary>
        // Remove the comment before the annotation if you want to make it work
        // [ClassInitialize]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start();
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        // Remove the comment before the annotation if you want to make it work
        // [ClassCleanup]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        //private RestClient client = new RestClient("http://localhost:60000/BoggleService/");


        //Tests register user with an emppty string as the name
        [TestMethod]
        public void TestMethod1()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user = "";
            Response r = client.DoMethodAsync("POST", "users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        //Tests register user with a null string as the name
        [TestMethod]
        public void TestMethod2()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user = null;
            Response r = client.DoMethodAsync("POST", "users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        //Tests register user with a larger than 50 character string as the name
        [TestMethod]
        public void TestMethod3()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user = "llllllllllllllllllllllllllllllllllllllllllllllllllllllllllll";
            Response r = client.DoMethodAsync("POST", "users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        //Tests register user with a valid string as the name
        [TestMethod]
        public void TestMethod4()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user = "Aric";
            Response r = client.DoMethodAsync("POST", "users", user).Result;
            Assert.AreEqual(OK, r.Status);
        }

        //Tests all conflicts involved with the join game method
        [TestMethod]
        public void TestMethod5()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user1 = "Aric";
            string user2 = "Andrew";
            Response r1 = client.DoMethodAsync("POST", "users", user1).Result;
            Response r2 = client.DoMethodAsync("POST", "users", user2).Result;
            Assert.AreEqual(OK, r1.Status);
            Assert.AreEqual(OK, r2.Status);

            JoinGameInput badJoinGame1 = new JoinGameInput(1, r1.Data);
            JoinGameInput badJoinGame2 = new JoinGameInput(150, r2.Data);
            JoinGameInput badJoinGame3 = new JoinGameInput(100, "poop");


            JoinGameInput goodJoinGame1 = new JoinGameInput(50, r1.Data);
            JoinGameInput goodJoinGame2 = new JoinGameInput(100, r2.Data);
            

            Response r3 = client.DoMethodAsync("POST", "games", badJoinGame1).Result;
            Assert.AreEqual(Forbidden, r3.Status);

            r3 = client.DoMethodAsync("POST", "games", badJoinGame2).Result;
            Assert.AreEqual(Forbidden, r3.Status);

            r3 = client.DoMethodAsync("POST", "games", badJoinGame3).Result;
            Assert.AreEqual(Forbidden, r3.Status);


            r3 = client.DoMethodAsync("POST", "games", goodJoinGame1).Result;
            Assert.AreEqual(OK, r3.Status);

            try
            {
                r3 = client.DoMethodAsync("POST", "games", goodJoinGame1).Result;
            }
            catch
            {
                Assert.AreEqual(Conflict, r3.Status);
            }

            r3 = client.DoMethodAsync("POST", "games", goodJoinGame2).Result;
            Assert.AreEqual(OK, r3.Status);
        }

        //Tests all conflicts involved with cancelJoinGame method
        [TestMethod]
        public void TestMethod6()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user1 = "Aric";
            Response r1 = client.DoMethodAsync("POST", "users", user1).Result;
            Assert.AreEqual(OK, r1.Status);
            string user1Token = r1.Data;
            JoinGameInput goodJoinGame1 = new JoinGameInput(50, r1.Data);


            r1 = client.DoMethodAsync("POST", "games", goodJoinGame1).Result;
            Assert.AreEqual(OK, r1.Status);

            r1 = client.DoMethodAsync("PUT", "games", user1Token).Result;
            Assert.AreEqual(NoContent, r1.Status);

            r1 = client.DoMethodAsync("PUT", "games", "whoyoucallinpinheadlarrypatrickstar???").Result;
            Assert.AreEqual(Forbidden, r1.Status);
        }

        //Tests all response codes for method put play word
        [TestMethod]
        public void TestMethod7()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user1 = "Aric";
            string user2 = "Andrew";
            Response r1 = client.DoMethodAsync("POST", "users", user1).Result;
            Response r2 = client.DoMethodAsync("POST", "users", user2).Result;
            Assert.AreEqual(OK, r1.Status);
            Assert.AreEqual(OK, r2.Status);

            string user1Token = r1.Data;
            string user2Token = r2.Data;

            JoinGameInput goodJoinGame1 = new JoinGameInput(50, r1.Data);
            JoinGameInput goodJoinGame2 = new JoinGameInput(100, r2.Data);

            Response r3 = client.DoMethodAsync("POST", "games", goodJoinGame1).Result;
            Assert.AreEqual(OK, r3.Status);

            r3 = client.DoMethodAsync("POST", "games", goodJoinGame2).Result;
            Assert.AreEqual(OK, r3.Status);

            PlayWordInput input = new PlayWordInput(user1Token, null);
            r3 = client.DoMethodAsync("PUT", "games/G1", input).Result;

            Assert.AreEqual(Forbidden, r3.Status);

            input = new PlayWordInput(user1Token, "lllllllllllllllllllllllllllllllllll");
            r3 = client.DoMethodAsync("PUT", "games/G1", input).Result;

            Assert.AreEqual(Forbidden, r3.Status);

            input = new PlayWordInput(user1Token, "Hi");
            r3 = client.DoMethodAsync("PUT", "games/GG3", input).Result;

            Assert.AreEqual(Forbidden, r3.Status);

            input = new PlayWordInput("clerenge", "Hi");
            r3 = client.DoMethodAsync("PUT", "games/G1", input).Result;

            Assert.AreEqual(Forbidden, r3.Status);


            
        }

        //Tests all response codes within the gameStatus method
        [TestMethod]
        public void TestMethod8()
        {
            RestClient client = new RestClient("http://localhost:60000/BoggleService/");
            string user1 = "Aric";
            string user2 = "Andrew";
            Response r1 = client.DoMethodAsync("POST", "users", user1).Result;
            Response r2 = client.DoMethodAsync("POST", "users", user2).Result;
            Assert.AreEqual(OK, r1.Status);
            Assert.AreEqual(OK, r2.Status);

            Response r3 = client.DoMethodAsync("GET", "games/G11/true").Result;

            Assert.AreEqual(Forbidden, r3.Status);

            r3 = client.DoMethodAsync("GET", "games/G1/true").Result;

            Assert.AreEqual(OK, r3.Status);

        }


    }



   

}

