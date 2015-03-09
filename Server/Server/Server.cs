﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

using System.Collections;

namespace Game
{
    class Server
    {
        private static TcpListener listener;
        private static List<Player> activePlayers = new List<Player>();
        private static List<Game_Generic> games = new List<Game_Generic>();
        private IPAddress local = IPAddress.Parse("127.0.0.1");
        private int port = 54389;
        private LoginDatabase db;
        protected static char delim = '%';

        public Server()
        {
            
            // Start the TCP listener and the server.
            
            startListener();
            startServer();

            // Initialize the login database and create a new database.

            db = new LoginDatabase();
            db.createNewDatabase();
            db.connectToDatabase();
            db.createTable();
            db.fillTable(2," anom@anom.net","Tom", "password");
            db.attemptToLogin("Tom", "password");
            db.printUsers();


            // Console.WriteLine("number players " + numberPlayers);
            // string stop = Console.ReadLine();

            // numberPlayers will be assigned by player 1 choosing a game
            // for now, defaulting value numberPlayers

            // numberPlayers = 1;
            // Console.WriteLine("player 0 ");
            // addPlayer(0);

            // activePlayers = new Player[numberPlayers];

            // counting by c
            /*
            for (int c = 1; c < numberPlayers; c++)
            {
                Console.WriteLine("player c " + c);
                addPlayer(c);
            }
             * */
        }

        private void startListener()
        {

            // Declare a new listener with local 127.0.0.1 and port 54389

            listener = new TcpListener(local, port);
            listener.Start();

            // Create a new ListenerThread.

            ListenerThread lt = new ListenerThread();
            Thread gameThread = new Thread(new ThreadStart(lt.addPlayers ) );
            gameThread.Start();

            // Console.WriteLine("TCP Listener is running");
        }

        private void startServer()
        {
            Console.Write("Press Enter to Start the server: ");
            Console.ReadLine();
        }

        private static int getIndexOfGameToJoin(Player p)
        {
            int numberGames = games.Capacity;
            String playerGame = p.getGame();

            // check each existing game for a game type match AND room for an available player 
            // counting by g
            for (int g = 0; g < numberGames; g++)
            {
                String gameType = games[g].getGameType();

                int currentPlayers = games[g].getNumberPlayers();
                int maxPlayers = games[g].getMaxPlayers();

                if ( (playerGame == gameType ) && (currentPlayers < maxPlayers) )
                {
                    return g;
                }
            }

            return -1;
        }

        /*
        private void addPlayer(int playerNumber)
        {
            Console.WriteLine("calling add player " + playerNumber);

            
            

            // activePlayers.Add(p);

            Console.WriteLine(" player number equals " + playerNumber);

            activePlayers[playerNumber].getPlayerWriter().WriteLine(playerNumber);
            activePlayers[playerNumber].connected = true;

            string data = activePlayers[playerNumber].getPlayerReader().ReadLine();
            Console.WriteLine(data);
            // Console.ReadLine();

            while (true)
            {
                Console.Write("Input a message:  ");
                data = Console.ReadLine();
                activePlayers[playerNumber].getPlayerWriter().WriteLine(data);

                data = activePlayers[playerNumber].getPlayerReader().ReadLine();
                Console.WriteLine("Message recieved : " + data);
            }
        }
         * 
         * */



        private class ListenerThread
        { // start thread for TCP listener
            public void addPlayers()
            {
                while (true) // Only add players 
                {
                    Socket sock = listener.AcceptSocket();

                    Console.WriteLine("   player socket has accepted the socket ");

                    NetworkStream nws = new NetworkStream(sock);
                    StreamReader sr = new StreamReader(nws);
                    StreamWriter sw = new StreamWriter(nws);
                    sw.AutoFlush = true;

                    Player p = new Player(nws, sock, sr, sw);
                    activePlayers.Add(p);

                    int playerNumber = activePlayers.IndexOf(p);

                    Console.WriteLine("new player number " + playerNumber);
                    p.getPlayerWriter().WriteLine(playerNumber.ToString() );

                    String startMessage = p.getPlayerReader().ReadLine();
                    String[] data = startMessage.Split(delim);

                    /*
                     * ATTEMPT TO LOG IN TO DATABASE HERE 
                     * GET EITHER SUCCESS OR FAILURE RESULTS FROM DATABASE
                     * 
                     */

                    /*
                     * 
                     if( login was successful)
                     *  startMessage = "Login was successful.";
                     * 
                     else if ( new user was created)
                     *  startMessage = "New User created.";
                     * 
                     else // login attempt failed
                     *  startMessage = "Login attempt failed. Try again.";
                     * 
                     p.getPlayerWriter().WriteLine
                     * 
                     */

                    p.setGame(data[2]);

                    int gameToJoin = getIndexOfGameToJoin(p);
                    String newPlayerNumber = "0";
                    String gameToPlay = "";

                    // there is no existing game of the type that the player wants to play
                    // that also has room for an additional player
                    if (gameToJoin == -1)
                    {
                        newPlayerNumber = "1";

                        if (p.getGame() == "connectFour")
                        {
                            Game_Generic newGame = new Game_ConnectFour();
                            games.Add(newGame);
                            newGame.addPlayer(p);
                            gameToPlay = p.getGame();
                        }
                    }

                    // found a matching game type that needs an additional player
                    else
                    {
                        newPlayerNumber = games[gameToJoin].getNumberPlayers().ToString();
                        games[gameToJoin].addPlayer(p);
                    }

                    startMessage = newPlayerNumber + delim + gameToPlay;

                    // start string is constructed to tell the client which game to start 
                    p.getPlayerWriter().WriteLine(startMessage);
                }
            }
        } // end thread for TCP listener

        private class GameThread
        { // start GameThread
            // int gameNumber = -1;

            GameThread(String newGame)
            {
                // gameNumber = newGame;
            }

        } // end GameThread
    }
}
