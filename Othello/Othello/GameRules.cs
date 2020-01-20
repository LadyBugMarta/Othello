using System;

namespace Othello
{
    public class GameRules
    {
        #region Board status
        
        /// <summary>
        /// Definiujemy szerokość planszy i metodę pozwalającą na odczyt stanu jej pól.
        /// </summary>
        public int boardWidth { get; private set; }
        /// <summary>
        /// Definiujemy wysokość planszy i metodę pozwalającą na odczyt stanu jej pól.
        /// </summary>
        public int boardHeight { get; private set; }


        /// <summary>
        /// Plansza jest deklarowana jako prywatna, a dostęp do niej jest możliwy dzięki publicznej metodzie DownloadFieldStatus.
        /// </summary>
        private int[,] board;
        /// <summary>
        /// Właśność identyfikująca gracza wykonującego następny ruch.
        /// </summary>
        public int nextPlayer { get; private set; } = 1;

        /// <summary>
        /// Wyznacza numer gracza, przeciwnego do podanego w argumencie 
        /// </summary>
        /// <param name="playerNumber"> Numer przeciwnika.</param>
        /// <returns></returns>
        private static int Competitor(int playerNumber)
        {
            return (playerNumber == 1) ? 2 : 1;
        }

        /// <summary>
        /// Metoda sprawdzająca poprawność podanej pozycji pola.
        /// </summary>
        /// <param name="horizontally"> Współrzędne poziome.</param>
        /// <param name="vertically"> Współrzędne pionowe.</param>
        /// <returns></returns>
        private bool isCoordinatesCorrect(int horizontally, int vertically) 
        {
            return horizontally >= 0 && horizontally < boardWidth &&
                vertically >= 0 && vertically < boardHeight;
        }

        /// <summary>
        /// Metoda pozwalająca na odczytanie stanu planszy.
        /// </summary>
        /// <param name="horizontally"> Współrzędne poziome.</param>
        /// <param name="vertically"> Współrzędne pionowe.</param>
        /// <returns></returns>
        public int DownloadFieldStatus(int horizontally, int vertically)
        {
            if (!isCoordinatesCorrect(horizontally, vertically))
                throw new Exception("The invalid coordinate fields."); 
            return board[horizontally, vertically];
        }
        #endregion

        #region Class Constructor
        /// <summary>
        /// Utworzenie planszy za pomocą metody pomocniczej && ustawienie na niej kamieni.
        /// </summary>
        private void createBoard()
        {
            for (int i = 0; i < boardWidth; i++)
                for (int j = 0; j < boardHeight; j++)
                    board[i, j] = 0;

            int middleWidth = boardWidth / 2; 
            int middleHeight = boardHeight / 2; 

            board[middleWidth - 1, middleHeight - 1] = board[middleWidth, middleHeight] = 1;
            board[middleWidth - 1, middleHeight] = board[middleWidth, middleHeight - 1] = 2;
        }
        /// <summary>
        /// Wyznaczenie gracza wykonującego pierwszy ruch za pomocą konstruktora.
        /// </summary>
        /// <param name="firstPlayer"> Gracz wykonujący ruch jako pierwszy.</param>
        /// <param name="boardWidth"> Szerokość planszy.</param>
        /// <param name="boardHeight"> Wysokość planszy.</param>
        public GameRules(int firstPlayer, int boardWidth=8, int boardHeight=8)
        {
            if (firstPlayer < 1 || firstPlayer > 2)
                throw new Exception("The invalid player number who is starting the game."); 

            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            board = new int[this.boardWidth, this.boardHeight];

            createBoard();

            nextPlayer = firstPlayer;
            Counter(); 
        }

        #endregion

        #region Implementation of the Game Rules
        private void changeCurrentPlayer()
        {
            nextPlayer = Competitor(nextPlayer);
        }

