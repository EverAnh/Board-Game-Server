import pygame
import os,sys
import Display
import Game
import GameManager
import GamePiece

#add Display.Display,game,gamemanager later
class GameDisplay(Display.Display):

    WINDOW_WIDTH = 1000
    WINDOW_HEIGHT = 1000
    SIDE_BAR_WIDTH = 200
    BOARD_WIDTH = WINDOW_WIDTH - SIDE_BAR_WIDTH
    BOARD_HEIGHT = WINDOW_HEIGHT

    GRID_WIDTH = 0
    GRID_HEIGHT = 0
    
    CELL_WIDTH = 0
    CELL_HEIGHT = 0

    BACKGROUND_COLOR = pygame.Color(160,160,160)
        
    #Needs gametype for future implementation of other games
    def __init__(self,GM,Game):
    
        pygame.init()
        pygame.font.init()
        
        self._should_quit = False
        self._coordinate = (0,0)
        self._game = Game
        self._GM = GM
        self._response = None
        

        
        

        GameDisplay.GRID_WIDTH =  self._game.get_width()
        GameDisplay.GRID_HEIGHT = self._game.get_height()
        GameDisplay.CELL_WIDTH = GameDisplay.BOARD_WIDTH / GameDisplay.GRID_WIDTH
        GameDisplay.CELL_HEIGHT = GameDisplay.WINDOW_HEIGHT / GameDisplay.GRID_HEIGHT
        
        self._screen = pygame.display.set_mode((GameDisplay.WINDOW_WIDTH, GameDisplay.WINDOW_HEIGHT))
        pygame.display.set_caption("Game Board Client")     

        #creates font object for displaying players,turns and scores
        self._PlayerFont = pygame.font.Font(None,35)
        self._TurnsFont = pygame.font.Font(None,35)
        self._ScoresFont = pygame.font.Font(None,35)

        #creates side bar
        pygame.draw.rect(self._screen, pygame.Color(141,141,141), pygame.Rect(GameDisplay.BOARD_WIDTH,0,GameDisplay.SIDE_BAR_WIDTH,GameDisplay.WINDOW_HEIGHT) )
        self._draw_side_bar(self._response)   

        self._load_images()



    #will draw the board every time there is a change
    def update(self):
        self._screen.fill(GameDisplay.BACKGROUND_COLOR)
        self._draw_board(self._game.get_board())
        for event in pygame.event.get():
            if (event.type == pygame.QUIT):
                pygame.quit()
                
            elif event.type == pygame.MOUSEBUTTONDOWN:
                #on click update will call game manager and check wether move is valid or not
                mousePosition = pygame.mouse.get_pos()
                if(mousePosition[0]>GameDisplay.BOARD_WIDTH):
                    print "you are clicking the side bar"
                else:
                    column = mousePosition[0]/GameDisplay.CELL_WIDTH
                    row = mousePosition[1]/GameDisplay.CELL_HEIGHT
                    print "column,row" ,(column,row)
                    self._coordinate = (column,row)
                #asks GameManager for a response on wether the move is valid or not
                self._response = self._GM.handle_my_move(self._coordinate)


                ######## DEBUG #####################
                print 'message(gd): ', self._response.message
                if (self._response=="INVALID"):
                    print 'invalid move!'
                    return
                ####################################

                #if winner, end game
                print 'checking for winner'
                if(self._response=="WINNER"):
                    self._end_game()

                #### DEBUG ######
                self._game.set_my_turn(True)
                #################
                self._drawMessage(self._response)

                
        self._draw_board(self._game.get_board())
        self._draw_side_bar(self._response)
        pygame.display.flip()

        if self._game.is_over():
                pygame.time.wait(3000)
                pygame.quit()


    def _draw_board(self,board):
        self._drawGrid()
        self._tempBoard = board
        for i in range(GameDisplay.GRID_WIDTH):
            for j in range(GameDisplay.GRID_HEIGHT):
                if self._tempBoard[i][j] is not None:
                    self._drawGamePiece((i,j), self._tempBoard[i][j].value)


    def _drawGrid(self):
        
        self._bgSize = self._background.get_rect()
        self._background = pygame.transform.scale(self._background, (GameDisplay.BOARD_WIDTH, GameDisplay.WINDOW_HEIGHT))
        self._piece0 = pygame.transform.scale(self._piece0, (GameDisplay.CELL_WIDTH, GameDisplay.CELL_HEIGHT))
        self._piece1 = pygame.transform.scale(self._piece1, (GameDisplay.CELL_WIDTH, GameDisplay.CELL_HEIGHT))
        self._screen.blit(self._background, (0,0))

        self._temp_board = self._game.get_board()
        for i in range(self._game.get_width()):
            for j in range(self._game.get_height()):
                if self._temp_board[i][j] != None:
                    self._drawGamePiece((i,j),1)


    def _drawGamePiece(self,pos,value):
        if(value == 1):
            self._screen.blit(self._piece0, (GameDisplay.CELL_WIDTH * pos[0], GameDisplay.CELL_HEIGHT * pos[1]))
        elif(value == 2):
            self._screen.blit(self._piece1, (GameDisplay.CELL_WIDTH * pos[0], GameDisplay.CELL_HEIGHT * pos[1]))
            

    def _drawMessage(self,response):
        if(response == ""):
            pass

        else:
            messageFontObject = pygame.font.Font(None,30)
            MessageRendered = messageFontObject.render(response.message,1,pygame.Color(0,0,0))
            self._screen.blit(MessageRendered,(50,GameDisplay.WINDOW_HEIGHT/2))
            pygame.display.flip()
            pygame.time.wait(1500)

    def _draw_side_bar(self,response):

        if(response == None):
            #draws players turn on side bar before a player has moved
            self._DisplayPlayers = self._PlayerFont.render(("Player " + str(self._game.get_player_turn())),1,pygame.Color(0,0,0))
            self._screen.blit(self._DisplayPlayers,(GameDisplay.BOARD_WIDTH+25,(GameDisplay.WINDOW_HEIGHT/4)))
                    
            #draws turns number on side bar before a player has moved
            self._DisplayTurns = self._TurnsFont.render(str(self._game.get_turn_number()),1,pygame.Color(0,0,0))
            self._screen.blit(self._DisplayTurns,(GameDisplay.BOARD_WIDTH+25,(GameDisplay.WINDOW_HEIGHT/4)*2))
                        
            #draw scores on side bar before a player has moved
            self._DisplayScores = self._ScoresFont.render(str(0),1,pygame.Color(0,0,0))
            self._screen.blit(self._DisplayScores,(GameDisplay.BOARD_WIDTH+25,(GameDisplay.WINDOW_HEIGHT/4)*3))

        else:
            #draws players turn on side bar
            self._DisplayPlayers = self._PlayerFont.render(str(response.player_turn),1,pygame.Color(0,0,0))
            self._screen.blit(self._DisplayPlayers,(GameDisplay.BOARD_WIDTH+25,(GameDisplay.WINDOW_HEIGHT/4)))
                    
            #draws turns number on side bar
            self._DisplayTurns = self._TurnsFont.render(str(response.turn_number),1,pygame.Color(0,0,0))
            self._screen.blit(self._DisplayTurns,(GameDisplay.BOARD_WIDTH+25,(GameDisplay.WINDOW_HEIGHT/4)*2))
                        
            #draw scores on side bar
            self._DisplayScores = self._ScoresFont.render(response.scores[0],1,pygame.Color(0,0,0))
            self._screen.blit(self._DisplayScores,(GameDisplay.BOARD_WIDTH+25,(GameDisplay.WINDOW_HEIGHT/4)*3))

        pygame.display.flip()


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

