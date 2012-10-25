using System;

namespace VolleyBall.Model
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Vector other)
        {
            return Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2);
        }
    }
}