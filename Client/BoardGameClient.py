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
        self._display = MainDisplay.MainDisplay()
        self._message = "Howdy"
        
        while not self._account.is_logged_in():
            try:
                session_info = self._display.retrieve_user_info(self._message)
                self._account.set_username(session_info[0])
                self._account.set_password(session_info[1])
                game_choice = session_info[2]
                
                
                #################### DEBUG ##########################
                game_choice = ("generic")
                print 'user info entered'
                #####################################################
                
                
                response = self._account.login(self._connection, game_choice)
                player_id = int(response[0])
                status = response[1]
                print 'bgc received messages 1 and 3'
                print 'player_id: ',player_id
                print 'status: ',status
                
                if Account.FAIL_KEYWORD not in status.upper():
                    self._account.set_logged_in(True)
                    player_num = int(self._connection.get_response())
                    print 'bgc received message 4'
                    print 'player_num: ',player_num
                else: 
                    self._message = status
            except Exception as e:
                print e
                sys.exit()
          
        self._game_package = self._board_game_factory(game_choice)
        self._game_package.game.set_my_player_number(player_num)
        self._game_package.game.set_my_player_id(player_id)

        ################### DEBUG ######################
        #Until message 5 is implemented in standard way#
        self._game_package.game.set_my_turn(True)
        ################################################
        
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
    
