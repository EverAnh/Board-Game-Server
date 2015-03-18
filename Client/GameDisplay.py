import pygame
import os,sys,time
import Display
import GamePiece

WINDOW_WIDTH = 640
WINDOW_HEIGHT = 640
BACKGROUND_COLOR = pygame.Color(160,160,160)

class GameDisplay(Display.Display):

    def __init__(self, gameObj):
        
        pygame.init()
        pygame.font.init()
        
        self._screen = pygame.display.set_mode((WINDOW_WIDTH, WINDOW_HEIGHT))
        self._game = gameObj
        self._cell_size = WINDOW_WIDTH / self._game.get_width()
        self._screen.fill(BACKGROUND_COLOR)
        pygame.display.set_caption("Grid Game-2")
        self._should_quit = False
        
        self._init_test_01() # place dummy pieces
        
        self._load_images()
        self._draw_grid()


    def update(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self._should_quit = True
                break
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mousePosition = pygame.mouse.get_pos()
                print "mouse pressed down at ", mousePosition
                column = mousePosition[0]/self._cell_size
                row = mousePosition[1]/self._cell_size
                print "row,column" ,(column,row)
                #will display valid move,invalid move text, or error
                if self.CheckValidMove() == 1:
                    #this is where we add to game board
                    self._drawGamePiece((column,row),1)
                if self.CheckValidMove() == 2:
                    self._drawError()
        pygame.display.flip()


    def end_game(self):
        time.sleep(3)
        pygame.quit()


    def CheckValidMove(coordinate):
        return 1


    def _draw_grid(self):
        
        self._bgSize = self._background.get_rect()
        self._background = pygame.transform.scale(self._background, (WINDOW_WIDTH, WINDOW_HEIGHT))
        self._piece0 = pygame.transform.scale(self._piece0, (self._cell_size, self._cell_size))
        self._piece1 = pygame.transform.scale(self._piece1, (self._cell_size, self._cell_size))
        self._screen.blit(self._background, (0,0))

        self._temp_board = self._game.get_board()
        for i in range(self._game.get_width()):
            for j in range(self._game.get_height()):
                if self._temp_board[i][j] != None:
                    self._drawGamePiece((i,j),1)

    def _drawGamePiece(self,pos,Player):
        self._screen.blit(self._piece0, (self._cell_size * pos[0], self._cell_size * pos[1]))
        

    def _drawError(self):
        fontObject = pygame.font.Font(None,100)
        ErrorMessage = fontObject.render("Error",1,(100,100,100))
        self._screen.blit(ErrorMessage,(400,400))


    def _init_test_01(self):
        self._game.add_piece(GamePiece.GamePiece(4,2,1,2))
        self._game.add_piece(GamePiece.GamePiece(1,4,1,2))


    def _load_images(self):
        print 'load images'
        self._background = pygame.image.load('images/grid_8x8_simple.png')
        self._piece0 = pygame.image.load('images/piece_round_blue.png')
        self._piece1 = pygame.image.load('images/piece_round_red.png')
        
    def shouldQuit(self):
        return self._should_quit