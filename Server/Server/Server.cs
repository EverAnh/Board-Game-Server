using System;
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
        private static List<GenericGame> games = new List<GenericGame>();
        private IPAddress local = IPAddress.Parse("127.0.0.1");
        private int port = 54389;
        private LoginDatabase db;

        public Server()
        {
            startListener();
            startServer();

            db = new LoginDatabase();

            db.createNewDatabase();

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

        private class Player
        { // start Player class 
            private NetworkStream stream;
            private Socket sock;
            private StreamReader reader;
            private StreamWriter writer;
            private int playerNumber;
            private String userName;

            // Player constructor

            public Player(NetworkStream newStream, Socket newSocket, StreamReader newReader, StreamWriter newWriter)
            {
                stream = newStream;
                sock = newSocket;
                reader = newReader;
                writer = newWriter;
            }

            public NetworkStream getPlayerStream()
            {
                return stream;
            }

            public Socket getPlayerSocket()
            {
                return sock;
            }

            public StreamReader getPlayerReader()
            {
                return reader;
            }

            public StreamWriter getPlayerWriter()
            {
                return writer;
            }

            public int getNumber()
            {
                return playerNumber;
            }

            public void setPlayerNumber(int newNum)
            {
                playerNumber = newNum;
            }

            public String getUserName()
            {
                return userName;
            }

            public void setUserName(String n)
            {
                userName = n;
            }
        } // end Player class 

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
                    p.getPlayerWriter().WriteLine(playerNumber);
                }
            }
        } // end thread for TCP listener

        private class GameThread
        { // start GameThread
            int gameNumber = -1;

            GameThread(int newGame)
            {
                gameNumber = newGame;
            }

        } // end GameThread
    }
}
