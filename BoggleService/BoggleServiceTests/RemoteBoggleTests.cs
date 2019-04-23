using System;
using System.Text;
 using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using static System.Net.HttpStatusCode;
using static System.Net.Http.HttpMethod;
using System.Diagnostics;
using Controller;

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
    } 
}

