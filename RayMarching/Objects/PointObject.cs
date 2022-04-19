namespace RayMarching
{
    public class PointObject : I3DObject
    {
        public IMaterial Material { get; set; } = SolidMaterial.Empty;
        public Vector3 Center { get; set; }
        public double GetDistanceTo(Vector3 source)
        {
            return Vector3.DistanceSquaredBetween(source, this.Center);
        }
    }
}
