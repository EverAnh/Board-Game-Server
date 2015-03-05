using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class GenericGame
    {
        private int[ , ] gameBoard;

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
    }
}
