WINNER = "WINNER"
INVALID = "INVALID"

class GameManager:
  
    def __init__(self, connection, game):
        self._connection = connection
        self._game = game
        self._stored_move = []

    def manage_turn(self):
        if not self._game.is_my_turn():
            self.handle_opponent_move()

    def handle_my_move(self, location):
        if self._game.is_my_turn():
            print 'my turn'
         #   if len(self._stored_move) < self._game.get_move_type():
            self._store_move(location)
         #       return
            self._connection.send_move(self._stored_move)
            self._stored_move = []
            response = self._connection.get_move()

            print 'message(gm): ', response.message
            
            if self.check_for_invalid(response) == True:
                return response
            if response.player_turn != self._game.get_my_player_number():
                self._game.set_my_turn(False)
            self._game.update_board(response.pieces)
            self._game.set_turn_number(response.turn_number)
            self._game.set_player_turn(response.player_turn)
            print 'entire gameboard: ',self._game.get_board()
            self.check_for_winner(response)
            return response
        else:
            print 'not my turn'
            

    def _store_move(self, location):
        self._stored_move.append(location)

    def handle_opponent_move(self):
        response = self._connection.get_move()
        self._game.update_board(response.pieces)
        if response.player_turn == self._game.get_my_player_number():
            self._game.set_my_turn(True)
        self._game.set_turn_number(response.turn_number)
        self._game.set_player_turn(response.player_turn)
        check_for_winner(response)

    def check_for_winner(self, response):
        if response.message == WINNER:
            self._game.set_winner(response.player_turn) ##player_turn == winner in this message
            #self._game.set_is_over(True)

    def check_for_invalid(self, response):
        if response.message == INVALID:
            return True
        else:
            return False
            





        
