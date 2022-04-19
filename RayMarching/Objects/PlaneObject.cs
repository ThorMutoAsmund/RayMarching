using System;

namespace RayMarching
{
    public class PlaneObject : I3DObject
    {
        public IMaterial Material { get; set; } = SolidMaterial.Empty;
        public double Y { get; set; }

        public double GetDistanceSquaredTo(Vector3 point)
        {
            var dy = point.Y - this.Y;
            return dy * dy;
        }

        public PlaneObject(double y, IMaterial? material = null)
        {
            this.Y = y;

            if (material != null)
            {
                this.Material = material;
            }
        }
    }
}
