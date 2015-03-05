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

        int numberPlayers;

        GenericGame(int x_size, int y_size)
        {
            gameBoard = new int[x_size, y_size];
        }

        public int getPiece(int getX, int getY)
        {
            return gameBoard[getX, getY];
        }

        public void assignPiece(int getX, int getY, int value)
        {
            gameBoard[getX, getY] = value;
        }

        public void setNumberPlayers(int p)
        {
            numberPlayers = p;
        }
    }
}
