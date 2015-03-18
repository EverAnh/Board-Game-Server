import pygame
import os, sys, time

import Game
import GameDisplay

class TestDriver:
    
    
    def __init__(self):
        self._game = Game.Game()
        self._display = GameDisplay.GameDisplay(self._game)
        self._game.set_my_turn(True)
        self._clock = pygame.time.Clock()
    
    
    def go(self):
        print 'running game'
        while not self._display.shouldQuit():
            self._display.update()
            self._clock.tick(30)
        self.handle_opponent_move()

    
    def manage_endgame(self):
        pass
    
    
    def handle_opponent_move(self):
        pass


td = TestDriver()
td.go()
