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
            gameBoard = new int[cols, rows];            // set the rows and columns into the gameboard we made earlier. 
            gameType = "connectFour";                   // Server will know that we made a connectFour game
            gamePieces = new List<Piece_Generic>();     // May not need this for our implementation
            currentPlayers = new List<Player>();        // list of players
            loop = new Server_GameLoop();               // Make a new server_gameloop (unecessary?)
            maxPlayers = 2;                             // 2 players max
            
            // Add two player's info in here, we don't really need to track all the pieces as gameBoard handles a lot of that for us.
            // We'll use gamePieces to track the last known move of each respective player.
            
            gamePieces.Add(new Piece_Generic());
            gamePieces[0].setX(0);
            gamePieces[0].setY(0);
            gamePieces[0].setValue(1);                  // Player 1 value: Black
            
            gamePieces.Add(new Piece_Generic());
            gamePieces[1].setX(0);
            gamePieces[1].setY(0);
            gamePieces[1].setValue(2);                  // Player 2 value: Red
        }

        // getPieces might need to be redefined here, for each specific player..?

        public override List<Piece_Generic> getPieces()
        {
            return gamePieces;                          
        }

        public override void endGame()
        {
            Console.Write("[Not yet implemented] Terminating game of connectFour!...thanks for playing!");
        }

        protected override bool checkWinCondition(int x, int y)
        {
            if (gamePieces[loop.getActivePlayer()].getX() == 0 && gamePieces[loop.getActivePlayer()].getY() == 0)
            {
                Console.Write("Player " + loop.getActivePlayer() + " has won!"); // print
                gameState = false;      // set to false
                endGame();              // attempt to end the game.
                return true;            // quick termination, return back to the caller
            }
            return false; // not a win
        }

        // override the statement from Generic Game
        public override string generateMoveString(int playerNumber, int turnNumber, string cur_x, string cur_y, string m)
        {
            string moveStatement = "";                                          // Make a string that we are going to send.
            /*             
             7& *
             2& *
             3$3& *
             someStuff&
             5%2%1%(1 for red, 2 for black)
            ^ col, row, value
             */

            // turn number, new player number
            // I prefer to use the values kept by this particular game instance rather than gameloop.

            moveStatement += loop.getTurnNumber() + "&";              // attach the current turn number
            moveStatement += loop.getActivePlayer() + "&";      // attach the active player number

            // score for this game is always -1, I'll jump ahead and state that we'll only ever have 2 players 
            moveStatement += "-1&";                          // append score (which is empty, sadly)

            // if the game is still running, indicating that no players made a "winning" move
            if (!gameState)     //gameState is true while game is running 
                moveStatement += "WINNER&";
            // OTHERWISE, message is blank 
            else
                moveStatement += "whatIsaMessage&";
            
            // position starting x and y
            moveStatement += cur_x + "%";                       // append the moveX
            
            // Note that the client didn't know where the Y value had to be. 
            // We calculated this server side.
            moveStatement += 
            gamePieces[loop.getActivePlayer()].getY() + "%";    // put our updated Y value here.

            // player who went last
            // Note: if the game is over, we need to append the winning player.
            if (!gameState) 
                moveStatement += loop.getActivePlayer();    // the player who just moved is obviously the winner!
            else // should give me either 0 or 1. beautiful.
                moveStatement += loop.getNextPlayerIndex(loop.getActivePlayer(), maxPlayers);

            moveStatement += "%";                           // place the delimiter.

            return moveStatement;                           //return the statement to the calling gameLoop 
        }

        // handlePlayerTurn METHOD THAT OVERRIDES THE ONE IN GENERIC GAME
        // Returns true always.
        // TODO: How do we end the game? Do we change the send packet so that the second character after the delim is the player's assigned number?

        public override bool handlePlayerTurn(String s) // each player will call this function with their input.
        {
            
            Console.Write("Recieved: " + s + " from client. Handling message...");

            String[] move = s.Split('%');                               // Split the string by the delimiter.

            int placeX = System.Convert.ToInt32(move[0]);               // Convert Strings to int for the placeX
            int placeY = System.Convert.ToInt32(move[1]);               // Technically we don't need this... TODO: is there something we can do?

            int thePlayer = loop.getActivePlayer();                     // should return either 0 or 1 provided server checked to make sure there are 2 players.
            int color = gamePieces[thePlayer].getValue();               // color: [1] for black, [2] for red. 

            // assignPiece to the place; if it is valid, the method will go ahead and place it.
            // On a not successful attempt, placeY will instead be null.
            
            placeY = assignPiece(placeX, placeY, color);                // assignPiece will "attempt" to place the piece there. it will then return the placeY value.

            // get input on where the piece has been placed and save it to gameBoard, check that the spot is already occupied.
            
            if (placeY == -1)
            {
                // invalid move, return early.
                return false;
            }

            else if (placeY != -1)                                           // if placeY is not the error code
            {
                // Set the "piece" of the player to be their last move so it is printed out. (Unecessary, but shows us something)
                gamePieces[thePlayer].setX(placeX);
                gamePieces[thePlayer].setY(placeX);              

                if (checkGameState(placeX, placeY))                     // using the recently placed piece, check if the state is a win condition. 
                {
                    Console.Write("[CONNECT 4] Game is over! Player: " + (loop.getActivePlayer()+1) + " is the victor!" );                      
                    // mark gameState as false, which will in turn trigger a specific message that will be sent.
                    gameState = false; 
                }  
            }
            return true; // the move was not valid
        }

        protected override bool checkGameState(int x, int y) // x is the column, y is the row.
        {
            // this function will take a game piece placement, and check all rows/columns next to it for a win condition.
            // If the game has not progressed past 4 turns, no need to even check.
            if (loop.getTurnNumber() < 4)   // there is no way you can end the game in less than 4 turns!
            {
                return false;
            }
            // if any of the checks return true, this statement will evaluate to true.
            if (checkLeftRight(x, y) || 
                checkUpDown(x, y) || 
                checkDiagonalLeftUpRightDown(x, y) || 
                checkDiagonalRightUpLeftDown(x, y)) 
            {
                return true;                                     // return true; the game is over
            }

            return false;                                        // game continues because none of the checks were true.
        }

        // ADDITION METHOD THAT OVERRIDES THE ONE IN GENERIC GAME
        // Returns either -1 (not valid move) or the Y value of the piece that was dropped (vertical value)

        public override int assignPiece(int getX, int getY, int value)              // We don't care about getY
        {
            // This function will drop it in the appropriate position
            int newGetY = returnTopOfRow(getX);                                     // calls helper method to return top of the current row

            if (newGetY != 100)                                                     // if no error code, good to go.
            {
                gameBoard[getX, newGetY] = value;                                   // assigns the element to the top.
                return newGetY;
            }
            else
            {
                Console.Write("Invalid move!");                                     // invalid move 
                return -1;                                                          // return an error value
            }
        }
        
        // HELPER METHOD THAT IS CALLED BY ASSIGNPIECE (assignPiece)
        // Returns either 100 (error code, the column is full) or the Y value of the piece that we intend to put it in. * does not yet assign!

        private int returnTopOfRow(int getX) // a helper method to get the "top" of each row.
        {
            for (int i = rows; i >= 0; i--) // searches from the bottom, going up
            {
                if (gameBoard[getX, i] == 0) // accesses the element. if it is 1 or 2, we keep looping.
                    return i;
            }
            return 100;                     // if we get to the end and it does not exist, return an error code of "100"
        }  

        private bool checkLeftRight(int x, int y)
        {
            Console.Write("Checking Left and Right.");

            int typeToSearch = getPiece(x, y);                      
            int[] snapShot = new int[7];                            //  temporary integer array that serves as a "snapshot" of what we are about tpo search

            for (int i = 0; i < 7; i++)                             // will fill the array with the array index values I SHOULD be searching for. 
            {
                snapShot[i] = x + 3 - i;                            // example if x = 0; [3,2,1,0,-1.-2,-3] if x = 4; [7,6,5,4,3,2,1] ; if x = 3 [6,5,4,3,2,1,0]     
                if (snapShot[i] < 0 || snapShot[i] > cols - 1)      // replace the out of bounds array index with -1, which will allow our checking algorithm to ignore the index.
                {
                    snapShot[i] = -1;                               // set it directly to -1
                }
            }
            // Now that the array is filled either with proper array indices or -1, we can now check "safely". [3,2,1,0,-1.-2,-3] ~ [3,2,1,0,-1.-1,-1] || 
            for (int i = 0; i < 4; i++)                             // we have a total of 4 arrays to search, max!
            {
                if (snapShot[i] != -1 && snapShot[i + 3] != -1)     // if the start or end indixes of the snapshot are -1, do not search it.
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
                        return true;                                // game end

                    }
                }


            }
            Console.WriteLine("No solutions found. Running next check.");
            return false;                                           // game continue
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
                        return true;                               // game end
                    }
                }
            }
            Console.WriteLine("No solutions found. Running next check.");
            return false;                                        // game continue

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
                        return true;                               // game end

                    }
                }

                xChange++; // increment xChange to allow us to search.

            }
            Console.WriteLine("No solutions found. Running next check.");
            return false;
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
                        return true;                               // game end

                    }
                }

                xChange++; // increment xChange to allow us to search.

            }
            Console.WriteLine("No solutions found. Running next check.");
            return false;
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


