using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// public partial class MainWindow : Window
    public partial class MainWindow : Window
    {
        private GameRules rule = new GameRules(1);
        private SolidColorBrush[] kolory = { Brushes.SpringGreen, Brushes.Black, Brushes.White };

        private Button[,] plansza;

        private bool planszaZaainicjowana
        {
            get
            {
                return plansza[rule.SzerokoscPlanszy - 1, rule.WysokoscPlanaszy - 1] != null;
            }
        }

        // kolory kamieni na planszy
        private void uzgodnijZawartoscPlanszy()
        {
            if (!planszaZaainicjowana) return;

            for (int i = 0; i < rule.SzerokoscPlanszy; i++)
                for (int j = 0; j < rule.WysokoscPlanaszy; j++)
                {
                    plansza[i, j].Background = kolory[rule.PobierzStanPola(i, j)];
                }

            playerColor.Background = kolory[rule.NumerGraczaWykonujacegoNastepnyRuch];
            blackField.Text = rule.LiczbaPolGracz1.ToString(); // wyświetlenie ilości punktów czarnego gracza
            whiteField.Text = rule.LiczbaPolGracz2.ToString(); // wyświetlenie ilości punktów białego gracza

        }
        private struct wspolrzednePola
        {
            public int Poziomo, Pionowo;
        }

        // historia ruchów obu graczy zapisana w ListBox
        // w planszy standardowej 8x8 oznaczenia od A1 do H8
        // przy powiększeniu planszy w konstruktorze potrzebne są większe wartości 
        private static string symbolPola(int poziomo, int pionowo)
        {
            // przy większej planszy wypisz wartość 2 liczb po przecinku w nawiasie
            if (poziomo > 25 || pionowo > 8) return "(" + poziomo.ToString() + "," + pionowo.ToString() + ")";
            // przy mniejszej planszy wypisz wartość od A1 do Z9 
            return "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[poziomo] + "123456789"[pionowo];
        }

        // metoda odczytująca własności Tag przycisku
        void kliknieciePolaPlanszy(object sender, RoutedEventArgs e)
        {
            Button kliknietyPrzycisk = sender as Button;
            wspolrzednePola wspolrzedne = (wspolrzednePola)kliknietyPrzycisk.Tag;
            int kliknieciePoziomo = wspolrzedne.Poziomo;
            int kliknieciePionowo = wspolrzedne.Pionowo;
       
            // wykonanie ruchu
            int zapamietanyNumerGracza = rule.NumerGraczaWykonujacegoNastepnyRuch;
            if (rule.PolozKamien(kliknieciePoziomo, kliknieciePionowo))
                uzgodnijZawartoscPlanszy();

            // lista ruchow
            switch (zapamietanyNumerGracza)
            {
                case 1:
                    blackMoves.Items.Add(symbolPola(kliknieciePoziomo, kliknieciePionowo));
                    break;
                case 2:
                    whiteMoves.Items.Add(symbolPola(kliknieciePoziomo, kliknieciePionowo));
                    break;
            }
            blackMoves.SelectedIndex = blackMoves.Items.Count - 1;
            whiteMoves.SelectedIndex = whiteMoves.Items.Count - 1;
        }
        public MainWindow()
        {
            InitializeComponent(); // dostęp do MainWindow

            // podział planszy na wiersze i kolumny
            for (int i = 0; i < rule.SzerokoscPlanszy; i++)
                drawBoard.ColumnDefinitions.Add(new ColumnDefinition()); // dodaj kolumne
            for (int j = 0; j < rule.WysokoscPlanaszy; j++)
                drawBoard.RowDefinitions.Add(new RowDefinition()); // dodaj wiersz

            // utworzenie przycisków
            plansza = new Button[rule.SzerokoscPlanszy, rule.WysokoscPlanaszy];
            for (int i = 0; i < rule.SzerokoscPlanszy; i++)
                for (int j = 0; j < rule.WysokoscPlanaszy; j++)
                {
                    Button przycisk = new Button(); // tworzymy nowy przycisk
                    przycisk.Margin = new Thickness();
                    drawBoard.Children.Add(przycisk); // dodaj przycisk
                    Grid.SetColumn(przycisk, i); // po wierszach
                    Grid.SetRow(przycisk, j); // po kolumnach
                    przycisk.Tag = new wspolrzednePola { Poziomo = i, Pionowo = j };
                    przycisk.Click += new RoutedEventHandler(kliknieciePolaPlanszy);
                    plansza[i, j] = przycisk; // rysuj przyciski po całej planszy
                }
            uzgodnijZawartoscPlanszy();

        }
        

    }
}
