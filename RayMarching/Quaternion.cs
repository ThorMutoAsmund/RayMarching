using System;

namespace RayMarching
{
    public struct Quaternion
    {
        public double Q0 { get; set; }
        public double Q1 { get; set; }
        public double Q2 { get; set; }
        public double Q3 { get; set; }

        public Quaternion(double q0, double q1, double q2, double q3)
        {
            this.Q0 = q0;
            this.Q1 = q1;
            this.Q2 = q2;
            this.Q3 = q3;
        }

        public static Quaternion FromAxisAngle(Vector3 axis, double angle)
        {
            return new Quaternion(Math.Cos(angle / 2d),
                axis.X * Math.Sin(angle / 2d),
                axis.Y * Math.Sin(angle / 2d),
                axis.Z * Math.Sin(angle / 2d));
        }

        public static Quaternion FromVector(Vector3 point)
        {
            return new Quaternion(0,
                point.X, point.Y, point.Z);
        }

        public Quaternion Inverse => new Quaternion(this.Q0, -this.Q1, -this.Q2, -this.Q3);

        public Vector3 ToVector()
        {
            return new Vector3(this.Q1, this.Q2, this.Q3);
        }

        public static Quaternion operator *(Quaternion r, Quaternion s)
        {
            return new Quaternion(
                r.Q0 * s.Q0 - r.Q1 * s.Q1 - r.Q2 * s.Q2 - r.Q3 * s.Q3,
                r.Q0 * s.Q1 + r.Q1 * s.Q0 - r.Q2 * s.Q3 + r.Q3 * s.Q2,
                r.Q0 * s.Q2 + r.Q1 * s.Q3 + r.Q2 * s.Q0 - r.Q3 * s.Q1,
                r.Q0 * s.Q3 - r.Q1 * s.Q2 + r.Q2 * s.Q1 + r.Q3 * s.Q0
            );
        }
    }
}
