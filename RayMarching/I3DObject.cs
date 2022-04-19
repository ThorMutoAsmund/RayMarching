namespace RayMarching
{
    public interface I3DObject
    {
        IMaterial Material { get; set; }
        double GetDistanceTo(Vector3 point);
    }
}
