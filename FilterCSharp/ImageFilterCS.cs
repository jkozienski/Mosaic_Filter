public class ImageFilterCS {


    public static unsafe void ApplyMosaic(IntPtr sourcePtr, IntPtr resultPtr, int height, int width, int start, int end, int tileSize) {
        byte* srcPtr = (byte*)sourcePtr;
        byte* resPtr = (byte*)resultPtr;

        int stride = width * 4;// Format 32bpp (RGB)

        int startRow = start / (stride);
        int endRow = end / (stride);

        startRow = (startRow / tileSize) * tileSize;

        for (int y = startRow; y < endRow; y += tileSize) {
            for (int x = 0; x < width; x += tileSize) {

                int currentTileWidth = Math.Min(tileSize, width - x);
                int currentTileHeight = Math.Min(tileSize, height - y);
                
                long totalR = 0, totalG = 0, totalB = 0;
                int pixelCount = 0;

                for (int ty = 0; ty < currentTileHeight; ty++) {
                    if ((y + ty) >= endRow) break;

                    for (int tx = 0; tx < currentTileWidth; tx++) {
                        int pixelOffset = ((y + ty) * stride) + ((x + tx) * 4);

                        byte b = srcPtr[pixelOffset];
                        byte g = srcPtr[pixelOffset + 1];
                        byte r = srcPtr[pixelOffset + 2];

                        totalB += b;
                        totalG += g;
                        totalR += r;
                        pixelCount++;
                    }
                }

                if (pixelCount > 0)  
                {
                    byte avgB = (byte)(totalB / pixelCount);
                    byte avgG = (byte)(totalG / pixelCount);
                    byte avgR = (byte)(totalR / pixelCount);

                    for (int ty = 0; ty < currentTileHeight; ty++) {
                        if ((y + ty) >= endRow) break;

                        for (int tx = 0; tx < currentTileWidth; tx++) {
                            int pixelOffset = ((y + ty) * stride) + ((x + tx) * 4);

                            resPtr[pixelOffset] = avgB;
                            resPtr[pixelOffset + 1] = avgG;
                            resPtr[pixelOffset + 2] = avgR;

                            resPtr[pixelOffset + 3] = srcPtr[pixelOffset + 3];
                        }
                    }
                }
            }
        }
    }
}
