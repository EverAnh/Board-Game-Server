from Game import Game

class Othello(Game):
    
    def __init__(self):
        Game.__init__(self)
        self._board_width = 8
        self._board_height = 8
