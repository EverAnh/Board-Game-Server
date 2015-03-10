

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace Game
{
    class LoginDatabase
    {
        // Holds our connection with the database
        SQLiteConnection m_dbConnection;

        //Creates a new database file but should really only be used for 
        //to clear the database and start fresh. 
        //WARNING ONLY USE TO CLEAR THE DB. IF CALLED THE DB WILL LOSE ALL DATA
        public void createNewDatabase()
        {
            SQLiteConnection.CreateFile("LoginDatabase.sqlite");
        }

        // Creates a connection with our database file or automatically creates the file
        // depending on whether the file exists or not.
        // will not clear the database and will hold all of the users even after the server is closed
        public void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=LoginDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        // Creates a table named 'users' with three columns: userID (an random int between 1 and 100,000) and password (a string of max 20 characters) 
        // and a LoginID that will be chosen by the player. pw = password
        public void createTable()
        {
            //SELECT name FROM sqlite_master WHERE type='table' AND name='table_name';

            string sql = "SELECT * FROM sqlite_master WHERE type='table' AND name='users'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("CREATING TABLE");

                sql = "create table users (userID integer NOT NULL, pw VARCHAR(20) NOT NULL,loginID VARCHAR(20), UNIQUE(userID) )";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                Console.WriteLine("CREATING TABLE 'USERS'!");
            }
            else
            {
                Console.WriteLine("TABLE EXISTS!");
            }
        }
        
        /*DELETED FILLTABLE() SINCE THERE IS NO NEED FOR IT.
         ADDNEWPLAYERS() BELOW IS THE ONLY THING NEEDED TO REGISTER NEW PLAYERS.*/

       
        //NO LONGER REQUIRES AN EMAIL- 03-9-15
        public void addNewPlayer(string newLoginId ,string newPw)
        {
            var ranInt = Unique_userId();
            string sql = "insert into users (userID,loginID,pw) values ('" + ranInt+ "','" +newLoginId+"','"+ newPw + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }
        

        //**NOW MODIFIED FOR BOARD GAME SERVER**
        // return 1 if the login attempt was a success
        // return 0 if the login attempt was a fail
       
        public bool verifyPassword(string loginId, string pw)
        {
            // check that newPW matches password of userName
            string sql = "select pw from users where pw = '" + pw + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            Console.Write(reader["pw"]);

            // the password is a match
            if (reader["pw"].Equals(pw))
            {
                Console.WriteLine("    Password is a match    " + loginId);
                // return true;

                // send TCP packet back to client setting player as logged in
                return true;
            }

            else
            {
                Console.WriteLine("    Password is NOT a match  " + loginId);
                // return false;

                // send TCP packet back to client that login failed
                return false;
            }
        }

        public bool checkIfLogInExists(string loginId)
        {
            string sql = "select loginID from users where loginID = '" +loginId+"'" ;
            Console.WriteLine("sql " + sql);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

           // Console.WriteLine("test");
 
           

            // command.Connection.Open();
            bool userExists = reader.Read();

            if (userExists)
            {
                // Console.WriteLine("  found it! " + userName);
                return true;
            }

            // Console.WriteLine("   didn't find it " + userName);
            return false;
        }

        // return 1 if login attempt was a success
        // return 0 if the login attempt was a fail and will create a new player
        //FIRST YOU WOULD ATTEMPT TO LOG IN AND IF THE LOGIN ATTEMP IS FAILS
        //THEN THE ADDNEWPLAYER() FUNCTION IS CALLED AND A NEW PLAYER IS ADDED.
 
        public bool attemptToLogin(string checkLogin, string checkPw)
        {
            bool returningUser = checkIfLogInExists(checkLogin);

            // Console.WriteLine("  calling login " + checkName);
            if (returningUser && verifyPassword(checkLogin, checkPw))
            {
                return true;
            }
            
            Console.WriteLine("Invalid Login username and password!");
            Console.WriteLine("New player profile being Created!");
            addNewPlayer(checkLogin, checkPw);
            Console.WriteLine("New player has been created!");
            return false;
        
        }

        // Writes the users to the console sorted on score in descending order.
        public void printUsers()
        {

            string sql = "select * from users order by pw desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("UserID: " + reader["userID"] + "\tPassword: " + reader["pw"] + "\tlogin ID: " + reader["loginID"]);
            }

            Console.ReadLine();
        }

        //creates a random numner for userID between 1 and 100,000
        int Unique_userId()
        {
            Random ranId = new Random();
            return ranId.Next(100000);
        }


    }
}
