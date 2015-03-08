using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game_ConnectFour : Game_Generic
    {

        // Declare a grid of 7 columns and 6 rows.

        private int cols = 7;
        private int rows = 6;
        
        public Game_ConnectFour()
        {
            gameBoard = new int[cols, rows];
            numberPlayers = 2;
        }



    }
}
