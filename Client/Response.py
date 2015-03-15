import GamePiece


class Response:

    def __init__(self, turn_number, player_turn, scores, message, pieces):
        self.turn_number = turn_number
        self.player_turn = player_turn
        self.message = message
        self.scores = scores
        self.pieces = pieces
