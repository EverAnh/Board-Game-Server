using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Server_GameLoop
    {
        public Server_GameLoop()
        {

        }

        public void gameLoop(Game_Generic game)
        {
            // needs number of players
            // needs the Player object for each player in the game
            //      which includes stream reader/writer
            // 

            int numberPlayers = game.getNumberPlayers();
            int turn = 0;
            int activePlayer = 0;

            while (allPlayersConnected(game.getPlayers(), game.getPlayers().Capacity) )
            {
                turn++;
                String turnMessage = "Starting Turn " + turn.ToString() + ".";

                sendToAllPlayers(game.getPlayers(), numberPlayers, turnMessage);

                game.getPlayers()[activePlayer].getPlayerWriter().WriteLine("It is your turn. Make a move.");
                String move = game.getPlayers()[activePlayer].getPlayerReader().ReadLine();

                if(game.handlePlayerTurn(move) )
                {
                    sendToAllPlayers(game.getPlayers(), numberPlayers, move);
                }

                
            }
        }

        private bool allPlayersConnected(List<Player> currentPlayers, int players)
        {
            // counting by p
            for (int p = 0; p < players; p++)
            {
                if (!currentPlayers[p].getSocketConnected() )
                {
                    return false;
                }
            }

            return true;
        }

        private void sendToAllPlayers(List<Player> players, int count, String message)
        {
            // counting by m
            for (int m = 0; m < count; m++)
            {
                players[m].getPlayerWriter().WriteLine(message);
            }
        }

        private int getNextPlayerIndex(int i, int max)
        {
            int index = 0;

            if (i++ < max)
            {
                index = i++;
            }

            else
            {
                index = 0;
            }

            return index;
        }
    }
}
