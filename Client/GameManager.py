class GameManager(connection: ServerConnection, game: Game):

    NEW_GAME = "NEWGAME"
    RESTART = "RESTART"
    QUIT = "QUIT"

    
    def __init__(self):
        self._connection = connection
        self._game = game

    def manage_turn(self):
        while game.is_my_turn():
            display.update()
        handle_opponent_move()

    def manage_endgame(self):
        
        ##### Some pseudocode below. Please define #####

        #while no endgame_user_response:
            display.update()
        #if endgame_user_response == NEW_GAME:
            connection
