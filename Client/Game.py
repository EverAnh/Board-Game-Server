import GamePiece

class Game:
    
    def __init__(self):
        self._my_player_number = 0
        self._my_player_id = 0
        self._winner = -1
        self._turn_number = 1
        self._my_turn = False
        self._player_turn = 0
        self._board_width = 8
        self._board_height = 8
        self._gameboard = [[None for x in range(self._board_width)] for y in range(self._board_height)]
        self._move_type = 1
        
        self._init_board()

    def _init_board(self):
        ###### DEBUG ######
        ## self.add_piece(GamePiece.GamePiece(2,2,1))
        ###################

    def update_board(self, changed_pieces):
        for piece in changed_pieces:
            print 'piece changed(g): ',piece
            if piece.value == 0:
                self.remove_piece(piece)
            else:
                self.add_piece(piece)


    def reset_board(self):
        self._init_board()


    def remove_piece(self, piece):
        self._gameboard[piece.col][piece.row] = None


    def add_piece(self, piece):
        self._gameboard[piece.col][piece.row] = piece


    def is_over(self):
        print 'checking if game is over'
        return self._winner != -1


    def is_my_turn(self):
        return self._my_turn


    def set_my_turn(self, a_bool):
        self._my_turn = a_bool


    def get_turn_number(self):
        return self._turn_number


    def set_turn_number(self, turn_number):
        self._turn = turn_number


    def get_player_turn(self):
        return self._player_turn


    def set_player_turn(self, player_turn):
        self._player_turn = player_turn

    def get_my_player_number(self):
        return self._my_player_number


    def set_my_player_number(self, player_number):
        self._my_player_number = player_number


    def get_my_player_id(self):
        return self._my_player_id


    def set_my_player_id(self, player_id):
        self._my_player_id = player_id


    def get_width(self):
        return self._board_width


    def get_height(self):
        return self._board_height

    def set_board_size(self, w, h):
        self._board_width = w
        self._board_height = h
        
    def get_board(self):
        return self._gameboard


    def get_move_type(self):
        return self._move_type


    def set_move_type(self, move_type):
        self._move_type = move_type


    def set_winner(self, player_num):
        self._winner = player_num
