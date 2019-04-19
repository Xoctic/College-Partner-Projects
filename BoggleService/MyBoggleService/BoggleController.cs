using BoggleService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading;

namespace BoggleService.Controllers
{
    class HttpResponseException : Exception
    {
        public HttpStatusCode Code { get; private set; }

        public HttpResponseException(HttpStatusCode c)
        {
            Code = c;
        }
    }


    /// <summary>
    /// Server controller that deals with a multitude of commands for dealing with a Boggle game.
    /// </summary>
    public class BoggleController
    {
        //Connection string to the database
        private static string DB;

        static BoggleController()
        {
            //NEW WAY OF GETTING CONNECTION STRING
            string dbFolder = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            DB = String.Format(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {0}\BoggleServer.mdf; Integrated Security = True", dbFolder);


            // Saves the connection string for the database.  A connection string contains the
            // information necessary to connect with the database server.  When you create a
            // DB, there is generally a way to obtain the connection string.  From the Server
            // Explorer pane, obtain the properties of DB to see the connection string.

            // The connection string of my BoggleDB.mdf shows as
            //
            //    Data Source = Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\Aric Campbell\Source\Repos\u1188031\BoggleService\BoggleDB\App_Data\BoggleDB.mdf";Integrated Security=True
            //
            // Unfortunately, this is absolute pathname on my computer, which means that it
            // won't work if the solution is moved.  Fortunately, it can be shorted to
            //
            //    Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ BoggleDB.mdf;Integrated Security=True
            //
            // You should shorten yours this way as well.
            //
            // Rather than build the connection string into the program, I store it in the Web.config
            // file where it can be easily found and changed.  You should do that too.

            //OLD WAY OF GETTING CONNECTION STRING
            //DB = ConfigurationManager.ConnectionStrings["BoggleServerDB"].ConnectionString;
        }

        /// <summary>
        /// Registers a new user with a provided string Nickname.
        /// If user is null or is empty after trimming, responds with status code Forbidden.
        /// Otherwise, creates a user, returns the user's token, and responds with status code Ok. 
        /// </summary>
        /// <param name="user">String sser nickname to be added to users</param>
        /// <returns>A unique string user token of the newly added user</returns>
        //[Route("BoggleService/users")]
        //public string PostRegister([FromBody]string user)
        public string PostRegister(string user)
        {
            if (user == "stall")
            {
                Thread.Sleep(5000);
            }
            if (user == null || user.Trim().Length == 0 || user.Trim().Length > 50)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            // The first step to using the DB is opening a connection to it.  Creating it in a
            // using block guarantees that the connection will be closed when control leaves
            // the block.  As you'll see below, I also follow this pattern for SQLTransactions,
            // SqlCommands, and SqlDataReaders.
            using (SqlConnection conn = new SqlConnection(DB))
            {
                // Connections must be opened
                conn.Open();

                // Database commands should be executed within a transaction.  When commands 
                // are executed within a transaction, either all of the commands will succeed
                // or all will be canceled.  You don't have to worry about some of the commands
                // changing the DB and others failing.
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //An SqlCommand executes a Sql statement on the database.
                    //Figure out what statement is needed to register a user
                    //The first parameter is the statement, second is the connection, third is the transaction
                    //
                    // Note that I use symbols like @UserID as placeholders for values that need to appear
                    // in the statement.  You will see below how the placeholders are replaced.  You may be
                    // tempted to simply paste the values into the string, but this is a BAD IDEA that violates
                    // a cardinal rule of DB Security 101.  By using the placeholder approach, you don't have
                    // to worry about escaping special characters and you don't have to worry about one form
                    // of the SQL injection attack.
                    using (SqlCommand command = new SqlCommand("insert into Users (UserID, Nickname) values(@UserID, @Nickname)", conn, trans))
                    {
                        //Generate a randomized userID
                        string userID = Guid.NewGuid().ToString();

                        //This is where placeholders are replaced
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@Nickname", user.Trim().ToString());

                        if (command.ExecuteNonQuery() != 1)
                        {
                            throw new Exception("Query failed unexpectedly");
                        }
                        // Immediately before each return that appears within the scope of a transaction, it is
                        // important to commit the transaction.  Otherwise, the transaction will be aborted and
                        // rolled back as soon as control leaves the scope of the transaction. 
                        trans.Commit();
                        return userID;
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to join a game, depending on if a game is pending or not.
        /// If UserToken is null, not of length 36, or not a token in users, responds with status code Forbidden.
        /// If Timelimit is less than 5 or greater than 120, responds with status code Forbidden.
        /// If UserToken is already in a pending game,  responds with status code Conflict.
        /// Otherwise, attempts to join a pending game, returns the gameID and game pending, and responds with status code Ok. 
        /// </summary>
        /// <param name="joinGameInput">Input object from the client which contains the user token and the time limit</param>
        /// <returns>A pending game info object which contains the game ID and a pending game status</returns>
        //[Route("BoggleService/games")]
        public PendingGameInfo PostJoinGame(JoinGameInput joinGameInput)
        {
            //checks if the Time Limit passed in is less than 5 or greater than 120 in seconds.
            if (joinGameInput.timeLimit < 5 || joinGameInput.timeLimit > 120)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //checks if user token is valid
                    if (!validToken(joinGameInput.userToken, conn, trans))
                    {
                        throw new HttpResponseException(HttpStatusCode.Forbidden);
                    }

                    bool pendingGameAvailable = false;

                    //Finds a pending game if it exists.
                    using (SqlCommand command = new SqlCommand("select GameState from Games where GameState = @GameState", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameState", "pending");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                pendingGameAvailable = true;
                            }
                        }
                    }

                    PendingGameInfo output = new PendingGameInfo();
                    if (pendingGameAvailable == false)
                    {

                        //Adds a new game to Games with some info.
                        using (SqlCommand command = new SqlCommand("insert into Games (Player1, TimeLimit, GameState) output inserted.GameID values(@Player1, @TimeLimit, @GameState)", conn, trans))
                        {

                            command.Parameters.AddWithValue("@Player1", joinGameInput.userToken.Trim());
                            command.Parameters.AddWithValue("@TimeLimit", joinGameInput.timeLimit);
                            command.Parameters.AddWithValue("@GameState", "pending");

                            // We execute the command with the ExecuteScalar method, which will return to
                            // us the requested auto-generated ItemID.
                            string gameID = command.ExecuteScalar().ToString();
                            output.GameID = gameID;
                            trans.Commit();
                        }
                        output.IsPending = true;

                        return output;
                    }
                    else
                    {
                        int player1TimeLimit = 0;
                        int currentGameID = 0;
                        string currentGame = "";
                        string player1Token = "";

                        //Sets necessary data needed for upcoming update command
                        using (SqlCommand command = new SqlCommand("select Player1, TimeLimit, GameID from Games where GameState = @GameState", conn, trans))
                        {
                            command.Parameters.AddWithValue("@GameState", "pending");

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();

                                player1Token = (string)reader["Player1"];
                                if (player1Token == joinGameInput.userToken)
                                {
                                    reader.Close();
                                    trans.Commit();
                                    throw new HttpResponseException(HttpStatusCode.Conflict);
                                }
                                player1TimeLimit = (int)reader["TimeLimit"];
                                currentGameID = (int)reader["GameID"];
                            }
                        }

                        currentGame = currentGameID.ToString();
                        BoggleBoard boggleBoard = new BoggleBoard();
                        string board = boggleBoard.ToString();

                        //Averages both players time limits
                        int newTimeLimit = (player1TimeLimit + joinGameInput.timeLimit) / 2;

                        // In this case the command is an update.  Updates the pending game and changes its GameState to "active".
                        using (SqlCommand command = new SqlCommand("update Games set Player2=@Player2, Board=@Board, " +
                            "TimeLimit=@TimeLimit, StartTime=@StartTime, GameState=@GameState, Player1Score=@Player1Score" +
                            ",Player2Score=@Player2Score where GameID=@GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@Player2", joinGameInput.userToken);
                            command.Parameters.AddWithValue("@Board", board);
                            command.Parameters.AddWithValue("@TimeLimit", newTimeLimit);
                            command.Parameters.AddWithValue("@StartTime", DateTime.Now);
                            command.Parameters.AddWithValue("@GameState", "active");
                            command.Parameters.AddWithValue("@Player1Score", 0);
                            command.Parameters.AddWithValue("@Player2Score", 0);
                            command.Parameters.AddWithValue("@GameID", currentGame);

                            command.ExecuteNonQuery();
                            trans.Commit();
                        }
                        output.IsPending = false;
                        output.GameID = currentGame;
                        return output;
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to cancel a pending game.
        /// If UserToken is null, not of length 36, or not a token in users, responds with status code Forbidden.
        /// Otherwise, remoes user token from the pending game and responds witht status code 204(NoContent).
        /// </summary>
        /// <param name="token">String user token</param>
        //[Route("BoggleService/games")]
        //public void PutCancelJoin([FromBody]string token)
        public void PutCancelJoin(string token)
        {
            string player1token = "";

            //creates a new SQL connection, transaction, and command
            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Ensures that the userToken is a valid token that exists in the data base
                    if (!validToken(token, conn, trans))
                    {
                        throw new HttpResponseException(HttpStatusCode.Forbidden);
                    }
                    //command to find the pending game by finding the game in the data base that has a gameState of pending
                    using (SqlCommand command = new SqlCommand("select Player1 from Games where GameState = @GameState", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameState", "pending");

                        //reads the row of the data base that results after the command is executed
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //if the reader does not have any rows, this means there was no row found that is associated with that specific command
                            //this means there is no existing pending game, which requires a forbidden exception to be thrown
                            if (!reader.HasRows)
                            {
                                reader.Close();
                                trans.Commit();
                                throw new HttpResponseException(HttpStatusCode.Forbidden);
                            }

                            //if the reader does have a row but the userToken associated with the row is not the userToken passed in
                            //throws a forbidden exception
                            reader.Read();

                            player1token = (string)reader["Player1"];

                            if (player1token != token)
                            {
                                reader.Close();
                                trans.Commit();
                                throw new HttpResponseException(HttpStatusCode.Forbidden);
                            }

                        }
                    }
                    //if the row found is a pending game with the same userToken that was passed in
                    //use an SQL command that deletes the row from the Games table, allowing us to cancel a pending game
                    using (SqlCommand command = new SqlCommand("Delete from Games where GameState = @GameState", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameState", "pending");
                        command.ExecuteNonQuery();
                        trans.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Plays a word and returns its score depending if it is valid or not.
        /// If UserToken is null, not of length 36, or not a token in users, responds with status code Forbidden.
        /// If GameID is null, or not found in games, responds with status code Forbidden.
        /// If the string word input is null or empty, responds with status code Forbidden.
        /// If game stats is anything other than "active", responds with status code Conflict.
        /// Otherwise, attempts to join a pending game, returns the , and responds with status code Ok.
        /// </summary>
        /// <param name="gameID">Input string of a game ID</param>
        /// <param name="play">Input object which contains the user token and the word played</param>
        /// /// <returns>Score of string word input</returns>
        //[Route("BoggleService/games/{gameID}")]
        public int PutPlayWord(string gameID, PlayWordInput play)
        {
            int score = 0;

            //ensures that the word trying to be played is valid
            if (play.word == null || play.word == "" || play.word.Trim().Length > 30)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                //retrieves all Game information related to the gameID passed in
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    string player1token = null;
                    string player2token = null;
                    string gameState = null;
                    int timeLimit = 0;
                    DateTime startTime = new DateTime();
                    int timeLeft = 0;
                    string turn = null;
                    string board = null;
                    //Ensures that the userToken is a valid token that exists in the data base, as well as the gameID.
                    if (!validID(gameID, conn, trans) || !validToken(play.userToken, conn, trans))
                    {
                        throw new HttpResponseException(HttpStatusCode.Forbidden);
                    }
                    using (SqlCommand command = new SqlCommand("select Player1, Player2, GameState, TimeLimit, StartTime, Board from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            gameState = (string)reader["GameState"];
                            //if the gameState of the game is "completed" or "pending", a word can not be played
                            //throws a forbidden exception
                            if (gameState == "pending" || gameState == "completed")
                            {
                                reader.Close();
                                trans.Commit();
                                throw new HttpResponseException(HttpStatusCode.Conflict);
                            }
                            player1token = (string)reader["Player1"];
                            player2token = (string)reader["Player2"];
                            timeLimit = (int)reader["TimeLimit"];
                            startTime = (DateTime)(reader["StartTime"]);
                            board = (string)reader["Board"];
                            timeLeft = calculateTimeLeft(startTime, timeLimit);

                            //if there is not more time left in the game throws a conflict exception
                            if (timeLeft <= 0)
                            {
                                reader.Close();
                                trans.Commit();
                                throw new HttpResponseException(HttpStatusCode.Conflict);
                            }

                            //if the userToken passed in does not match either userToken in the game
                            //throws a forbidden exception
                            if (player1token != play.userToken && player2token != play.userToken)
                            {
                                reader.Close();
                                trans.Commit();
                                throw new HttpResponseException(HttpStatusCode.Forbidden);
                            }

                            //figures out which player is playing the word
                            if (play.userToken == player1token)
                            {
                                turn = "player1";
                            }

                            if (play.userToken == player2token)
                            {
                                turn = "player2";
                            }
                        }
                    }


                    if (turn == "player1")
                    {

                        bool beenPlayed = false;

                        //sees if the word has already been played by player1
                        using (SqlCommand command = new SqlCommand("select Word from Words where Player = @Player1 and Word = @Word", conn, trans))
                        {
                            command.Parameters.AddWithValue("@Player1", player1token);
                            command.Parameters.AddWithValue("@Word", play.word);


                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    score = 0;
                                    beenPlayed = true;
                                }
                            }

                        }

                        //if the word has been played, the word is added to the words table with a score of 0
                        if (beenPlayed)
                        {
                            using (SqlCommand command = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                            {
                                command.Parameters.AddWithValue("@Word", play.word);
                                command.Parameters.AddWithValue("@GameID", gameID);
                                command.Parameters.AddWithValue("@Player", player1token);
                                command.Parameters.AddWithValue("@Score", 0);
                                command.ExecuteNonQuery();
                                trans.Commit();
                            }
                        }
                        //if the word has not been played, the score is calculated through the boggleBoard class
                        else
                        {
                            BoggleBoard b = new BoggleBoard(board);
                            score = b.score(play.word);
                            int player1Score = 0;

                            //creates a new played word in the Word table with the score awarded to player1
                            using (SqlCommand command = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                            {
                                command.Parameters.AddWithValue("@Word", play.word);
                                command.Parameters.AddWithValue("@GameID", gameID);
                                command.Parameters.AddWithValue("@Player", player1token);
                                command.Parameters.AddWithValue("@Score", score);
                                command.ExecuteNonQuery();
                            }

                            //retrieves the total score that player1 has
                            using (SqlCommand command = new SqlCommand("select Player1Score from Games where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();

                                    player1Score = (int)reader["Player1Score"];
                                }
                            }


                            //updates player1Score by adding the score awarded by playng the word
                            using (SqlCommand command = new SqlCommand("update Games set Player1Score = @Player1Score where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@Player1Score", player1Score + score);
                                command.Parameters.AddWithValue("@GameID", gameID);


                                command.ExecuteNonQuery();
                                trans.Commit();
                            }
                        }
                    }
                    else
                    {
                        bool beenPlayed = false;

                        //sees if the word has already been played by player2
                        using (SqlCommand command = new SqlCommand("select Word from Words where Player = @Player2 and Word = @Word", conn, trans))
                        {
                            command.Parameters.AddWithValue("@Player2", player2token);
                            command.Parameters.AddWithValue("@Word", play.word);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    score = 0;
                                    beenPlayed = true;
                                }
                            }
                        }

                        //if the word has been played, the word is added to the words table with a score of 0
                        if (beenPlayed)
                        {

                            using (SqlCommand command = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                            {
                                command.Parameters.AddWithValue("@Word", play.word);
                                command.Parameters.AddWithValue("@GameID", gameID);
                                command.Parameters.AddWithValue("@Player", player2token);
                                command.Parameters.AddWithValue("@Score", 0);
                                command.ExecuteNonQuery();
                                trans.Commit();
                            }
                        }
                        //if the word has not been played, the score is calculated through the boggleBoard class
                        else
                        {
                            BoggleBoard b = new BoggleBoard(board);
                            score = b.score(play.word);
                            int player2Score = 0;

                            //creates a new played word in the Word table with the score awarded to player2
                            using (SqlCommand command = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                            {
                                command.Parameters.AddWithValue("@Word", play.word);
                                command.Parameters.AddWithValue("@GameID", gameID);
                                command.Parameters.AddWithValue("@Player", player2token);
                                command.Parameters.AddWithValue("@Score", score);
                                command.ExecuteNonQuery();
                            }

                            //retrieves the total score that player2 has
                            using (SqlCommand command = new SqlCommand("select Player2Score from Games where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();

                                    player2Score = (int)reader["Player2Score"];
                                }
                            }

                            //updates player2Score by adding the score awarded by playng the word
                            using (SqlCommand command = new SqlCommand("update Games set Player2Score = @Player2Score where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@Player2Score", player2Score + score);
                                command.Parameters.AddWithValue("@GameID", gameID);

                                command.ExecuteNonQuery();
                                trans.Commit();
                            }
                        }
                    }
                }
            }

            return score;
        }

        /// <summary>
        /// Attempts to join a game, depending on if a game is pending or not.
        /// Otherwise, returns information about the game name by gameID.
        /// Note that the information returned depends on whether brief is true or false. Responds with status code Ok. 
        /// </summary>
        /// <param name="gameID">Input string of a game ID</param>
        /// <param name="brief">Input boolean ,true for minimal information, false for maximum information</param>
        /// <returns>A GameInfo object which contains information depending on brief</returns>
        //[Route("BoggleService/games/{gameID}/{brief}")]
        public GameInfo GetGameStatus(string gameID, bool brief)
        {
            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //checks if game token is valid
                    if (!validID(gameID, conn, trans))
                    {
                        throw new HttpResponseException(HttpStatusCode.Forbidden);
                    }
                    string gameState = null;
                    int timeLimit = 0;
                    DateTime startTime = new DateTime();
                    int timeLeft = 0;
                    bool setToCompleted = false;
                    //Checks the time remaining for gameID, which determines if the game is complete.
                    using (SqlCommand command = new SqlCommand("select TimeLimit, StartTime, GameState from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            gameState = (string)reader["GameState"];
                            if (!(gameState == "pending"))
                            {
                                timeLimit = (int)reader["TimeLimit"];
                                startTime = (DateTime)reader["StartTime"];
                                if (gameState == "active")
                                {
                                    timeLeft = calculateTimeLeft(startTime, timeLimit);
                                    if (timeLeft <= 0)
                                    {
                                        setToCompleted = true;
                                    }
                                }
                            }
                        }
                    }
                    if (setToCompleted == true)
                    {
                        //Updates the GameState to "completed" if we get to this point.
                        using (SqlCommand command = new SqlCommand("update Games set GameState=@GameState where GameID = @GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@GameState", "completed");
                            command.Parameters.AddWithValue("@GameID", gameID);

                            command.ExecuteNonQuery();
                            trans.Commit();
                        }
                    }
                    gameState = null;
                    string player1 = null;
                    string player2 = null;
                    string board = null;
                    int player1Score = 0;
                    int player2Score = 0;
                    //Sets necessary data needed for upcoming checks and commands.
                    using (SqlCommand command = new SqlCommand("select Player1, Player2, Board, TimeLimit, " +
                                "StartTime, GameState, Player1Score, Player2Score from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            gameState = (string)reader["GameState"];
                            if (!(gameState == "pending"))
                            {
                                player1 = (string)reader["Player1"];
                                player2 = (string)reader["Player2"];
                                board = (string)reader["Board"];
                                timeLimit = (int)reader["TimeLimit"];
                                startTime = (DateTime)reader["StartTime"];
                                player1Score = (int)reader["Player1Score"];
                                player2Score = (int)reader["Player2Score"];
                            }
                        }
                    }
                    GameInfo output = new GameInfo();
                    //If brief is true
                    if (brief == true)
                    {
                        if (gameState == "pending")
                        {
                            output.GameState = "pending";
                            return output;
                        }
                        else if (gameState == "active")
                        {
                            output.GameState = "active";
                            output.Player1 = new PlayerInfo();
                            output.Player2 = new PlayerInfo();
                            timeLeft = calculateTimeLeft(startTime, timeLimit);
                            output.TimeLeft = timeLeft;
                            output.Player1.Score = player1Score;
                            output.Player2.Score = player2Score;
                            return output;
                        }
                        else if (gameState == "completed")
                        {
                            output.GameState = "completed";
                            output.Player1 = new PlayerInfo();
                            output.Player2 = new PlayerInfo();
                            output.Player1.Score = player1Score;
                            output.Player2.Score = player2Score;
                            return output;
                        }
                    }
                    //If brief is false
                    else
                    {
                        if (gameState == "pending")
                        {
                            output.GameState = "pending";
                            return output;
                        }
                        output.Player1 = new PlayerInfo();
                        output.Player2 = new PlayerInfo();
                        using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@UserID", player1);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                string player1Nickname = (string)reader["Nickname"];
                                output.Player1.Nickname = player1Nickname;
                            }
                        }
                        using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@UserID", player2);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                string player2Nickname = (string)reader["Nickname"];
                                output.Player2.Nickname = player2Nickname;
                            }
                        }
                        if (gameState == "active")
                        {
                            output.GameState = "active";
                            output.Board = board;
                            output.TimeLimit = timeLimit;

                            if(timeLeft > 0 && timeLeft < 1)
                            {
                                timeLeft = calculateTimeLeftRounded(startTime, timeLimit);
                            }
                            else
                            {
                                timeLeft = calculateTimeLeft(startTime, timeLimit);
                            }

                            
                            if (timeLeft <= 0)
                            {
                                timeLeft = 0;
                            }
                            output.TimeLeft = timeLeft;
                            output.Player1.Score = player1Score;
                            output.Player2.Score = player2Score;
                            return output;
                        }
                        if (gameState == "completed")
                        {
                            output.GameState = "completed";
                            output.Player1.WordsPlayed = new List<PlayedWord>();
                            output.Player2.WordsPlayed = new List<PlayedWord>();
                            output.Board = board;
                            output.TimeLimit = timeLimit;
                            output.Player1.Score = player1Score;
                            output.Player2.Score = player2Score;

                            //Finds all words played by player 1.
                            using (SqlCommand command = new SqlCommand("select Word, Score from Words, " +
                                "Games where Words.GameID = Games.GameID and Words.GameID = @GameID and " +
                                "Words.Player = Games.Player1 and Words.Player = @UserID  ", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);
                                command.Parameters.AddWithValue("@UserID", player1);
                                List<PlayedWord> result = new List<PlayedWord>();

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(new PlayedWord
                                        {
                                            Word = (string)reader["Word"],
                                            Score = (int)reader["Score"]
                                        });
                                    }
                                    output.Player1.WordsPlayed = result;
                                }
                            }
                            //Finds all words played by player 2.
                            using (SqlCommand command = new SqlCommand("select Word, Score from Words, " +
                                "Games where Words.GameID = Games.GameID and Words.GameID = @GameID and " +
                                "Words.Player = Games.Player2 and Words.Player = @UserID  ", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);
                                command.Parameters.AddWithValue("@UserID", player2);
                                List<PlayedWord> result = new List<PlayedWord>();

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(new PlayedWord
                                        {
                                            Word = (string)reader["Word"],
                                            Score = (int)reader["Score"]
                                        });
                                    }
                                    output.Player2.WordsPlayed = result;
                                }
                            }
                            return output;
                        }
                    }
                    //We should never get to this line of code.
                    return output;
                }
            }
        }

        /// <summary>
        /// ensures that the userToken passed in is a valid token
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private bool validToken(string userToken, SqlConnection conn, SqlTransaction trans)
        {
            if (userToken == null || userToken.Trim().Length != 36)
            {
                return false;
            }
            //Here the SqlCommand is a select query. We are interested in whether userToken exists
            //in the Users table
            using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
            {
                command.Parameters.AddWithValue("@UserID", userToken);

                //This executes a query (i.e. a select statement). The result is an
                //SqlDataReader that you can use to iterate through the rows in response.
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //In this we don't actually need to read any data; we only need
                    //to know whether a row was returned
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// esures the gameID passed in is a valid gameId
        /// </summary>
        /// <param name="gID"></param>
        /// <returns></returns>
        private bool validID(string gID, SqlConnection conn, SqlTransaction trans)
        {
            if (gID == null || gID.Trim().Length == 0)
            {
                return false;
            }
            //Here the SqlCommand is a select query. We are interested in whether userToken exists
            //in the Users table
            using (SqlCommand command = new SqlCommand("select GameID from Games where GameID = @GameID", conn, trans))
            {
                command.Parameters.AddWithValue("@GameID", gID);

                //This executes a query (i.e. a select statement). The result is an
                //SqlDataReader that you can use to iterate through the rows in response.
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //In this we don't actually need to read any data; we only need
                    //to know whether a row was returned
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Helper method to calculate the TimeLeft in seconds based on the time of day.
        /// </summary>
        private int calculateTimeLeft(DateTime startTime, int timeLimit)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            return timeLimit - Convert.ToInt32(elapsed.TotalSeconds);
        }


        /// <summary>
        /// Helper method to calculate the rounded version of TimeLeft in seconds based on the time of day.
        /// </summary>
        private int calculateTimeLeftRounded(DateTime startTime, int timeLimit)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            double timeLeft = timeLimit - elapsed.TotalSeconds;
            double dec = timeLeft - Math.Floor(timeLeft);

            if(dec >= .5)
            {
                timeLeft = Math.Ceiling(timeLeft);
            }
            else
            {
                timeLeft = Math.Floor(timeLeft);
            }

            


            return Convert.ToInt32(timeLeft);
        }


        

    }
}