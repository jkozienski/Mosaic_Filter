public class ImageFilterCS {
    public static unsafe void ApplyMosaic(IntPtr sourcePtr, IntPtr resultPtr, int height, int width, int start, int end, int tileSize) {
        byte* srcPtr = (byte*)sourcePtr;
        byte* resPtr = (byte*)resultPtr;

        int stride = width * 4;// Format 32bpp (RGB)

        // Iteracja po kafelkach
        for (int y = 0; y < height; y += tileSize) {
            for (int x = 0; x < width; x += tileSize) {
                int currentTileWidth = Math.Min(tileSize, width - x);
                int currentTileHeight = Math.Min(tileSize, height - y);

                // Oblicz średni kolor dla kafelka
                long totalR = 0, totalG = 0, totalB = 0;
                int pixelCount = 0;

                // Obliczanie średniego koloru
                for (int ty = 0; ty < currentTileHeight; ty++) {
                    for (int tx = 0; tx < currentTileWidth; tx++) {
                        int pixelOffset = ((y + ty) * stride) + ((x + tx) * 4);

                        // RGB format
                        byte b = srcPtr[pixelOffset];
                        byte g = srcPtr[pixelOffset + 1];
                        byte r = srcPtr[pixelOffset + 2];
                        // Czwarty bajt ignorujemy

                        totalB += b;
                        totalG += g;
                        totalR += r;
                        pixelCount++;
                    }
                }

                // Oblicz średnie wartości kolorów
                byte avgB = (byte)(totalB / pixelCount);
                byte avgG = (byte)(totalG / pixelCount);
                byte avgR = (byte)(totalR / pixelCount);

                // Wypełnij kafelek średnim kolorem
                for (int ty = 0; ty < currentTileHeight; ty++) {
                    for (int tx = 0; tx < currentTileWidth; tx++) {
                        int pixelOffset = ((y + ty) * stride) + ((x + tx) * 4);

                        resPtr[pixelOffset] = avgB;
                        resPtr[pixelOffset + 1] = avgG;
                        resPtr[pixelOffset + 2] = avgR;
                        // Czwarty bajt zostawiamy nienaruszony
                    }
                }
            }
        }
    }
}