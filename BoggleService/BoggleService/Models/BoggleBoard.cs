// Written by Joe Zachary for CS 3500, November 2012.

using System;
using System.IO;
using BoggleService.Models;

namespace BoggleService
{
    /// <summary>
    /// Represents a Boggle board.
    /// </summary>
    public class BoggleBoard
    {
        // The 4x4 Boggle board
        private char[,] board;

        // The 16 cubes that make up a standard Boggle Board
        private string[] cubes =
            {
                "RIFOBX",
                "IFEHEY",
                "DENOWS",
                "UTOKND",
                "HMSRAO",
                "LUPETS",
                "ACITOA",
                "YLGKUE",
                "QBMJOA",
                "EHISPN",
                "VETIGN",
                "BALIYT",
                "EZAVND",
                "RALESC",
                "UWILRG",
                "PACEMD"
            };

        /// <summary>
        /// Creates a randomly-generated BoggleBoard 
        /// </summary>
        public BoggleBoard()
        {
            // Shuffle the cubes
            Random r = new Random();
            for (int i = cubes.Length - 1; i >= 0; i--)
            {
                int j = r.Next(i + 1);
                string temp = cubes[i];
                cubes[i] = cubes[j];
                cubes[j] = temp;
            }

            // Make a string by choosing one character at random
            // frome each cube.
            string letters = "";
            for (int i = 0; i < cubes.Length; i++)
            {
                letters += cubes[i][r.Next(6)];
            }

            // Make the board
            MakeBoard(letters);
        }

        /// <summary>
        /// Creates a BoggleBoard from the provided 16-letter string.  The
        /// method is case-insensitive.  If there aren't exactly 16 letters
        /// in the string, throws an ArgumentException.  The string consists
        /// of the first row, then the second row, then the third, then the fourth.
        /// </summary>
        public BoggleBoard(string letters)
        {
            // Use upper case
            letters = letters.ToUpper();

            // Make sure letters are legal
            if (letters.Length != 16)
            {
                throw new ArgumentException();
            }
            foreach (char c in letters)
            {
                if (!Char.IsLetter(c))
                {
                    throw new ArgumentException();
                }
            }

            // Make the board
            MakeBoard(letters);
        }

        /// <summary>
        /// Makes a board from the 16-letter string
        /// </summary>
        private void MakeBoard(string letters)
        {
            board = new char[4, 4];
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = letters[index++];
                }
            }
        }


        /// <summary>
        /// Returns the 16 letters that make up this board.  It is formed
        /// by appending the first row to the second row to the third row
        /// to the fourth row.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string letters = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    letters += board[i, j];
                }
            }
            return letters;
        }


        /// <summary>
        /// Reports whether the provided word can be formed by tracking through
        /// this Boggle board as described in the rules of Boggle.  The method
        /// is case-insensitive.
        /// </summary>
        public bool CanBeFormed(string word)
        {
            // Work in upper case
            word = word.ToUpper();

            // Mark every square on the board as unvisited.
            bool[,] visited = new bool[4, 4];

            // See if there is any starting point on the board from which
            // the word can be formed.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (CanBeFormed(word, i, j, visited))
                    {
                        return true;
                    }
                }
            }

            // If no starting point worked, return false.
            return false;
        }

        /// <summary>
        /// Reports whether the provided word can be formed by tracking through
        /// this Boggle board by beginning at location [i,j] and avoiding any
        /// squares marked as visited.
        /// </summary>
        private bool CanBeFormed(string word, int i, int j, bool[,] visited)
        {
            // If the word is empty, report success.
            if (word.Length == 0)
            {
                return true;
            }

            // If an index is out of bounds, report failure.
            if (i < 0 || i >= 4 || j < 0 || j >= 4)
            {
                return false;
            }

            // If this square has already been visited, report failure.
            if (visited[i, j])
            {
                return false;
            }

            // If the first letter of the word doesn't match the letter on
            // this square, report failure.  Otherwise, obtain the remainder
            // of the word that we should match next.
            // (Note that Q gets special treatment.)

            char firstChar = word[0];
            string rest = word.Substring(1);

            if (firstChar != board[i, j])
            {
                return false;
            }

            if (firstChar == 'Q')
            {
                if (rest.Length == 0)
                {
                    return false;
                }
                if (rest[0] != 'U')
                {
                    return false;
                }
                rest = rest.Substring(1);
            }

            // Mark this square as visited.
            visited[i, j] = true;

            // Try to match the remainder of the word, beginning at a neighboring square.
            if (CanBeFormed(rest, i - 1, j - 1, visited)) return true;
            if (CanBeFormed(rest, i - 1, j, visited)) return true;
            if (CanBeFormed(rest, i - 1, j + 1, visited)) return true;
            if (CanBeFormed(rest, i, j - 1, visited)) return true;
            if (CanBeFormed(rest, i, j + 1, visited)) return true;
            if (CanBeFormed(rest, i + 1, j - 1, visited)) return true;
            if (CanBeFormed(rest, i + 1, j, visited)) return true;
            if (CanBeFormed(rest, i + 1, j + 1, visited)) return true;

            // We failed.  Unmark this square and return false.
            visited[i, j] = false;
            return false;
        }

        public int score(string word)
        {
            //check in the dictionary if the word is in the dictionary

            //Use its Copy to Output Directory property to Copy if Newer.  Access it from your code with the pathname
            StreamReader s = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\dictionary.txt");
            string line = s.ReadLine();
            bool inDictionary = false;

            while(line != null)
            {
                if (line == word)
                {
                    inDictionary = true;
                }
                line = s.ReadLine();
            }

            if(!inDictionary)
            {
                return -1;
            }
            

          

            if (!CanBeFormed(word))
            {
                return -1;
            }
            else
            {
                switch (word.Length)
                {
                    case 0:
                        return -1;
                    case 1:
                        return 0;
                    case 2:
                        return 0;
                    case 3:
                        return 1;
                    case 4:
                        return 1;
                    case 5:
                        return 2;
                    case 6:
                        return 3;
                    case 7:
                        return 5;
                    default:
                        if (word.Length >= 8)
                        {
                            return 11;
                        }
                        break;
                }

                return -2;
            }

        }
    }
}
