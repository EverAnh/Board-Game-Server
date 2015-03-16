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

    CONNECT_FOUR = "CONNECTFOUR"
    OTHELLO      = "OTHELLO"
    BATTLESHIP   = "BATTLESHIP"

    # methods ------------------------------------------------------------------

    def __init__(self, game_type):
        self._connection = ServerConnection()
        self._account = Account()


    def start(self):
        self._display = MainDisplay()

        while not self._account.is_logged_in():
            session_info = self._display.retrieve_user_info()
            self._account.set_username(session_info[0])
            self._account.set_password(session_info[1])
            game_choice = session_info[2]
            
            response = self._account.log_in(self._connection, game_choice)
            player = int(response[0])
            status = response[1]
            
            if Account.FAIL_KEYWORD not in status.upper():
                self._connection.logged_in = True
                turn = int(self._connection.get_response())
                
            self._display.update(message)
            
        self._game_package = board_game_factory(game_choice)
        self._game_package.game.set_my_player_number(player)
        self._game_package.game.set_turn_number(turn)
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
    
