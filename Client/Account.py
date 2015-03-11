## Account.py
## INF 122 - W15
##
import ServerConnection

class Account:
    def __init__(self):
        self._username = ""
        self._password = ""
        self._connection = ServerConnection()

    def login(self, username, password):
        pass

    def create(self, username, password):
        pass
        
## do getters and setters
