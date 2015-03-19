using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Board_Piece
        {
            int index;    // index value of this board Piece
            int type;     // 0 for norm, does nothing || 1 for ladder || 2 for snake
            int valx;      // Value that the piece might lead [if 1 or 2]
			int valy; 
			String text = "";  // If you want to display text when you land on the piece

            public Board_Piece()
            {   }

            // Getter/setter methods

            public String getText()
            {   return text;    }

            public void setText(String set)
            {   text = set;     }
            
            public int getIndex()
            {   return index;   }

            public void setIndex(int i)
            {   index = i;      }

            public int getType()
            {   return type;    }

            public void setType(int t)
            {   type = t;       }

            public int getValuex()
            {   return valx;   }

            public void setValuex(int v)
            {   valx = v;      }

			public int getValuey()
			{   return valy;   }

			public void setValuey(int v)
			{   valy = v;      }
        }
}
