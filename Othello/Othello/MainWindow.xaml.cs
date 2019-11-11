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

        // wyświetlanie numeru gracza na przycisku
        private void uzgodnijZawartoscPlanszy()
        {
            if (!planszaZaainicjowana) return;

            for (int i = 0; i < rule.SzerokoscPlanszy; i++)
                for (int j = 0; j < rule.WysokoscPlanaszy; j++)
                {
                    plansza[i, j].Background = kolory[rule.PobierzStanPola(i, j)];
                }

            playerColor.Background = kolory[rule.NumerGraczaWykonujacegoNastepnyRuch];
            blackField.Text = rule.LiczbaPolGracz1.ToString();
            whiteField.Text = rule.LiczbaPolGracz2.ToString();

        }

        public MainWindow()
        {
            InitializeComponent();

            // podział planszy na wiersze i kolumny
            for (int i = 0; i < rule.SzerokoscPlanszy; i++)
                drawBoard.ColumnDefinitions.Add(new ColumnDefinition());
            for (int j = 0; j < rule.WysokoscPlanaszy; j++)
                drawBoard.RowDefinitions.Add(new RowDefinition());

            // utworzenie przycisków
            plansza = new Button[rule.SzerokoscPlanszy, rule.WysokoscPlanaszy];
            for(int i=0; i<rule.SzerokoscPlanszy;i++)
                for(int j=0; j<rule.WysokoscPlanaszy; j++)
                {
                    Button przycisk = new Button();
                    przycisk.Margin = new Thickness(0);
                    drawBoard.Children.Add(przycisk);
                    Grid.SetColumn(przycisk, i);
                    Grid.SetRow(przycisk, j);
                    plansza[i, j] = przycisk;
                }
            uzgodnijZawartoscPlanszy();
            //rule.PolozKamien(2, 4);
        }
    }
}
