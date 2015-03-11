using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Game
{
    public class Player
    { // start Player class 
        private NetworkStream stream;
        private Socket sock;
        private StreamReader reader;
        private StreamWriter writer;
        private int playerNumber;
        private String userName;
        private String gameChosen;

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

        public String getGame()
        {
            return gameChosen;
        }

        public void setGame(String game)
        {
            gameChosen = game;
        }

        public bool getSocketConnected()
        {
            /*
            bool canRead = sock.Poll(500, SelectMode.SelectRead);
            bool canWrite = sock.Poll(500, SelectMode.SelectWrite);
            return (canRead && canWrite);
             */

            return true;
        }

    } // end Player class 
}
