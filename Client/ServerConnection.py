## ServerConnection.py
## INF 122 - W15
##
import socket

SERVER_IP = "127.0.0.1"
SERVER_PORT = 54389

class ServerConnection:
    def __init__(self):
        self._connection_active = False
        self._session_id = ""
        self._connection_ip = SERVER_IP
        self._connection_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    def connect(self, host = SERVER_IP, port = SERVER_PORT):
        try:
            self._connection_socket.connect( (host, port) )
            print 'socket connected'
            return True;
        except socket.error:
            print 'connection failed'
            return False;

    def send_request(self, request):
        self._connection_socket.sendall(request)

    def receive_response(self):
        return s.recv(4096)

    def handle_response(self):
        pass

    def close_connection(self):
        self._connection_socket.close()
        print 'connection closed'

## do getters and setters

#sc = ServerConnection()
#sc.connect()
#sc.close_connection()
