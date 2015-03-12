import GameBoard


class Game:
    #board = GameBoard()
    #board = [8][8]
    my_turn = 0 # bool

    WINDOW_LENGTH = 800
    GRID_LENGTH = 8
    CELL_SIZE = WINDOW_LENGTH / GRID_LENGTH
    
    def __init__(self):
        # initialize the game board & graphics
        self._init_board()
        #self._load_assets()

        # set flags
        self._should_quit = False
        self.is_turn = True

        # add test pieces
        self._gameboard[4][2] = (1,2)
        self._gameboard[1][4] = (1,2)


    def _init_board(self):
        self._gameboard = [[(0,0) for x in range(Game.GRID_LENGTH)] for y in range(Game.GRID_LENGTH)]

    def MakeMove():
        #TODO
        return

    def printToConsole(self):
        #for x in range(Game.GRID_LENGTH):
            #for y in range(Game.GRID_LENGTH):
                #print(self._gameboard[x][y])
        for x in range(Game.GRID_LENGTH):
            print(self._gameboard[x])

game = Game()
game.printToConsole()
#game.MakeMove(3, 5)
