using System;

namespace Othello
{
    public class GameRules
    {
        #region Stan Planszy
        // definiujemy planszę i metodę pozwalającą na odczyt stanu jej pól
        public int BoardWidth { get; private set; }
        public int boardHeight { get; private set; }

        // plansza jest deklarowana jako prywatna, a dostęp do niej jest możliwy dzięki publicznej metodzie PobierzStanPola
        private int[,] board;
        // właśność identyfikująca gracza wykonującego następny ruch
        public int NextPlayer { get; private set; } = 1;
        // wyznacza numer gracza, przeciwnego do podanego w argumencie (1 dla 2 && 2 dla 1)
        private static int competitor(int playerNumber) // numer przeciwnika
        {
            return (playerNumber == 1) ? 2 : 1;
        }
        // metoda sprawdzająca poprawność podanej pozycji pola
        private bool isCoordinatesCorrect(int horizontally, int vertically) // czy współrzędne pola są prawidłowe
        {
            return horizontally >= 0 && horizontally < BoardWidth &&
                vertically >= 0 && vertically < boardHeight;
        }
        // metoda pozwalająca na odczytanie stanu planszy 
        public int DownloadFieldStatus(int horizontally, int vertically)
        {
            if (!isCoordinatesCorrect(horizontally, vertically))
                throw new Exception("The invalid coordinate fields."); // Nieprawidlowe wspolrzedne pola
            return board[horizontally, vertically];
            #endregion
        }
        #region Konstruktor Klasy 
        // utworzenie planszy za pomocą metody pomocniczej && ustawienie na niej kamieni
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
        // wyznaczenie gracza wykonującego pierwszy ruch za pomocą konstruktora
        public GameRules(int firstPlayer, int boardWidth=8, int boardHeight=8)
        {
            if (firstPlayer < 1 || firstPlayer > 2)
                throw new Exception("The invalid player number who is starting the game."); // Nieprawidlowy numer gracza rozpoczynajacego gre

            BoardWidth = boardWidth;
            this.boardHeight = boardHeight;
            board = new int[BoardWidth, this.boardHeight];

            createBoard();

            NextPlayer = firstPlayer;
            counter(); // licznik punktów
        }

        #endregion
        #region Implementacja zasad gry
        private void changeCurrentPlayer()
        {
            NextPlayer = competitor(NextPlayer);
        }

        // sprawdzenie czy argumenty metody należą do planszy && pole jest wolne
        protected int PutStone(int horizontally, int vertically, bool test)  // połóż kamień
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
                            if (board[i, j] == competitor(NextPlayer)) foundRivalStone = true; // dopóki przeciwnik kładzie kamień to wykonuje się pętla
                        }

                    } while (!(boardEdge || foundNextPlayerStone || foundEmptyField)); // game over, bye bye

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
                    counter(); // licznik punktów

                } // koniec pętli po kierunkach

            // zmiana gracza, jeżeli ruch został wykonany 
            if (howManyFields > 0 && !test)
                changeCurrentPlayer();
            // zmienna ilePolPrzejętych nie uwzględnia dostawionego kamienia
            return howManyFields;
        }

        // przeciążona wersja metody, sprawdza czy ruch jest możliwy
        public bool PutStone(int poziomo, int pionowo)
        {
            return PutStone(poziomo, pionowo, false) > 0;
        }
        #endregion
        // pola zajęte przez obu graczy, ocena przewagi
        #region Obliczanie pól zajętych przez graczy

        private int[] fieldsNumber = new int[3]; // [puste pola, liczbaPolGracz1, liczbaPolGracz2]

        private void counter()  // licznik punktów
        {
            for (int i = 0; i < fieldsNumber.Length; ++i)
                fieldsNumber[i] = 0;

            for (int i = 0; i < BoardWidth; ++i)
                for (int j = 0; j < boardHeight; ++j)
                    fieldsNumber[board[i, j]]++;
        }
        // 3 właściwości tylko do odczytu 
        public int EmptyFieldNumber {  get { return fieldsNumber[0]; } } 
        public int PointsPlayer1 {  get { return fieldsNumber[1]; } }
        public int PointsPlayer2 {  get { return fieldsNumber[2]; } }
        #endregion
        #region Wykrywanie szczególnych sytuacji w grze

        // metoda sprawdzająca czy gracz może położyć kamień na planszy
        private bool canMakeMove() // czy bieżący gracz może wykonać ruch
        {
            int correctFields = 0;
            for (int i = 0; i < BoardWidth; i++)
                for (int j = 0; j < boardHeight; j++)
                    if (board[i, j] == 0 && PutStone(i, j, true) > 0) // jeżeli pola są puste i możliwe jest położenie kamienia 
                        correctFields++; // zwiększaj liczbaPoprawnychPol
            return correctFields > 0;
        }

        // metoda odpowiedzialna za oddanie ruchu przeciwnikowi, jeśli gracz nie może położyć kamienia na planszy
        public void giveMove()
        {
            if (canMakeMove())
                // Gracz nie może oddać ruchu jeśli wykonanie ruchu jest możliwe
                throw new Exception("A player may not return a move if it is possible to make a move.");
            changeCurrentPlayer();
        }

        // typ wyliczeniowy obejmujący wszystkie możliwe sytuacje w grze 
        public enum Situation
        {
            MoveIsPossible,
            CurrentPlayerCantMove,
            BothPlayersCantMove,
            BusyFields
        }

        // co się dzieje na planszy 
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

        // wyłonienie zwycięzcy lub ustalenie remisu
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
