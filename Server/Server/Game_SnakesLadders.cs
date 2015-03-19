using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// TODO: Change the game pieces to movable pieces.

namespace Game
{
    public class Game_SnakesLadders : Game_Generic
    {
		private int cols = 10;
		private int rows = 10;

//        protected Board_Piece[] gameBoardSL;            // different from gameBoard, note.
        
        public Game_SnakesLadders()
        {
			gameBoard = new Board_Piece[cols, rows];         // set the rows and columns into the gameboard we made earlier. 
//            gameBoardSL = new Board_Piece[100];          // size 100 array of boardPieces
            // specific to S&L's logic
            Board_Piece[,] gameBoardSLlogic = new Board_Piece[cols, rows];
            
            gameType = "snakesLadders";                 // Server will know that we made a connectFour game
            gamePieces = new List<Piece_Generic>();     // May not need this for our implementation
            currentPlayers = new List<Player>();        // list of players
            loop = new Server_GameLoop();               // Make a new server_gameloop (unecessary?)
            maxPlayers = 2;                             // 2 players max for now.
            gameState = true;

            // Add two player's info in here, we don't really need to track all the pieces as gameBoard handles a lot of that for us.
            // We'll use gamePieces to track the last known move of each respective player.
            
            // Player 1
            gamePieces.Add(new Piece_Generic());
            gamePieces[0].setX(0);
            gamePieces[0].setY(9);
            gamePieces[0].setValue(1);    

            // Player 2
            gamePieces.Add(new Piece_Generic());
            gamePieces[1].setX(0);
            gamePieces[1].setY(9);
            gamePieces[1].setValue(2);                  

            initializeBoardState();                     // Initialize the board.
        }

        public override List<Piece_Generic> getPieces()
        {
            return gamePieces;                          
        }

        // Since we're using our own game board, overwrite this method from Generic.

//        public override int getPiece(int getX, int getY)
//        {
//            return gameBoardSL[getX, getY];
//        }






		public virtual bool handlePlayerTurn(String s)
		{
            // we don't care client's move, we made our move by server rolling the dice
			Console.Write("Recieved: " + s + " from client. Handling message...");
			if (s != null && s != " " && s!= "" ) // if it not a null string or empty string or empty string with one little character AHH
			{
				String[] move = s.Split('%'); 
				return checkGameState( System.Convert.ToInt32(move[0]), System.Convert.ToInt32(move[1]) );
			}

			Console.Write("String was null, can't split that bro.");
			return true;        // chill, man!

		}

		public override void endGame()
		{
			Console.Write("[Snakes And Ladders] Terminating game...thanks for playing! Come back soon");
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
			return false;               // not a win
		}

		// returns true if the move is a valid move, otherwise returns false
		protected override bool checkGameState(int x, int y)
		{
			

			( (Piece_Movable) gamePieces[loop.getActivePlayer()] ).setXPrev(gamePieces[loop.getActivePlayer()].getX() );
			( (Piece_Movable) gamePieces[loop.getActivePlayer()] ).setYPrev(gamePieces[loop.getActivePlayer()].getY() );


			// server roll the dice
			int diceRoll = returnDiceRoll();
			int currentLocationx = gamePieces[loop.getActivePlayer()].getX();
			int currentLocationy = gamePieces[loop.getActivePlayer()].getY();
			// server move the piece
			movePiece(diceRoll, currentLocationx, currentLocationy);



			if (checkWinCondition(
				gamePieces[loop.getActivePlayer()].getX(),
				gamePieces[loop.getActivePlayer()].getY()))
			{
				// Do something!
				Console.Write("We got a winner!");
			}

			return true;
		}

		// Override as necessary
		// This method can be used to track a winner by simply setting gameState to false
		public override string generateMoveString(int playerNumber, int turnNumber, string cur_x, string cur_y, string m)
		{
			string moveStatement = "";

			// turn number, new player number
			moveStatement += turnNumber.ToString() + "&";
			moveStatement += playerNumber + "&";

			// score for Game_Generic is always -1
			moveStatement += "-1&";

			// message is "THISISAMESSAGE" if game is not over
			// if the game is still running, indicating that no players made a "winning" move
			if (!gameState)     //gameState is true while game is running 
				moveStatement += "WINNER&";
			else
				moveStatement += "THISISAMESSAGE&";

			// position starting x and y    
			moveStatement += cur_x + "%";
			moveStatement += cur_y + "%";

			Console.Write("Critical point here:" + moveStatement);

			// player who went last
			moveStatement += "1#";

			// coordinates that have changed, use a "#" sign.
			moveStatement += ( ( (Piece_Movable) gamePieces[loop.getActivePlayer()]).getXPrev() ).ToString() + "%"
				+ ( ( (Piece_Movable) gamePieces[loop.getActivePlayer()]).getYPrev() ).ToString() + "%"
				+ "0";


			Console.Write("Sending message to all clients: " + moveStatement);

			return moveStatement;
		}



        // Methods that our main game loop calls on

        // Gameloop is as follows:
        
//            Player rolls die.                       returnDiceRoll
//            Player gets current location to end.    calcDistToEnd()
//            Player moves to location, if able       movePiece()
//            Board piece at location is activated    boardAction(index)
//            Action is done (nothing, ladder, snake) movePiece/extraTurn...
//            -- If extra turn, start over from roll
//            Otherwise, pass to next player


