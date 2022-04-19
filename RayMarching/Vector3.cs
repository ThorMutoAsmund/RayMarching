using System;

namespace RayMarching
{
    public struct Vector3
    {
        public double X, Y, Z;

        public static double DistanceSquaredBetween(Vector3 a, Vector3 b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z);
        }
        public static double DistanceBetween(Vector3 a, Vector3 b)
        {
            return Math.Sqrt(DistanceSquaredBetween(a, b));
        }

        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 Left { get; } = new Vector3(-1d, 0, 0);
        public static Vector3 Right { get; } = new Vector3(1d, 0, 0);
        public static Vector3 Up { get; } = new Vector3(0, 1d, 0);

        public Vector3 Normalized
        {
            get
            {
                var l = this.Length; 
                return new Vector3(this.X / l, this.Y / l, this.Z / l);
            }
        }

        public double Length => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        public double LengthSquared => this.X * this.X + this.Y * this.Y + this.Z * this.Z;

        public static Vector3 operator *(Vector3 self, double v)
        {
            return new Vector3(self.X * v, self.Y * v, self.Z * v);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public override string ToString()
        {
            return $"{this.X},{this.Y},{this.Z}";
        }
    }
}
