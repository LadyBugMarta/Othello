using System;

namespace Othello
{
    public class GameRules
    {
        #region Board status
        
        /// <summary>
        /// Definiujemy szerokość planszy i metodę pozwalającą na odczyt stanu jej pól.
        /// </summary>
        public int BoardWidth { get; private set; }
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
        public int NextPlayer { get; private set; } = 1;

        /// <summary>
        /// Wyznacza numer gracza, przeciwnego do podanego w argumencie (1 dla 2 && 2 dla 1).
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
        /// <param name="horizontally"> Współrzędnę horyzontalne.</param>
        /// <param name="vertically"> Współrzędne wertykalne.</param>
        /// <returns></returns>
        private bool isCoordinatesCorrect(int horizontally, int vertically) 
        {
            return horizontally >= 0 && horizontally < BoardWidth &&
                vertically >= 0 && vertically < boardHeight;
        }

        /// <summary>
        /// Metoda pozwalająca na odczytanie stanu planszy.
        /// </summary>
        /// <param name="horizontally"> Współrzędne horyzontalne.</param>
        /// <param name="vertically"> Współrzędne wertykalne.</param>
        /// <returns></returns>
        public int DownloadFieldStatus(int horizontally, int vertically)
        {
            if (!isCoordinatesCorrect(horizontally, vertically))
                throw new Exception("The invalid coordinate fields."); // Nieprawidlowe wspolrzedne pola
            return board[horizontally, vertically];
        }
        #endregion

        #region Class Constructor
        /// <summary>
        /// Utworzenie planszy za pomocą metody pomocniczej && ustawienie na niej kamieni.
        /// </summary>
        private void createBoard()
        {
            for (int i = 0; i < BoardWidth; i++)
                for (int j = 0; j < boardHeight; j++)
                    board[i, j] = 0;

            int middleWidth = BoardWidth / 2; // środek szerokości
            int middleHeight = boardHeight / 2; // środek wysokości

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
                throw new Exception("The invalid player number who is starting the game."); // Nieprawidlowy numer gracza rozpoczynajacego gre

            BoardWidth = boardWidth;
            this.boardHeight = boardHeight;
            board = new int[BoardWidth, this.boardHeight];

            createBoard();

            NextPlayer = firstPlayer;
            Counter(); // licznik punktów
        }

        #endregion

        #region Implementation of the Game Rules
        private void changeCurrentPlayer()
        {
            NextPlayer = Competitor(NextPlayer);
        }

