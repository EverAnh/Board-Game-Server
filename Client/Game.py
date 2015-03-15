import GameBoard


class Game:
    
    def __init__(self):
        self._player_number = None
        self._winner = None
        self._my_turn = False
        self._gameboard = [[None for x in range(self._board_width)] for y in range(self._board_height)]


    def update_board(self, changed_pieces):
        for piece in changed_pieces:
            if piece.value == 0:
                remove_piece(piece)
            else:
                add_piece(piece)


    def clear_board(self):
        self._create_board()


    def remove_piece(self, piece):
        self._gameboard[piece.col][piece.row] = None


    def add_piece(self, piece):
        self._gameboard[piece.col][piece.row] = piece


    def is_over(self):
        return winner is not None


    def is_my_turn(self):
        return self._my_turn
