using System.Drawing;

namespace RayMarching
{
    public static class ColorExtensions
    {
        public static unsafe void SetTo(this Color color, byte* p, int x)
        {
            p[x * 4 + 0] = color.B;
            p[x * 4 + 1] = color.G;
            p[x * 4 + 2] = color.R;
            p[x * 4 + 3] = 255;
        }
        public static unsafe void SetTo(this Color color, byte* p, int x, double scale)
        {
            p[x * 4 + 0] = (byte)(color.B * scale);
            p[x * 4 + 1] = (byte)(color.G * scale);
            p[x * 4 + 2] = (byte)(color.R * scale);
            p[x * 4 + 3] = 255;
        }
    }
}
