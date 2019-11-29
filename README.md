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
-Logic of game:

Access to the privat game board is provided by the DownloadFieldStatus method.
NextPlayer it is a property that identifies the player making the next move. The isCoordinatesCorrect method verifies the correctness of a given field position. In the GameRules class constructor we specify the player who will make the next move.
The board is created using the createBoard method and also puts stones on the board.
The main PutStone method is responsible for checking if the fields belong to the board and whether the field is free so that the player can lay a stone. Player points are calculated using the Counter method. A private method called canMakeMove checks to see if the player can make a move. If a player cannot make a move, he gives his opponent a move thanks to the giveMove method. Public method Leader chooses the winner of the game or draws.

-Graphic representation:

The game view is implemented with xaml.
Player points are displayed by the boardContent method. The coordinates where the player laid the stone are displayed by the symbolField.
In the MainWindow class constructor, we divided the board into rows and columns and drew grid.

-GameRules_UnitTest:

The public NumberFieldTest method tests the arrangement of the first four stones on the board. The LoadFieldStatusTest method checks if the given fields are set to empty. The last method LoadFieldStatusTest_OusideBoard checks if the stone cannot be located outside the designated board.

