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
        private bool gameState = true; // true while the game is active? may be unused
        
        public Game_ConnectFour()
        {
            gameBoard = new int[cols, rows];
            gameType = "connectFour";
            maxPlayers = 1;
        }

        public override int assignPiece(int getX, int getY, int value) // We don't care about getY, so override it.
        {
            // This function will instead drop it in the appropriate row; we don't care about the getY.
            // GetX is all that matters here. 

            int newGetY = returnTopOfRow(getX); // calls helper method to return top of the current row

            if (newGetY != 100)
            {
                gameBoard[getX, newGetY] = value; // assigns the element to the top.
                return newGetY;
            }
            else
            {
                Console.Write("Invalid move!");// invalid move 
                return -1;          // return an error value
            }
                
                
           
        }

        private int returnTopOfRow(int getX) // a helper method to get the "top" of each row.
        {
            for (int i = rows; i >= 0; i--) // searches from the bottom, going up
            {
                if (gameBoard[getX, i] == 0) // accesses the element. if it is 1 or 2, we keep looping.
                    return i;
            }
            return 100;                     // if we get to the end and it does not exist, return an "error code"
        }


        public override bool handlePlayerTurn(String s) // each player will call this function with their input.
        {
            int placeX = 0; 
            int placeY = 0; //will not need Y input.
            int color = 0;  // color: [1] for black, [2] for red.
         
            parsePlayerInput(s);   // store what you parse into an array?
            placeY = assignPiece(placeX, placeY, color);        // assignPiece will "attempt" to place the piece there. it will then return the placeY value.

            // get input on where the piece has been placed and save it to gameBoard, check that the spot is already occupied.
            if (placeY != -1 && checkOccupiedState(placeX, placeY)) // if the spot is not yet occupied, and there is no error code.
            {
                assignPiece(placeX, placeY, color); // place the piece in the spot. we'd like posX, posY, color [1] for black, [2] for red. [0] indicates not occupied

                if (checkGameState(placeX, placeY)) // using the piece, check if the state is a win condition. if the placement happens to be a win condition
                {
                    gameState = false;              // game is over!
                    Console.Write("Game over!");    // print to console log 
                }

                // Code here to echo back to server the string that we just got.
                return true;            
            }

            // the move was not valid
            return true;
        }

        // Game Logic

        private bool checkGameState(int x, int y) // x is the column, y is the row.
        {
           // this function will take a game piece placement, and check all rows/columns next to it for a win condition.
            // note: if the game has not progressed past 4 turns, no need to even check.


            if (!checkLeftRight(x, y) || !checkUpDown(x,y) || !checkDiagonalLeftUpRightDown(x,y) || !checkDiagonalRightUpLeftDown(x,y)) // if any of the checks return false, this statement will evaluate to true.
            {
                return false;       // return immediately; the game is over
            }
            
            return true;            // game continues because none of the checks were valid.
        }


        private bool checkLeftRight(int x, int y) // x is column!
        {
            // depends on column number (x)
            // 0 and col only need to check one possibility. 

            Console.Write("Checking Left and Right.");

            int typeToSearch = getPiece(x, y); // to be used later

            // my plan is to generate an array "snapshot" of the values next to the piece just implemented to check for the win.
            // in this function, i'd generate an array of length [7]

            int[] snapShot = new int[7];                        //  temporary integer array that serves as a "snapshot" of what we are about tpo search

            for (int i = 0; i < 7; i++)                        // will fill the array with the array index values I SHOULD be searching for. 
            {
                snapShot[i] = x + 3 - i;                        // example if x = 0; [3,2,1,0,-1.-2,-3] if x = 4; [7,6,5,4,3,2,1] ; if x = 3 [6,5,4,3,2,1,0]     
                // Snapshot is now filled with array indexes

                if (snapShot[i] < 0 || snapShot[i] > cols - 1)  // replace the out of bounds array index with -1, which will allow our checking algorithm to ignore the index.
                {
                    snapShot[i] = -1;                           // set it directly to -1
                }

            }

            // Now that the array is filled either with proper array indices or -1, we can now check "safely". [3,2,1,0,-1.-2,-3] ~ [3,2,1,0,-1.-1,-1] || 

            for (int i = 0; i < 4; i++)                            // we have a total of 4 arrays to search, max!
            {
                if (snapShot[i] != -1 && snapShot[i + 3] != -1)      // if the start or end indixes of the snapshot are -1, do not search it.
                {
                    int totalToExpect = 4 * typeToSearch;           // generate a total to check against. Either 4, or 8.

                    int totalAmt = getPiece(snapShot[i], y)
                        + getPiece(snapShot[i + 1], y)
                        + getPiece(snapShot[i + 2], y)
                        + getPiece(snapShot[i + 3], y);

                    Console.WriteLine("Comparing: " + totalToExpect + "to " + totalAmt);
                    Console.WriteLine("" + getPiece(snapShot[i], y)
                        + getPiece(snapShot[i + 1], y)
                        + getPiece(snapShot[i + 2], y)
                        + getPiece(snapShot[i + 3], y));

                    if (totalToExpect == totalAmt)                  // if they are equal... THE GAME IS OVER.
                    {
                        Console.WriteLine("Solution found. Game over.");
                        return false;                               // game end

                    }
                }


            }
            Console.WriteLine("No solutions found. Running next check.");
            return true;                                        // game continue
        }

        private bool checkUpDown(int x, int y)
        {
            // depends on column number (x)
            // 0 and col only need to check one possibility. 

            Console.Write("Checking Up and Down.");

            int typeToSearch = getPiece(x, y); // to be used later

            // my plan is to generate an array "snapshot" of the values next to the piece just implemented to check for the win.
            // in this function, i'd generate an array of length [7]

            int[] snapShot = new int[7];                        //  temporary integer array that serves as a "snapshot" of what we are about tpo search

            for (int i = 0; i < 7; i++)                        // will fill the array with the array index values I SHOULD be searching for. 
            {
                snapShot[i] = y + 3 - i;                       // Snapshot is now filled with array indexes

                if (snapShot[i] < 0 || snapShot[i] > rows - 1)  // replace the out of bounds array index with -1, which will allow our checking algorithm to ignore the index.
                {
                    snapShot[i] = -1;                           // set it directly to -1
                }

            }

            for (int i = 0; i < 4; i++)                            // we have a total of 4 arrays to search, max!
            {
                if (snapShot[i] != -1 && snapShot[i + 3] != -1)      // if the start or end indixes of the snapshot are -1, do not search it.
                {
                    int totalToExpect = 4 * typeToSearch;           // generate a total to check against. Either 4, or 8.

                    int totalAmt = getPiece(x, snapShot[i])
                        + getPiece(x, snapShot[i + 1])
                        + getPiece(x, snapShot[i + 2])
                        + getPiece(x, snapShot[i + 3]);

                    Console.WriteLine("Comparing: " + totalToExpect + "to " + totalAmt);
                    Console.WriteLine("" + getPiece(x, snapShot[i])
                        + getPiece(x, snapShot[i + 1])
                        + getPiece(x, snapShot[i + 2])
                        + getPiece(x, snapShot[i + 3]));

                    if (totalToExpect == totalAmt)                  // if they are equal... THE GAME IS OVER.
                    {
                        Console.WriteLine("Solution found. Game over.");
                        return false;                               // game end

                    }
                }


            }
            Console.WriteLine("No solutions found. Running next check.");
            return true;                                        // game continue

        }

        private bool checkDiagonalLeftUpRightDown(int x, int y)
        {
            Console.Write("Checking Left Up and Right Down.");

            int typeToSearch = getPiece(x, y); // to be used later
            int[] snapShot = new int[7];                        //  temporary integer array that serves as a "snapshot" of what we are about tpo search

            for (int i = 0; i < 7; i++)                        // will fill the array with the array index values I SHOULD be searching for. 
            {
                snapShot[i] = y + 3 - i;                       // Snapshot is now filled with array indexes

                if (snapShot[i] < 0 || snapShot[i] > rows - 1)  // replace the out of bounds array index with -1, which will allow our checking algorithm to ignore the index.
                {
                    snapShot[i] = -1;                           // set it directly to -1
                }

            }

            int xChange = x; // save X in here.

            for (int i = 0; i < 4; i++)                            // we have a total of 4 arrays to search, max!
            {

                Console.WriteLine("xChange value:" + xChange);
                if (snapShot[i] != -1 && snapShot[i + 3] != -1 && xChange > 2 && xChange < cols)      // if the start or end indixes of the snapshot are -1, do not search it.
                {
                    int totalToExpect = 4 * typeToSearch;           // generate a total to check against. Either 4, or 8.

                    int totalAmt =
                         getPiece(xChange, snapShot[i])
                        + getPiece(xChange - 1, snapShot[i + 1])
                        + getPiece(xChange - 2, snapShot[i + 2])
                        + getPiece(xChange - 3, snapShot[i + 3]);

                    Console.WriteLine("Comparing: " + totalToExpect + "to " + totalAmt);
                    Console.WriteLine("" + getPiece(xChange, snapShot[i])
                        + getPiece(xChange - 1, snapShot[i + 1])
                        + getPiece(xChange - 2, snapShot[i + 2])
                        + getPiece(xChange - 3, snapShot[i + 3]));



                    if (totalToExpect == totalAmt)                  // if they are equal... THE GAME IS OVER.
                    {
                        Console.WriteLine("Solution found. Game over.");
                        return false;                               // game end

                    }
                }

                xChange++; // increment xChange to allow us to search.

            }
            Console.WriteLine("No solutions found. Running next check.");
            return true;
        }

        private bool checkDiagonalRightUpLeftDown(int x, int y)
        {
            Console.Write("Checking Left Up and Right Down.");

            int typeToSearch = getPiece(x, y); // to be used later
            int[] snapShot = new int[7];                        //  temporary integer array that serves as a "snapshot" of what we are about tpo search

            for (int i = 0; i < 7; i++)                        // will fill the array with the array index values I SHOULD be searching for. 
            {
                snapShot[i] = y + 3 - i;                       // Snapshot is now filled with array indexes

                if (snapShot[i] < 0 || snapShot[i] > rows - 1)  // replace the out of bounds array index with -1, which will allow our checking algorithm to ignore the index.
                {
                    snapShot[i] = -1;                           // set it directly to -1
                }

            }

            int xChange = x; // save X in here.

            for (int i = 0; i < 4; i++)                            // we have a total of 4 arrays to search, max!
            {

                Console.WriteLine("xChange value:" + xChange);
                if (snapShot[i] != -1 && snapShot[i + 3] != -1 && xChange > 2 && xChange < cols)      // if the start or end indixes of the snapshot are -1, do not search it.
                {
                    int totalToExpect = 4 * typeToSearch;           // generate a total to check against. Either 4, or 8.

                    int totalAmt =
                         getPiece(xChange - 3, snapShot[i])
                        + getPiece(xChange - 2, snapShot[i + 1])
                        + getPiece(xChange - 1, snapShot[i + 2])
                        + getPiece(xChange, snapShot[i + 3]);

                    Console.WriteLine("Comparing: " + totalToExpect + "to " + totalAmt);
                    Console.WriteLine("" + getPiece(xChange - 3, snapShot[i])
                        + getPiece(xChange - 2, snapShot[i + 1])
                        + getPiece(xChange - 1, snapShot[i + 2])
                        + getPiece(xChange, snapShot[i + 3]));


                    if (totalToExpect == totalAmt)                  // if they are equal... THE GAME IS OVER.
                    {
                        Console.WriteLine("Solution found. Game over.");
                        return false;                               // game end

                    }
                }

                xChange++; // increment xChange to allow us to search.

            }
            Console.WriteLine("No solutions found. Running next check.");
            return true;
        }


        private bool parsePlayerInput(String s) // boolean for now 
        {
            // Parses player input and store into local vars

            // recieve inputString here:

            return true;

        }

        private bool checkOccupiedState(int x, int y)
        {
            // will take in the recently added piece.
            // will return true if you are allowed to place a piece (unoccupied)
            // otherwise, will return false if it is actually occupied.

            if (0 == getPiece(x,y))
            {
                return true;
            }
            else
                return false;
        }
    }
}


