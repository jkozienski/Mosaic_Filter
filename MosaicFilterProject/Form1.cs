using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FilterCS;  // Dodaj przestrzeñ nazw dla klasy imageFilterCS

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
           // MessageBox.Show($"CSLIB {res12}", "Wynik obliczeñ");

            string imageLocation = "";
            try {
                // Wywo³anie funkcji z ASM
                int result = imageFilterAsm(12, 3);
               // MessageBox.Show($"Wynik z MyProc1: {result}", "Wynik obliczeñ");

                // Tworzymy obiekt dialogu do wyboru pliku
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png";  // Tylko pliki PNG

                // Sprawdzamy, czy u¿ytkownik wybra³ plik
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Sprawdzenie, czy plik jest poprawnym obrazem
                    try {
                        Bitmap originalImage = new Bitmap(dialog.FileName);
                        GrayScale(originalImage);  // Przekazujemy wczytany obraz do funkcji
                    }
                    catch (ArgumentException ex) {
                        MessageBox.Show("Wczytano nieprawid³owy plik obrazu. " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) {
                // W przypadku b³êdu, wyœwietl komunikat
                MessageBox.Show($"Wyst¹pi³ b³¹d: {ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GrayScale(Bitmap originalImage) {
            // Zmienna pomocnicza do trzymania danych obrazu
            Bitmap bmap = (Bitmap)originalImage.Clone();

            // Uzyskujemy dane obrazu
            Rectangle rect = new Rectangle(0, 0, bmap.Width, bmap.Height);
            System.Drawing.Imaging.BitmapData data = bmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmap.PixelFormat);

            // Uzyskujemy wskaŸnik do danych pikseli
            IntPtr ptr = data.Scan0;
            int bytes = Math.Abs(data.Stride) * bmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Kopiowanie danych pikseli do tablicy
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Zmieniamy kolory na szaroœæ
            for (int i = 0; i < rgbValues.Length; i += 4) {
                byte r = rgbValues[i + 2];  // Czerwony
                byte g = rgbValues[i + 1];  // Zielony
                byte b = rgbValues[i];      // Niebieski

                // Formu³a konwersji na szaroœæ
                byte gray = (byte)Math.Max(0, Math.Min(255, (0.299 * r + 0.587 * g + 0.114 * b)));

                // Ustawienie wartoœci RGB na szaro
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
