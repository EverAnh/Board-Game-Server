import pygame
import os, sys, time
import socket

class GridGame:
    WINDOW_LENGTH = 600
    GRID_LENGTH = 8
    CELL_SIZE = WINDOW_LENGTH / GRID_LENGTH
    
    BACKGROUND_COLOR = pygame.Color(160,160,160)
                                    
    CELL_COLOR_1 = pygame.Color(0,0,0)
    CELL_COLOR_2 = pygame.Color(255,255,255)
    
    GAMEPIECE_BORDER_COLOR = pygame.Color(218,165,32)
    GAMEPIECE_COLOR_1 = pygame.Color(222,184,135)
    GAMEPIECE_COLOR_2 = pygame.Color(139,69,19)
    
    
  
    
    def __init__(self):
        pygame.init()
        pygame.font.init()
        self._screen = pygame.display.set_mode((GridGame.WINDOW_LENGTH, GridGame.WINDOW_LENGTH))
        pygame.display.set_caption("Grid Game")
        ##self._clock = pygame.time.Clock()

        # initialize the game board & graphics
        self._init_board()
        self._load_assets()

        # set flags
        self._should_quit = False
        self.is_turn = True

        # add initial piece
        self._last_location = (2,2)
        self._gameboard[self._last_location[0]][self._last_location[1]] = (1,2)

        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM) ##socket needs to stay in scope, thus its here for now!!!



    def update(self):
        self._screen.fill(GridGame.BACKGROUND_COLOR)
        self._draw_board()
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self._should_quit = True
                break
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mousePosition = pygame.mouse.get_pos()
                print "mouse pressed down at ", mousePosition
                column = mousePosition[0]/self.CELL_SIZE
                row = mousePosition[1]/self.CELL_SIZE
                move = str(column) + '%' + str(row) + '\n'
                self.s.sendall(move)
                print move
                move_response = self.s.recv(4096)
                print 'server sends: ' + move_response
                if move_response[0] == move[0]: ##kludge. for some reason, havin issues seeing if they're equal. blame it on tired
                    #remove old piece
                    print 'removing old'
                    self._gameboard[self._last_location[0]][self._last_location[1]] = (0,0)
                    
                    #add new piece
                    print 'adding new'
                    self._gameboard[column][row] = (1,2)
                    self.placeNewPiece(mousePosition)

                    #make new location the last location
                    print 'last is new'
                    self._last_location = (column,row)
                else:
                    print 'invalid move!'

                    
                
                '''
                if self._gameboard[column][row] != (0,0): ## this removes a piece 
                    self._gameboard[column][row] = (0,0)
                '''
                
        pygame.display.flip()

    def placeNewPiece(self, mousePosition):
        game._draw_gamepiece(mousePosition[0], mousePosition[1],1)
        ##game._draw_gamepieces()
        

    def _init_board(self):
        self._gameboard = [[(0,0) for x in range(GridGame.GRID_LENGTH)] for y in range(GridGame.GRID_LENGTH)]
        
        
    def _load_assets(self):
        pass
    

    def _draw_board(self):
        self._draw_grid()
        self._draw_gamepieces()
        

    def _draw_grid(self):
        for i in range(GridGame.GRID_LENGTH):
            for j in range(GridGame.GRID_LENGTH):
                if (i + j) % 2:
                    pygame.draw.rect(self._screen, GridGame.CELL_COLOR_1, pygame.Rect(i*GridGame.CELL_SIZE, j*GridGame.CELL_SIZE, GridGame.CELL_SIZE, GridGame.CELL_SIZE).inflate(-1,-1), 0)
                else:
                    pygame.draw.rect(self._screen, GridGame.CELL_COLOR_2, pygame.Rect(i*GridGame.CELL_SIZE, j*GridGame.CELL_SIZE, GridGame.CELL_SIZE, GridGame.CELL_SIZE).inflate(-1,-1), 0)
            

    def _draw_gamepieces(self):
        for i in range(GridGame.GRID_LENGTH):
            for j in range(GridGame.GRID_LENGTH):
                if self._gameboard[i][j][0] == 1 and self._gameboard[i][j][1] > 0:
                    self._draw_gamepiece(i,j,1)
                if self._gameboard[i][j][0] == 2 and self._gameboard[i][j][1] > 0:
                    self._draw_gamepiece(i,j,2)
                    

    def _draw_gamepiece(self, i, j, player):
        if player == 1:
            pygame.draw.circle(self._screen, GridGame.GAMEPIECE_COLOR_1, (i*GridGame.CELL_SIZE + GridGame.CELL_SIZE/2, j*GridGame.CELL_SIZE + GridGame.CELL_SIZE/2), GridGame.CELL_SIZE/2 - GridGame.CELL_SIZE/18, 0)
        else:
            pygame.draw.circle(self._screen, GridGame.GAMEPIECE_COLOR_2, (i*GridGame.CELL_SIZE + GridGame.CELL_SIZE/2, j*GridGame.CELL_SIZE + GridGame.CELL_SIZE/2), GridGame.CELL_SIZE/2 - GridGame.CELL_SIZE/18, 0)


    def connect_to_server(self):
        host = '169.234.99.167'
        port = 3445

        ##s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        self.s.connect( (host, port) )
        print 'socket connected'

        print 'received message 1: '
        playerID = self.s.recv(4096)
        print playerID

        DELIM = "%"
        user_name = "Heath"
        user_pass = "abc123"
        game_type = "generic"

        message_two = user_name + DELIM + user_pass + DELIM + game_type + '\n'

        print 'client sends: ' + message_two
        try :
            #Send message 2
            self.s.sendall(message_two)
            
            #Receive message 3
            message_three = self.s.recv(4096)
            print 'server sends msg 3: ' + message_three

            message_four = self.s.recv(4096)
            print 'server sends msg 4: ' + message_four

            ## time.sleep(1)
                    
            ## message_five = self.s.recv(4096)
            ## print 'server sends msg 5: ' + message_five
            
        except socket.error:
            #Send failed
            print 'Send failed'
            sys.exit()

        


##if __name__ == 'main':
game = GridGame()
clock = pygame.time.Clock()
game.connect_to_server()

while not game._should_quit:
    clock.tick(30)
    game.update()
pygame.quit()
