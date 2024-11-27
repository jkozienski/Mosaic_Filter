using System;
using System.Drawing;  // Dla klasy Bitmap

namespace FilterCSharp {
    public class ImageFilterCS {
        // Przykładowa metoda testowa
        public static int test() {
            return 133;
        }

        // Metoda zmieniająca obraz na odcienie czerwone
        public static Bitmap ApplyRedScale(Bitmap originalImage) {
            // Tworzymy kopię obrazu
            Bitmap newImage = new Bitmap(originalImage);

            // Iteruje przez każdy piksel obrazu
            for (int y = 0; y < originalImage.Height; y++) {
                for (int x = 0; x < originalImage.Width; x++) {
                    // Pobieram kolor piksela
                    Color originalColor = originalImage.GetPixel(x, y);

                   
                    byte red = originalColor.R;
                    byte green = 0;
                    byte blue = 0;

                    // Tworze nowy kolor z czerwonym kanałem
                    Color newColor = Color.FromArgb(red, green, blue);

                    // Ustawiamy nowy kolor na pikselu
                    newImage.SetPixel(x, y, newColor);
                }
            }

            // Zwracam zmodyfikowany obraz
            return newImage;
        }


        public static Bitmap ApplyMosaic(Bitmap originalImage, int tileSize) {
            // Tworzymy kopię obrazu
            Bitmap newImage = new Bitmap(originalImage);

            // Iteruje po obrazie, dzieląc go na kafelki
            for (int y = 0; y < originalImage.Height; y += tileSize) {
                for (int x = 0; x < originalImage.Width; x += tileSize) {
                    // Obliczamy średni kolor dla danego kafelka
                    Color averageColor = GetAverageColor(originalImage, x, y, tileSize);

                    // Zastępujemy wszystkie piksele w tym kafelku średnim kolorem
                    for (int i = x; i < x + tileSize && i < originalImage.Width; i++) {
                        for (int j = y; j < y + tileSize && j < originalImage.Height; j++) {
                            newImage.SetPixel(i, j, averageColor);
                        }
                    }
                }
            }

            // Zwracam obraz
            return newImage;
        }

        // Funkcja obliczająca średni kolor pikseli w obrębie danego kafelka
        private static Color GetAverageColor(Bitmap image, int xStart, int yStart, int tileSize) {
            int r = 0, g = 0, b = 0;
            int count = 0;

            for (int y = yStart; y < yStart + tileSize && y < image.Height; y++) {
                for (int x = xStart; x < xStart + tileSize && x < image.Width; x++) {
                    Color pixelColor = image.GetPixel(x, y);
                    r += pixelColor.R;
                    g += pixelColor.G;
                    b += pixelColor.B;
                    count++;
                }
            }

            // Oblicza średnią wartość dla RGB
            r /= count;
            g /= count;
            b /= count;

            // Tworzymy nowy kolor z wyliczonych wartości RGB
            return Color.FromArgb(r, g, b);
        }
    }
}

