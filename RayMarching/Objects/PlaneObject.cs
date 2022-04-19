using System;

namespace RayMarching
{
    public class PlaneObject : I3DObject
    {
        public IMaterial Material { get; set; } = SolidMaterial.Empty;
        public double Y { get; set; }
        public double SizeSquared { get; set; }

        public double GetDistanceTo(Vector3 point)
        {
            return (point.X * point.X * point.X + point.Z* point.Z) > this.SizeSquared ? double.MaxValue : point.Y - this.Y;
        }

        public PlaneObject(double y, double size, IMaterial? material = null)
        {
            this.Y = y;
            this.SizeSquared = size * size;

            if (material != null)
            {
                this.Material = material;
            }
        }
    }
}
