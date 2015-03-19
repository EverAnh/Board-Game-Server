from GameDisplay import GameDisplay
import pygame

class OthelloDisplay(GameDisplay):
    
    def __init__(self, GM, Game):
        GameDisplay.__init__(self,GM,Game)
        self._set_dimensions()
        self._screen = pygame.display.set_mode((GameDisplay.WINDOW_WIDTH, GameDisplay.WINDOW_HEIGHT))
        pygame.display.set_caption("Othello | Board Game Server | Team #1")
        self._background = pygame.image.load('images/grid_8x8_simple.png')
        self._piece0 = pygame.image.load('images/pieceWhite.png')
        self._piece1 = pygame.image.load('images/pieceBlack.png')


    def _set_dimensions(self):
        OthelloDisplay.BOARD_WIDTH = 720
        OthelloDisplay.WINDOW_WIDTH = OthelloDisplay.BOARD_WIDTH + OthelloDisplay.SIDE_BAR_WIDTH
        OthelloDisplay.WINDOW_HEIGHT = 720
        OthelloDisplay.BOARD_HEIGHT = OthelloDisplay.WINDOW_HEIGHT
        OthelloDisplay.GRID_WIDTH =  self._game.get_width()
        OthelloDisplay.GRID_HEIGHT = self._game.get_height()
        OthelloDisplay.CELL_WIDTH = OthelloDisplay.BOARD_WIDTH / OthelloDisplay.GRID_WIDTH
        OthelloDisplay.CELL_HEIGHT = OthelloDisplay.WINDOW_HEIGHT / OthelloDisplay.GRID_HEIGHT


    def _drawGamePiece(self,pos,value):
        if(value == 1):
            self._screen.blit(self._piece0, (GameDisplay.CELL_WIDTH * pos[0], GameDisplay.CELL_HEIGHT * pos[1]))
        elif(value == 2):
            self._screen.blit(self._piece1, (GameDisplay.CELL_WIDTH * pos[0], GameDisplay.CELL_HEIGHT * pos[1]))

