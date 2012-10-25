using System;
using System.Windows.Controls;
using VolleyBall.Model;

namespace VolleyBall.Controls
{
    public class Ball : Image
    {
        public Vector Velocity { get; set; }
        public double Speed { get; set; }

        bool isCollisionPlayer;

        public void Move(double width, double height, Score score, Fence fence, Player[] players)
        {
            Velocity.Y = PowerOfAttraction();

            double x = this.Left() + Speed * Velocity.X, y = this.Top() + Speed * Velocity.Y;
            ImpactOnTheBorder(x, y, width, height);
            HitTheGround(x, y, width, height, score);

            this.SetLeft(x);
            this.SetTop(y);

            double nextX = this.Left() + Speed * Velocity.X, nextY = this.Top() + Speed * PowerOfAttraction();
            CollisionWithTheFence(nextX, nextY, fence);
            for (int i = 0; i < players.Length; ++i)
                CollisionWithThePlayer(nextX, nextY, players[i]);
        }

        double PowerOfAttraction()
        {
            double result = Velocity.Y;
            if (result < 0)
            {
                result /= Physics.GravityBall;
                if (result > -Physics.HangBall)
                    result = -result;
            }
            else
            {
                result *= Physics.GravityBall;
                if (result >= Physics.MaxSpeedBall)
                    result = Physics.MaxSpeedBall;
            }
            return result;
        }
        void ImpactOnTheBorder(double x, double y, double width, double height)
        {
            if (x < 0)
                Velocity.X = Math.Abs(Velocity.X);
            if (x + this.RealWidth() > width)
                Velocity.X = -Math.Abs(Velocity.X);
            if (y < 0)
                Velocity.Y = Math.Abs(Velocity.Y);
        }
        void HitTheGround(double x, double y, double width, double height, Score score)
        {
            if (y + this.RealHeight() > height)
                if (2.0 * x + this.RealWidth() < width)
                    ++score.Player2;
                else
                    ++score.Player1;
        }

        void CollisionWithTheFence(double nextX, double nextY, Fence fence)
        {
            // Отскок сбоку
            if (this.Bottom() >= fence.Top()
                && (this.Right() > fence.Left() && this.Left() < fence.Right()
                || nextX + this.RealWidth() > fence.Left() && nextX < fence.Right()))
                Velocity.X = -Velocity.X;

            // Отскок сверху
            double radius2 = Math.Pow(this.RealWidth() / 2.0, 2);
            Vector center = new Vector(nextX + this.RealWidth() / 2.0, nextY + this.RealHeight() / 2.0);
            if (center.DistanceTo(new Vector(fence.Left(), fence.Top())) <= radius2
            || center.DistanceTo(new Vector(fence.Right(), fence.Top())) <= radius2)
                Velocity.Y = -Velocity.Y;
        }
        void CollisionWithThePlayer(double nextX, double nextY, Player player)
        {
            double left = player.Left() + player.RealWidth() / 2.0, top = player.Top() + player.RealWidth() / 2.0;
            double radius2 = this.RealWidth() * this.RealWidth();
            double centerX = nextX + this.RealWidth() / 2.0, centerY = nextY + this.RealHeight() / 2.0;
            double dx = left - centerX, dy = top - centerY;
            if (!isCollisionPlayer && Math.Pow(dx, 2) + Math.Pow(dy, 2) <= radius2)
            {
                Velocity.X = -dx * Physics.ImpactPlayer;
                Velocity.Y = -dy * Physics.ImpactPlayer;
                isCollisionPlayer = true;
            }
            else
                isCollisionPlayer = false;
        }
    }
}