using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Globalization;
using System.Configuration;

namespace Othello
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// public partial class MainWindow : Window
    public partial class MainWindow : Window
    {
        private GameRules rule = new GameRules(1);
        private SolidColorBrush[] colors = { Brushes.SpringGreen, Brushes.Black, Brushes.White };
        string[] names = { "", "black", "white" }; // do wyświetlenia komunikatu o zwycięzcy

        private Button[,] board;

        private bool initiatedBoard
        {
            get
            {
                return board[rule.boardWidth - 1, rule.boardHeight - 1] != null;
            }
        }

        // kolory kamieni na planszy
        private void boardContent()
        {
            if (!initiatedBoard) return;

            for (int i = 0; i < rule.boardWidth; i++)
                for (int j = 0; j < rule.boardHeight; j++)
                {
                    board[i, j].Background = colors[rule.DownloadFieldStatus(i, j)];
                }

            playerColor.Background = colors[rule.nextPlayer];
            blackField.Text = rule.PointsPlayer1.ToString(); // wyświetlenie ilości punktów czarnego gracza
            whiteField.Text = rule.PointsPlayer2.ToString(); // wyświetlenie ilości punktów białego gracza
        }
        private struct coordinates // współrzędne
        {
            public int Horizontally, Vertically;
        }
        private static string symbolField(int horizontally, int vertically)
        {
            // gdy nie wystarcza liter i cyfr wyświetlane są zwykłe współrzędne pola w nawiasach
            if (horizontally > 25 || vertically > 8) return "(" + horizontally.ToString() + "," + vertically.ToString() + ")";
            // pole (0,0) to A1, pole (7,7) to H8
            return "" + "ABCDEFGHIJKLMNOPQRSTUVXYZ"[horizontally] + "123456789"[vertically];
        }

        // metoda odczytująca własności Tag przycisku
        void boardClickField(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            coordinates coord = (coordinates)clickedButton.Tag;
            int clickedHorizontally = coord.Horizontally;
            int clickedVertically = coord.Vertically;

            // wykonanie ruchu
            int savedPlayerNumber = rule.nextPlayer;
            if (rule.PutStone(clickedHorizontally, clickedVertically))
            {
                boardContent();
                // lista ruchów 
                switch (savedPlayerNumber)
                {
                    case 1:
                        blackMoves.Items.Add(symbolField(clickedHorizontally, clickedVertically));
                        break;
                    case 2:
                        whiteMoves.Items.Add(symbolField(clickedHorizontally, clickedVertically));
                        break;
                }
                blackMoves.SelectedIndex = blackMoves.Items.Count - 1;
                whiteMoves.SelectedIndex = whiteMoves.Items.Count - 1;

                // sytuacje specjalne
                GameRules.Situation situation = rule.CheckSituation();
                bool gameOver = false;
                switch (situation)
                {
                    case GameRules.Situation.CurrentPlayerCantMove:
                        MessageBox.Show("Player" + names[rule.nextPlayer] + " is forced to give up the movement"); // zmuszony do oddania ruchu
                        rule.giveMove(); // oddaj ruch
                        boardContent();
                        break;
                    case GameRules.Situation.BothPlayersCantMove:
                        MessageBox.Show("Both players can't make a move."); // obaj nie mogą wykonać ruchu
                        gameOver = true;
                        break;
                    case GameRules.Situation.BusyFields: // wszystkie pola są zajęte
                        gameOver = true;
                        break;
                }

                // koniec gry - info o wyniku
                if (gameOver)
                {
                    int winnerNumber = rule.Lider; // zwycięzca = gracz z przewagą
                    // wygrana
                    if (winnerNumber != 0) MessageBox.Show("Congratulations! The winner is " + names[winnerNumber] + " player.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    // remis
                    else MessageBox.Show("It's a tie.", Title, MessageBoxButton.OK, MessageBoxImage.Information); 
                    // nowa gra
                    if (MessageBox.Show("Do you wanna play again?", "The Othello", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        newBoard(1, rule.boardWidth, rule.boardHeight);
                    }
                    else
                    {
                        Close();
                    }
                }
            }
    }
    private void newBoard(int firstPlayer, int widthBoard = 8, int heightBoard = 8)
        {
            rule = new GameRules(firstPlayer, widthBoard, heightBoard);
            blackMoves.Items.Clear();
            whiteMoves.Items.Clear();
            boardContent();
            drawBoard.IsEnabled = true;
            playerColor.IsEnabled = true;
        }
    public MainWindow()
        {
            Properties.Resources.Culture = new CultureInfo(ConfigurationManager.AppSettings["Culture"]);
            InitializeComponent(); // dostęp do MainWindow

            // podział planszy na wiersze i kolumny
            for (int i = 0; i < rule.boardWidth; i++)
                drawBoard.ColumnDefinitions.Add(new ColumnDefinition()); // dodaj kolumne
            for (int j = 0; j < rule.boardHeight; j++)
                drawBoard.RowDefinitions.Add(new RowDefinition()); // dodaj wiersz

            // utworzenie przycisków
            board = new Button[rule.boardWidth, rule.boardHeight];
            for (int i = 0; i < rule.boardWidth; i++)
                for (int j = 0; j < rule.boardHeight; j++)
                {
                    Button button = new Button(); // tworzymy nowy przycisk
                    drawBoard.Children.Add(button); // dodaj przycisk
                    Grid.SetColumn(button, i); // po wierszach
                    Grid.SetRow(button, j); // po kolumnach
                    button.Tag = new coordinates { Horizontally = i, Vertically = j };
                    button.Click += new RoutedEventHandler(boardClickField); //delegat używany do zdarzeń
                    board[i, j] = button; // rysuj przyciski po całej planszy
                }
            boardContent();
        }

        #region Zamykanie okna
        // zamykanie okna klawiszem escape
        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }
        
        // animacja zanikania okna
        bool closingAnimation = false;
        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closingAnimation)
            {
                Storyboard closingScenario = this.Resources["closingScenario"] as Storyboard; // import Animation
                closingScenario.Begin();
                e.Cancel = true; // blokuje zamknięcie okna, aby można było zobaczyć animacje
            }
        }
        // zamkniecie okna
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            closingAnimation = true;
            Close();
        }
        #endregion
    }
}
