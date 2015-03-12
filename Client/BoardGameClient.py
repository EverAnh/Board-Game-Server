import pygame
import os, sys

import GameDisplay
import DemoGameBoard
import ConnectFourGameBoard

# ------------------------------------------------------------------------------
#
# This file is the main entry point of the client-side program, and it sets up
# a single instance of the class BoardGameClient to handle the rest of the
# program flow.
#
# BoardGameClient is the central component that manages the references to all
# other components involved with the GUI. It is the one with an overall loop to
# refresh the drawing of these managed GUI components.
#
# The BoardGameClient class takes an argument called "game_type" that determines
# which board game's grid, pieces, etc. will be provided.
#
# ------------------------------------------------------------------------------

class BoardGameClient:

    # private member variables -------------------------------------------------

    _game_type = "demo"
    GD = GameDisplay.GameDisplay("demo")

    # methods ------------------------------------------------------------------

    def __init__(self, game_type):
        self._game_type = game_type
        self._gameboard = self._make_game_board(game_type)

    def draw(self):
        pass

    def _make_game_board(self, game_type):
        if game_type == "connectfour":
            return ConnectFourGameBoard.ConnectFourGameBoard()
        else:
            return DemoGameBoard.DemoGameBoard()

    # debugging ----------------------------------------------------------------

    def get_current_game(self):
        return self._game_type

    def report_game_type(self):
        print self._gameboard.get_game_type()

# ------------------------------------------------------------------------------
##if __name__ == 'main':

game = BoardGameClient("demo")
clock = pygame.time.Clock()
#game.connect_to_server()

while not game.GD._should_quit:
    clock.tick(30)
    game.GD.update()
pygame.quit()

print "(end reached)"
