using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Othello;

namespace GameRules_UnitTest
{
    [TestClass]
    public class GameRules_UnitTest
    {
        const int boardWidth = 8;
        const int boardHeight = 8;
        const int firstPlayer = 1;
        
        private GameRules createRules()
        {
            return new GameRules(firstPlayer, boardWidth, boardHeight);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            GameRules gameRules = createRules();

            Assert.AreEqual(boardWidth, gameRules.boardWidth);
            Assert.AreEqual(boardHeight, gameRules.boardHeight);
            Assert.AreEqual(firstPlayer, gameRules.nextPlayer);
        }

        [TestMethod]
        public void NumberFieldTest()
        {
            GameRules gameRules = createRules();

            int intFields = boardWidth * boardHeight; 
            Assert.AreEqual(intFields - 4, gameRules.EmptyFieldNumber);
            Assert.AreEqual(2, gameRules.PointsPlayer1); 
            Assert.AreEqual(2, gameRules.PointsPlayer2);
        }

        [TestMethod]
        public void LoadFieldStatusTest()
        {
            GameRules gameRules = createRules();

            int fieldState = gameRules.DownloadFieldStatus(0, 0); 
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth - 1, 0); 
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(0, boardHeight - 1); 
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth - 1, boardHeight - 1); 
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2 - 1, 0); 
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(0, boardHeight / 2 - 1);
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2 - 1, boardHeight - 1);
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth - 1, boardHeight / 2 - 1);
            Assert.AreEqual(0, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth/ 2-1, boardHeight / 2-1); 
            Assert.AreEqual(1, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2, boardHeight / 2); 
            Assert.AreEqual(1, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2-1, boardHeight / 2); 
            Assert.AreEqual(2, fieldState);

            fieldState = gameRules.DownloadFieldStatus(boardWidth / 2, boardHeight / 2 -1); 
            Assert.AreEqual(2, fieldState);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LoadFieldStatusTest_OusideBoard()
        {
            GameRules gameRules = createRules();
            int fieldState = gameRules.DownloadFieldStatus(-1, -1);
            fieldState = gameRules.DownloadFieldStatus(0, -1);
            fieldState = gameRules.DownloadFieldStatus(-1, 0);
            fieldState = gameRules.DownloadFieldStatus(-1, boardHeight-1);
            fieldState = gameRules.DownloadFieldStatus(boardWidth - 1, -1);
        }

    }
}
