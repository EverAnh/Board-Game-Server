using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game_Generic
    {                         
        protected int[ , ] gameBoard;                   // Default gameboard; feel free to use your own
        protected List<Piece_Generic> gamePieces;       // List of current gamePieces - use to track various players
        protected List<Player> currentPlayers;          // List of players currently in the game, should be max 2
        protected int numberPlayers;                    // Current number of players in the game, should be 0 or 1
        protected int maxPlayers;                       // The maximum amount of players allowed, shouldb e 2
        protected Server_GameLoop loop;                 // We need a gameloop to tell the client which turn it is
        protected String gameType;                      // gameType will be generic by default.
        protected bool gameState = true;                       // True for running / False for not
        protected bool gameWaiting;                     // True once game has been created; false when game has started.

        public Game_Generic()
        {
        }

        public Game_Generic(int x_size, int y_size)
        {
            gameBoard = new int[x_size, y_size];
            gamePieces = new List<Piece_Generic>();
            currentPlayers = new List<Player>();
            loop = new Server_GameLoop();
            gameType = "generic";
            maxPlayers = 2;

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
                numberPlayers = currentPlayers.Count;
            }
        }

        public virtual bool handlePlayerTurn(String s)
        {
            
            Console.Write("Recieved: " + s + " from client. Handling message...");
            if (s != null && s != " " && s!= "" ) // if it not a null string or empty string or empty string with one little character AHH
            {
                String[] move = s.Split('%'); 
                return checkGameState( System.Convert.ToInt32(move[0]), System.Convert.ToInt32(move[1]) );
            }

            Console.Write("String was null, can't split that bro.");
            return true;        // chill, man!

        }

        public virtual void endGame()
        {
            Console.Write("[Not yet implemented] Terminating game...thanks for playing! Come back soon");
        }

        protected virtual bool checkWinCondition(int x, int y)
        {
            if (gamePieces[loop.getActivePlayer()].getX() == 0 && gamePieces[loop.getActivePlayer()].getY() == 0)
            {
                Console.Write("Player " + loop.getActivePlayer() + " has won!"); // print
                gameState = false;      // set to false
                endGame();              // attempt to end the game.
                return true;            // quick termination, return back to the caller
            }
            return false;               // not a win
        }

        // returns true if the move is a valid move, otherwise returns false
        protected virtual bool checkGameState(int x, int y)
        {
            // setting up additional logic so once any player reaches 0,0
            // we'll pass a "game over" message.   
            // move up or down, not left/right

            ( (Piece_Movable) gamePieces[loop.getActivePlayer()] ).setXPrev(gamePieces[loop.getActivePlayer()].getX() );
            ( (Piece_Movable) gamePieces[loop.getActivePlayer()] ).setYPrev(gamePieces[loop.getActivePlayer()].getY() );

            if (gamePieces[loop.getActivePlayer()].getX() == x)
            {
                // check to see if moved exactly 1
                if ((gamePieces[loop.getActivePlayer()].getY() - 1 == y) || (gamePieces[loop.getActivePlayer()].getY() + 1 == y))
                {
                    //gamePieces[loop.getActivePlayer()+2].setY(gamePieces[loop.getActivePlayer()].getY());
                    gamePieces[loop.getActivePlayer()].setY(y);
                    
                    if (checkWinCondition(
                       gamePieces[loop.getActivePlayer()].getX(),
                        gamePieces[loop.getActivePlayer()].getY()))
                    {
                        // Do something!
                        // Console.Write("Code this in, Jason 2!");
                    }
                    return true;
                }
            }

            else if (gamePieces[loop.getActivePlayer()].getY() == y)
            {
                // check to see if moved exactly 1
                if ((gamePieces[loop.getActivePlayer()].getX() - 1 == x) || (gamePieces[loop.getActivePlayer()].getX() + 1 == x))
                {
                    // This will save the piece location so I can send a "delete" message to the client.
                    //gamePieces[loop.getActivePlayer()].setX(gamePieces[loop.getActivePlayer()].getX());   
                    gamePieces[loop.getActivePlayer()].setX(x);
                    
                    if (checkWinCondition(
                        gamePieces[loop.getActivePlayer()].getX(),
                        gamePieces[loop.getActivePlayer()].getY()))
                    {
                        // Do something!
                        // Console.Write("Code this in, Jason 2!");
                    }

                    return true;
                }
            }

            return false;
        }

        // Override as necessary
        // This method can be used to track a winner by simply setting gameState to false
        public virtual string generateMoveString(int turnNumber, int playerNumber, string cur_x, string cur_y, string m)
        {
            string moveStatement = "";

            // turn number, new player number
            moveStatement += turnNumber.ToString() + "&";
            moveStatement += playerNumber + "&";
            
            // score for Game_Generic is always -1
            moveStatement += "-1&";
            
            // message is "THISISAMESSAGE" if game is not over
            // if the game is still running, indicating that no players made a "winning" move
            if (!gameState)     //gameState is true while game is running 
                moveStatement += "WINNER&";
            else
                moveStatement += "&";

            // position starting x and y    
            moveStatement += cur_x + "%";
            moveStatement += cur_y + "%";

            Console.Write("Critical point here:" + moveStatement);

            // player who went last
            moveStatement += "1#";

            // coordinates that have changed, use a "#" sign.
            moveStatement += ( ( (Piece_Movable) gamePieces[loop.getActivePlayer()]).getXPrev() ).ToString() + "%"
                           + ( ( (Piece_Movable) gamePieces[loop.getActivePlayer()]).getYPrev() ).ToString() + "%"
                            + "0" + "*";


            Console.Write("Sending message to all clients: " + moveStatement);

            return moveStatement;
        }
    }
}
