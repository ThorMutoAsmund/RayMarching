using System.Drawing;

namespace RayMarching
{
    public class SolidMaterial : IMaterial
    {
        public Color Color { get; set; }

        public static SolidMaterial Empty { get; } = new SolidMaterial(Color.FromArgb(255, 0, 0, 0));
        public Color GetColor()
        {
            return this.Color;
        }

        public SolidMaterial(Color color)
        {
            this.Color = color;
        }
    }
}
