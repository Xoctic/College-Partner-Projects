
using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Express
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

        private StringSocketClient client;

        // All the clients that have connected but haven't closed
        private List<StringSocketClient> clients = new List<StringSocketClient>();

        // Read/write lock to coordinate access to the clients list
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        

        public BoggleExpress(int port)
        {
            client = new StringSocketClient(" ", port, new System.Text.UTF8Encoding());
            server = new StringSocketListener(port, new System.Text.UTF8Encoding());

            server.Start();

            

            server.BeginAcceptStringSocket(ConnectionRequested, null);

            
        }

        private void ConnectionRequested(StringSocket ss, object payload)
        {
            ss = server.AcceptStringSocket();


            server.BeginAcceptStringSocket(ConnectionRequested, null);

            try
            {
                sync.EnterWriteLock();
                

               
            }
            catch
            {

            }

        }




        public class ClientConnection
        {
            private StringSocket ss;

            private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            // Text that has been received from the client but not yet dealt with
            private String incoming;

            // Text that needs to be sent to the client but which we have not yet started sending
            private String outgoing;

            // For decoding incoming UTF8-encoded byte streams.
            private Decoder decoder = encoding.GetDecoder();

            // Records whether an asynchronous send attempt is ongoing
            private bool sendIsOngoing = false;

            // For synchronizing sends
            private readonly object sendSync = new object();

            private BoggleExpress server;

            public ClientConnection(StringSocketClient sc, BoggleExpress ex)
            {
                this.server = server;
                ss = sc.Client;

                incoming = "";
                outgoing = "";


                try
                {
                    ss.BeginReceive();

                    ReceiveCallback c = new ReceiveCallback();
                }
                catch
                {

                }



            }
           
            


           


        }







        


      

    }
}
