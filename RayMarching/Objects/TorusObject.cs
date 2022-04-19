namespace RayMarching
{
    public class TorusObject : I3DObject
    {
        public IMaterial Material { get; set; } = SolidMaterial.Empty;
        public Vector3 Center { get; set; }
        public double Radius { get; set; }

        public double GetDistanceTo(Vector3 point)
        {
            return Vector3.DistanceBetween(point, this.Center) - this.Radius;
        }

        public TorusObject(Vector3 center, double radius, IMaterial? material = null)
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
//return length(vec2(length(p.xz) - t.x, p.y)) - t.y;