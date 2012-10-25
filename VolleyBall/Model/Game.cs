using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;
using VolleyBall.Controls;

namespace VolleyBall.Model
{
    public class Game
    {
        #region Define
        Canvas canvas;
        Fence fence;
        Ball ball;
        Player[] players;
        TextBlock result;

        private const int speed = 300000; // 0.03 секунд
        private const int win = 15; // Условие победы
        private const int timeWait = 500; // Задержка мяча на земле в миллисекундах

        Score score;
        DispatcherTimer timer, wait;

        bool isFirstForWait;
        #endregion

        public Game(Canvas canvas, Fence fence, Ball ball, Player[] players, TextBlock result)
        {
            this.canvas = canvas;
            this.fence = fence;
            this.ball = ball;
            this.players = players;
            this.result = result;

            timer = new DispatcherTimer();
            timer.Interval = new System.TimeSpan(speed);
            timer.Tick += new System.EventHandler(timer_Tick);

            wait = new DispatcherTimer();
            wait.Tick += new System.EventHandler(wait_Tick);
            wait.Interval = new TimeSpan(0, 0, 0, 0, timeWait);

            fence.SetLeft((canvas.RealWidth() - fence.Width) / 2.0);
            fence.SetTop(canvas.RealHeight() - fence.Height);

            Reset();
        }

        #region Management
        public void Start()
        {
            timer.Start();
        }
        public void KeyDown(Key key)
        {
            switch (key)
            {
                case Key.Left: players[0].Direction = Direction.Left; break;
                case Key.Right: players[0].Direction = Direction.Right; break;
                case Key.Up: players[0].IsJump = true; break;
            }
        }
        public void KeyUp(Key key)
        {
            switch (key)
            {
                case Key.Left: if (players[0].Direction == Direction.Left) players[0].Direction = Direction.None; break;
                case Key.Right: if (players[0].Direction == Direction.Right) players[0].Direction = Direction.None; break;
                case Key.R: Reset(); break;
            }
        }
        #endregion

        #region Extra
        void UpdateResult()
        {
            result.Text = string.Format("{0}-{1}", score.Player1, score.Player2);
        }
        void Reset(bool isNewGame = true, bool queueFirstPlayer = true)
        {
            if (isNewGame)
                score = new Score();
            UpdateResult();
            
            players[0].SetLeft((fence.Left() - players[0].RealWidth()) / 2.0);
            players[0].SetTop(canvas.RealHeight() - players[0].RealHeight());
            players[1].SetLeft(fence.Left() * 1.5 + fence.Width - players[1].RealWidth() / 2.0);
            players[1].SetTop(canvas.RealHeight() - players[1].RealHeight());
            ball.SetLeft(players[0].RealWidth() + (queueFirstPlayer ? fence.Left() / 2.0 - ball.RealWidth() : fence.Left() + ball.RealWidth()));
            ball.SetTop(canvas.RealHeight() - players[1].RealHeight() - ball.RealHeight() * 1.5);
            players[0].Floor = players[0].Top();
            players[1].Floor = players[1].Top();

            players[0].VelocityJump = players[1].VelocityJump = 0;
            players[0].IsJump = players[1].IsJump = false;
            players[0].Direction = players[1].Direction = Direction.None;

            ball.Velocity = new Vector(0, 0);
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (players[0].IsJump)
                players[0].Jump();
            if (players[1].IsJump)
                players[1].Jump();

            if (!wait.IsEnabled)
            {
                players[0].Move(fence.Left());
                players[1].AI(ball, fence.Left());
                players[1].Move(fence.Left(), false, fence.Width, canvas.RealWidth());

                ball.Move(canvas.RealWidth(), canvas.RealHeight(), score, fence, players);

                int[] oldScore = result.Text.Split('-').Select(x => Convert.ToInt32(x)).ToArray();
                UpdateResult();
                if (oldScore[0] != score.Player1 || oldScore[1] != score.Player2)
                {
                    isFirstForWait = oldScore[0] != score.Player1;
                    ball.SetTop(canvas.RealHeight() - ball.RealHeight());
                    wait.Start();
                }

                if (score.Player1 == win || score.Player2 == win)
                {
                    MessageBox.Show(string.Format("Победил {0}-й игрок!", score.Player1 == win ? '1' : '2'),
                        score.Player1 == win ? "Победа" : "Поражение", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset(true);
                }
            }
        }
        void wait_Tick(object sender, EventArgs e)
        {
            Reset(false, isFirstForWait);
            wait.Stop();
        }
        #endregion
    }
}