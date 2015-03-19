from GameDisplay import GameDisplay
import pygame

class ConnectFourDisplay(GameDisplay):
    
    def __init__(self, GM, Game):
        GameDisplay.__init__(self,GM,Game)
        self._set_dimensions()
        self._screen = pygame.display.set_mode((GameDisplay.WINDOW_WIDTH, GameDisplay.WINDOW_HEIGHT))
        pygame.display.set_caption("Connect Four | Board Game Server | Team #1")
        #self._load_images()
        self._background = pygame.image.load('images/grid_connect_four.png')
        self._piece0 = pygame.image.load('images/pieceRed.png')
        self._piece1 = pygame.image.load('images/pieceBlack.png')

    def _set_dimensions(self):
        GameDisplay.BOARD_WIDTH = 747
        GameDisplay.WINDOW_WIDTH = GameDisplay.BOARD_WIDTH + GameDisplay.SIDE_BAR_WIDTH
        GameDisplay.WINDOW_HEIGHT = 640
        GameDisplay.BOARD_HEIGHT = GameDisplay.WINDOW_HEIGHT
        GameDisplay.GRID_WIDTH =  self._game.get_width()
        GameDisplay.GRID_HEIGHT = self._game.get_height()
        GameDisplay.CELL_WIDTH = GameDisplay.BOARD_WIDTH / GameDisplay.GRID_WIDTH
        GameDisplay.CELL_HEIGHT = GameDisplay.WINDOW_HEIGHT / GameDisplay.GRID_HEIGHT

    def _load_images(self):
        self._background = pygame.image.load('images/grid_connect_four.png')
        self._piece0 = pygame.image.load('images/pieceRed.png')
        self._piece1 = pygame.image.load('images/pieceBlack.png')

    def _drawGamePiece(self,pos,value):
        if(value == 1):
            self._screen.blit(self._piece0, (GameDisplay.CELL_WIDTH * pos[0], GameDisplay.CELL_HEIGHT * pos[1]))
        elif(value == 2):
            self._screen.blit(self._piece1, (GameDisplay.CELL_WIDTH * pos[0], GameDisplay.CELL_HEIGHT * pos[1]))

