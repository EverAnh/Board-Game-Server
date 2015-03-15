import pygame
import os, sys

class MainDisplay(Display):
    WINDOW_LENGTH = 1200

    BACKGROUND_COLOR = pygame.Color(160,160,160)

    def __init__(self):
        pygame.init()
        pygame.font.init()
        self._screen = pygame.display.set_mode((MainDisplay.WINDOW_LENGTH, MainDisplay.WINDOW_LENGTH))

        ## set flags
        self._should_quit = False
        
    def update(self):
        self._screen.fill(MainDisplay.BACKGROUND_COLOR)
        self._draw_screen()
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self._should_quit = True
                break
        
        pygame.display.flip()

    def retrieve_user_info(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self._should_quit = True
        

    def _draw_screen(self):
        
    
##if __name__ == 'main':

mainDisplay = MainDisplay()
while not mainDisplay._should_quit:
    ## clock.tick(30)
    mainDisplay.update()
pygame.quit()
