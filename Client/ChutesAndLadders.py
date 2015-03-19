from Game import Game

class ChutesAndLadders(Game):
    
    def __init__(self):
        Game.__init__(self)
        self._board_width = 10
        self._board_height = 10
        self._init_board()

    def _init_board(self):
        self.add_piece(GamePiece.Gamepiece(0,0,3))


    def add_piece(self, piece):
        source,dest,player = piece.split("%")
        
        
        