        private int returnDiceRoll()
        {
            // Roll the die, returning a random number from 1-6.
            // If we roll 6, make sure to let user know they get another one!
            Random rnd = new Random();          // Find a random number
            int dice = rnd.Next();
            dice = (dice % 6) + 1;
            return dice;
        }
//
//        private int calcDistToEnd(int l)
//        {
//            // takes in the player current location and returns the distance to the end
//            return 99-l; // or 99-l, could lead to indexing errors. (Jason C.)
//        }

		private void boardAction(int x, int y)
        {
            // takes in player current location (via index) and checks the board piece at his spot
            // if 0, nothing happens
            // if 1, player is bumped up
            // if 2, player is bumped down

			int newLocationx = x;
			int newLocationy = y;

            if (gameBoardSLlogic[y, x].getType() == 0)
            {
				gamePieces[loop.getActivePlayer()].setX(newLocationx);
				gamePieces[loop.getActivePlayer()].setY(newLocationy);
            }
            else 
            {
				gamePieces[loop.getActivePlayer()].setX(gameBoard[x, y].getValuex());
				gamePieces[loop.getActivePlayer()].setY(gameBoard[x, y].getValuey());
            }
           

            
        }

		private bool movePiece(int diceRoll, int currentLocationx, int currentLocationy)
        {
			int newLocationx = 0;
			int newLocationy = 0;
            // moves a piece
			if (currentLocationy % 2 == 0) {
				if (currentLocationx - diceRoll < 0) {
                    newLocationy = currentLocationy - 1;
					newLocationx = (Math.Abs (currentLocationx - diceRoll) - 1);
				} else {
					newLocationy = currentLocationy;
					newLocationx = currentLocationx - diceRoll;
				}
			} else {
				if (currentLocationx + diceRoll >= cols) {
                    newLocationy = currentLocationy - 1;
					newLocationx = cols - (Math.Abs (currentLocationx + diceRoll) % cols - 1);
				} else {
					newLocationy = currentLocationy;
					newLocationx = currentLocationx + diceRoll;
				}
			}

			boardAction(newLocationx, newLocationy);
			return true;
        }


        private void initializeBoardState()
        {
            // Set up the board with appropriate values.
			for (int r = 0; r < rows; r++) {
				for (int c = 0; c < cols; c++) {
             //       gameBoardSLlogic[r, c].setIndex(v);    // set the index of each piece
                    gameBoardSLlogic[c, r].setValuex(r);    // We'll set this seperately
                    gameBoardSLlogic[c, r].setValuey(c);    // We'll set this seperately
                    gameBoardSLlogic[c, r].setType(0);     // Same as above
				}
			}

            
            // format : [y_coordinate, x_coordinate]
            // [0,0] is top left

            // ladders
            gameBoardSLlogic[1, 6].setType(1);
            gameBoardSLlogic[1, 6].setValuex(3);
            gameBoardSLlogic[1, 6].setValuey(0);


            gameBoardSLlogic[4, 0].setType(1);
            gameBoardSLlogic[4, 0].setValuex(1);
            gameBoardSLlogic[4, 0].setValuey(2);



            gameBoardSLlogic[8, 2].setType(1);
            gameBoardSLlogic[8, 2].setValuex(0);
            gameBoardSLlogic[8, 2].setValuey(6);



            gameBoardSLlogic[7, 4].setType(1);
            gameBoardSLlogic[7, 4].setValuex(6);
            gameBoardSLlogic[7, 4].setValuey(3);



            gameBoardSLlogic[8, 9].setType(1);
            gameBoardSLlogic[8, 9].setValuex(7);
            gameBoardSLlogic[8, 9].setValuey(6);


            // snakes
            gameBoardSLlogic[5, 2].setType(1);
            gameBoardSLlogic[5, 2].setValuex(1);
            gameBoardSLlogic[5, 2].setValuey(7);


            gameBoardSLlogic[1, 2].setType(1);
            gameBoardSLlogic[1, 2].setValuex(4);
            gameBoardSLlogic[1, 2].setValuey(3);



            gameBoardSLlogic[3, 7].setType(1);
            gameBoardSLlogic[3, 7].setValuex(4);
            gameBoardSLlogic[3, 7].setValuey(5);



            gameBoardSLlogic[0, 6].setType(1);
            gameBoardSLlogic[0, 6].setValuex(6);
            gameBoardSLlogic[0, 6].setValuey(2);



            gameBoard[6, 5].setType(1);
            gameBoard[6, 5].setValuex(7);
            gameBoard[6, 5].setValuey(8);
//            // Set snake type/positions here
//
//            gameBoardSL[62].setValue = 19;
//            gameBoardSL[87].setValue = 24;
//            gameBoardSL[49].setValue = 11;
//            gameBoardSL[93].setValue = 73;
//            gameBoardSL[65].setValue = 53;
//            gameBoardSL[64].setValue = 41;
//
//            // Set ladder locations here
//
//            gameBoardSL[2].setType = 1;
//            gameBoardSL[4].setType = 1;
//            gameBoardSL[28].setType = 1;
//            gameBoardSL[21].setType = 1;
//            gameBoardSL[9].setType = 1;
//            gameBoardSL[71].setType = 1;
//
//            gameBoardSL[2].setValue = 38;
//            gameBoardSL[4].setValue = 14;
//            gameBoardSL[28].setValue = 84;
//            gameBoardSL[21].setValue = 42;
//            gameBoardSL[9].setValue = 31;
//            gameBoardSL[71].setValue = 91;
        }
    }
}
