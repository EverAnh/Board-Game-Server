import socket, time

import Response

DEFAULT_IP = "127.0.0.1"
DEFAULT_PORT = 3445
    
CREATE_SUCC  = "CREATESUCC"
CREATE_FAIL  = "CREATEFAIL"
LOGIN_SUCC   = "LOGINSUCCD"
LOGIN_FAIL   = "LOGINFAILD"

WINNER = "WINNER"
INVALID = "INVALID"

CATG_DELIM = "%"
MOVE_DELIM = "#"
SCOR_DELIM = "$"
VALU_DELIM = "&"
    
class ServerConnection:

    
    


    def __init__(self):
        self._host_address = DEFAULT_IP
        self._host_port = DEFAULT_PORT
        self._socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)


    def open_connection(self):
        self._socket.connect( (self._host_address, self._host_port) )

            
    def send_request(self, request):
        self._socket.sendall(request + '\n')


    def get_response(self):
        print 'sc receiving packet'
        return self._socket.recv(4096)
        print 'sc received packet'

        
    def send_move(self, move):
        send_string = ""
        for coord in move:
            send_string.append(coord[1] + VALU_DELIM + coord[2] + MOVE_DELIM)
        self.send_request(send_string[:-1]) #cut off last move delimiter


    def get_move(self):
        raw_response = self._socket.recv(4096)
        category_tokens = raw_response.split(CATG_DELIM)
        turn_number = category_tokens[0]
        player_turn = category_tokens[1]
        raw_scores = category_tokens[2]
        message = category_tokens[3]
        raw_pieces = category_tokens[4].split(MOVE_DELIM)

        scores = [score for score in scores.split(SCORE_DELIM)] #list comprehension ftw

        pieces = []
        for piece in raw_pieces:
            p = piece.split(VALU_DELIM)
            pieces.add(GamePiece(p[0], p[1], p[2], p[3]))
            return Response(turn_number, player_turn, message, scores, pieces)
        

    def close_connection(self):
        self._socket.close()
