import pygame
import os, sys

import Account
import MainDisplay
import Game
import GameDisplay
import GamePackage
import GameManager
import ServerConnection

# ------------------------------------------------------------------------------
#
# This file is the main entry point of the client-side program, and it sets up
# a single instance of the class BoardGameClient to handle the rest of the
# program flow.
#
# ------------------------------------------------------------------------------

CONNECT_FOUR = "CONNECTFOUR"
OTHELLO      = "OTHELLO"
BATTLESHIP   = "BATTLESHIP"

class BoardGameClient:
    
    def __init__(self):
        self._connection = ServerConnection.ServerConnection()
        self._account = Account.Account()


    def start(self):
        ##UNCOMMENT##self._display = MainDisplay.MainDisplay()

        while not self._account.is_logged_in():
            try:
                ##UNCOMMENT##
                '''
                session_info = self._display.retrieve_user_info()
                self._account.set_username(session_info[0])
                self._account.set_password(session_info[1])
                game_choice = session_info[2]
                '''
                #################### DEBUG ##########################
                ### hardcoded username, password, and game_choice ###
                self._account.set_username("Lucas")
                self._account.set_password("thecat")
                game_choice = ("generic")
                print 'user info entered'
                #####################################################
                
                response = self._account.login(self._connection, game_choice)
                player = int(response[0])
                status = response[1]
                print 'login response received'
                
                if Account.FAIL_KEYWORD not in status.upper():
                    self._account.set_logged_in(True)
                    turn = int(self._connection.get_response())
                    
                ##UNCOMMENT##self._display.update(message)
            except:
                print 'failed to start game!'
                sys.exit(1)
            
        self._game_package = self._board_game_factory(game_choice)
        self._game_package.game.set_my_player_number(player)
        self._game_package.game.set_turn_number(turn)

        ################### DEBUG ####################
        self._game_package.game.set_my_turn(True)
        ##############################################
        
        self._game_manager = GameManager.GameManager(self._connection, self._game_package)
        self._run_game()

  
    def _board_game_factory(self, choice):
        if choice == CONNECT_FOUR:
            game = ConnectFourGame.ConnectFourGame()
            display = ConnectFourGameDisplay.ConnectFourGameDisplay(manager, game)
            
        elif choice == OTHELLO:
            game = OthelloGame.OthelloGame()
            display = OthelloGameDisplay.OthelloGameDisplay(manager, game)
            
        elif choice == BATTLESHIP:
            game = BattleShipGame.BattleshipFourGame()
            display = BattleshipGameDisplay.BattleshipGameDisplay(manager, game)

        ############### DEBUG ######################
        else:
            game = Game.Game()
            display = GameDisplay.GameDisplay()
        ############################################
            
        return GamePackage.GamePackage(game, display)

    def _run_game(self):
        print 'running game'
        while not self._game_package.game.is_over():
            self._game_manager.manage_turn()
            
        self._game_manager.manage_endgame()

# ------------------------------------------------------------------------------
#if __name__ == 'main':
bgc = BoardGameClient()
print 'starting client'
bgc.start()
    
