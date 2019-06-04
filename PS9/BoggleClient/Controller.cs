using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        /// Variable to store the URL of the server.
        /// </summary>
        private string serverURL;

        /// <summary>
        /// For cancelling joining a game
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// Keeps track if the game has started or not.
        /// </summary>
        private bool gameBegun;

        /// <summary>
        /// State of the current game.
        /// </summary>
        private string gameState;

        /// <summary>
        /// Board of the current game.
        /// </summary>
        private string gameBoard;

        /// <summary>
        /// Average timelimit of the 2 players.
        /// </summary>
        private int timeLimit;

        /// <summary>
        /// Time remaining in the game.
        /// </summary>
        private int timeLeft;

        /// <summary>
        /// Name of player 1
        /// </summary>
        private string player1Nickname;

        /// <summary>
        /// Score for player 1
        /// </summary>
        private string player1Score;

        /// <summary>
        /// Name of player 2
        /// </summary>
        private string player2Nickname;

        /// <summary>
        /// Score for player 2
        /// </summary>
        private string player2Score;
        /// <summary>
        /// Timer used to check multiple aspects of the game.
        /// </summary>
        private System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        

        /// <summary>
        /// Constructor for the controller which deals with all the back end work of the gui
        /// </summary>
        /// <param name="view"></param>
        public Controller(BoggleView view)
        {
            this.view = view;
            

            view.RegisterPressed += Register;
            view.JoinGamePressed += JoinGame;
            view.QuitGamePressed += QuitGame;
            view.CancelJoinGamePressed += CancelJoinGame;
            view.CancelRegisterPressed += CancelRegister;
            view.EnterPressedInWordTextBox += SendWord;
            view.HelpMenuPressed += Helpmenu;
            myTimer.Tick += TimerEventProcessor;
            myTimer.Interval = 1000;
            gameBegun = false;
        }

        /// <summary>
        /// Displays a help menu to explain how the game works
        /// </summary>
        private void Helpmenu()
        {
            HelpMenu helpMenu = new HelpMenu();
            helpMenu.Show();
        }

        /// <summary>
        /// Allows user to cancel in the middle of registering
        /// </summary>
        private void CancelRegister()
        {
            tokenSource.Cancel();
            view.RegistrationComplete = false;
            view.IsUserRegistered = false;
            view.EnableControls(true);
        }

        /// <summary>
        /// Handles when the user types in a word into the wordTextBox and presses enter
        /// Sends a Put request to the server with the gameId, userToken, and the word enetered
        /// Displays an error window box to let the user know if there is a problem 
        /// </summary>
        /// <param name="word"></param>
        private async void SendWord(string word)
        {

            using (HttpClient client = CreateClient(serverURL))
            {
                //Create Parameter
                // add more stuffs
                dynamic wordInfo = new ExpandoObject();
                wordInfo.UserToken = userToken;
                wordInfo.Word = word;

                //Compose & Send Request
                StringContent content = new StringContent(JsonConvert.SerializeObject(wordInfo), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("BoggleService/games/" + gameID, content);

                //Deal With Response
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    player1Score += Convert.ToInt32(JsonConvert.DeserializeObject(result));

                }
                else
                {
                    MessageBox.Show("Error sending word: " + response.StatusCode + "\n" + response.ReasonPhrase);
                }

            }
        }

        /// <summary>
        /// Calls update method every second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerEventProcessor(object sender, EventArgs e)
        {
            try
            {
                Update();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Updates information on the board
        /// If the game state is active but is just starting then the startGameUpdate method is called
        /// Otherwise activeUpdate method is called
        /// If the game state is completed then the completedUpdate is called
        /// </summary>
        private async void Update()
        {
            if (gameID != null)
            {
                using (HttpClient client = CreateClient(serverURL))
                {
                    HttpResponseMessage response = await client.GetAsync("BoggleService/games/" + gameID + "/" + "false");
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        dynamic items = JsonConvert.DeserializeObject(result);
                        gameState = (string)items.GameState;

                    switch (gameState)
                    {
                        case "active":
                            if(gameBegun == false || gameBoard == "                ")
                            {
                                view.EnableControls(true);
                                view.DisableControls(false);
                                gameBegun = true;
                                startGameUpdate(items);
                            }
                            else
                            {
                                response = await client.GetAsync("BoggleService/games/" + gameID + "/" + "true");
                                result = await response.Content.ReadAsStringAsync();
                                items = JsonConvert.DeserializeObject(result);
                                activeUpdate(items);
                            }
                            break;
                      
                        case "completed":

                                completedUpdate(items);

                                break;
                            default:
                                break;
                        }
                    }
                }
            }


        }

        /// <summary>
        /// Updates the game board for the begining of the game, only should occur once for each game
        /// requires the gameState, gameBoard, timeLimit, timeLeft, player1NickName, player2NickNAME, both players scores, to be updated in the view
        /// </summary>
        /// <param name="items"></param>
        private void startGameUpdate(dynamic items)
        {
            gameState = items.GameState;
            gameBoard = items.Board;
            timeLimit = items.TimeLimit;
            timeLeft = items.TimeLeft;
            player1Nickname = items.Player1.Nickname;
            player1Score = items.Player1.Score;
            player2Nickname = items.Player2.Nickname;
            player2Score = items.Player2.Score;

            view.SetBoard(gameBoard);
            view.SetPlayer1NameLabel(player1Nickname);
            view.SetPlayer2NameLabel(player2Nickname);
            

        }

        /// <summary>
        /// updates the board for an ongoing game, this update usually occurs every second during the game
        /// requires timeLeft, both players scores to be updated in the view
        /// </summary>
        /// <param name="items"></param>
        private void activeUpdate(dynamic items)
        {
            timeLeft = items.TimeLeft;
            player1Score = items.Player1.Score;
            player2Score = items.Player2.Score;
            view.SetSecondsLabel(timeLeft.ToString());
            view.SetPlayer1Score("Score: " + player1Score);
            view.SetPlayer2Score("Score: " + player2Score);
        }

        /// <summary>
        /// updates the board for a completed game, only should occur once for each game
        /// requires gameState to be updated, player scores, and all words played by each players should be entered into the view
        /// </summary>
        /// <param name="items"></param>
        private void completedUpdate(dynamic items)
        {
            gameState = items.GameState;
            player1Score = items.Player1.Score;
            player2Score = items.Player2.Score;
            List<string> player1Words = new List<string>();
            List<string> player2Words = new List<string>();
            foreach(dynamic item in items.Player1.WordsPlayed)
            {
                player1Words.Add(item.Word.ToString() + ": " + item.Score.ToString());
                
            }

            foreach(dynamic item in items.Player2.WordsPlayed)
            {
                player2Words.Add(item.Word.ToString() + ": " + item.Score.ToString());
            }

            view.SetWordsPlayedTitle(true);
            view.SetWordsPlayed(player1Words, player2Words);
            view.SetSecondsLabel("0");

            

           
                int.TryParse(player1Score, out int score1);
                int.TryParse(player2Score, out int score2);

                if (score1 > score2)
                {
                    view.SetPlayer1Title(true);
                }
                else if (score2 > score1)
                {
                    view.SetPlayer2Title(true);
                }
               

        }

        /// <summary>
        /// cancels an attempt to join a game with a registered user
        /// stops the timer, and calls a put to the server using the userToken
        /// </summary>
        private async void CancelJoinGame()
        {
            tokenSource.Cancel();
            myTimer.Stop();
            myTimer = new System.Windows.Forms.Timer();

            view.EnableControls(false);
            using (HttpClient client = CreateClient(serverURL))
            {
                //tokenSource = new CancellationTokenSource();
                StringContent content = new StringContent(JsonConvert.SerializeObject(userToken), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("BoggleService/games", content);
                //should check to see if it is a 204
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Something wrong with cancelling join game");
                }
            }
            view.EnableControls(true);

        }

        /// <summary>
        /// sets all the values in the view back to a state that looks as though the program was just opened
        /// also sets the timer to a new timer and sets the isUserRegistered boolean to false so the controller knows not to keep trying to update an old game
        /// </summary>
        private void QuitGame()
        {
            myTimer = new System.Windows.Forms.Timer();
            gameID = null;
            gameState = "";
            gameBegun = false;
            gameBoard = "                ";
            timeLimit = 0;
            timeLeft = 0;
            player1Nickname = "";
            player1Score = "";
            player2Nickname = "";
            player2Score = "";
            

            view.SetBoard(gameBoard);
            view.SetPlayer1NameLabel(player1Nickname);
            view.SetPlayer2NameLabel(player2Nickname);
            view.SetPlayer1Score(player1Score);
            view.SetPlayer2Score(player2Score);
            view.SetSecondsLabel("");
            view.SetNameTextBox("");
            view.SetWordsPlayedTitle(false);
            view.SetWordsPlayed(new List<string>(), new List<string>());
            view.SetPlayer1Title(false);
            view.SetPlayer2Title(false);
            view.IsUserRegistered = false;
            view.RegistrationComplete = false;
            view.EnableControls(true);
           
        }

        /// <summary>
        /// sends a request to the server to join a game with another registered user
        /// 
        /// </summary>
        /// <param name="time"></param>
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
                            counter++;
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
                    StringContent content = new StringContent(JsonConvert.SerializeObject("Name: " + name), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("BoggleService/users", content, tokenSource.Token);

                    //Deal With Response
                    if(response.IsSuccessStatusCode)
                    {
                        serverURL = server;
                        String result = await response.Content.ReadAsStringAsync();
                        userToken = (string)JsonConvert.DeserializeObject(result);
                        view.IsUserRegistered = true;
                        gameState = "pending";
                    }
                    else
                    {
                        MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }

                }
            }
            catch(Exception f)
            {
                if(f.GetType() != typeof(TaskCanceledException))
                {
                    MessageBox.Show("Error registering");
                }
               

            }
            finally
            {
                view.EnableControls(true);
                view.RegistrationComplete = true;
            }
        }
        
        /// <summary>
        /// creates a new client to access the server
        /// </summary>
        /// <param name="_server"></param>
        /// <returns></returns>
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
