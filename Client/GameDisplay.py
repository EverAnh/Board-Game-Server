import pygame
import os,sys,time
import Display

#PUT Display.Display in after everything is done
class GameDisplay():

    WINDOW_LENGTH = 800
    GRID_LENGTH = 8
    CELL_SIZE = WINDOW_LENGTH / GRID_LENGTH

    BACKGROUND_COLOR = pygame.Color(160,160,160)

    CELL_COLOR_1 = pygame.Color(0,0,0)
    CELL_COLOR_2 = pygame.Color(255,255,255)

    PLAYER = 1
    
    GAMEPIECE_BORDER_COLOR = pygame.Color(218,165,32)
    GAMEPIECE_COLOR_1 = pygame.Color(222,184,135)
    GAMEPIECE_COLOR_2 = pygame.Color(139,69,19)

    def __init__(self):
        self._should_quit = False
        self._turn = 1
        #temporarily to test pygame
        self._gameboard = [[(0,0) for x in range(GameDisplay.GRID_LENGTH)] for y in range(GameDisplay.GRID_LENGTH)]
        pygame.init()
        pygame.font.init()
        self._screen = pygame.display.set_mode((GameDisplay.WINDOW_LENGTH, GameDisplay.WINDOW_LENGTH))
        pygame.display.set_caption("Grid Game-2")

        #test pieces
        self._gameboard[4][2] = (1,2)
        self._gameboard[1][4] = (1,2)

    #will draw the board every time there is a change
    def update(self):
        self._screen.fill(GameDisplay.BACKGROUND_COLOR)
        self._draw_board(self._gameboard)
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self._should_quit = True
                break
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mousePosition = pygame.mouse.get_pos()
                print "mouse pressed down at ", mousePosition
                column = mousePosition[0]/self.CELL_SIZE
                row = mousePosition[1]/self.CELL_SIZE
                print "row,column" ,(column,row)
                #will display valid move,invalid move text, or error
                if self.CheckValidMove() == 1:
                    #this is where we add to game board temporarily inside here
                    self._gameboard[column][row] = (1,2)
                    self._drawGamePiece((column,row),1)
                if self.CheckValidMove() == 2:
                    self._drawError()

        pygame.display.flip()
        

    def end_game(self):
        time.sleep(3)
        pygame.quit()

        
    def getGameBoard(self):
        return self._gameboard

    #0 = invalid move. 1 = valid move. 2 = error message
    def CheckValidMove(coordinate):
        return 1

    def _draw_board(self,board):
        self._drawGrid()
        self._tempBoard = board
        for i in range(GameDisplay.GRID_LENGTH):
            for j in range(GameDisplay.GRID_LENGTH):    
                if self._tempBoard[i][j][0] == 1 and  self._tempBoard[i][j][1] > 0:
                    self._drawGamePiece((i,j),1)
        
    #draws board at the beginning of the game
    def _drawGrid(self):
        for i in range(GameDisplay.GRID_LENGTH):
            for j in range(GameDisplay.GRID_LENGTH):
                if (i + j) % 2:
                    pygame.draw.rect(self._screen, GameDisplay.CELL_COLOR_1, pygame.Rect(i*GameDisplay.CELL_SIZE, j*GameDisplay.CELL_SIZE, GameDisplay.CELL_SIZE, GameDisplay.CELL_SIZE).inflate(-1,-1), 0)
                else:
                    pygame.draw.rect(self._screen, GameDisplay.CELL_COLOR_2, pygame.Rect(i*GameDisplay.CELL_SIZE, j*GameDisplay.CELL_SIZE, GameDisplay.CELL_SIZE, GameDisplay.CELL_SIZE).inflate(-1,-1), 0)
               

    #will draw new pieces every time a valid move has been made
    def _drawGamePiece(self,pos,Player):
        pygame.draw.circle(self._screen,pygame.Color(222,184,135),(pos[0]*GameDisplay.CELL_SIZE+GameDisplay.CELL_SIZE/2,pos[1]*GameDisplay.CELL_SIZE + GameDisplay.CELL_SIZE/2),50,0)
   

    def _drawError(self):
        fontObject = pygame.font.Font(None,100)
        ErrorMessage = fontObject.render("Error",1,(100,100,100))
        self._screen.blit(ErrorMessage,(400,400))
        
        
        


##if __name__ == 'main':
game = GameDisplay()
clock = pygame.time.Clock()
#game.connect_to_server()

while not game._should_quit:
    clock.tick(30)
    game.update()
pygame.quit()
