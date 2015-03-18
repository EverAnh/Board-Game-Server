using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game_SnakesLadders : Game_Generic
    {
        // We'll use a 100 boardPiece array.
        class boardPiece
        {
            int index;    // index value of this board Piece
            int type;     // 0 for norm, does nothing || 1 for ladder || 2 for snake
            int val;      // Value that the piece might lead [if 1 or 2]
            String text = '';  // If you want to display text when you land on the piece

            // Getter/setter methods

            public String getText()
            {
                return text;
            }

            public void setText(String set)
            {
                test = set;
            }
            
            public int getIndex()
            {
                return index;
            }

            public void setIndex(int i)
            {
                index = i;
            }

            public int getType()
            {
                return type;
            }

            public void setType(int t)
            {
                type = t;
            }

            public int getValue()
            {
                return value;
            }

            public int setValue(int v)
            {
                value = v
            }
        }

         protected boardPiece[] gameBoardSL;

        
        public Game_SnakesLadders()
        {
            // gameBoard = new int[cols, rows];         // set the rows and columns into the gameboard we made earlier. 
            gameBoardSL = new boardPiece[100];          // size 100 array of boardPieces
            gameType = "snakesLadders";                 // Server will know that we made a connectFour game
            gamePieces = new List<Piece_Generic>();     // May not need this for our implementation
            currentPlayers = new List<Player>();        // list of players
            loop = new Server_GameLoop();               // Make a new server_gameloop (unecessary?)
            maxPlayers = 2;                             // 2 players max for now.
            gameState = true;

            // Add two player's info in here, we don't really need to track all the pieces as gameBoard handles a lot of that for us.
            // We'll use gamePieces to track the last known move of each respective player.
            
            gamePieces.Add(new Piece_Generic());
            gamePieces[0].setX(0);
            gamePieces[0].setY(0);
            gamePieces[0].setValue(1);                  // Player 1
            
            gamePieces.Add(new Piece_Generic());
            gamePieces[1].setX(0);
            gamePieces[1].setY(0);
            gamePieces[1].setValue(2);                  // Player 2
        }

        public override List<Piece_Generic> getPieces()
        {
            return gamePieces;                          
        }
        public virtual List<Piece_Generic> getPieces()
        {
            return gamePieces;
        }

        public bool getGameState()
        {
            return gameState;
        }

        // Since we're using our own game board, overwrite this method from Generic.

        public override int getPiece(int getX, int getY)
        {
            return gameBoard[getX, getY];
        }

         // Since we're using our own game board, overwrite this method from Generic.

        public virtual int assignPiece(int getX, int getY, int value)
        {
            gameBoard[getX, getY] = value;

            return 0;
        }

        public virtual bool handlePlayerTurn(String s)
        {
            String[] move = s.Split('%');
            return checkGameState( System.Convert.ToInt32(move[0]), System.Convert.ToInt32(move[1]) );
        }

        public virtual void endGame()
        {
            Console.Write("[Not yet implemented] Terminating game...thanks for playing! Come back soon");
        }

        protected virtual bool checkWinCondition(int x, int y)
        {
            /*
            if (gamePieces[loop.getActivePlayer()].getX() == 0 && gamePieces[loop.getActivePlayer()].getY() == 0)
            {
                Console.Write("Player " + loop.getActivePlayer() + " has won!"); // print
                gameState = false;      // set to false
                endGame();              // attempt to end the game.
                return true;            // quick termination, return back to the caller
            }
            */
            return false; // not a win
        }
        // returns true if the move is a valid move, otherwise returns false
        protected virtual bool checkGameState(int x, int y)
        {
            
            
            // setting up additional logic so once any player reaches 0,0
            // we'll pass a "game over" message.   
            // move up or down, not left/right
            if (gamePieces[loop.getActivePlayer()].getX() == x)
            {
                // check to see if moved exactly 1
                if ((gamePieces[loop.getActivePlayer()].getY() - 1 == y) || (gamePieces[loop.getActivePlayer()].getY() + 1 == y))
                {
                    gamePieces[loop.getActivePlayer()].setY(y);
                    if (checkWinCondition(
                       gamePieces[loop.getActivePlayer()].getX(),
                        gamePieces[loop.getActivePlayer()].getY()))
                    {
                        // Do something!
                        Console.Write("Code this in, Jason 2!");
                    }
                    return true;
                }
            }

            else if (gamePieces[loop.getActivePlayer()].getY() == y)
            {
                // check to see if moved exactly 1
                if ((gamePieces[loop.getActivePlayer()].getX() - 1 == x) || (gamePieces[loop.getActivePlayer()].getX() + 1 == x))
                {
                    gamePieces[loop.getActivePlayer()].setX(x);
                     if (checkWinCondition(
                        gamePieces[loop.getActivePlayer()].getX(),
                        gamePieces[loop.getActivePlayer()].getY()))
                    {
                        // Do something!
                        Console.Write("Code this in, Jason 2!");
                    }

                    return true;
                }
            }


            
            return false;
        }


        // Methods that our main game loop calls on

        // Gameloop is as follows:
        /*
            Player rolls die.                       returnDiceRoll
            Player gets current location to end.    calcDistToEnd()
            Player moves to location, if able       movePiece()
            Board piece at location is activated    boardAction(index)
            Action is done (nothing, ladder, snake) movePiece/extraTurn...
            -- If extra turn, start over from roll
            Otherwise, pass to next player
        */

        private int returnDiceRoll()
        {
            // Roll the die, returning a random number from 1-6.
            // If we roll 6, make sure to let user know they get another one!
            return 1;
        }

        private int calcDistToEnd(int l)
        {
            // takes in the player current location and returns the distance to the end
            // run this every loop and store it as a player value?
            return 0;
        }

        private int boardAction(int l)
        {
            // takes in player current location (via index)
            // checks the board piece at his spot
            // if 0, nothing happens
            // if 1, player is bumped up
            // if 2, player is bumped down
            if (l == 0)
            {
                // fill out
            }
            else if (l == 1)
            {
                // fill out
            }
            else if (l == 2)
            {
                // fill out
            }

            return -1; // invalid action
        }

        private bool movePiece(int l)
        {
            // attempts to move a piece
        }

        private void initializeBoardState()
        {
            // Set up the board with appropriate values.
            for (int v = 0; v < 100; v++)
            {
                gameBoardSL[v].setIndex = v;    // set the index of each piece
                gameBoardSL[v].setValue = 0;    // We'll set this seperately
                gameBoardSL[v].setType = 0;     // Same as above
            }

            // Set snake type/positions here

            gameBoardSL[62].setType = 2;
            gameBoardSL[87].setType = 2;
            gameBoardSL[49].setType = 2;
            gameBoardSL[93].setType = 2;
            gameBoardSL[65].setType = 2;
            gameBoardSL[64].setType = 2;

            gameBoardSL[62].setValue = 19;
            gameBoardSL[87].setValue = 24;
            gameBoardSL[49].setValue = 11;
            gameBoardSL[93].setValue = 73;
            gameBoardSL[65].setValue = 53;
            gameBoardSL[64].setValue = 41;

            // Set ladder locations here

            gameBoardSL[2].setType = 1;
            gameBoardSL[4].setType = 1;
            gameBoardSL[28].setType = 1;
            gameBoardSL[21].setType = 1;
            gameBoardSL[9].setType = 1;
            gameBoardSL[71].setType = 1;

            gameBoardSL[2].setValue = 38;
            gameBoardSL[4].setValue = 14;
            gameBoardSL[28].setValue = 84;
            gameBoardSL[21].setValue = 42;
            gameBoardSL[9].setValue = 31;
            gameBoardSL[71].setValue = 91;

        }



    }
}
