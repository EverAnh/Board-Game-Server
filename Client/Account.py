## Account.py
## INF 122 - W15
##
import ServerConnection

class Account:

    FAIL_KEYWORD = "FAIL"
    
    def __init__(self):
        self._username = ""
        self._password = ""
        self._logged_in = False

            
    def login(self, connection, game_choice):
        login_string = self._username + ServerConnection.CATG_DELIM + self._password + ServerConnection.CATG_DELIM + game_choice
        connection.open_connection()
        player_number = connection.get_response()  #message 1 <-        
        connection.send_request(login_string)      #message 2 ->
        status_message = connection.get_response() #message 3 <-
        
        return (player_number, status_message)
            

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


    def is_logged_in(self):
        return self._logged_in
