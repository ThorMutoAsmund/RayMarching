using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RayMarching
{
    // https://www.shadertoy.com/view/Xds3zN
    public class RayMarcher
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public List<I3DObject> Objects { get; private set; } = new List<I3DObject>();
        public StandardCamera Camera { get; private set; } = new StandardCamera();
        public double Epsilon { get; set; } = 0.001d;
        public double SceneSize { get; set; } = 320d;
        public double SceneSizeSquared => this.SceneSize * this.SceneSize;

        private int imageWidth;
        private int imageHeight;
        private Bitmap? bmp;
        private Rectangle lockRegion;

        public void AddObject(I3DObject obj)
        {
            this.Objects.Add(obj);
        }

        public void AddCamera(StandardCamera camera)
        {
            this.Camera = camera;
        }

        public void SetSize(int width, int height)
        {
            this.imageWidth = width;
            this.imageHeight = height;

            this.bmp = null;
        }

        public void RenderImageTo(System.Windows.Controls.Image image)
        {
            if (this.Camera == null)
            {
                Debug.Write("No camera");
                return;
            }

            if (this.bmp == null)
            {
                this.bmp = new Bitmap(this.imageWidth, this.imageHeight);
                this.lockRegion = new Rectangle(0, 0, this.imageWidth, this.imageHeight);
            }

            var bmpData = this.bmp.LockBits(this.lockRegion, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            RenderImage(bmpData);

            this.bmp.UnlockBits(bmpData);
            var hBmp = bmp.GetHbitmap();
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hBmp);

            image.Source = bitmapSource;
        }

        private void RenderImage(BitmapData bmpData)
        {
            var height2 = this.imageHeight / 2d; 
            var width2 = this.imageWidth / 2d;

            unsafe
            {
                byte* p;
                int tries = 0;
                for (int y = 0; y < this.imageHeight; ++y)
                {
                    var fovy = Math.Atan2((y - height2), this.Camera.ViewPlaneDistance);
                    var roty = Quaternion.FromAxisAngle(Vector3.Right, fovy);

                    p = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
                    for (int x = 0; x < this.imageWidth; ++x)
                    {
                        var fovx = Math.Atan2((x - width2), this.Camera.ViewPlaneDistance);
                        var rotx = Quaternion.FromAxisAngle(Vector3.Up, fovx);

                        var r0 = rotx * roty;
                        
                        var color = March((r0.Inverse * Quaternion.FromVector(this.Camera.LookDirection) * r0).ToVector(), ref tries);
                        color.SetTo(p, x);
                    }
                }

                Debug.WriteLine($"Number of tries: {tries}. Tries per pixel: {tries / (this.imageWidth*this.imageHeight)}");
            }
        }

        private void RenderImageAA(BitmapData bmpData)
        {
            var height2 = this.imageHeight / 2d;
            var width2 = this.imageWidth / 2d;

            unsafe
            {
                byte* p;
                int tries = 0;
                for (int y = 0; y < this.imageHeight; ++y)
                {
                    var fovy = Math.Atan2((y - height2), this.Camera.ViewPlaneDistance);
                    var roty = Quaternion.FromAxisAngle(Vector3.Right, fovy);

                    var fovy2 = Math.Atan2((y + 0.5d - height2), this.Camera.ViewPlaneDistance);
                    var roty2 = Quaternion.FromAxisAngle(Vector3.Right, fovy2);

                    p = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
                    for (int x = 0; x < this.imageWidth; ++x)
                    {
                        var fovx = Math.Atan2((x - width2), this.Camera.ViewPlaneDistance);
                        var rotx = Quaternion.FromAxisAngle(Vector3.Up, fovx);

                        var fovx2 = Math.Atan2((x + 0.5d - width2), this.Camera.ViewPlaneDistance);
                        var rotx2 = Quaternion.FromAxisAngle(Vector3.Up, fovx2);

                        var r0 = rotx * roty;
                        var r1 = rotx * roty2;
                        var r2 = rotx2 * roty;
                        var r3 = rotx2 * roty2;


                        var c0 = March((r0.Inverse * Quaternion.FromVector(this.Camera.LookDirection) * r0).ToVector(), ref tries);
                        var c1 = March((r1.Inverse * Quaternion.FromVector(this.Camera.LookDirection) * r1).ToVector(), ref tries);
                        var c2 = March((r2.Inverse * Quaternion.FromVector(this.Camera.LookDirection) * r2).ToVector(), ref tries);
                        var c3 = March((r3.Inverse * Quaternion.FromVector(this.Camera.LookDirection) * r3).ToVector(), ref tries);
                        var avgColor = Color.FromArgb(255,
                            (byte)(((int)c0.R + c1.R + c2.R + c3.R) / 4f),
                            (byte)(((int)c0.G + c1.G + c2.G + c3.G) / 4f),
                            (byte)(((int)c0.B + c1.B + c2.B + c3.B) / 4f));
                        avgColor.SetTo(p, x);
                    }
                }

                Debug.WriteLine($"Number of tries: {tries}. Tries per pixel: {tries / (this.imageWidth * this.imageHeight)}");
            }
        }

        private Color March(Vector3 ray, ref int tries)
        {
            var pos = this.Camera.Center;
            double dist, minDist, distSum = 0d;
            I3DObject? target = default;
            do
            {
                tries++;
                minDist = double.MaxValue;
                foreach (var obj in this.Objects)
                {
                    dist = obj.GetDistanceTo(pos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = obj;
                    }
                }
                pos = pos + (ray * minDist);
                distSum += minDist;
            }
            while (minDist > this.Epsilon && distSum < this.SceneSize);

            if (pos.LengthSquared >= this.SceneSizeSquared || target == null)
            {
                return this.Camera.BackgroundColor;
            }
            else
            {
                var normal = new Vector3(
                    target.GetDistanceTo(pos.Add(x: this.Epsilon)) - target.GetDistanceTo(pos.Add(x: -this.Epsilon)),
                    target.GetDistanceTo(pos.Add(y: this.Epsilon)) - target.GetDistanceTo(pos.Add(y: -this.Epsilon)),
                    target.GetDistanceTo(pos.Add(z: this.Epsilon)) - target.GetDistanceTo(pos.Add(z: -this.Epsilon))).Normalized;

                double diffuse = Math.Max(0d, (-ray) * normal);
                //double specular = pow(diffuse, 32.0);

                var color = target.Material.GetColor();

                return Color.FromArgb(255, (byte)(color.R * diffuse), (byte)(color.G * diffuse), (byte)(color.B * diffuse));
            }
        }

        private void RenderTestImage(BitmapData bmpData)
        {
            unsafe
            {
                byte* p;

                for (int y = 0; y < this.imageHeight; ++y)
                {
                    p = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
                    for (int x = 0; x < this.imageWidth; ++x)
                    {
                        Color.FromArgb(0, (byte)(x * 256 / this.imageWidth), (byte)(y * 256 / this.imageHeight)).SetTo(p, x);
                    }
                }
            }
        }

    }
}
