using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RayMarching
{
    internal class Unused
    {
        public BitmapSource CreateBitmap(int width, int height)
        {
            var pixels = new Int32[width, height];

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    pixels[x, y] = Color.FromArgb(255, x * 256 / width, y * 256 / height, 0).ToArgb();
                }
            }

            int resX = pixels.GetUpperBound(0) + 1;
            int resY = pixels.GetUpperBound(1) + 1;

            WriteableBitmap writableImg = new WriteableBitmap(resX, resY, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);

            //lock the buffer
            writableImg.Lock();

            for (int i = 0; i < resX; i++)
            {
                for (int j = 0; j < resY; j++)
                {
                    IntPtr backbuffer = writableImg.BackBuffer;
                    //the buffer is a monodimensionnal array...
                    backbuffer += j * writableImg.BackBufferStride;
                    backbuffer += i * 4;
                    System.Runtime.InteropServices.Marshal.WriteInt32(backbuffer, pixels[i, j]);
                }
            }

            //specify the area to update
            writableImg.AddDirtyRect(new Int32Rect(0, 0, resX, resY));
            //release the buffer and show the image
            writableImg.Unlock();

            return writableImg;
        }
    }
}
