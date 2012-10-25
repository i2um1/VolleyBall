using System.Windows;
using System.Windows.Input;
using VolleyBall.Model;

namespace VolleyBall
{
    public partial class MainWindow : Window
    {
        Game game;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game = new Game(canvas, fence, ball, new VolleyBall.Controls.Player[] { player1, player2 }, result);
            game.Start();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            game.KeyDown(e.Key);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            game.KeyUp(e.Key);
        }
    }
}