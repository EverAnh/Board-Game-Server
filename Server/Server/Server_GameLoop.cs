using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Server_GameLoop
    {
        private int activePlayer = 0;
        private int turn = 1;               // I moved it outside so I could access it.        
        private bool activeGame = true;     // redundant, but will help it work for now.

        public Server_GameLoop()
        { 
            Console.WriteLine("Game loop has been created!");

        }

        public void gameLoop(Game_Generic game)
        {
            // needs number of players
            // needs the Player object for each player in the game
            //      which includes stream reader/writer
            // 
            int numberPlayers = game.getPlayers().Count;
            // turn = 0;       
            
            Console.WriteLine("Starting game loop!");

            // should be message 5
            sendToAllPlayers(game.getPlayers(), numberPlayers, turn.ToString() + "&" + activePlayer.ToString() + "&&Starting Turn&1%2%1#2%1%2*");

            while (game.getGameState() )
            {
                System.Threading.Thread.Sleep(1500);

                turn++;

                // String turnMessage = turn.ToString() + "&" + activePlayer.ToString() + "&&Starting Turn";
                // Console.WriteLine(turnMessage);

                // System.Threading.Thread.Sleep(1500);

                // message 6
                // game.getPlayers()[activePlayer].getPlayerWriter().WriteLine("It is your turn. Make a move.");

                Console.WriteLine("You are located at " + game.getPieces()[activePlayer].getX().ToString() + " " + game.getPieces()[activePlayer].getY().ToString() );

                String move = game.getPlayers()[activePlayer].getPlayerReader().ReadLine();

                // player tried to make an invalid move. Force them to try again until they send a valid move
                // the condition of the while loop makes the move. 
                while(! game.handlePlayerTurn(move) )
                {
                    //String notValid = "Player " + activePlayer.ToString() + " attempted a move that was not valid.";
                    //sendToAllPlayers(game.getPlayers(), numberPlayers, notValid);
                    String notValid = turn + "&0&0&INVALID&0%0%0%";
                    // construct a string that contains the turn number and the "invalid" message.
                   sendToAllPlayers(game.getPlayers(), numberPlayers, notValid);

                   System.Threading.Thread.Sleep(6500);

                   move = game.getPlayers()[activePlayer].getPlayerReader().ReadLine();
                }

                

                Console.WriteLine("The number of players is " + numberPlayers.ToString() );

                string currentX = game.getPieces()[activePlayer].getX().ToString();
                string currentY = game.getPieces()[activePlayer].getY().ToString();

                // players need to be told who the next active player is, along with the move of the current active player
                // this is crude, but since the passbyref is confusing me.. (i mean, it works)

                String toSend = game.generateMoveString(turn, getNextPlayerIndex(activePlayer, game.getMaxPlayers()), currentX, currentY, move);
                Console.WriteLine("String sent: " + toSend);
                // toSend should now hold the string.
                // if the move was valid, then it was made when handlePlayerTurn is called 
                // notify all players that a valid move was made 
                sendToAllPlayers(game.getPlayers(), numberPlayers, toSend);
                System.Threading.Thread.Sleep(6500);

                // increment the value of active player to the next player
                activePlayer = getNextPlayerIndex(activePlayer, game.getMaxPlayers());

                /*
                // check the gameState after each loop so we know when to "end" the game.
                activeGame = game.getGameState();       // gets the existing game state

                if (!activeGame)        // if game is no longer active
                {
                    // This is Jason2 - I CLAIM CREDIT FOR THIS HORRIBLE SOLUTION
                    break;              // break out of the game loop
                }
                 * */
            }
        }

        // Made a function that allows me to get the turn number.
        public int getTurnNumber()
        {
            return turn;        
        }

        /*
        private bool allPlayersConnected(List<Player> currentPlayers, int players)
        {
            /*
            // counting by p
            for (int p = 0; p < players; p++)
            {
                if (!currentPlayers[p].getSocketConnected() )
                {
                    Console.WriteLine("Not all players are connected.");
                    return false;
                }
            }

            return true;
        }
        */

        private void sendToAllPlayers(List<Player> players, int count, String message)
        {
            // counting by m
            for (int m = 0; m < count; m++)
            {
                players[m].getPlayerWriter().WriteLine(message);
            }
        }

        // for a 2 player game, max will equal 2
        // player values will be either 0 or 1
        // increments from 0 to (max -1) for multi player games 
        public int getNextPlayerIndex(int i, int max)
        {
            int index = 0;

            if ( (i +1) < max)
            {
                index = i +1;
            }

            else
            {
                index = 0;
            }

            Console.WriteLine("The next player to take a turn is " + index.ToString() );

            return index;
        }

        public int getActivePlayer()
        {
            return activePlayer;
        }
    }
}
