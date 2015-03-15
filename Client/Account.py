## Account.py
## INF 122 - W15
##
import ServerConnection

class Account:
    def __init__(self):
        self._username = ""
        self._password = ""


    def create(self, connection):
        create_string = self._username + ServerConnection.CATG_DELIM + self._password + ServerConnection.CATG_DELIM
        connection.open_connection()
        connection.send_request(create_string)

            
    def login(self, connection, game_choice):
        login_string = self._username + ServerConnection.CATG_DELIM + self._password + ServerConnection.CATG_DELIM + game_choice
        connection.send_request(login_string)
            

    def logout(self, connection):
        connection.close_connection()


    def set_username(self, name):
        self._username = name


    def set_password(self, password):
        self._password = password


    def get_username(self):
        return self._username


    def get_password(self):
        return self._password
