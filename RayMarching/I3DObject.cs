namespace RayMarching
{
    public interface I3DObject
    {
        IMaterial Material { get; set; }
        double GetDistanceSquaredTo(Vector3 point);
    }
}
