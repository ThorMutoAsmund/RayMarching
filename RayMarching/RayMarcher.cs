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
        public StandardCamera? Camera { get; private set; }
        public double MarchMinDistance { get; set; } = 0.001d;
        public double SceneSize { get; set; } = 1280d;
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
            if (this.Camera == null)
            {
                return;
            }

            var height2 = this.imageHeight / 2d; 
            var width2 = this.imageWidth / 2d;

            unsafe
            {
                byte* p;

                for (int y = 0; y < this.imageHeight; ++y)
                {
                    var fovy = Math.Atan2((y - height2), this.Camera.ViewPlaneDistance);
                    var roty = Quaternion.FromAxisAngle(Vector3.Right, fovy);

                    p = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
                    for (int x = 0; x < this.imageWidth; ++x)
                    {
                        var fovx = Math.Atan2((x - width2), this.Camera.ViewPlaneDistance);
                        var rotx = Quaternion.FromAxisAngle(Vector3.Up, fovx) * roty;
                        
                        var vec = (rotx.Inverse * Quaternion.FromVector(this.Camera.LookDirection) * rotx).ToVector();

                        var march = this.Camera.Center;
                        double dist, minDist;
                        I3DObject? minDistObj = default;
                        do
                        {
                            minDist = double.MaxValue;
                            foreach (var obj in this.Objects)
                            {
                                dist = obj.GetDistanceSquaredTo(march);
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    minDistObj = obj;
                                }
                            }
                            march = march + (vec * Math.Sqrt(minDist));
                        }
                        while (minDist > this.MarchMinDistance && march.LengthSquared < this.SceneSizeSquared);

                        if (march.LengthSquared >= this.SceneSizeSquared)
                        {
                            Color.Black.SetTo(p, x);
                        }
                        else
                        {
                            var scale = 1d / Math.Sqrt(0.01d*Vector3.DistanceBetween(this.Camera.Center, march) + 1d);
                            minDistObj?.Material.GetColor().SetTo(p, x, scale);
                        }
                    }
                }
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
