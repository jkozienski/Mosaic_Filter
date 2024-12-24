using System.Drawing.Imaging;
using System.Drawing;

public class ImageFilterCS {
    public static Bitmap ApplyMosaic(Bitmap imageFragment, int tileSize, Point position) {
        var workArea = new Rectangle(0, 0, imageFragment.Width, imageFragment.Height);
        var imageData = imageFragment.LockBits(workArea, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

        try {
            unsafe {
                byte* ptr = (byte*)imageData.Scan0;

                // Iteracja po kafelkach
                for (int tileY = 0; tileY < imageFragment.Height; tileY += tileSize) {
                    for (int tileX = 0; tileX < imageFragment.Width; tileX += tileSize) {
                        // Oblicz rzeczywisty rozmiar kafelka (może być mniejszy na krawędziach)
                        int currentTileWidth = Math.Min(tileSize, imageFragment.Width - tileX);
                        int currentTileHeight = Math.Min(tileSize, imageFragment.Height - tileY);

                        // Oblicz średni kolor dla kafelka
                        var averageColor = CalculateAverageTileColor(
                            ptr,
                            imageData.Stride,
                            tileX,
                            tileY,
                            currentTileWidth,
                            currentTileHeight
                        );

                        // Wypełnij kafelek średnim kolorem
                        FillTileWithColor(
                            ptr,
                            imageData.Stride,
                            tileX,
                            tileY,
                            currentTileWidth,
                            currentTileHeight,
                            averageColor
                        );
                    }
                }
            }
        }
        finally {
            imageFragment.UnlockBits(imageData);
        }

        return imageFragment;
    }

    private static unsafe Color CalculateAverageTileColor(
        byte* ptr,
        int stride,
        int startX,
        int startY,
        int width,
        int height) 
        {
        long totalR = 0, totalG = 0, totalB = 0;
        int pixelCount = 0;

        for (int y = 0; y < height; y++) {
            byte* row = ptr + ((startY + y) * stride);
            for (int x = 0; x < width; x++) {
                int offset = (startX + x) * 4;
                totalB += row[offset];
                totalG += row[offset + 1];
                totalR += row[offset + 2];
                pixelCount++;
            }
        }

        return Color.FromArgb(
            (int)(totalR / pixelCount),
            (int)(totalG / pixelCount),
            (int)(totalB / pixelCount)
        );
    }

    private static unsafe void FillTileWithColor(
        byte* ptr,
        int stride,
        int startX,
        int startY,
        int width,
        int height,
        Color color)
        {
        for (int y = 0; y < height; y++) {
            byte* row = ptr + ((startY + y) * stride);
            for (int x = 0; x < width; x++) {
                int offset = (startX + x) * 4;
                row[offset] = color.B;
                row[offset + 1] = color.G;
                row[offset + 2] = color.R;
                row[offset + 3] = 255;
            }
        }
    }
}