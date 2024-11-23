using System;
using System.Drawing;  // Dodano, aby umożliwić używanie klasy Bitmap

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

            // Iterujemy przez każdy piksel obrazu
            for (int y = 0; y < originalImage.Height; y++) {
                for (int x = 0; x < originalImage.Width; x++) {
                    // Pobieramy kolor piksela
                    Color originalColor = originalImage.GetPixel(x, y);

                    // Ustawiamy tylko kanał czerwony, a zielony i niebieski na 0
                    byte red = originalColor.R;
                    byte green = 0;
                    byte blue = 0;

                    // Tworzymy nowy kolor z czerwonym kanałem
                    Color newColor = Color.FromArgb(red, green, blue);

                    // Ustawiamy nowy kolor na pikselu
                    newImage.SetPixel(x, y, newColor);
                }
            }

            // Zwracamy zmodyfikowany obraz
            return newImage;
        }

    }
}
