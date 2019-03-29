﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    class Controller
    {
        /// <summary>
        /// The view controlled by this Controller
        /// </summary>
        private BoggleView view;

        /// <summary>
        /// The token of the most recently registered user, or "0" if no user
        /// has ever registered
        /// </summary>
        private string userToken;

        /// <summary>
        /// The ID of the current game.
        /// </summary>
        private string gameID;

        /// <summary>
        /// Variable to keep track if player is waiting to join game.
        /// </summary>
        private bool isPending;

        /// <summary>
        /// Variable to store the URL of the server.
        /// </summary>
        private string serverURL;

        /// <summary>
        /// For cancelling joining a game
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// Timer used to check multiple aspects of the game.
        /// </summary>
        private System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public Controller(BoggleView view)
        {
            this.view = view;
            userToken = Guid.NewGuid().ToString();

            view.RegisterPressed += Register;
            view.JoinGamePressed += JoinGame;
            view.QuitGamePressed += QuitGame;
            view.CancelPressed += CancelJoinGame;
            myTimer.Tick += TimerEventProcessor;
            myTimer.Interval = 1000;
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            Update();
        }

        private async void Update()
        {
            view.EnableControls(false);
            using (HttpClient client = CreateClient(serverURL))
            {
                tokenSource = new CancellationTokenSource();
                StringContent content = new StringContent(gameID, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.GetAsync("BoggleService/games/" + gameID + "/" + "false");

                if (response.IsSuccessStatusCode)
                {

                    String result = await response.Content.ReadAsStringAsync();
                    dynamic items = JsonConvert.DeserializeObject(result);



                    int counter = 0;
                    string temp;
                    foreach (dynamic item in items)
                    {
                        temp = item.ToString();
                        //temp = temp.Substring(14, temp.Length - 15);

                        switch (counter)
                        {
                            //gameState
                            case 0:
                                //This is how to set temp to current item.  There are other properties 
                                //of item that exist that may be needed, such as item.Next.
                                //The debugger will show you all the properties that exist for item.
                                temp = item.Last;

                                //temp = temp.Substring(14, temp.Length - 15);
                                break;

                            case 1:
                                string tester = item.Board;
                                temp = temp.Substring(10, temp.Length - 11);
                                break;

                            case 2:
                                temp = temp.Substring(13, temp.Length - 14);
                                break;

                            case 3:
                                temp = temp.Substring(12, temp.Length - 13);
                                break;

                            case 4:
                                break;
                            case 5:
                                break;

                        }





                        counter++;

                    }

                }
            }
        }

        private async void CancelJoinGame()
        {
            tokenSource.Cancel();
            view.Refresh();
            view.EnableControls(true);
            //try
            //{
            //    view.EnableControls(false);
            //    using (HttpClient client = CreateClient(serverURL))
            //    {
            //        tokenSource = new CancellationTokenSource();
            //        StringContent content = new StringContent(JsonConvert.SerializeObject(userToken), Encoding.UTF8, "application/json");
            //        HttpResponseMessage response = await client.PutAsync("BoggleService/games", content, tokenSource.Token);
            //        //should check to see if it is a 204
            //        if(!response.IsSuccessStatusCode)
            //        {
            //            MessageBox.Show("Something wrong with cancelling join game");
            //        }
            //    }


            //}
            //catch(TaskCanceledException)
            //{
            //}
            //finally
            //{
            //    view.EnableControls(true);
            //}
        }

        private void QuitGame(string arg1, string arg2)
        {
            throw new NotImplementedException();
        }

        private async void JoinGame(int time)
        {
            try
            {
                view.EnableControls(false);
                using (HttpClient client = CreateClient(serverURL))
                {
                    //Create Parameter
                    // add more stuffs
                    dynamic gameInfo = new ExpandoObject();
                    gameInfo.UserToken = userToken;
                    gameInfo.TimeLimit = time;
                    
                    //Compose & Send Request
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(gameInfo), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("BoggleService/games", content, tokenSource.Token);

                    //Deal With Response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = await response.Content.ReadAsStringAsync();
                        dynamic items = JsonConvert.DeserializeObject(result);
                        int counter = 0;
                        foreach(dynamic item in items)
                        {
                            if (counter == 0)
                            {
                                gameID = item.ToString();
                                gameID = gameID.Substring(11, gameID.Length - 12);
                            }
                            if (counter == 1)
                            {
                                string test = item.ToString();
                                test = test.Substring(12, test.Length - 12);
                                test = test.Trim();
                                if(test == "true")
                                {
                                    isPending = true;
                                }
                                if(test == "false")
                                {
                                    isPending = false;
                                }

                            }
                            counter++;
                        }

                       // MessageBox.Show("isPending: " + isPending);

                    }
                    else
                    {
                        MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }

                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                view.EnableControls(true);
                myTimer.Start();
            }
        }

        /// <summary>
        /// Registers a user with the given name and server
        /// </summary>
        /// <param name="name"></param>
        /// <param name="server"></param>
        private async void Register(string name, string server)
        {
            try
            {
                view.EnableControls(false);
                using (HttpClient client = CreateClient(server))
                {
                    //Compose & Send Request
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(name), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("BoggleService/users", content, tokenSource.Token);

                    //Deal With Response
                    if(response.IsSuccessStatusCode)
                    {
                        serverURL = server;
                        String result = await response.Content.ReadAsStringAsync();
                        userToken = (string)JsonConvert.DeserializeObject(result);
                        view.IsUserRegistered = true;
                        
                    }
                    else
                    {
                        MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }

                }
            }
            catch(TaskCanceledException)
            {
            }
            finally
            {
                view.EnableControls(true);
            }
        }

        private HttpClient CreateClient(string _server)
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_server);

            // Tell the server that the client will accept this particular type of response data
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // There is more client configuration to do, depending on the request.
            return client;
        }
    }
}
