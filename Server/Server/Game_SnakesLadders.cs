using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// TODO: Change the game pieces to movable pieces.

namespace Game
{
    public class Game_SnakesLadders : Game_Generic
    {
        protected Board_Piece[] gameBoardSL;            // different from gameBoard, note.
        
        public Game_SnakesLadders()
        {
            // gameBoard = new int[cols, rows];         // set the rows and columns into the gameboard we made earlier. 
            gameBoardSL = new Board_Piece[100];          // size 100 array of boardPieces
            gameType = "snakesLadders";                 // Server will know that we made a connectFour game
            gamePieces = new List<Piece_Generic>();     // May not need this for our implementation
            currentPlayers = new List<Player>();        // list of players
            loop = new Server_GameLoop();               // Make a new server_gameloop (unecessary?)
            maxPlayers = 2;                             // 2 players max for now.
            gameState = true;

            // Add two player's info in here, we don't really need to track all the pieces as gameBoard handles a lot of that for us.
            // We'll use gamePieces to track the last known move of each respective player.
            
            // Player 1
            gamePieces.Add(new Piece_Generic());
            gamePieces[0].setX(0);
            gamePieces[0].setY(0);
            gamePieces[0].setValue(1);    

            // Player 2
            gamePieces.Add(new Piece_Generic());
            gamePieces[1].setX(0);
            gamePieces[1].setY(0);
            gamePieces[1].setValue(2);                  

            initializeBoardState();                     // Initialize the board.
        }

        public override List<Piece_Generic> getPieces()
        {
            return gamePieces;                          
        }

        // Since we're using our own game board, overwrite this method from Generic.

        public override int getPiece(int getX, int getY)
        {
            return gameBoardSL[getX, getY];
        }

         // Since we're using our own game board, overwrite this method from Generic.

        public virtual bool handlePlayerTurn(String s)
        {
            String[] move = s.Split('%');           // Split the string
            return checkGameState( System.Convert.ToInt32(move[0]),
                                   System.Convert.ToInt32(move[1]) );
        }

        // Override this from generic

        public override void endGame()
        {
            Console.Write("[Not yet implemented] Terminating game...thanks for playing! Come back soon");
        }

        protected virtual bool checkWinCondition(int x, int y)
        {
            
            if (gamePieces[loop.getActivePlayer()].getX() == 99)    // cell 99 is the win condition
            {
                Console.Write("Player " + loop.getActivePlayer() + " has won!"); // print
                gameState = false;      // set to false
                endGame();              // attempt to end the game.
                return true;            // quick termination, return back to the caller
            }
            
            return false; // not a win
        }
        
        // returns true if the move is a valid move, otherwise returns false
        
        protected override bool checkGameState(int x, int y)
        {
            return true;
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
            Random rnd = new Random();          // Find a random number
            int dice = rnd.Next(1, 7);          // Between 1 and 6.
            return dice;
        }

        private int calcDistToEnd(int l)
        {
            // takes in the player current location and returns the distance to the end
            return 100-l; // or 99-l, could lead to indexing errors. (Jason C.)
        }

        private int boardAction(int index)
        {
            // takes in player current location (via index) and checks the board piece at his spot
            // if 0, nothing happens
            // if 1, player is bumped up
            // if 2, player is bumped down

            if (gameBoardsSL[index].getType() == 0)
            {
                return index;   // nothing happens, just return the index?
            }
            else if (gameBoardsSL[index].getType() == 1)
            {
                gamePieces[loop.getActivePlayer()].setX(gameBoardSL[index].getValue());
                return gameBoardSL[index].getValue();
            }
            else if (gameBoardsSL[index].getType() == 2)
            {
                gamePieces[loop.getActivePlayer()].setX(gameBoardSL[index].getValue());
                return gameBoardSL[index].getValue();
            }

            return -1; // invalid action
        }

        private bool movePiece(int newLocation)
        {

            gamePieces[loop.getActivePlayer()].setX(newLocation);            // moves a piece
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
