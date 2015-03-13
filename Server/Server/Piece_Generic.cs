using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Piece_Generic
    {
        protected int x_pos;
        protected int y_pos;
        protected int value;

        public Piece_Generic()
        {

        }

        public void setPosition(int newX, int newY)
        {
            setX(newX);
            setY(newY);
        }

        public int getX()
        {
            return x_pos;
        }

        public void setX(int newX)
        {
            x_pos = newX;
        }

        public int getY()
        {
            return y_pos;
        }

        public void setY(int newY)
        {
            y_pos = newY;
        }

        public int getValue()
        {
            return value;
        }

        public void setValue(int v)
        {
            value = v;
        }
    }
}
