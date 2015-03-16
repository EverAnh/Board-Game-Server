import GameBoard


class Game:
    
    def __init__(self):
        self._my_player_number = 0
        self._winner = False
        self._turn_number = 0
        self._my_turn = False
        self._gameboard = [[None for x in range(self._board_width)] for y in range(self._board_height)]
        self._move_type = 1
        self._board_width = 3
        self._board_height = 6
        self._init_board()

    def _init_board(self):
        pass
    

    def update_board(self, changed_pieces):
        for piece in changed_pieces:
            if piece.value == 0:
                remove_piece(piece)
            else:
                add_piece(piece)


    def reset_board(self):
        self._init_board()


    def remove_piece(self, piece):
        self._gameboard[piece.col][piece.row] = None


    def add_piece(self, piece):
        self._gameboard[piece.col][piece.row] = piece


    def is_over(self):
        return winner is not None


    def is_my_turn(self):
        return self._my_turn


    def set_my_turn(self, a_bool):
        self._my_turn = a_bool


    def get_turn_number(self):
        return self._turn_number


    def set_turn_number(self, turn_number):
        self._turn = turn_number


    def get_my_player_number(self):
        return self._my_player_number


    def set_my_player_number(self, player_number):
        self._my_player_number = player_number
