using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Othello;

namespace GameRules_UnitTest
{
    [TestClass]
    public class GameRules_UnitTest
    {
        // arrange (ang.przygotwanie) - sprawdza rozmiar planszy z argumentami konstruktora oraz czy jest poprawny numer gracza ropoczynającego
        const int boardWidth = 8;
        const int boardHeight = 8;
        const int firstPlayer = 1;
        
        // act (ang.działanie)
        private GameRules createRules()
        {
            return new GameRules(firstPlayer, boardWidth, boardHeight);
        }

        // assert (ang.weryfikacja)
        [TestMethod]
        public void ConstructorTest()
        {
            GameRules gameRules = createRules();

            Assert.AreEqual(boardWidth, gameRules.BoardWidth);
            Assert.AreEqual(boardHeight, gameRules.boardHeight);
            Assert.AreEqual(firstPlayer, gameRules.NextPlayer);
        }

        // ułożenie początkowych kamieni na planszy
        [TestMethod]
        public void NumberFieldTest()
        {
            GameRules gameRules = createRules();

            int intFields = boardWidth * boardHeight; // liczba pól całkowitych
            Assert.AreEqual(intFields - 4, gameRules.EmptyFieldNumber); // czy pozostałych pól jest o 4 mniej niż wszsytkich pól na planszy
            Assert.AreEqual(2, gameRules.PointsPlayer1); // czy obaj gracze mają po 2 kamienie
            Assert.AreEqual(2, gameRules.PointsPlayer2);
        }

        [TestMethod]
        public void TestPobierzStanPola()
        {
            GameRules gameRules = createRules();

            // sprawdzenie pustych pól
            int fieldState = gameRules.DownloadFieldStatus(0, 0); // prawy dolny róg
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth - 1, 0); // lewy dolny róg
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(0, boardHeight - 1); // prawy górny róg
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth - 1, boardHeight - 1); // lewy górny róg
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2 - 1, 0); 
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(0, boardHeight / 2 - 1);
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2 - 1, boardHeight - 1);
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth - 1, boardHeight / 2 - 1);
            Assert.AreEqual(0, fieldState);

            // sprawdzanie pól gracza czarnego (1)
            fieldState = gameRules.DownloadFieldStatus(boardWidth/ 2-1, boardHeight / 2-1); // lewy górny róg miniSzachownicy
            Assert.AreEqual(1, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2, boardHeight / 2); // prawy dolny róg miniSzachownicy
            Assert.AreEqual(1, fieldState);

            // sprawdzanie pól gracza białego (2)
            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2-1, boardHeight / 2); // lewy dolny róg miniSzachownicy
            Assert.AreEqual(2, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2, boardHeight / 2 -1); // prawy górny róg miniSzachownicy
            Assert.AreEqual(2, fieldState);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestPobierzStanPola_PozaPlansza()
        {
            GameRules gameRules = createRules();
            int stanPola = gameRules.DownloadFieldStatus(-1, -1);
            stanPola = gameRules.DownloadFieldStatus(0, -1);
            stanPola = gameRules.DownloadFieldStatus(-1, 0);
            stanPola = gameRules.DownloadFieldStatus(-1, boardHeight-1);
            stanPola = gameRules.DownloadFieldStatus(boardWidth - 1, -1);
        }

    }
}
