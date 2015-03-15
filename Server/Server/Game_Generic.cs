using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game_Generic
    {
        protected int[ , ] gameBoard;
        protected List<Piece_Generic> gamePieces;
        protected List<Player> currentPlayers;
        protected int numberPlayers;
        protected int maxPlayers;
        protected Server_GameLoop loop;
        protected String gameType;
        protected bool gameState;         // True for running / False for not

        public Game_Generic()
        {
            gamePieces = new List<Piece_Generic>();
            currentPlayers = new List<Player>();
            loop = new Server_GameLoop();
        }

        public Game_Generic(int x_size, int y_size)
        {
            gameBoard = new int[x_size, y_size];
            gamePieces = new List<Piece_Generic>();
            currentPlayers = new List<Player>();
            loop = new Server_GameLoop();
            gameType = "generic";
            maxPlayers = 1;
            gamePieces.Add(new Piece_Generic());
            gamePieces[0].setX(2);
            gamePieces[0].setY(2);
            gameState = true;                // set to true for running.
        }

        public List<Player> getPlayers()
        {
            return currentPlayers;
        }

        public virtual List<Piece_Generic> getPieces()
        {
            return gamePieces;
        }

        public int getNumberPlayers()
        {
            return numberPlayers;
        }

        public int getMaxPlayers()
        {
            return maxPlayers;
        }

        public Server_GameLoop getLoop()
        {
            return loop;
        }

        public String getGameType()
        {
            return gameType;
        }

        public bool getGameState()
        {
            return gameState;
        }

        public int getPiece(int getX, int getY)
        {
            return gameBoard[getX, getY];
        }

        public virtual int assignPiece(int getX, int getY, int value)
        {
            gameBoard[getX, getY] = value;

            return 0;
        }

        public void addPlayer(Player p)
        {
            if (numberPlayers < maxPlayers)
            {
                currentPlayers.Add(p);
                numberPlayers = currentPlayers.Capacity;
            }
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

        // returns true if the move is a valid move, otherwise returns false
        protected virtual bool checkGameState(int x, int y)
        {
            // setting up additional logic so once any player reaches 0,0
            // we'll pass a "game over" message.   
            if (gamePieces[loop.getActivePlayer()].getX() == 0 && gamePieces[loop.getActivePlayer()].getY() == 0)
            {
                Console.Write("Player " + loop.getActivePlayer() + " has won!"); // print
                gameState = false;      // set to false
                endGame();              // attempt to end the game.
                return true;            // quick termination, return back to the caller
            }
            // move up or down, not left/right
            if (gamePieces[loop.getActivePlayer()].getX() == x)
            {
                // check to see if moved exactly 1
                if ((gamePieces[loop.getActivePlayer()].getY() - 1 == y) || (gamePieces[loop.getActivePlayer()].getY() + 1 == y))
                {
                    gamePieces[loop.getActivePlayer()].setY(y);

                    return true;
                }
            }

            else if (gamePieces[loop.getActivePlayer()].getY() == y)
            {
                // check to see if moved exactly 1
                if ((gamePieces[loop.getActivePlayer()].getX() - 1 == x) || (gamePieces[loop.getActivePlayer()].getX() + 1 == x))
                {
                    gamePieces[loop.getActivePlayer()].setX(x);

                    return true;
                }
            }


            
            return false;
        }
    }
}
