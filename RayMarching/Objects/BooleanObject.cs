using System;

namespace RayMarching
{
    public enum BooleanOperator
    {
        Combined,
        SmoothCombined,
        Intersection,
        Subtraction
    }

    public class BooleanObject : I3DObject
    {
        public IMaterial Material { get; set; } = SolidMaterial.Empty;

        public BooleanOperator Operator { get; set; }
        public I3DObject PrimaryObject { get; set; }
        public I3DObject SecondaryObject { get; set; }
        public double Smoothness { get; set; }
        public double GetDistanceTo(Vector3 point)
        {
            switch (this.Operator)
            {
                case BooleanOperator.Combined:
                    return Math.Min(this.PrimaryObject.GetDistanceTo(point), this.SecondaryObject.GetDistanceTo(point));
                case BooleanOperator.SmoothCombined:
                    return SmoothMin(this.PrimaryObject.GetDistanceTo(point), this.SecondaryObject.GetDistanceTo(point), this.Smoothness);
                case BooleanOperator.Intersection:
                    return Math.Max(this.PrimaryObject.GetDistanceTo(point), this.SecondaryObject.GetDistanceTo(point));
                case BooleanOperator.Subtraction:
                    return Math.Max(-this.PrimaryObject.GetDistanceTo(point), this.SecondaryObject.GetDistanceTo(point));
                default:
                    return Double.MaxValue;
            }
            //return Vector3.DistanceBetween(point, this.Center) - this.Radius;
        }

        private static double SmoothMin(double dstA, double dstB, double k)
        {
            double h = Math.Max(k - Math.Abs(dstA - dstB), 0) / k;
            return Math.Min(dstA, dstB) - h * h * h * k / 6d;
        }

        public BooleanObject(I3DObject primaryObject, I3DObject secondaryObject, BooleanOperator booleanOperator, 
            double smoothness = 1f,
            IMaterial? material = null)
        {
            this.PrimaryObject = primaryObject;
            this.SecondaryObject = secondaryObject;
            this.Operator = booleanOperator;


            this.Smoothness = Math.Max(0.01d, smoothness);
            if (material != null)
            {
                this.Material = material;
            }
        }
    }
}
