using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game_Othello : Game_Generic
    {
        private int cols = 8;
        private int rows = 8;

        public Game_Othello()
        {
            gameBoard = new int[cols, rows];
            gameType = "othello";
            maxPlayers = 2;
        }
    }
}
