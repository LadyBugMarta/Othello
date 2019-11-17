# :rocket: The Othello game
Othello game is also known as Reversi. It is played on a game board that has got a dimension 8x8. When the game has started the four middle fields are occupied and it has consisted of the small chessboard.

## :dart: Game rules
- The players change the fields of the board, taking over all the opposing fields are between the newly occupied field and other areas of the rival player who is making the move. There must be no empty spaces between them.
- The goal of the game is to score more fields than your opponent.
- The player can only occupy such the field that allows him to pass the entire opposite area. If this area doesn't exist player must give away the movement. To fully understand the rules you should just play :)

## :crown: End of the game
The game ends when all fields are occupied or when none of the players can make a move. The number of gained fields determines victory.

## :white_check_mark: Code Architecture
In this game, the model (game rules) will be separated from the view (window class).
