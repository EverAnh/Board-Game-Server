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
        protected int numberPlayers;
        protected int maxPlayers;
        protected Server_GameLoop loop;
        protected String gameType;

        public Game_Generic()
        {
            // Empty constructor
        }

        public Game_Generic(int x_size, int y_size)
        {
            gameBoard = new int[x_size, y_size];
            gamePieces = new List<Piece_Generic>();
        }

        public int getPiece(int getX, int getY)
        {
            return gameBoard[getX, getY];
        }

        public void assignPiece(int getX, int getY, int value)
        {
            gameBoard[getX, getY] = value;
        }

        public void addPlayer(int p)
        {
            numberPlayers = p;
        }
    }
}
