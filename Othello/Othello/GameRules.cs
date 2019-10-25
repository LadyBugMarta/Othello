using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    public class GameRules
    {
        #region Stan Planszy
        // definiujemy planszę i metodę pozwalającą na odczyt stanu jej pól
        public int SzerokoscPlanszy { get; private set; }
        public int WysokoscPlanaszy { get; private set; }

        // plansza jest deklarowana jako prywatna, a dostęp do niej jest możliwy dzięki publicznej metodzie PobierzStanPola
        private int[,] plansza;
        // właśność identyfikująca gracza wykonującego następny ruch
        public int NumerGraczaWykonujacegoNastepnyRuch { get; private set; } = 1;
        // wyznacza numer gracza, przeciwnego do podanego w argumencie (1 dla 2 && 2 dla 1)
        private static int numerPrzeciwnika(int numerGracza)
        {
            return (numerGracza == 1) ? 2 : 1;
        }
        // metoda sprawdzająca poprawność podanej pozycji pola
        private bool czyWspolrzednePolaPrawidlowe(int poziomo, int pionowo)
        {
            return poziomo >= 0 && poziomo < SzerokoscPlanszy &&
                pionowo >= 0 && pionowo < WysokoscPlanaszy;
        }
        // pozwala na odczytanie stanu planszy 
        private int PobierzStanPola(int poziomo, int pionowo)
        {
            if (!czyWspolrzednePolaPrawidlowe(poziomo, pionowo))
                throw new Exception("Nieprawidlowe wspolrzedne pola");
            return plansza[poziomo, pionowo];
            #endregion
        }
        #region Konstruktor Klasy 
        // utworzenie planszy za pomocą metody pomocniczej && ustawienie na niej kamieni
        private void czyscPlansze()
        {
            for (int i = 0; i < SzerokoscPlanszy; i++)
                for (int j = 0; j < WysokoscPlanaszy; j++)
                    plansza[i, j] = 0;

            int srodekSzerokosci = SzerokoscPlanszy / 2;
            int srodekWysokosci = WysokoscPlanaszy / 2;

            plansza[srodekSzerokosci - 1, srodekWysokosci - 1] = plansza[srodekSzerokosci, srodekWysokosci] = 1;
            plansza[srodekSzerokosci - 1, srodekWysokosci] = plansza[srodekSzerokosci, srodekWysokosci - 1] = 2;
        }
        // wyznaczenie gracza wykonującego pierwszy ruch za pomocą konstruktora
        public GameRules(int numerGraczaRozpoczynajacego, int szerokoscPlanszy=8, int wysokoscPlanszy=8)
        {
            if (numerGraczaRozpoczynajacego < 1 || numerGraczaRozpoczynajacego > 2)
                throw new Exception("Nieprawidlowy numer gracza rozpoczynajacego gre");

            SzerokoscPlanszy = szerokoscPlanszy;
            WysokoscPlanaszy = wysokoscPlanszy;
            plansza = new int[SzerokoscPlanszy, WysokoscPlanaszy];

            czyscPlansze();

            NumerGraczaWykonujacegoNastepnyRuch = numerGraczaRozpoczynajacego;
        }
        #endregion
    }
}
