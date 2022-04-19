using System.Drawing;

namespace RayMarching
{
    public class StandardCamera
    {
        public Vector3 Center { get; set; }
        public Vector3 LookDirection { get; set; }
        public double ViewPlaneDistance { get; set; }
        public Color BackgroundColor { get; set; } = Color.Black;
        public StandardCamera(Vector3? center = null, Vector3? lookAt = null, double viewPlaneDistance = 800d)
        {
            this.Center = center ?? Vector3.Zero;
            this.LookDirection = ((lookAt ?? Vector3.Forward) - center)?.Normalized ?? Vector3.Forward;
            this.ViewPlaneDistance = viewPlaneDistance;
        }
    }
}
