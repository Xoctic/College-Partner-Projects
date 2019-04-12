using BoggleService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace BoggleService.Controllers
{
    /// <summary>
    /// Server controller that deals with a multitude of commands.
    /// </summary>
    public class BoggleController : ApiController
    {
        //Connection string to the database
        private static string DB;

        static BoggleController()
        {
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
            DB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
        }

        /// <summary>
        /// Registers a new user with a provided string Nickname.
        /// If user is null or is empty after trimming, responds with status code Forbidden.
        /// Otherwise, creates a user, returns the user's token, and responds with status code Ok. 
        /// </summary>
        /// <param name="user">String sser nickname to be added to users</param>
        /// <returns>A unique string user token of the newly added user</returns>
        [Route("BoggleService/users")]
        public string PostRegister2([FromBody]string user)
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
        [Route("BoggleService/games")]
        public PendingGameInfo PostJoinGame(JoinGameInput joinGameInput)
        {
            if (!(validToken(joinGameInput.userToken)))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (joinGameInput.timeLimit < 5 || joinGameInput.timeLimit > 120)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (pendingInfo.UserToken == joinGameInput.userToken)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    // Here, the SqlCommand is a select query.  We are interested in whether joinGameInput.userToken exists in
                    // the Users table.
                    //using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    //{
                    //    command.Parameters.AddWithValue("@UserID", joinGameInput.userToken);

                    //    // This executes a query (i.e. a select statement).  The result is an
                    //    // SqlDataReader that you can use to iterate through the rows in the response.
                    //    using (SqlDataReader reader = command.ExecuteReader())
                    //    {
                    //        // In this we don't actually need to read any data; we only need
                    //        // to know whether a row was returned.
                    //        if (!reader.HasRows)
                    //        {
                    //            reader.Close();
                    //            trans.Commit();
                    //            throw new HttpResponseException(HttpStatusCode.Forbidden);
                    //        }
                    //    }
                    //}

                    PendingGameInfo output = new PendingGameInfo();
                    if (pendingInfo.IsPending == false)
                    {    
                        //string letterG = "G";               
                        using (SqlCommand command = new SqlCommand("insert into Games (Player1, TimeLimit, GameState) output inserted.GameId values(@Player1, @TimeLimit, @GameState)", conn, trans))
                        {
                           
                            command.Parameters.AddWithValue("@Player1", joinGameInput.userToken.Trim());
                            command.Parameters.AddWithValue("@TimeLimit", joinGameInput.timeLimit);
                            command.Parameters.AddWithValue("@GameState", "pending");

                            // We execute the command with the ExecuteScalar method, which will return to
                            // us the requested auto-generated ItemID.
                            string gameID = command.ExecuteScalar().ToString();
                            pendingInfo.GameID = gameID;
                            output.GameID = gameID;
                            trans.Commit();
                        }
                        pendingInfo.TimeLimit = joinGameInput.timeLimit;
                        pendingInfo.UserToken = joinGameInput.userToken;
                        pendingInfo.IsPending = true;
                        output.IsPending = true;
             
                        return output;
                    }
                    else
                    {
                        int player1TimeLimit = 0;
                        string currentGame = pendingInfo.GameID;
                        // In this case the command is a select.
                        using (SqlCommand command = new SqlCommand("select TimeLimit from Games where GameID=@GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@UserID", currentGame);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();                              
                                player1TimeLimit = (int)reader["TimeLimit"];                               
                            }
                            trans.Commit();
                        }
                        BoggleBoard boggleBoard = new BoggleBoard();
                        string board = boggleBoard.ToString();

                        //Averages both players time limits
                        int newTimeLimit = (player1TimeLimit + joinGameInput.timeLimit) / 2;

                        // In this case the command is an update.
                        using (SqlCommand command = new SqlCommand("update Games set Player2=@Player2, Board=@Board " +
                            "TimeLimit=@TimeLimit, StartTime=@StartTime, GameState=@GameState, Player1Score=@Player1Score" +
                            ",Player2Score=@Player2Score where GameID=@GameID", conn, trans))
                        {                      
                            command.Parameters.AddWithValue("@Player2", joinGameInput.userToken);
                            command.Parameters.AddWithValue("@Board", board);
                            command.Parameters.AddWithValue("@TimeLimit", newTimeLimit);
                            command.Parameters.AddWithValue("@StartTime", (DateTime.Now.Minute * 60) + DateTime.Now.Second);
                            command.Parameters.AddWithValue("@GameState", "active");
                            command.Parameters.AddWithValue("@Player1Score", 0);
                            command.Parameters.AddWithValue("@Player2Score", 0);
                            command.Parameters.AddWithValue("@GameID", currentGame);

                            // We pay attention to the number of rows modified.  If no rows were modified,
                            // we know that there was no game with the given gameID, and we report an error.
                            //int result = command.ExecuteNonQuery();
                            trans.Commit();
                            //if (result == 0)
                            //{
                            //    throw new HttpResponseException(HttpStatusCode.Forbidden);
                            //}
                        }
                       
                        output.IsPending = false;
                        output.GameID = currentGame;
                        pendingInfo = new PendingGameInfo();
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
        [Route("BoggleService/games")]
        public void PutCancelJoin([FromBody]string token)
        {
            if (!(validToken(token)))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            if (pendingInfo.UserToken == null || pendingInfo.UserToken != token)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            pendingInfo = new PendingGameInfo();

            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
                    // the Users table.
                    using (SqlCommand command = new SqlCommand("Delete from Games where GameState = @GameState", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameState", "pending");
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
        [Route("BoggleService/games/{gameID}")]
        public int PutPlayWord(string gameID, PlayWordInput play)
        {
            
            
            int score = 0;
            

            if (play.word == null || play.word == "" || play.word.Trim().Length > 30 || !validToken(play.userToken) || !validID(gameID))
            {
                 throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

           

            //OLD CODE
                games[gameID].calculateTimeLeft();
                GameInfo temp = games[gameID];

                if (temp.Player1.PlayerToken != play.userToken && temp.Player2.PlayerToken != play.userToken)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                if (temp.GameState == "completed" || temp.GameState == "pending" || temp.TimeLeft <= 0)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }
            //



            //NEW CODE
            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("select Player1, Player2 from Games where GameID = @GameID"))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            string player1token = (string)reader["Player1"];
                            string player2token = (string)reader["Player2"];

                            if (player1token != play.userToken && player2token != play.userToken)
                            {
                                throw new HttpResponseException(HttpStatusCode.Forbidden);
                            }
                        }
                    }



                    using (SqlCommand command = new SqlCommand("select GameState from Games where GameID = @GameID"))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            string gameState = (string)reader["GameState"];

                            if (gameState == "completed" || gameState == "pending")
                            {
                                throw new HttpResponseException(HttpStatusCode.Forbidden);
                            }
                        }
                    }




                    using (SqlCommand command = new SqlCommand("select TimeLimit, StartTime from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                reader.Close();
                                trans.Commit();
                                throw new HttpResponseException(HttpStatusCode.Forbidden);
                            }

                            reader.Read();

                            int timeLimit = (int)reader["TimeLimit"];
                            int startTime = (int)reader["StartTime"];
                            int timeLeft = calculateTimeLeft(timeLimit, startTime);

                            if (timeLeft <= 0)
                            {
                                throw new HttpResponseException(HttpStatusCode.Conflict);
                            }

                        }
                    }
                }
            }
            //



            play.word = play.word.Trim();




            





            PlayedWord playedWord = new PlayedWord(play.word);


                
                if (temp.Player1.PlayerToken == play.userToken)
                {
                    int index = temp.Player1.WordsPlayed.FindIndex(f => f.Word == play.word);

                    if (index >= 0)
                    {
                        playedWord.Score = 0;
                        score = 0;
                        temp.Player1.WordsPlayed.Add(playedWord);
                    }
                    else
                    {
                        score = temp.MisterBoggle.score(play.word);
                        playedWord.Score = score;
                        temp.Player1.WordsPlayed.Add(playedWord);
                    }
                    temp.Player1.Score += score;
                    games[gameID] = temp;
                }
                else
                {
                    int index = temp.Player2.WordsPlayed.FindIndex(f => f.Word == play.word);
                    if (index >= 0)
                    {
                        playedWord.Score = 0;
                        score = 0;
                        temp.Player2.WordsPlayed.Add(playedWord);
                    }
                    else
                    {
                        score = temp.MisterBoggle.score(play.word);
                        playedWord.Score = score;
                        temp.Player2.WordsPlayed.Add(playedWord);
                    }
                    temp.Player2.Score += score;
                    games[gameID] = temp;
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
        [Route("BoggleService/games/{gameID}/{brief}")]
        public GameInfo GetGameStatus(string gameID, bool brief)
        {

            if (!validID(gameID))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    string gameState = null;
                    bool setToCompleted = false;
                    //Checks the time remaining for gameID, which determines if the game is complete.
                    using (SqlCommand command = new SqlCommand("select TimeLimit, StartTime, GameState from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            gameState = (string)reader["GameState"];
                            int timeLimit = (int)reader["TimeLimit"];
                            int startTime = (int)reader["StartTime"];
                            if (gameState == "active")
                            {
                                int timeLeft = calculateTimeLeft(timeLimit, startTime);
                                if (timeLeft <= 0)
                                {
                                    setToCompleted = true;
                                }
                            }
                        }
                        trans.Commit();
                    }
                    if (setToCompleted == true)
                    {
                        //Updates the GameState to "completed" if we get to this point.
                        using (SqlCommand command = new SqlCommand("update Games set GameState=@GameState where GameID = @GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@GameState", "completed");
                            command.Parameters.AddWithValue("@GameID", gameID);
                            trans.Commit();
                        }
                    }
                    gameState = null;
                    using (SqlCommand command = new SqlCommand("select GameState from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            gameState = (string)reader["GameState"];
                        }
                        trans.Commit();
                    }
                    GameInfo output = new GameInfo();
                    if (brief == true)
                    {
                        if (gameState == "pending")
                        {
                            output.GameState = "pending";
                            return output;
                        }
                        if (gameState == "active")
                        {
                            using (SqlCommand command = new SqlCommand("select TimeLimit, StartTime, Player1Score," +
                                " Player2Score from Games where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);

                                output.GameState = "active";
                                output.Player1 = new PlayerInfo();
                                output.Player2 = new PlayerInfo();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    int timeLimit = (int)reader["TimeLimit"];
                                    int startTime = (int)reader["StartTime"];
                                    int player1Score = (int)reader["Player1Score"];
                                    int player2Score = (int)reader["Player2Score"];
                                    int timeLeft = calculateTimeLeft(timeLimit, startTime);

                                    output.TimeLeft = timeLeft;
                                    output.Player1.Score = player1Score;
                                    output.Player2.Score = player2Score;
                                }
                                trans.Commit();
                                return output;
                            }
                        }
                        if (gameState == "completed")
                        {
                            using (SqlCommand command = new SqlCommand("select Player1Score, Player2Score from Games where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);
                                output.GameState = "completed";
                                output.Player1 = new PlayerInfo();
                                output.Player2 = new PlayerInfo();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    int player1Score = (int)reader["Player1Score"];
                                    int player2Score = (int)reader["Player2Score"];

                                    output.Player1.Score = player1Score;
                                    output.Player2.Score = player2Score;
                                }
                                trans.Commit();
                                return output;
                            }
                        }
                    }
                    else
                    {
                        if (gameState == "pending")
                        {
                            output.GameState = "pending";
                            return output;
                        }
                        if (gameState == "active")
                        {
                            string player1 = null;
                            string player2 = null;
                            output.GameState = "active";
                            output.Player1 = new PlayerInfo();
                            output.Player2 = new PlayerInfo();
                            using (SqlCommand command = new SqlCommand("select Player1, Player2, Board, TimeLimit, " +
                                "StartTime, Player1Score, Player2Score from Games where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    player1 = (string)reader["Player1"];
                                    player2 = (string)reader["Player2"];
                                    string board = (string)reader["Board"];
                                    int timeLimit = (int)reader["TimeLimit"];
                                    int startTime = (int)reader["StartTime"];
                                    int player1Score = (int)reader["Player1Score"];
                                    int player2Score = (int)reader["Player2Score"];
                                    int timeLeft = calculateTimeLeft(timeLimit, startTime);

                                    output.Board = board;
                                    output.TimeLimit = timeLimit;
                                    output.TimeLeft = timeLeft;
                                    output.Player1.Score = player1Score;
                                    output.Player2.Score = player2Score;
                                }
                                trans.Commit();
                            }
                            using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@UserID", player1);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    string nickname = (string)reader["Nickname"];
                                    output.Player1.Nickname = nickname;
                                }
                                trans.Commit();
                            }
                            using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@UserID", player2);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    string nickname = (string)reader["Nickname"];
                                    output.Player2.Nickname = nickname;
                                }
                                trans.Commit();
                            }
                            return output;
                        }
                        if (gameState == "completed")
                        {
                            string player1 = null;
                            string player2 = null;
                            output.GameState = "completed";
                            output.Player1 = new PlayerInfo();
                            output.Player2 = new PlayerInfo();
                            output.Player1.WordsPlayed = new List<PlayedWord>();
                            output.Player2.WordsPlayed = new List<PlayedWord>();
                            using (SqlCommand command = new SqlCommand("select Player1, Player2, Board, TimeLimit, " +
                                "StartTime, Player1Score, Player2Score from Games where GameID = @GameID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@GameID", gameID);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    player1 = (string)reader["Player1"];
                                    player2 = (string)reader["Player2"];
                                    string board = (string)reader["Board"];
                                    int timeLimit = (int)reader["TimeLimit"];
                                    int startTime = (int)reader["StartTime"];
                                    int player1Score = (int)reader["Player1Score"];
                                    int player2Score = (int)reader["Player2Score"];
                                    int timeLeft = calculateTimeLeft(timeLimit, startTime);

                                    output.Board = board;
                                    output.TimeLimit = timeLimit;
                                    output.TimeLeft = timeLeft;
                                    output.Player1.Score = player1Score;
                                    output.Player2.Score = player2Score;
                                }
                                trans.Commit();
                            }
                            using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@UserID", player1);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    string nickname = (string)reader["Nickname"];
                                    output.Player1.Nickname = nickname;
                                }
                                trans.Commit();
                            }
                            using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command.Parameters.AddWithValue("@UserID", player2);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    string nickname = (string)reader["Nickname"];
                                    output.Player2.Nickname = nickname;
                                }
                                trans.Commit();
                            }
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
                                trans.Commit();
                            }
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
                                trans.Commit();
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
        /// <returns></returns>
        private bool validToken(string userToken)
        {
            if(userToken == null || userToken.Trim().Length != 36)
            {
                return false;
            }

            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
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
                            if(!reader.HasRows)
                            {
                                reader.Close();
                                trans.Commit();
                                return false;
                            }
                        }
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
        private bool validID(string gID)
        {
            if(gID == null || gID.Trim().Length == 0)
            {
                return false;
            }

            using (SqlConnection conn = new SqlConnection(DB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
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
                                trans.Commit();
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Helper method to calculate the TimeLeft based on the time of day.
        /// </summary>
        private int calculateTimeLeft(int timeLimit, int startTime)
        {
            return timeLimit - (((DateTime.Now.Minute * 60) + DateTime.Now.Second) - startTime);
        }

        /// <summary>
        /// The pending game info for the current pending game.
        /// </summary>
        private static PendingGameInfo pendingInfo = new PendingGameInfo();       
    }
}