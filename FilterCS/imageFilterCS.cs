using System;
using System.Drawing;  // Dodano, aby umożliwić używanie klasy Bitmap

namespace FilterCS {
    public class imageFilterCS {
        // Przykładowa metoda testowa
        public static int test() {
            return 12;
        }

        // Metoda do zmiany kolorów pikseli obrazu
        //public static Bitmap ChangeImageColors(Bitmap originalImage, int changeAmount) {
        //    // Utwórz nowy obraz o tych samych wymiarach
        //    Bitmap newImage = new Bitmap(originalImage);

        //    for (int y = 0; y < originalImage.Height; y++) {
        //        for (int x = 0; x < originalImage.Width; x++) {
        //            // Pobierz kolor piksela
        //            Color originalColor = originalImage.GetPixel(x, y);

        //            // Zmniejsz wartości RGB o "changeAmount" (tutaj 50)
        //            int r = Math.Max(originalColor.R - changeAmount, 0);  // Zapewnia, że kolor nie przekroczy wartości 0
        //            int g = Math.Max(originalColor.G - changeAmount, 0);
        //            int b = Math.Max(originalColor.B - changeAmount, 0);

        //            // Ustaw nowy kolor piksela
        //            Color newColor = Color.FromArgb(r, g, b);

        //            // Zaktualizuj piksel na nowy kolor
        //            newImage.SetPixel(x, y, newColor);
        //        }
        //    }

        //    return newImage;
        //}
    }
}
