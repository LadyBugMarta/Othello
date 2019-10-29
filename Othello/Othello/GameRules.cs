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
            obliczLiczbyPol(); // licznik
        }

        #endregion
        #region Implementacja zasad gry
        private void zmieńBiezacegoGracza()
        {
            NumerGraczaWykonujacegoNastepnyRuch = numerPrzeciwnika(NumerGraczaWykonujacegoNastepnyRuch);
        }

        // sprawdzenie czy argumenty metody należą do planszy && pole jest wolne
        protected int PolozKamien(int poziomo, int pionowo, bool tylkoTest) 
        {
            // czy wsplółrzędne są prawidłowe
            if (!czyWspolrzednePolaPrawidlowe(poziomo, pionowo))
                throw new Exception("Nieprawidłowe współrzędne pola");

            // czy pole nie jest już zajęte
            if (plansza[poziomo, pionowo] != 0) return -1;

            int ilePolPrzejetych = 0;

            //pętla po 8 kierunkach przyjmuje wartości {-1,0,1}
            for (int kierunekPoziomo = -1; kierunekPoziomo <= 1; kierunekPoziomo++) // sprawdzamy czy możemy przejąć pole w każdą stronę
                for (int kierunekPionowo = -1; kierunekPionowo <= 1; kierunekPionowo++)
                {
                    // wymuszenie pominięcia przypadku, gdy obie zmienne są równe 0
                    if (kierunekPoziomo == 0 && kierunekPionowo == 0) continue;

                    // szukanie kamieni gracza w jednym z 8 kierunków
                    int i = poziomo;
                    int j = pionowo;
                    bool znalezionyKamienPrzeciwnika = false;
                    bool znalezionyKamienGraczaWykonujacegoRuch = false;
                    bool znalezionePustePole = false;
                    bool osiagnietaKrawedzPlanszy = false;
                    do // przejmowanie pól
                    {
                        i += kierunekPoziomo;
                        j += kierunekPionowo;
                        if (!czyWspolrzednePolaPrawidlowe(i, j)) osiagnietaKrawedzPlanszy = true;
                        if (!osiagnietaKrawedzPlanszy)
                        {
                            if (plansza[i, j] == NumerGraczaWykonujacegoNastepnyRuch) znalezionyKamienGraczaWykonujacegoRuch = true; // dopóki kładziemy kamień to wykonuje się pętla
                            if (plansza[i, j] == 0) znalezionePustePole = true; // dopóki znajdziemy puste pole to wykonuje się pętla
                            if (plansza[i, j] == numerPrzeciwnika(NumerGraczaWykonujacegoNastepnyRuch)) znalezionyKamienPrzeciwnika = true; // dopóki przeciwnik kładzie kamień to wykonuje się pętla
                        }

                    } while (!(osiagnietaKrawedzPlanszy || znalezionyKamienGraczaWykonujacegoRuch || znalezionePustePole)); // game over, bye bye

                    // sprawdzenie warunku poprawności ruchu
                    bool polozenieKamieniaJestMozliwe = znalezionyKamienPrzeciwnika && znalezionyKamienGraczaWykonujacegoRuch && !znalezionePustePole;
                 
                    // "odwrócenie" kamieni w przypadku spełnionego warunku
                    if (polozenieKamieniaJestMozliwe)
                    {
                        int max_index = Math.Max(Math.Abs(i - poziomo), Math.Abs(j - pionowo)); // przejmujemy jak najwięcej pól

                        if (!tylkoTest)
                        {
                            for (int index = 0; index < max_index; index++)
                                plansza[poziomo + index * kierunekPoziomo, pionowo + index * kierunekPionowo] = NumerGraczaWykonujacegoNastepnyRuch;
                        }
                        ilePolPrzejetych += max_index - 1; // bez tego kamienia który kładzie 
                    }
                    obliczLiczbyPol(); // licznik

                } // koniec pętli po kierunkach

            // zmiana gracza, jeżeli ruch został wykonany 
            if (ilePolPrzejetych > 0 && !tylkoTest)
                zmieńBiezacegoGracza();
            // zmienna ilePolPrzejętych nie uwzględnia dostawionego kamienia
            return ilePolPrzejetych;
            
        }

        // przeciążona wersja metody, sprawdza czy ruch jest możliwy
        public bool PolozKamien(int poziomo, int pionowo)
        {
            return PolozKamien(poziomo, pionowo, false) > 0;
        }
        #endregion
        // pola zajęte przez obu graczy
        #region Obliczanie pól zajętych przez graczy

        private int[] liczbyPol = new int[3]; // [puste pola], [liczbaPolGracz1], [liczbaPolGracz2]

        private void obliczLiczbyPol()  
        {
            for (int i = 0; i < liczbyPol.Length; ++i)
                liczbyPol[i] = 0;

            for (int i = 0; i < SzerokoscPlanszy; i++)
                for (int j = 0; j < WysokoscPlanaszy; ++j)
                    liczbyPol[plansza[i, j]]++;
        }
        public int LiczbaPustychPol {  get { return liczbyPol[0]; } } // get - tylko do odczytu
        public int LiczbaPolGracz1 {  get { return liczbyPol[1]; } }
        public int LiczbaPolGracz2 {  get { return liczbyPol[2]; } }
        #endregion
    }
}
