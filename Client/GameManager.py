class GameManager:

    NEW_GAME = "NEWGAME"
    RESTART = "RESTART"
    QUIT = "QUIT"

    
    def __init__(self, connection, game_package):
        self._connection = connection
        self._game = game_package.game
        self._display = game_package.display
        self._stored_move = []

    def manage_turn(self):
        print 'managing turn'
        while self._game.is_my_turn():
            self._display.update()
        handle_opponent_move()

    def manage_endgame(self):
        self._display.end_game()

    def handle_my_move(self, location):
        if self._game.is_my_turn():
            if len(stored_move) < self._game.move_type:
                store_move(location)
                return
            self._connection.send_move(stored_move)
            response = self._connection.get_move()
            if response.player_number != self._game.get_my_player_number():
                self._game.set_my_turn(False)
            self._game.update_board(response.pieces)
            self._game.set_turn_number(response.turn_number)
            return response

    def handle_opponent_move(self):
        response = self._connection.get_move()
        self._game.update_board(response.pieces)
        if response.player_turn == self._game.get_my_player_number():
            self._game.set_my_turn(True)
        self._game.set_turn_number(response.turn_number)
        check_for_winner(response)

    def check_for_winner(self, response):
        if response.message == ServerConnection.WINNER:
            self._game.set_winner(response.player_turn) ##player_turn == winner in this message
            self._game.set_is_over(True)





        
