import pygame
import os, sys

import Account
import Display
import BoardGamePackage
import GameManager
import ServerConnection

# ------------------------------------------------------------------------------
#
# This file is the main entry point of the client-side program, and it sets up
# a single instance of the class BoardGameClient to handle the rest of the
# program flow.
#
# ------------------------------------------------------------------------------

class BoardGameClient:
    
    # class variables -------------------------------------------------

    # messages received from server indicating whether a login or
    # create account attempt was successful
    CREATE_SUCC  = "CREATESUCC"
    LOGIN_FAIL   = "LOGINFAIL"
    CREATE_FAIL  = "CREATEFAIL"

    # available games
    CONNECT_FOUR = "CONNECTFOUR"
    OTHELLO      = "OTHELLO"
    BATTLESHIP   = "BATTLESHIP"


    # methods ------------------------------------------------------------------

    def __init__(self, game_type):
        self._connection = ServerConnection()
        self._account = Account()


    def start(self):
        self._display = MainDisplay(account)
        
        ###### Some pseudocode below. Please Define #######
        
        #run main display to receive game_choice, username, and password.
        while not account.logged_in:
            # if login, and success:
                    self._connection.logged_in = True
                    temp_player_number = # number received from server
            # elif create, and success:
                    message = CREATE_SUCC
                    # clear maindisplay
            # elif login, and fail:
                    message = LOGIN_FAIL
            # elif create, and fail:
                    message = CREATE_FAIL
            self._display.update(message)
            
        self._game_package = board_game_factory(game_choice)
        self._game_package.game.set_my_player_number(temp_player_number)
        self._game_manager = GameManager(self._connection, self._game_package.game)
        run_game()

  
    def _board_game_factory(self, choice):
        if choice == CONNECT_FOUR:
            game = ConnectFourGame()
            display = ConnectFourGameDisplay(manager, game)
            
        elif choice == OTHELLO:
            game = OthelloFourGame()
            display = OthelloGameDisplay(manager, game)
            
        elif choice == BATTLESHIP:
            game = BattleshipFourGame()
            display = BattleshipGameDisplay(manager, game)
            
        return GamePackage(game, display)


    def _run_game(self):
        while not self._game_package.game.is_over():
            self._game_manager.manage_turn()
            
        self._game_manager.manage_endgame()
        

# ------------------------------------------------------------------------------
if __name__ == 'main':
    bgc = BoardGameClient()
    bgc.start()
    
