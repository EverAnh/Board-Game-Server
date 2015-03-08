using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game_ConnectFour : Game_Generic
    {

        // Declare a grid of 7 columns and 6 rows.

        private int cols = 7;
        private int rows = 6;
        private bool gameState = true; // true while the game is active? may be unused
        
        public Game_ConnectFour()
        {
            gameBoard = new int[cols, rows];
            gameType = "connectFour";
            maxPlayers = 2;
        }

        public void handlePlayerTurn(String s) // each player will call this function with their input.
        {
            int placeX = 0;
            int placeY = 0;
            int color = 0;  // color: [1] for black, [2] for red.
         
            // store what you parse into an array?
            parsePlayerInput(s);


            // get input on where the piece has been placed and save it to gameBoard
            // make sure to check that the spot is already occupied.
            if (checkOccupiedState(placeX, placeY)) // if the spot is not yet occupied
            {
                // place the piece in the spot
                assignPiece(placeX, placeY, color); // we'd like posX, posY, color [1] for black, [2] for red. [0] indicates not occupied

                // update the game board graphics
                // using the piece, check if the state is a win condition
                if (checkGameState(placeX, placeY)) // if the placement happens to be a win condition
                {
                    gameState = false; // game is over!
                    Console.Write("Game over!"); // print to console log 
                }

            }

        }

        // Game Logic

        public Boolean checkGameState(int x, int y)
        {
           // this function will take a game piece placement, and check all rows/columns next to it for a win condition.

            checkRowFour(x, y);
            checkColumnFour(x, y);

            
            return false;
        }

        public Boolean checkRowFour(int x, int y)
        {
            return true;
        }

        public Boolean checkColumnFour(int x, int y)
        {
            return true;
        }



        public Boolean parsePlayerInput(String s) // boolean for now 
        {
            // Parses player input and store into local vars

            // recieve inputString here:

            return true;

        }

        public Boolean checkOccupiedState(int x, int y)
        {
            // will take in the recently added piece.
            // will return true if you are allowed to place a piece (unoccupied)
            // otherwise, will return false if it is actually occupied.

            if (0 == getPiece(x,y))
            {
                return true;
            }
            else
                return false;
        }
        
        





    }
}


