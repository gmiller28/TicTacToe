
using System.Windows.Controls;
using System.Windows;
using System.Numerics;
using System.Media;
using System;
using static TicTacToe.MainWindow;
using System.Windows.Threading;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Media;

namespace TicTacToe
{
    // Sets The Game Board And The Starting Player To X. As Well As The Timer, Also Sets The Starting Time To 10.
    public partial class MainWindow : Window
    {
        private string[,] gameBoard = new string[3, 3];
        private string currentPlayer = "X";
        private DispatcherTimer timer;
        private int remainingTime = 10;

        public MainWindow()
        {
            // Initialize The Componets Of The Game And The Game Board. Also Plays A Sound When The Game Is Started. Sets The Amount Of Seconds Taken Off From Each Timer Tick To 1. Starts The Timer.
       
            InitializeBoard();
            WindowState = WindowState.Maximized;
            MediaPlayer player = new MediaPlayer();
            player.Open (new Uri($"Assets/ES_Magic Chime - SFX Producer.wav", UriKind.Relative));
            player.Play();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // Code For The Game Board Start Up.
        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    gameBoard[i, k] = "";
                }
            }
        }

        // This Class Is Used To Add The X And O Player Symbol.
        public class Symbol
        {
            private string playerSymbol;

            public Symbol(string symbol)
            {
                this.playerSymbol = symbol;
            }

            public override string ToString()
            {
                return playerSymbol;
            }
        }

        // When The Button Is Clicked It Sends Either The X Or O Text In The Boxes. Also Plays A Sound Every Player Click To Show That The Current Players Turn Is Over. Sets The Time On Timer For Each Turn To 6.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button space = (Button)sender;

            MediaPlayer player1 = new MediaPlayer();
            player1.Open(new Uri($"Assets/ES_Bubble Blip 2 - SFX Producer.wav", UriKind.Relative));
            player1.Play();
            remainingTime = 6;

            // Variables For Rows And Columns. Also Gets The Rows And Columns To Set Them.
            int row = Grid.GetRow(space);
            int column = Grid.GetColumn(space);

            // Switches The Player Each Turn.
            if (gameBoard[row, column] == "")
            {
                gameBoard[row, column] = currentPlayer;
                space.Content = currentPlayer;


                // Checks To See If Someone Has Won The Game. As Well Plays A Sound When The Game Is Over
                if (CheckGameWinner())
                {
                    WinCounter(currentPlayer);
                    MessageBox.Show(currentPlayer + " Wins!");
                    MediaPlayer player2 = new MediaPlayer();
                    player2.Open(new Uri($"Assets/ES_Magic Chime - SFX Producer.wav", UriKind.Relative));
                    player2.Play();

                    ResetGameBoard();
                }
                // Checks To See If The Board Is Full And If There Is A Tie.
                else if (CheckGameTie())
                {
                    MessageBox.Show("Tie game!");
                    MediaPlayer player3 = new MediaPlayer();
                    player3.Open(new Uri($"Assets/ES_Magic Chime - SFX Producer.wav", UriKind.Relative));
                    player3.Play();
                    ResetGameBoard();
                }

                // Sets The Current Player Back To X.
                else
                {
                    currentPlayer = currentPlayer == "X" ? "O" : "X";
                }
            }
        }

        // Sets The Win Count To 0 Upon Start Up Of Each Game For The X And Os.
        private int xWins = 0;
        private int oWins = 0;
        private void WinCounter(string currentPlayer)
        {
            if (currentPlayer == "X")
            {
                xWins++;
                xWinsCount.Text = "X Wins: " + xWins;
            }
            else if (currentPlayer == "O")
            {
                oWins++;
                oWinsCount.Text = "O Wins: " + oWins;
            }
        }

        // Code For Checking The Winner That Coresponds By The Placement Of The Symbols. Can Tell If a Game Win Is True Or Not.
        private bool CheckGameWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (gameBoard[i, 0] != "" && gameBoard[i, 0] == gameBoard[i, 1] && gameBoard[i, 1] == gameBoard[i, 2])
                {
                    return true;
                }
            }

            for (int j = 0; j < 3; j++)
            {
                if (gameBoard[0, j] != "" && gameBoard[0, j] == gameBoard[1, j] && gameBoard[1, j] == gameBoard[2, j])
                {
                    return true;
                }
            }

            if (gameBoard[0, 0] != "" && gameBoard[0, 0] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 2])
            {
                return true;
            }
            if (gameBoard[0, 2] != "" && gameBoard[0, 2] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 0])
            {
                return true;
            }

            return false;
        }

        // Checks To See If There Is No Possible Way A Player Can Win So Will Check For A Tie.
        private bool CheckGameTie()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (gameBoard[i, k] == "")
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // When The Game Ends This Will Reset The Gameboard And Remove All Symbols So Another Round Can Be Played. Resets The Current Player To X.
        private void ResetGameBoard()
        {
            InitializeBoard();

            space00.Content = "";
            space01.Content = "";
            space02.Content = "";
            space03.Content = "";
            space04.Content = "";
            space05.Content = "";
            space06.Content = "";
            space07.Content = "";
            space08.Content = "";

            currentPlayer = "X";
        }

        // When The Game Begins Each Player Will Have 6 Seconds To Make There Next Move. If A Player Does Not Make A Move In Time, It Will Then Reset The Timer And Will Result In The Other Players Turn. A Clock Sound Is Played When The Timer Gets To 
        // 2 And Stops At 0. Also The Text Block Of The Current Player WIll Update Accordingly. 
        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTime--;

            if (remainingTime <= 0 && currentPlayer == "X")
            {
                currentPlayer = "O";
                remainingTime = 6;
                MessageBox.Show("Times Up! Its Now Player Os Turn.");
            }

            if (remainingTime <= 0 && currentPlayer == "O")
            {
                currentPlayer = "X";
                remainingTime = 6;
                MessageBox.Show("Times Up! Its Now Player Xs Turn.");

            }

            playerTurnTextBlock.Text = "Current Player: " + currentPlayer;

            if (remainingTime <= 0)
            {
                timer.Stop();
            }

            if (remainingTime <= 2)
            {
                MediaPlayer player4 = new MediaPlayer();
                player4.Open (new Uri($"Assets/ES_Timer Clock 1 - SFX Producer.wav", UriKind.Relative));
                player4.Position = new TimeSpan(0);
                player4.Play();
            }

          else if (remainingTime <= 0)
            {
                MediaPlayer player4 = new MediaPlayer();
                player4.Open( new Uri($"Assets/ES_Timer Clock 1 - SFX Producer.wav",UriKind.Relative));
                player4.Stop();
            }

            timerTextBlock.Text = $"Time Left On Turn: {remainingTime}";
        }
    }
}