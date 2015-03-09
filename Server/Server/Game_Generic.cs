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

        

        public Game_Generic()
        {
            // Empty constructor for subclasses
        }

        public Game_Generic(int x_size, int y_size)
        {
            gameBoard = new int[x_size, y_size];
            gamePieces = new List<Piece_Generic>();
        }

        public List<Player> getPlayers()
        {
            return currentPlayers;
        }

        public int getNumberPlayers()
        {
            return numberPlayers;
        }

        public int getMaxPlayers()
        {
            return maxPlayers;
        }

        public String getGameType()
        {
            return gameType;
        }

        public int getPiece(int getX, int getY)
        {
            return gameBoard[getX, getY];
        }

        public void assignPiece(int getX, int getY, int value)
        {
            gameBoard[getX, getY] = value;
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
            return true;
        }
    }
}
