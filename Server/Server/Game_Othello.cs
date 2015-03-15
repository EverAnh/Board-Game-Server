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
        private Board board;
        private GameState gameState;
        private int currentColor;
        private int moveNumber;
        private int lastMoveColor;

        //available gamestates
        private enum GameState
        {
            GameOver,			// The game is over (also used for the initial state).
            InMoveAnimation,	// A move has been made and the animation is active.
            InPlayerMove,		// Waiting for the user to make a move.
            InComputerMove,		// Waiting for the computer to make a move.
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
                this.gameState = Game_Othello.GameState.InPlayerMove;
            }

            return true;
        }
        protected override bool checkGameState(int x, int y)
        {
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
            this.gameState = Game_Othello.GameState.GameOver;

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

            // Make a copy of the board (for doing move animation).
            Board oldBoard = new Board(this.board);

            // Make the move on the board.
            this.board.MakeMove(this.currentColor, row, col);

            // Save the player color.
            this.lastMoveColor = this.currentColor;
        }

        //
        // Called when a move has been completed (including any animation) to
        // start the next turn.
        //
        private void EndMove()
        {
            // Set the game state.
            this.gameState = Game_Othello.GameState.MoveCompleted;

            // Switch players and start the next turn.
            this.currentColor *= -1;

            //needs a real input for the parameter
            this.handlePlayerTurn("x,y");
        }

    }



}
