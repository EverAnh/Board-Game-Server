using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Piece_Movable : Piece_Generic
    {
        protected int x_prev; // { get; set; }
        protected int y_prev; // { get; set; }

        public int getXPrev()
        {
            return x_prev;
        }

        public void setXPrev(int x)
        {
            x_prev = x;
        }

        public int getYPrev()
        {
            return y_prev;
        }

        public void setYPrev(int y)
        {
            y_prev = y;
        }
    }
}
