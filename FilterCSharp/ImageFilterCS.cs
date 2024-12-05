using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Drawing.Imaging;
namespace FilterCSharp {
    public class ImageFilterCS {
        public static Bitmap ApplyMosaic(Bitmap fragment, int tileSize, Point position) {
            var bmpData = fragment.LockBits(
                new Rectangle(0, 0, fragment.Width, fragment.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            try {
                unsafe {
                    byte* ptr = (byte*)bmpData.Scan0;

                    // Sredni kolor dla fragmentu
                    var avgColor = GetAverageColor(ptr, bmpData.Stride, fragment.Width, fragment.Height);

                    // Wypełnia fragment średnim kolorem
                    FillWithColor(ptr, bmpData.Stride, fragment.Width, fragment.Height, avgColor);
                }
            }
            finally {
                fragment.UnlockBits(bmpData);
            }

            return fragment;
        }

        private static unsafe Color GetAverageColor(byte* ptr, int step, int width, int height) {
            long r = 0, g = 0, b = 0;
            int pixels = 0;

            for (int y = 0; y < height; y++) {
                var row = ptr + (y * step);
                for (int x = 0; x < width; x++) {
                    int i = x * 4;
                    b += row[i];
                    g += row[i + 1];
                    r += row[i + 2];
                    pixels++;
                }
            }

            return pixels == 0 ? Color.Black : Color.FromArgb(
                (int)(r / pixels),
                (int)(g / pixels),
                (int)(b / pixels)
            );
        }

        private static unsafe void FillWithColor(byte* ptr, int step, int width, int height, Color color) {
            for (int y = 0; y < height; y++) {
                var row = ptr + (y * step);
                for (int x = 0; x < width; x++) {
                    int i = x * 4;
                    row[i] = color.B;
                    row[i + 1] = color.G;
                    row[i + 2] = color.R;
                    row[i + 3] = 255;
                }
            }
        }
    }
}
