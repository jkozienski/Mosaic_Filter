using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FilterCS;  // Dodaj przestrze� nazw dla klasy imageFilterCS

namespace MosaicFilterProject {
    public partial class Form1 : Form {
        // Deklaracja funkcji z DLL ASM
        [DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")]
        static extern int imageFilterAsm(int a, int b);

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void label1_Click(object sender, EventArgs e) {
        }

        private void button1_Click(object sender, EventArgs e) {
            int res12 = imageFilterCS.test();
           // MessageBox.Show($"CSLIB {res12}", "Wynik oblicze�");

            string imageLocation = "";
            try {
                // Wywo�anie funkcji z ASM
                int result = imageFilterAsm(12, 3);
               // MessageBox.Show($"Wynik z MyProc1: {result}", "Wynik oblicze�");

                // Tworzymy obiekt dialogu do wyboru pliku
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png";  // Tylko pliki PNG

                // Sprawdzamy, czy u�ytkownik wybra� plik
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Sprawdzenie, czy plik jest poprawnym obrazem
                    try {
                        Bitmap originalImage = new Bitmap(dialog.FileName);
                        GrayScale(originalImage);  // Przekazujemy wczytany obraz do funkcji
                    }
                    catch (ArgumentException ex) {
                        MessageBox.Show("Wczytano nieprawid�owy plik obrazu. " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) {
                // W przypadku b��du, wy�wietl komunikat
                MessageBox.Show($"Wyst�pi� b��d: {ex.Message}", "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GrayScale(Bitmap originalImage) {
            // Zmienna pomocnicza do trzymania danych obrazu
            Bitmap bmap = (Bitmap)originalImage.Clone();

            // Uzyskujemy dane obrazu
            Rectangle rect = new Rectangle(0, 0, bmap.Width, bmap.Height);
            System.Drawing.Imaging.BitmapData data = bmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmap.PixelFormat);

            // Uzyskujemy wska�nik do danych pikseli
            IntPtr ptr = data.Scan0;
            int bytes = Math.Abs(data.Stride) * bmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Kopiowanie danych pikseli do tablicy
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Zmieniamy kolory na szaro��
            for (int i = 0; i < rgbValues.Length; i += 4) {
                byte r = rgbValues[i + 2];  // Czerwony
                byte g = rgbValues[i + 1];  // Zielony
                byte b = rgbValues[i];      // Niebieski

                // Formu�a konwersji na szaro��
                byte gray = (byte)Math.Max(0, Math.Min(255, (0.299 * r + 0.587 * g + 0.114 * b)));

                // Ustawienie warto�ci RGB na szaro
                rgbValues[i] = gray;       // Niebieski
                rgbValues[i + 1] = gray;   // Zielony
                rgbValues[i + 2] = gray;   // Czerwony
            }

            // Kopiowanie zmodyfikowanych danych do obrazu
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Odblokowanie danych
            bmap.UnlockBits(data);

            // Ustawienie zmodyfikowanego obrazu w PictureBox
            imageAfterFilter.Image = bmap;
        }

        private void filterButton_Click(object sender, EventArgs e) {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
        }
    }
}
