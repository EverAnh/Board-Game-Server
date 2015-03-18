using System;
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
        }
        public override bool handlePlayerTurn(String s)
        {

            Console.Write("Recieved: " + s + " from client. Handling message...");


            String[] move = s.Split('%');

            MakeMove(System.Convert.ToInt32(move[0]), System.Convert.ToInt32(move[1]));

            return true;
        }
        public bool StartTurn()
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

        }

        private void MakeMove(int row, int col)
        {

            // Bump the move number.
            this.moveNumber++;

            // Make the move on the board.
            this.board.MakeMove(this.currentColor, row, col);
        }

        //
        // Called when a move has been completed (including any animation) to
        // start the next turn.
        //
        private void EndMove()
        {
            // Set the game state.
            this.state = Game_Othello.GameState.MoveCompleted;

            // Switch players and start the next turn.
            this.currentColor *= -1;

            //needs a real input for the parameter
            this.StartTurn();
        }

    }



}