        /// <summary>
        /// Metoda sprawdza czy argumenty metody należą do planszy && pole jest wolne.
        /// </summary>
        /// <param name="horizontally"> Współrzędne horyzontalne.</param>
        /// <param name="vertically"> Współrzędne wertykalne.</param>
        /// <param name="test"></param>
        /// <returns></returns>
        protected int PutStone(int horizontally, int vertically, bool test)  
        {
            // czy wsplółrzędne są prawidłowe
            if (!isCoordinatesCorrect(horizontally, vertically))
                throw new Exception("The invalid field coordinates."); // Nieprawidłowe współrzędne pola

            // czy pole nie jest już zajęte
            if (board[horizontally, vertically] != 0) return -1;

            int howManyFields = 0; // ile pól przejętych

            //pętla po 8 kierunkach przyjmuje wartości {-1,0,1}
            for (int horizontallyDirection = -1; horizontallyDirection <= 1; horizontallyDirection++) // sprawdzamy czy możemy przejąć pole w każdą stronę
                for (int verticallyDirection = -1; verticallyDirection <= 1; verticallyDirection++)
                {
                    // wymuszenie pominięcia przypadku, gdy obie zmienne są równe 0
                    if (horizontallyDirection == 0 && verticallyDirection == 0) continue;

                    // szukanie kamieni gracza w jednym z 8 kierunków
                    int i = horizontally;
                    int j = vertically;
                    bool foundRivalStone = false;
                    bool foundNextPlayerStone = false;
                    bool foundEmptyField = false;
                    bool boardEdge = false; // osiagnieta krawędź planszy
                    do // przejmowanie pól
                    {
                        i += horizontallyDirection;
                        j += verticallyDirection;
                        if (!isCoordinatesCorrect(i, j)) boardEdge = true;
                        if (!boardEdge)
                        {
                            if (board[i, j] == NextPlayer) foundNextPlayerStone = true; // dopóki kładziemy kamień to wykonuje się pętla
                            if (board[i, j] == 0) foundEmptyField = true; // dopóki znajdziemy puste pole to wykonuje się pętla
                            if (board[i, j] == Competitor(NextPlayer)) foundRivalStone = true; // dopóki przeciwnik kładzie kamień to wykonuje się pętla
                        }

                    } while (!(boardEdge || foundNextPlayerStone || foundEmptyField)); // game over

                    // sprawdzenie warunku poprawności ruchu
                    bool stonePlacementIsPossible = foundRivalStone && foundNextPlayerStone && !foundEmptyField;
                 
                    // "odwrócenie" kamieni w przypadku spełnionego warunku
                    if (stonePlacementIsPossible)
                    {
                        int max_index = Math.Max(Math.Abs(i - horizontally), Math.Abs(j - vertically)); // przejmujemy jak najwięcej pól

                        if (!test) // ustawienie wartości zmienna true
                        {
                            for (int index = 0; index < max_index; index++)
                                board[horizontally + index * horizontallyDirection, vertically + index * verticallyDirection] = NextPlayer;
                        }
                        howManyFields += max_index - 1; // bez tego kamienia który kładzie 
                    }
                    Counter(); // licznik punktów

                } // koniec pętli po kierunkach

            // zmiana gracza, jeżeli ruch został wykonany 
            if (howManyFields > 0 && !test)
                changeCurrentPlayer();
            // zmienna ilePolPrzejętych nie uwzględnia dostawionego kamienia
            return howManyFields;
        }

        /// <summary>
        /// Przeciążona wersja metody PutStone, sprawdza czy ruch jest możliwy.
        /// </summary>
        /// <param name="poziomo"> Współrzędne poziomo.</param>
        /// <param name="pionowo"> Współrzędne pionowo.</param>
        /// <returns></returns>
        public bool PutStone(int poziomo, int pionowo)
        {
            return PutStone(poziomo, pionowo, false) > 0;
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

            for (int i = 0; i < BoardWidth; ++i)
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
            for (int i = 0; i < BoardWidth; i++)
                for (int j = 0; j < boardHeight; j++)
                    if (board[i, j] == 0 && PutStone(i, j, true) > 0) // jeżeli pola są puste i możliwe jest położenie kamienia 
                        correctFields++; // zwiększaj liczbaPoprawnychPol
            return correctFields > 0;
        }

        /// <summary>
        /// Metoda odpowiedzialna za oddanie ruchu przeciwnikowi, jeśli gracz nie może położyć kamienia na planszy.
        /// </summary>
        public void giveMove()
        {
            if (canMakeMove())
                // Gracz nie może oddać ruchu jeśli wykonanie ruchu jest możliwe
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
            if (EmptyFieldNumber == 0) return Situation.BusyFields; // brak pustych pól

            // wykrycie możliwości ruchu gracza
            bool isMovePossible = canMakeMove();
            if (isMovePossible) return Situation.MoveIsPossible;
            else
            {
                // wykrycie możliwości ruchu przeciwnika
                changeCurrentPlayer();
                bool isPossibleRivalMove = canMakeMove();
                changeCurrentPlayer();
                if (isPossibleRivalMove)
                    return Situation.CurrentPlayerCantMove;
                else return Situation.BothPlayersCantMove; // może wystąpić tylko na wielkiej planszy 
            }
        }

        /// <summary>
        /// Wyłonienie zwycięzcy lub ustalenie remisu.
        /// </summary>
        public int Lider
        {
            // tylko do odczytu
            get
            { 
                if (PointsPlayer1 == PointsPlayer2) return 0; // w przypadku remisu własność zwraca 0
                else return (PointsPlayer1 > PointsPlayer2) ? 1 : 2; // ustalenie zwycięzcy
            }
        }
        #endregion
    }
}
