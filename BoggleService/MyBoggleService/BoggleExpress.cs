
using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

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
        private List<ClientConnection> clients = new List<ClientConnection>();

        // Read/write lock to coordinate access to the clients list
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        

        public BoggleExpress(int port)
        {
            client = new StringSocketClient("localhost", port, new System.Text.UTF8Encoding());
            server = new StringSocketListener(port, new System.Text.UTF8Encoding());
            

            server.Start();

            

            server.BeginAcceptStringSocket(ConnectionRequested, null);

            
        }

        private void ConnectionRequested(StringSocket ss, object payload)
        {

            ss = client.Client;
            

            server.BeginAcceptStringSocket(ConnectionRequested, null);

            try
            {
                sync.EnterWriteLock();
                clients.Add(new ClientConnection(ss, this));

               
            }
            finally
            {
                sync.ExitWriteLock();
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

            private BoggleController bController;

            public ClientConnection(StringSocket sc, BoggleExpress ex)
            {
                this.server = ex;
                ss = sc;
                incoming = "";
                outgoing = "";

                bController = new BoggleController();

                try
                {
                    ss.BeginReceive(MessageRecieved, new object(), 0);

                    
                }
                catch
                {

                }



            }

            private void MessageRecieved(string s, object payload)
            {
                string[] words;
                
                dynamic info = JsonConvert.DeserializeObject(payload.ToString());
                StringReader reader = new StringReader(s);
                string line = reader.ReadLine();
                line = line.Remove(line.Length - 5, 4);
                char[] array = line.ToCharArray();
                

                while(line != null)
                {
                    if(array[0] == 'P')
                    {
                        words = line.Split('/');

                        if (array[1] == 'U' && array[2] == 'T')
                        {
                            

                            if(words.Length == 4)
                            {
                                bController.PutCancelJoin(payload.ToString());
                                //send message
                            }
                            else if(words.Length == 5)
                            {
                                PlayWordInput input;
                                String userT;
                                String word;

                                userT = info.UserToken;
                                word = info.Word;

                                input = new PlayWordInput(userT, word);
                                words[3] = words[3].Remove(words[3].Length - 6, 5);

                                bController.PutPlayWord(words[3] , input);
                                //send message

                            }

                        }
                        else if(array[1] == 'O' && array[2] == 'S' && array[3] == 'T')
                        {
                            if(words.Length == 5)
                            {
                                

                                if (words[3] == "users")
                                {
                                    bController.PostRegister(payload.ToString());
                                    //send message
                                }
                                else if(words[3] == "games")
                                {
                                    string userT;
                                    int time;

                                    userT = info.UserToken;
                                    time = info.TimeLimit;

                                    JoinGameInput input = new JoinGameInput(time, userT);

                                    bController.PostJoinGame(input);
                                    //send message
                                }
                            }


                        }
                    }
                    else if(array[0] == 'G' && array[1] == 'E' && array[2] == 'T')
                    {
                        words = line.Split('/');

                        if(words.Length == 6)
                        {
                            words[4] = words[4].Remove(words[4].Length - 6, 5);
                            words[3] = words[3].Remove(words[3].Length - 6, 5);

                            if (words[4] == "true")
                            {
                                bController.GetGameStatus(words[3], true);
                                //send message
                            }
                            else if(words[4] == "false")
                            {
                                bController.GetGameStatus(words[3], false);
                                //send message
                            }
                        }
                    }

                    line = reader.ReadLine();
                    line = line.Remove(line.Length - 5, 4);
                    array = line.ToCharArray();
                }



            }
           
            


           


        }







        


      

    }
}
