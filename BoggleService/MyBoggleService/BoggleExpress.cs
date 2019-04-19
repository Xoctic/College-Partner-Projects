using BoggleService.Controllers;
using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyBoggleService
{
    public class BoggleExpress
    {
        static void Main()
        {
            //BoggleController controller = new BoggleController();
            //String userToken = controller.PostRegister("Joe");
            //Console.WriteLine(userToken);

            new BoggleExpress(60000);

            // This is our way of preventing the main thread from            
            // exiting while the server is in use            
            Console.ReadLine();
        }

        //Listens for incoming connection requests
        private StringSocketListener server;

        public BoggleExpress(int port)
        {
            server = new StringSocketListener(port, new System.Text.UTF8Encoding());

            server.Start();

            server.BeginAcceptStringSocket(ConnectionRequested, null);
        }


      

    }
}
