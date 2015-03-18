using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game_Othello : Game_Generic
    {
        //game parameters
        private int cols = 8;
        private int rows = 8;
        private Game_Othello_Board board;
        private GameState state;
        private int currentColor;
        private int moveNumber;

        //available gamestates
        private enum GameState
        {
            GameOver,			// The game is over (also used for the initial state).
            InMoveAnimation,	// A move has been made and the animation is active. NEVER USED
            InPlayerMove,		// Waiting for the user to make a move.
            InComputerMove,		// Waiting for the computer to make a move. NEVER USED
            MoveCompleted		// A move has been completed (including the animation, if active).
        }

        public Game_Othello()
        {
            gameBoard = new int[cols, rows];
            gameType = "othello";
            maxPlayers = 2;
            currentColor = Game_Othello_Board.White; //STARTS with white
            board = new Game_Othello_Board();
            state = GameState.InPlayerMove;
        }
        public override bool handlePlayerTurn(String s)
        {

            Console.Write("Recieved: " + s + " from client. Handling message...");

            String[] move = s.Split('%');
            int x = System.Convert.ToInt32(move[0]);
            int y = System.Convert.ToInt32(move[1]);
            bool tryMove = board.IsValidMove(this.currentColor, x, y);

            if (tryMove)
            {
                EndMove(MakeMove(x, y));
            }

            return tryMove;
        }
        public bool StartNextTurn(List<ArrayList> flipped)
        {
            // If the current player cannot make a valid move, forfeit the turn.
            if (!this.board.HasAnyValidMove(this.currentColor))
            {
                // Switch back to the other player.
                this.currentColor *= -1;

                // If the original player cannot make a valid move either, the game is over.
                if (!this.board.HasAnyValidMove(this.currentColor))
                {
                    this.EndGame();
                    return true;
                }
            }
            // Otherwise, set up for a user move.
            else
            {
                // Set the game state.
                this.state = Game_Othello.GameState.InPlayerMove;
                if (currentColor == 1)
                    generateMoveString(1, moveNumber, flipped);
                else
                    generateMoveString(2, moveNumber, flipped);
            }
            return true;
        }

        private void EndGame()
        {
            this.EndGame(false);
        }

        //
        // Ends the current game, optionally by player resignation.
        //
        private void EndGame(bool isResignation)
        {
            // Set the game state.
            this.state = Game_Othello.GameState.GameOver;

            // Handle a resignation.
            if (isResignation)
            {
                // Determine which player is resigning. In a computer vs.
                // user game, the computer will never resign so it must be
                // the user. In a user vs. user game we'll assume it is
                // the current player.
                int resigningColor = this.currentColor;

            }
            if (board.WhiteCount > board.BlackCount)
                generateMoveString(1, moveNumber, false);
            else
                generateMoveString(2, moveNumber, false);
        }

        private List<ArrayList> MakeMove(int row, int col)
        {

            // Bump the move number.
            this.moveNumber++;

            // Make the move on the board.
            return this.board.MakeMove(this.currentColor, row, col);
        }

        //
        // Called when a move has been completed (including any animation) to
        // start the next turn.
        //
        private void EndMove(List<ArrayList> flipped)
        {
            // Set the game state.
            this.state = Game_Othello.GameState.MoveCompleted;

            // Switch players and start the next turn.
            this.currentColor *= -1;

            //needs a real input for the parameter
            this.StartNextTurn(flipped);

        }
        public string generateMoveString(int playerNumber, int turnNumber, bool moreMoves)
        {
            string moveStatement = "";

            // turn number, new player number
            moveStatement += turnNumber.ToString() + "&"; //next turn
            moveStatement += playerNumber + "&"; //winning player

            // score for Game_Generic is always -1
            moveStatement += "-1&"; //score
            moveStatement += "WINNER&";
            Console.Write("Sending message to all clients: " + moveStatement);
            return moveStatement;
        }
        public string generateMoveString(int playerNumber, int turnNumber, List<ArrayList> flipped)
        {
            string moveStatement = "";

            // turn number, new player number
            moveStatement += turnNumber.ToString() + "&"; //next turn
            moveStatement += playerNumber + "&"; //next player

            // score for Game_Generic is always -1
            moveStatement += "-1&"; //score

            // message is "THISISAMESSAGE" if game is not over
            moveStatement += "THISISAMESSAGE&";

            // position starting x and y    

            for (int i = 0; i < flipped[0].Count; i++)
            {
                moveStatement += flipped[0][i] + "%";
                moveStatement += flipped[1][i] + "%";
                moveStatement += playerNumber + "%";
            }


            Console.Write("Sending message to all clients: " + moveStatement);

            return moveStatement;
        }


    }



}
