using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    public class GameRules
    {
        public int SzerokoscPlanszy { get; private set; }
        public int WysokoscPlanaszy { get; private set; }

        private int[,] plansza;
        public int NumerGraczaWykonujacegoNastepnyRuch { get; private set; } = 1;

        private static int numerPrzeciwnika(int numerGracza)
        {
            return (numerGracza == 1) ? 2 : 1;
        }

        private bool czyWspolrzednePolaPrawidlowe(int poziomo, int pionowo)
        {
            return poziomo >= 0 && poziomo < SzerokoscPlanszy &&
                pionowo >= 0 && pionowo < WysokoscPlanaszy;
        }

        private int PobierzStanPola(int poziomo, int pionowo)
        {
            if (!czyWspolrzednePolaPrawidlowe(poziomo, pionowo))
                throw new Exception("Nieprawidlowe wspolrzedne pola");
            return plansza[poziomo, pionowo];
        }
    }
}
