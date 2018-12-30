using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Program
    {
        private static int turn;
        private static int[] taken=new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private static string[] board=new string[8];
        static void Main(string[] args)
        {
            //initialize board
            InitializeBoard();
            DrawBoard();
            while(!CheckWinner())
            {
                AskQuestion();
                DrawBoard();
            }
            Console.Read();
        }
        private static void InitializeBoard()
        {
           board = new string[]{ "1", "2", "3", "4", "5", "6", "7", "8", "9" };
           taken =new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 };
           turn = 1;
        }
        private static void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine("     |     |     \n  {0}  |  {1}  |  {2}  \n_____|_____|_____\n     |     |     \n  {3}  |  {4}  |  {5}  \n" +
                "_____|_____|_____\n     |     |     \n  {6}  |  {7}  |  {8}  \n     |     |     ", board[0], board[1], board[2], board[3], board[4], board[5], board[6], board[7], board[8]);

        }
        private static void AskQuestion()
        {
            bool flag = false;
          

            while(!flag)
            {
                Console.WriteLine("Which spot would you like to take player {0}?", turn);
                if (int.TryParse(Console.ReadLine(), out int spot))
                {
                    bool takenCheck = false;
                    //checks if the spot is within range
                    if(spot>0 && spot<10)
                    {
                        int index = spot - 1;
                        //check whether or not the spot has already been taken
                        foreach (int value in taken)
                        {
                            if (value == spot)
                            {
                                Console.WriteLine("Sorry, but spot {0} is already taken", value);
                                takenCheck = true;
                            }
                        }
                        //means spot is not taken 
                        //Check if it is player 1 or 2
                        if(turn==1 && !takenCheck)
                        {
                            board[index] = "O";
                            TakenSpot(spot);
                            turn++;
                            flag = true;
                        }
                        else if(turn==2 && !takenCheck)
                        {
                            board[index] = "X";
                            TakenSpot(spot);
                            turn--;
                            flag = true;
                        }
                        
                    }
                }
                else
                {
                    Console.WriteLine("Not a valid input.  Please try again");
                }
            }
        
        }
        private static bool CheckWinner()
        {
            //check possible winning solutions
            //check list goes horizontal checks, then vertical checks, then diagonal checks
            //could the checking be done easier with 2d arrays?  Yes but I don't like them.
            if ((board[0].Equals(board[1]) && board[1].Equals(board[2])) || (board[3].Equals(board[4]) && board[4].Equals(board[5])) ||
                (board[6].Equals(board[7]) && board[7].Equals(board[8])) || (board[0].Equals(board[3]) && board[3].Equals(board[6])) ||
                (board[1].Equals(board[4]) && board[4].Equals(board[7])) || (board[2].Equals(board[5]) && board[5].Equals(board[8])) ||
                (board[0].Equals(board[4]) && board[4].Equals(board[8])) || (board[2].Equals(board[4]) && board[4].Equals(board[6])))
            {
                //fix the problem in which it goes to the next player
                if (turn == 2)
                    turn--;
                else
                    turn++;
                Console.WriteLine("Congratulations Player {0}, you won the game!", turn);
                return true;
            }
            else if(taken[0]!=0 && taken[1]!=0 && taken[2] != 0 && taken[3] != 0 && taken[4] != 0 && taken[5] != 0 && taken[6] != 0 && taken[7] != 0 && taken[8] != 0)
            {
                //checks if all possible spots have been taken
                //return true to end the game
                Console.WriteLine("Good Job Players 1 and 2, but sadly this game ended in a Tie.");
                return true;
            }
            return false;
        }
        private static void TakenSpot(int index)
        {
            for(int i=0;i<taken.Length;i++)
            {
                if(taken[i]==0)
                {
                    taken[i] = index;
                    break;
                }
            }
        }
    }
}
