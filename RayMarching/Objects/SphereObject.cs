namespace RayMarching
{
    public class SphereObject : I3DObject
    {
        public IMaterial Material { get; set; } = SolidMaterial.Empty;
        public Vector3 Center { get; set; }
        public double Radius { get; set; }

        public double GetDistanceSquaredTo(Vector3 point)
        {
            return Vector3.DistanceSquaredBetween(point, this.Center) - this.Radius * this.Radius;
        }

        public SphereObject(Vector3 center, double radius, IMaterial? material = null)
        {
            this.Center = center;
            this.Radius = radius;
            if (material != null)
            {
                this.Material = material;
            }
        }
    }
}
