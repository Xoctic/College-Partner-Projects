
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
        
        //constructor for the server, initializes a stringSocketListener to listen for a connection request
        public BoggleExpress(int port)
        {
            server = new StringSocketListener(port, new System.Text.UTF8Encoding());

            server.Start();

            server.BeginAcceptStringSocket(ConnectionRequested, "yo mama");
        }

        /// <summary>
        ///callback method for beginAcceptStringSocket, handles when a connection has been requested
        ///adds a clientConnection to the clients list using the stringSocket created after the connection was requested and passed in to this method
        ///the client connection also recieves in its parameter the current BoggleExpress object
        ///all of this is done within a read/write lock
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="payload"></param>
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
        /// Remove the client connection c from the client list.
        /// does this in a read/write lock
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

        /// <summary>
        /// class which deals with sending and recieving messages through a stringSocket
        /// able to be used on multiple threads
        /// </summary>
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

            //Represents the length of the content(body) of a message
            private int contentLength = 0;

            //Flag wich signifies the payload is ready to be read from the string s in the messageRecieved method
            private bool payloadReady = false;

            //Flag which signifies the header contains a Get Request
            private bool isGetRequest = false;

            // For decoding incoming UTF8-encoded byte streams.
            private Decoder decoder = encoding.GetDecoder();

            private string code = null;

            // For synchronizing sends
            private readonly object sendSync = new object();

            //Represents the BoggleExpress object that is associated with the stringSocket contained in this clientConnection
            private BoggleExpress server;
            
            //Represents the BoggleController object that is associated with this clientConnection
            private BoggleController bController;

            /// <summary>
            /// constructor for the clientConnection class
            /// the parameters sc(stringSocket), ex(BoggleExpress), payload(object) are all passed in from the connectionRequested callback method
            /// initializes the stringSocket to be used for this clientConnection equal to the stringSocket passed in from the connectionRequested callback method
            /// initializes the BoggleController to be used for this client connection to be equal to the one passed in from the connectionRequested callback method
            /// initializes a new BoggleController object to be used for this specific clientConnection
            /// </summary>
            /// <param name="sc"></param>
            /// <param name="ex"></param>
            /// <param name="payload"></param>
            public ClientConnection(StringSocket sc, BoggleExpress ex, object payload)
            {
                this.server = ex;
                ss = sc;
                incoming = null;
                outgoing = null;

                bController = new BoggleController();

                try
                {
                    ss.BeginReceive(MessageRecieved, null, 0);            
                }
                catch(ObjectDisposedException)
                {

                }
            }         

            /// <summary>
            /// Handles when a meesage has been recieved from the client through the stringSocket
            /// </summary>
            /// <param name="s"></param>
            /// <param name="payload"></param>
            private void MessageRecieved(string s, object payload)
            {
                //array of words in the command from the client
                string[] words;
                try
                {
                    if (s != null && payloadReady)
                    {
                        payload = s;
                        s = null;
                    }
                    if (s != null && !payloadReady)
                    {
                        words = s.Split(':');
                        if (words[0] == "Content-Length")
                        {
                            if (words[1] != null)
                            {
                                words[1] = words[1].Trim();
                                contentLength = Convert.ToInt32(words[1]);
                            }
                        }
                        if (words[0] == "\r")
                        {
                            payloadReady = true;
                            if (isGetRequest == true)
                            {
                                s = null;
                            }
                            else
                            {
                                ss.BeginReceive(MessageRecieved, payload, contentLength);
                            }
                        }
                        incoming += s + "\n";
                    }

                    if (payloadReady != true)
                    {
                        words = s.Split('/');
                        if (words[0].Trim() == "GET")
                        {
                            isGetRequest = true;
                        }
                        ss.BeginReceive(MessageRecieved, payload, 0);
                    }
                    if (s == null)
                    {
                        dynamic info = new ExpandoObject();
                        StringReader reader = new StringReader(incoming);
                        if ((string)payload != null)
                        {
                            info = JsonConvert.DeserializeObject(payload.ToString());
                        }
                        string line = reader.ReadLine();
                        bool httpExceptionThrown = false;
                        char[] array = line.ToCharArray();
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
                                        catch (Exception e)
                                        {
                                            if (e is HttpResponseException)
                                            {
                                                code = "403 Forbidden";
                                                httpExceptionThrown = true;
                                            }
                                            if (httpExceptionThrown == false)
                                            {
                                                code = "500 Internal Server Error";
                                            }
                                        }
                                        try
                                        {
                                            SetOutgoingMessage(code, contentLength);
                                            ss.BeginSend(outgoing, MessageSent, new object());
                                        }
                                        catch (Exception ex)
                                        {
                                            ss.Close();
                                        }

                                    }
                                    //putPlayWord
                                    else if (words.Length == 5)
                                    {
                                        PlayWordInput input;
                                        String userT;
                                        String word;
                                        string output = null;
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
                                                    httpExceptionThrown = true;
                                                }
                                                if (httpResponse.Code == HttpStatusCode.Conflict)
                                                {
                                                    code = "409 Conflict";
                                                    httpExceptionThrown = true;
                                                }
                                            }
                                            if (httpExceptionThrown == false)
                                            {
                                                code = "500 Internal Server Error";
                                            }
                                        }
                                        try
                                        {
                                            SetOutgoingMessage(code, contentLength);
                                            outgoing += output;
                                            ss.BeginSend(outgoing, MessageSent, new object());
                                        }
                                        catch (Exception ex)
                                        {
                                            ss.Close();
                                        }
                                    }

                                }
                                else if (array[1] == 'O' && array[2] == 'S' && array[3] == 'T')
                                {
                                    if (words.Length == 4)
                                    {
                                        //postRegister
                                        if (words[2] == "users HTTP")
                                        {
                                            dynamic output;
                                            string output2 = null;

                                            try
                                            {
                                                //if((string)payload == "null" || (string)payload == "")
                                                if (info == null || info == "")
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
                                                if (e is HttpResponseException)
                                                {
                                                    code = "403 Forbidden";
                                                    httpExceptionThrown = true;
                                                }
                                                if (httpExceptionThrown == false)
                                                {
                                                    code = "500 Internal Server Error";
                                                }
                                            }
                                            try
                                            {
                                                SetOutgoingMessage(code, contentLength);
                                                outgoing += output2;
                                                ss.BeginSend(outgoing, MessageSent, new object());
                                            }
                                            catch (Exception ex)
                                            {
                                                ss.Close();
                                            }
                                        }
                                        //postJoinGame
                                        else if (words[2] == "games HTTP")
                                        {
                                            string userT;
                                            int time;
                                            dynamic output;
                                            string output2 = null;

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
                                                    if (httpResponse.Code == HttpStatusCode.Forbidden)
                                                    {
                                                        code = "403 Forbidden";
                                                        httpExceptionThrown = true;
                                                    }
                                                    if (httpResponse.Code == HttpStatusCode.Conflict)
                                                    {
                                                        code = "409 Conflict";
                                                        httpExceptionThrown = true;
                                                    }
                                                }
                                                if (httpExceptionThrown == false)
                                                {
                                                    code = "500 Internal Server Error";
                                                }
                                            }
                                            try
                                            {
                                                SetOutgoingMessage(code, contentLength);
                                                outgoing += output2;
                                                ss.BeginSend(outgoing, MessageSent, new object());
                                            }
                                            catch(Exception ex)
                                            {
                                                ss.Close();
                                            }
                                        }
                                    }
                                }
                            }
                            else if (array[0] == 'G' && array[1] == 'E' && array[2] == 'T')
                            {
                                words = line.Split('/');

                                if (words.Length == 6)
                                {
                                    GameInfo output;
                                    string output2 = null;

                                    if (words[4] == "True HTTP")
                                    {
                                        try
                                        {
                                            output = bController.GetGameStatus(words[3], true);

                                            if (output.GameState != "pending")
                                            {
                                                output.Player1.Nickname = output.Player1.Nickname.Replace("\"", "");
                                                output.Player2.Nickname = output.Player2.Nickname.Replace("\"", "");
                                            }

                                            output2 = JsonConvert.SerializeObject(output);

                                            contentLength = encoding.GetByteCount(output2.ToCharArray());

                                            code = "200 OK";
                                        }
                                        catch (Exception e)
                                        {
                                            if (e is HttpResponseException)
                                            {
                                                code = "403 Forbidden";
                                                httpExceptionThrown = true;
                                            }
                                            if (httpExceptionThrown == false)
                                            {
                                                code = "500 Internal Server Error";
                                            }
                                        }
                                        try
                                        {
                                            SetOutgoingMessage(code, contentLength);
                                            outgoing += output2;
                                            ss.BeginSend(outgoing, MessageSent, new object());
                                        }
                                        catch (Exception ex)
                                        {
                                            ss.Close();
                                        }
                                    }
                                    else if (words[4] == "False HTTP")
                                    {
                                        try
                                        {
                                            output = bController.GetGameStatus(words[3], false);


                                            if (output.GameState != "pending")
                                            {
                                                output.Player1.Nickname = output.Player1.Nickname.Replace("\"", "");
                                                output.Player2.Nickname = output.Player2.Nickname.Replace("\"", "");
                                            }

                                        //sets ouput2 to the serialized version of output
                                        output2 = JsonConvert.SerializeObject(output);

                                        //sets contentLength to the length in bytes of output2
                                        contentLength = encoding.GetByteCount(output2.ToCharArray());

                                            code = "200 OK";
                                        }
                                        catch (Exception e)
                                        {
                                            if (e is HttpResponseException)
                                            {
                                                code = "403 Forbidden";
                                                httpExceptionThrown = true;
                                            }
                                            if (httpExceptionThrown == false)
                                            {
                                                code = "500 Internal Server Error";
                                            }
                                        }
                                        try
                                        {
                                            SetOutgoingMessage(code, contentLength);
                                            outgoing += output2;
                                            ss.BeginSend(outgoing, MessageSent, new object());
                                        }
                                        catch (Exception ex)
                                        {
                                            ss.Close();
                                        }
                                    }
                                }
                            }
                            line = reader.ReadLine();
                            if (line != null)
                            {
                                array = line.ToCharArray();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        code = "500 Internal Server Error";
                        SetOutgoingMessage(code, contentLength);
                        ss.BeginSend(outgoing, MessageSent, new object());
                    }
                    catch (Exception ex)
                    {
                        ss.Close();
                    }
                }      
            }

            /// <summary>
            /// Callback methos for messageSent, ensures that the message was sent, in which case the socket is closed
            /// and the clientConnection is removed from clients list
            /// </summary>
            /// <param name="wasSent"></param>
            /// <param name="payload"></param>
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
                        server.RemoveClient(this);
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
