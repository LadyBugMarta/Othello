using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Othello;

namespace GameRules_UnitTest
{
    [TestClass]
    public class GameRules_UnitTest
    {
        // arrange (ang.przygotwanie) - sprawdza rozmiar planszy z argumentami konstruktora oraz czy jest poprawny numer gracza ropoczynającego
        const int szerokoscPlanszy = 8;
        const int wysokoscPlanszy = 8;
        const int numerGraczaRozpoczynajacego = 1;
        
        // act (ang.działanie)
        private GameRules tworzZasady()
        {
            return new GameRules(numerGraczaRozpoczynajacego, szerokoscPlanszy, wysokoscPlanszy);
        }

        // assert (ang.weryfikacja)
        [TestMethod]
        public void TestKonstruktora()
        {
            GameRules gameRules = tworzZasady();

            Assert.AreEqual(szerokoscPlanszy, gameRules.SzerokoscPlanszy);
            Assert.AreEqual(wysokoscPlanszy, gameRules.WysokoscPlanaszy);
            Assert.AreEqual(numerGraczaRozpoczynajacego, gameRules.NumerGraczaWykonujacegoNastepnyRuch);
        }

        // ułożenie początkowych kamieni na planszy
        [TestMethod]
        public void TestLiczbyPol()
        {
            GameRules gameRules = tworzZasady();

            int calkowitaLiczbaPol = szerokoscPlanszy * wysokoscPlanszy;
            Assert.AreEqual(calkowitaLiczbaPol - 4, gameRules.LiczbaPustychPol); // czy pozostałych pól jest o 4 mniej niż wszsytkich pól na planszy
            Assert.AreEqual(2, gameRules.LiczbaPolGracz1); // czy obaj gracze mają po 2 kamienie
            Assert.AreEqual(2, gameRules.LiczbaPolGracz2);
        }

        [TestMethod]
        public void TestPobierzStanPola()
        {
            GameRules gameRules = tworzZasady();

            // sprawdzenie pustych pól
            int stanPola = gameRules.PobierzStanPola(0, 0); // prawy dolny róg
            Assert.AreEqual(0, stanPola);

            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy - 1, 0); // lewy dolny róg
            Assert.AreEqual(0, stanPola);

            stanPola = gameRules.PobierzStanPola(0, wysokoscPlanszy - 1); // prawy górny róg
            Assert.AreEqual(0, stanPola);

            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy - 1, wysokoscPlanszy - 1); // lewy górny róg
            Assert.AreEqual(0, stanPola);

            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy / 2 - 1, 0); 
            Assert.AreEqual(0, stanPola);

            stanPola = gameRules.PobierzStanPola(0, wysokoscPlanszy / 2 - 1);
            Assert.AreEqual(0, stanPola);

            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy / 2 - 1, wysokoscPlanszy - 1);
            Assert.AreEqual(0, stanPola);

            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy - 1, wysokoscPlanszy / 2 - 1);
            Assert.AreEqual(0, stanPola);

            // sprawdzanie pól gracza czarnego (1)
            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy/ 2-1, wysokoscPlanszy / 2-1); // lewy górny róg miniSzachownicy
            Assert.AreEqual(1, stanPola);

            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy / 2, wysokoscPlanszy / 2); // prawy dolny róg miniSzachownicy
            Assert.AreEqual(1, stanPola);

            // sprawdzanie pól gracza białego (2)
            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy / 2-1, wysokoscPlanszy / 2); // lewy dolny róg miniSzachownicy
            Assert.AreEqual(2, stanPola);

            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy / 2, wysokoscPlanszy / 2 -1); // prawy górny róg miniSzachownicy
            Assert.AreEqual(2, stanPola);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestPobierzStanPola_PozaPlansza()
        {
            GameRules gameRules = tworzZasady();
            int stanPola = gameRules.PobierzStanPola(-1, -1);
            stanPola = gameRules.PobierzStanPola(0, -1);
            stanPola = gameRules.PobierzStanPola(-1, 0);
            stanPola = gameRules.PobierzStanPola(-1, wysokoscPlanszy-1);
            stanPola = gameRules.PobierzStanPola(szerokoscPlanszy - 1, -1);

        }
       

    }
}
