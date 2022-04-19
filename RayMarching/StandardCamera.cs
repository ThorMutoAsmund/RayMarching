namespace RayMarching
{
    public class StandardCamera
    {
        public Vector3 Center { get; set; }
        public Vector3 LookDirection { get; set; }
        public double ViewPlaneDistance { get; set; }
        public StandardCamera(Vector3 center, Vector3 lookAt, double viewPlaneDistance)
        {
            this.Center = center;
            this.LookDirection = (lookAt - center).Normalized;
            this.ViewPlaneDistance = viewPlaneDistance;
        }
    }
}
