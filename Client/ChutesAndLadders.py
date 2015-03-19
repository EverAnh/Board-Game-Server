from Game import Game

class ChutesAndLadders(Game):
    
    def __init__(self):
        Game.__init__(self)
        self._board_width = 10
        self._board_height = 10
