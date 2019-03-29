using Newtonsoft.Json;
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



        public Controller(BoggleView view)
        {
            this.view = view;
            userToken = Guid.NewGuid().ToString();

            view.RegisterPressed += Register;
            view.JoinGamePressed += JoinGame;
            view.QuitGamePressed += QuitGame;


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
                    dynamic user = new ExpandoObject();
                    string gameInfo = "{/n" + "\"UserToken\": " + userToken + ",/n" + "\"TimeLimit\": " + time + "/n" + "}";
                    //user.UserGameInfo = gameInfo;
                    

                    //Compose & Send Request
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(gameInfo), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("BoggleService/games", content, tokenSource.Token);

                    //Deal With Response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = await response.Content.ReadAsStringAsync();
                        dynamic items = JsonConvert.DeserializeObject(result);
                        //userToken = (string)JsonConvert.DeserializeObject(result);
                        foreach(dynamic item in items)
                        {
                            isPending = false;
                        }

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
                    //Create Parameter
                    // add more stuffs
                    dynamic user = new ExpandoObject();
                    user.Name = name;
                    //user.Server = server;

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