        /// <summary>
        /// Metoda sprawdza czy argumenty metody należą do planszy && pole jest wolne.
        /// </summary>
        /// <param name="horizontally"> Współrzędne pionowe.</param>
        /// <param name="vertically"> Współrzędne poziome.</param>
        protected int PutStone(int horizontally, int vertically, bool test)  
        {
            if (!isCoordinatesCorrect(horizontally, vertically))
                throw new Exception("The invalid field coordinates."); 

            if (board[horizontally, vertically] != 0) return -1;

            int howManyFields = 0;

            for (int horizontallyDirection = -1; horizontallyDirection <= 1; horizontallyDirection++) 
                for (int verticallyDirection = -1; verticallyDirection <= 1; verticallyDirection++)
                {
                    if (horizontallyDirection == 0 && verticallyDirection == 0) continue;

                    int i = horizontally;
                    int j = vertically;
                    bool foundRivalStone = false;
                    bool foundNextPlayerStone = false;
                    bool foundEmptyField = false;
                    bool boardEdge = false; 
                    do 
                    {
                        i += horizontallyDirection;
                        j += verticallyDirection;
                        if (!isCoordinatesCorrect(i, j)) boardEdge = true;
                        if (!boardEdge)
                        {
                            if (board[i, j] == nextPlayer) foundNextPlayerStone = true; 
                            if (board[i, j] == 0) foundEmptyField = true;
                            if (board[i, j] == Competitor(nextPlayer)) foundRivalStone = true;
                        }

                    } while (!(boardEdge || foundNextPlayerStone || foundEmptyField));

                    bool stonePlacementIsPossible = foundRivalStone && foundNextPlayerStone && !foundEmptyField;
                 
                   
                    if (stonePlacementIsPossible)
                    {
                        int max_index = Math.Max(Math.Abs(i - horizontally), Math.Abs(j - vertically));

                        if (!test) 
                        {
                            for (int index = 0; index < max_index; index++)
                                board[horizontally + index * horizontallyDirection, vertically + index * verticallyDirection] = nextPlayer;
                        }
                        howManyFields += max_index - 1; 
                    }
                    Counter(); 

                } 

            if (howManyFields > 0 && !test)
                changeCurrentPlayer();
            return howManyFields;
        }

        /// <summary>
        /// Przeciążona wersja metody PutStone, sprawdza czy ruch jest możliwy.
        /// </summary>
        /// <param name="horizontally"> Współrzędne poziomo.</param>
        /// <param name="vertically"> Współrzędne pionowo.</param>
        /// <returns></returns>
        public bool PutStone(int horizontally, int vertically)
        {
            return PutStone(horizontally, vertically, false) > 0;
        }
        #endregion
        
        #region Calculation of fields occupied by players
        /// <summary>
        /// Tablica zawierająca [puste pola, liczbaPolGracz1, liczbaPolGracz2].
        /// </summary>
        private int[] fieldsNumber = new int[3];
        /// <summary>
        /// Metoda licząca punkty obydwóch graczy.
        /// </summary>
        private void Counter() 
        {
            for (int i = 0; i < fieldsNumber.Length; ++i)
                fieldsNumber[i] = 0;

            for (int i = 0; i < boardWidth; ++i)
                for (int j = 0; j < boardHeight; ++j)
                    fieldsNumber[board[i, j]]++;
        }
        
        /// <summary>
        /// Właściwość odczytująca puste pola.
        /// </summary>
        public int EmptyFieldNumber {  get { return fieldsNumber[0]; } } 
        /// <summary>
        /// Właściwość odczytująca pola zajęte przez gracza 1.
        /// </summary>
        public int PointsPlayer1 {  get { return fieldsNumber[1]; } }
        /// <summary>
        /// Właściwość odczytująca pola zajęte przez gracza 2.
        /// </summary>
        public int PointsPlayer2 {  get { return fieldsNumber[2]; } }
        #endregion

        #region Detection of specific game situations

        /// <summary>
        /// Metoda sprawdzająca czy bieżacy gracz może położyć kamień na planszy.
        /// </summary>
        /// <returns></returns>
        private bool canMakeMove() 
        {
            int correctFields = 0;
            for (int i = 0; i < boardWidth; i++)
                for (int j = 0; j < boardHeight; j++)
                    if (board[i, j] == 0 && PutStone(i, j, true) > 0)
                        correctFields++; 
            return correctFields > 0;
        }

        /// <summary>
        /// Metoda odpowiedzialna za oddanie ruchu przeciwnikowi, jeśli gracz nie może położyć kamienia na planszy.
        /// </summary>
        public void giveMove()
        {
            if (canMakeMove())
                throw new Exception("A player may not return a move if it is possible to make a move.");
            changeCurrentPlayer();
        }

        /// <summary>
        /// Typ wyliczeniowy obejmujący wszystkie możliwe sytuacje w grze.
        /// </summary>
        public enum Situation
        {
            MoveIsPossible,
            CurrentPlayerCantMove,
            BothPlayersCantMove,
            BusyFields
        }

        /// <summary>
        /// Metoda sprawdzająca co się dzieje na planszy.
        /// </summary>
        /// <returns></returns>
        public Situation CheckSituation()
        {
            if (EmptyFieldNumber == 0) return Situation.BusyFields;

            bool isMovePossible = canMakeMove();
            if (isMovePossible) return Situation.MoveIsPossible;
            else
            {
                changeCurrentPlayer();
                bool isPossibleRivalMove = canMakeMove();
                changeCurrentPlayer();
                if (isPossibleRivalMove)
                    return Situation.CurrentPlayerCantMove;
                else return Situation.BothPlayersCantMove;
            }
        }

        /// <summary>
        /// Wyłonienie zwycięzcy lub ustalenie remisu.
        /// </summary>
        public int Lider
        {
            get
            { 
                if (PointsPlayer1 == PointsPlayer2) return 0; 
                else return (PointsPlayer1 > PointsPlayer2) ? 1 : 2; 
            }
        }
        #endregion
    }
}
