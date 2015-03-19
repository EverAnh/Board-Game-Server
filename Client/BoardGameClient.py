import pygame
import os, sys

import Account
import MainDisplay
import Game
import GameDisplay
import GameManager
import ServerConnection
import ConnectFour, ConnectFourDisplay

# ------------------------------------------------------------------------------
#
# This file is the main entry point of the client-side program, and it sets up
# a single instance of the class BoardGameClient to handle the rest of the
# program flow.
#
# ------------------------------------------------------------------------------

CONNECT_FOUR = "connectFour"
OTHELLO      = "othello"
SNAKESLADDERS = "snakesLadders"

class BoardGameClient:
    
    def __init__(self):
        self._connection = ServerConnection.ServerConnection()
        self._account = Account.Account()


    def start(self):
        self._display = MainDisplay.MainDisplay()
        self._message = "Howdy"
        self._connection.open_connection()
        
        while not self._account.is_logged_in():
            try:
                session_info = self._display.retrieve_user_info(self._message)
                self._account.set_username(session_info[0])
                self._account.set_password(session_info[1])
                game_choice = session_info[2]
                
                
                #################### DEBUG ##########################
                ##game_choice = ("generic")
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

        self._game, self._display = self._board_game_factory(game_choice)
        self._game.set_my_player_number(player_num)
        self._game.set_my_player_id(player_id)

        ################### DEBUG ######################
        #Until message 5 is implemented in standard way#
        #self._game.set_my_turn(True)
        ################################################
        
        self._run_game()

  
    def _board_game_factory(self, choice):
        if choice == CONNECT_FOUR:
            game = ConnectFour.ConnectFour()
            self._game_manager = GameManager.GameManager(self._connection, game)
            display = ConnectFourDisplay.ConnectFourDisplay(self._game_manager, game)
            
        elif choice == OTHELLO:
            game = Othello.Othello()
            self._game_manager = GameManager.GameManager(self._connection, game)
            display = OthelloDisplay.OthelloDisplay(self._game_manager, game)
            
        elif choice == SNAKESLADDERS:
            game = SnakesLadders.SnakesLadders()
            self._game_manager = GameManager.GameManager(self._connection, game)
            display = SnakesLaddersDisplay.SnakesLaddersDisplay(self._game_manager, game)

        ############### DEBUG ######################
        else:
            game = Game.Game()
            self._game_manager = GameManager.GameManager(self._connection, game)
            display = GameDisplay.GameDisplay(self._game_manager, game)
        ############################################
            
        return (game, display)

    def _run_game(self):
        print 'running game'
        while not self._game.is_over():
            print 'updating display'
            self._display.update()
            print 'managing turn'
            self._game_manager.manage_turn()
        print 'Game over! Logging out...'
        self._account.logout(self._connection)

# ------------------------------------------------------------------------------
#if __name__ == 'main':
bgc = BoardGameClient()
print 'starting client'
bgc.start()
    
