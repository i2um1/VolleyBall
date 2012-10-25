using System.Windows.Controls;
using VolleyBall.Model;

namespace VolleyBall.Controls
{
    public class Player : Image
    {
        public double Speed { get; set; }
        public Direction Direction { get; set; }

        public double Floor { get; set; }
        public double SpeedJump { get; set; }
        public double StartVelocityJump { get; set; }
        public double VelocityJump { get; set; }
        public bool IsJump { get; set; }

        public Player()
        {
            Direction = Direction.None;
        }

        public void Move(double leftFence, bool isFirst = true, double widthFence = 0, double widthField = 0)
        {
            double shift = 0;
            switch (Direction)
            {
                case Direction.Left: shift = -Speed; break;
                case Direction.Right: shift = Speed; break;
            }
            double left = this.Left() + shift;
            if ((isFirst && left >= 0 && left <= leftFence - this.RealWidth())
                || (!isFirst && left >= leftFence + widthFence && left <= widthField - this.RealWidth()))
                this.SetLeft(left);
        }
        public void Jump()
        {
            double top = this.Top();
            if (Floor <= top)
                if (VelocityJump > 0)
                {
                    top = Floor;
                    VelocityJump = 0;
                    IsJump = false;
                }
                else
                    VelocityJump = -StartVelocityJump;
            if (IsJump)
            {
                top += SpeedJump * VelocityJump;
                if (VelocityJump < 0)
                {
                    VelocityJump /= Physics.GravityPlayer;
                    if (VelocityJump > -Physics.HangPlayer)
                        VelocityJump = -VelocityJump;
                }
                else
                    VelocityJump *= Physics.GravityPlayer;
            }
            this.SetTop(top);
        }

        const double MOVE = -2;
        public void AI(Ball ball, double leftFence)
        {
            double left = (double)this.GetValue(Canvas.LeftProperty);
            if (ball.Left() - this.Left() > -MOVE)
                Direction = Direction.Right;
            else if (ball.Left() - this.Left() < MOVE)
                Direction = Direction.Left;
            else
            {
                this.IsJump = true;
                Direction = Direction.None;
            }
        }
    }
}