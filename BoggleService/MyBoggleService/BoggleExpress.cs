
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

        // All the clients that have connected but haven't closed
        private List<ClientConnection> clients = new List<ClientConnection>();

        // Read/write lock to coordinate access to the clients list
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        
        public BoggleExpress(int port)
        {
            
            server = new StringSocketListener(port, new System.Text.UTF8Encoding());

            server.Start();

            server.BeginAcceptStringSocket(ConnectionRequested, "yo");

            
        }

        private void ConnectionRequested(StringSocket ss, object payload)
        {
            
            server.BeginAcceptStringSocket(ConnectionRequested, payload);

            try
            {
                sync.EnterWriteLock();
                clients.Add(new ClientConnection(ss, this, payload));              
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }

        /// <summary>
        /// Remove c from the client list.
        /// </summary>
        public void RemoveClient(ClientConnection c)
        {
            try
            {
                sync.EnterWriteLock();
                clients.Remove(c);
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }

        public class ClientConnection
        {
            // Incoming/outgoing is UTF8-encoded.  This is a multi-byte encoding.  The first 128 Unicode characters
            // (which corresponds to the old ASCII character set and contains the common keyboard characters) are
            // encoded into a single byte.  The rest of the Unicode characters can take from 2 to 4 bytes to encode.
            private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            //The string socket through which we communicate with the remote client.
            private StringSocket ss;

            // Text that has been received from the client but not yet dealt with
            private String incoming;

            // Text that needs to be sent to the client but which we have not yet started sending
            private String outgoing;

            private int contentLength = 0;

            private bool payloadReady = false;

            // For decoding incoming UTF8-encoded byte streams.
            private Decoder decoder = encoding.GetDecoder();

            // Records whether an asynchronous send attempt is ongoing
            private bool sendIsOngoing = false;

            // For synchronizing sends
            private readonly object sendSync = new object();

            private BoggleExpress server;

            private BoggleController bController;

            public ClientConnection(StringSocket sc, BoggleExpress ex, object payload)
            {
                this.server = ex;
                ss = sc;
                incoming = "";
                outgoing = "";

                bController = new BoggleController();

                try
                {
                    ss.BeginReceive(MessageRecieved, null, 0);   
                    
                }
                catch
                {

                }
            }         

            private void MessageRecieved(string s, object payload)
            {
                string[] words;
                

                if (s != null && payloadReady)
                {
                    payload = s;                 
                    s = null;
                }
                if (s != null && !payloadReady)
                {
                    words = s.Split(':');
                    if(words[0] == "Content-Length")
                    {
                        if(words[1] != null)
                        {
                            words[1] = words[1].Trim();
                            contentLength = Convert.ToInt32(words[1]);
                        }
                    }
                    if(words[0] == "\r")
                    {
                        payloadReady = true;
                        ss.BeginReceive(MessageRecieved, payload, contentLength);
                    }
                    incoming += s + "\n";
                }

                if(payloadReady != true)
                {
                    ss.BeginReceive(MessageRecieved, payload, 0);
                }

                if (s == null)
                {
                    
                    dynamic info = new ExpandoObject();
                    StringReader reader = new StringReader(incoming);
                    if((string)payload != null)
                    {

                        info = JsonConvert.DeserializeObject(payload.ToString());

                        //if (payload.ToString()[0] == '{')
                        //{
                        //    info = JsonConvert.DeserializeObject(payload.ToString());
                        //}
                    }             
                    string line = reader.ReadLine();
                    char[] array = line.ToCharArray();
                    outgoing = null;
                    string code = null;
                    int contentLength = 0;

                    while (line != "" && line != null)
                    {
                        if (array[0] == 'P')
                        {
                            words = line.Split('/');

                            if (array[1] == 'U' && array[2] == 'T')
                            {
                                //putCancel
                                if (words.Length == 4)
                                {
                                    try
                                    {
                                        string token = payload.ToString();

                                        token = token.Replace("\"", "");

                                        bController.PutCancelJoin(token);

                                        code = "204 NoContent";
                                    }
                                    catch(Exception e)
                                    {
                                        if (e is HttpResponseException)
                                        {
                                            code = "403 Forbidden";
                                        }
                                    }
                                    SetOutgoingMessage(code, contentLength);


                                    //payload = null;

                                    ss.BeginSend(outgoing, MessageSent, new object());

                                }
                                //putPlayWord
                                else if (words.Length == 5)
                                {
                                    PlayWordInput input;
                                    String userT;
                                    String word;
                                    string output = "";
                                    int score = 0;

                                    userT = info.UserToken;
                                    word = info.Word;

                                    input = new PlayWordInput(userT, word);
                                    words[3] = words[3].Remove(words[3].Length - 5, 5);
                                    

                                    try
                                    {                  
                                        score = bController.PutPlayWord(words[3], input);
                                        output = JsonConvert.SerializeObject(score);

                                        contentLength = encoding.GetByteCount(output.ToCharArray());

                                        code = "200 OK";
                                    }
                                    catch (Exception e)
                                    {
                                        if (e is HttpResponseException)
                                        {
                                            HttpResponseException httpResponse = (HttpResponseException)e;
                                            if (httpResponse.Code == HttpStatusCode.Forbidden)
                                            {
                                                code = "403 Forbidden";
                                            }
                                            if (httpResponse.Code == HttpStatusCode.Conflict)
                                            {
                                                code = "409 Conflict";
                                            }
                                        }
                                    }
                                    SetOutgoingMessage(code, contentLength);
                                    outgoing += output;

                                    ss.BeginSend(outgoing, MessageSent, new object());
                                    //send message
                                }

                            }
                            else if (array[1] == 'O' && array[2] == 'S' && array[3] == 'T')
                            {
                                if (words.Length == 4)
                                {
                                    //postRegister
                                    if (words[2] == "users HTTP")
                                    {
                                        dynamic output = new ExpandoObject();
                                        string output2 = "";

                                        try
                                        {
                                            //if((string)payload == "null" || (string)payload == "")
                                            if(info ==null || info == "")
                                            {
                                                output = bController.PostRegister(null);
                                            }
                                            else
                                            {
                                                output = bController.PostRegister(payload.ToString());
                                            }
                                            output2 = JsonConvert.SerializeObject(output);

                                            contentLength = encoding.GetByteCount(output2.ToCharArray());

                                            code = "200 OK";
                                        }
                                        catch (Exception e)
                                        {
                                            if(e is HttpResponseException)
                                            {
                                                code = "403 Forbidden";
                                            }
                                        }
                                        SetOutgoingMessage(code, contentLength);
                                        outgoing += output2;

                                        ss.BeginSend(outgoing, MessageSent, new object());
                                        //send message
                                    }
                                    //postJoinGame
                                    else if (words[2] == "games HTTP")
                                    {
                                        string userT;
                                        int time;
                                        dynamic output;
                                        string output2 = "";

                                        userT = info.UserToken;
                                        time = info.TimeLimit;

                                        JoinGameInput input = new JoinGameInput(time, userT);

                                        try
                                        {
                                            output = bController.PostJoinGame(input);
                                            output2 = JsonConvert.SerializeObject(output);

                                            contentLength = encoding.GetByteCount(output2.ToCharArray());

                                            code = "200 OK";
                                        }
                                        catch (Exception e)
                                        {
                                            if (e is HttpResponseException)
                                            {
                                                HttpResponseException httpResponse = (HttpResponseException)e;
                                                if(httpResponse.Code == HttpStatusCode.Forbidden)
                                                {
                                                    code = "403 Forbidden";
                                                }
                                                if(httpResponse.Code == HttpStatusCode.Conflict)
                                                {
                                                    code = "409 Conflict";
                                                }
                                            }
                                        }
                                        SetOutgoingMessage(code, contentLength);
                                        outgoing += output2;

                                        ss.BeginSend(outgoing, MessageSent, new object());
                                        //send message
                                    }
                                }
                            }
                        }
                        else if (array[0] == 'G' && array[1] == 'E' && array[2] == 'T')
                        {
                            words = line.Split('/');

                            if (words.Length == 6)
                            {
                                //words[4] = words[4].Remove(words[4].Length - 6, 5);
                                words[4] = words[4].Trim();
                                //words[3] = words[3].Remove(words[3].Length - 6, 5);
                                words[3] = words[3].Trim();

                                if (words[4] == "true")
                                {
                                    try
                                    {
                                        payload = bController.GetGameStatus(words[3], true);
                                        payload = JsonConvert.SerializeObject(payload);

                                        contentLength = encoding.GetByteCount(payload.ToString().ToCharArray());


                                        code = "200 OK";
                                    }
                                    catch (Exception e)
                                    {
                                        if (e is HttpResponseException)
                                        {
                                            code = "403 Forbidden";
                                        }
                                    }
                                    SetOutgoingMessage(code, contentLength);

                                    ss.BeginSend(outgoing, MessageSent, payload);
                                    //send message
                                }
                                else if (words[4] == "false")
                                {
                                    try
                                    {
                                        payload = bController.GetGameStatus(words[3], false);
                                        payload = JsonConvert.SerializeObject(payload);

                                        contentLength = encoding.GetByteCount(payload.ToString().ToCharArray());

                                        code = "200 OK";
                                    }
                                    catch (Exception e)
                                    {
                                        if (e is HttpResponseException)
                                        {
                                            code = "403 Forbidden";
                                        }
                                    }
                                    SetOutgoingMessage(code, contentLength);

                                    ss.BeginSend(outgoing, MessageSent, payload);
                                    //send message
                                }
                            }
                        }
                        line = reader.ReadLine();
                        //line = line.Trim();
                        if(line != null)
                        {
                            array = line.ToCharArray();
                        }
                    }
                }
                //try
                //{
                //    ss.BeginReceive(MessageRecieved, payload, );
                //}
                //catch
                //{

                //}
            }

            private void MessageSent(bool wasSent, object payload)
            {
                lock (sendSync)
                {
                    if (wasSent == true)
                    {
                        ss.Close();
                        server.RemoveClient(this);
                        Console.WriteLine("Socket closed");
                    }
                    else
                    {

                    }
                }
            }

            /// <summary>
            /// Helper method to assist in setting the outgoing string properly.
            /// </summary>
            /// <param name="code"></param>
            /// <param name="contentLength"></param>
            private void SetOutgoingMessage(string code, int contentLength)
            {
                outgoing += "HTTP/1.1 " + code + "\r\n";
                outgoing += "Content-Length: " + contentLength + "\r\n";
                outgoing += "Content-Type: application/json; charset=utf-8\r\n";
                outgoing += "\r\n";
            }
        }
    }
}
