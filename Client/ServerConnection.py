import socket, time

import Response
import GamePiece

#DEFAULT_IP = "169.234.206.158"
DEFAULT_IP = "127.0.0.1"
DEFAULT_PORT = 3445
    
CREATE_SUCC  = "CREATESUCC"
CREATE_FAIL  = "CREATEFAIL"
LOGIN_SUCC   = "LOGINSUCCD"
LOGIN_FAIL   = "LOGINFAILD"


MAIN_DELIM = "*"
CATG_DELIM = "&"
MOVE_DELIM = "#"
SCOR_DELIM = "$"
VALU_DELIM = "%"
    
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
        print 'sending move: ', move
        send_string = ""
        for coord in move:
            send_string += str(coord[0]) + VALU_DELIM + str(coord[1])## + MOVE_DELIM
        self.send_request(send_string) #cut off last move delimiter


    def get_move(self):
        print 'getting raw_response(sc)'
        raw_response = self._socket.recv(4096)
        print 'move: ' + raw_response
        main_token = raw_response.split(MAIN_DELIM)
        category_tokens = main_token[0].split(CATG_DELIM)

        if len(category_tokens) == 5:
            turn_number = 1
            player_turn = int(category_tokens[0])
            raw_scores = category_tokens[1]
            message = category_tokens[2]
            raw_pieces = category_tokens[3].split(MOVE_DELIM)
        else:
            turn_number = int(category_tokens[0])
            player_turn = int(category_tokens[1])
            raw_scores = category_tokens[2]
            message = category_tokens[3]
            raw_pieces = category_tokens[4].split(MOVE_DELIM)
            
        print 'player turn: ',player_turn
        print 'message(sc):',message        
        print 'raw_scores: ',raw_scores
        scores = [score for score in raw_scores.split(SCOR_DELIM)] #list comprehension ftw
        print 'scores: ', scores
        pieces = []
        
        #if message.strip() != 'Starting Turn':
        print 'raw pieces: ',raw_pieces
        for piece in raw_pieces:
            p = piece.split(VALU_DELIM)
            pieces.append(GamePiece.GamePiece(int(p[0]), int(p[1]), int(p[2])))
        print 'pieces: ',pieces
        return Response.Response(turn_number, player_turn, scores, message, pieces)

    def close_connection(self):
        pass
        #self._socket.close()
