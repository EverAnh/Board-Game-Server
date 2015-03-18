from Game import Game

class ConnectFour(Game):
    
    def __init__(self):
        Game.__init__(self)
        self._board_width = 7
        self._board_height = 6
