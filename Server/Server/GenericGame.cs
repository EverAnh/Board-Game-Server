using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class GenericGame
    {
        private int[ , ] gameBoard;
        private List<GenericPiece> gamePieces;
        private int numberPlayers;

        public GenericGame(int x_size, int y_size)
        {
            gameBoard = new int[x_size, y_size];
            gamePieces = new List<GenericPiece>();
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
