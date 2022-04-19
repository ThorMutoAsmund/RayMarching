using System;

namespace RayMarching
{
    public class CubeObject : I3DObject
    {
        public IMaterial Material { get; set; } = SolidMaterial.Empty;
        public Vector3 Center { get; set; }
        public Vector3 Size { get; set; }

        public double GetDistanceSquaredTo(Vector3 point)
        {
            point = point - this.Center;
            var mx = Math.Max(0d, Math.Abs(point.X) - this.Size.X);
            var my = Math.Max(0d, Math.Abs(point.Y) - this.Size.Y);
            var mz = Math.Max(0d, Math.Abs(point.Z) - this.Size.Z);
            return mx * mx + my * my + mz * mz;
        }

        public CubeObject(Vector3 center, Vector3 size, IMaterial? material = null)
        {
            this.Center = center;
            this.Size = size;

            if (material != null)
            {
                this.Material = material;
            }
        }
    }
}
