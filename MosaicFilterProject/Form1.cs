using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FilterCSharp;  // Klasa imageFilterCS


namespace MosaicFilterProject {
    public partial class Form1 : Form {
        //[DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")]//laptop
        [DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]//komputer
        private static extern void ApplyMosaicASM(
            IntPtr sourcePtr,   // wskaźnik do danych obrazu
            int width,          // szerokość fragmentu
            int height          // wysokość fragmentu
        );

        //static extern int imageFilterAsm(int a, int b);
        //[DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]//komputer
        //static extern int subtracting(int a, int b);




        private string selectedImageLocation;
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void label1_Click(object sender, EventArgs e) {
        }

      
        private void filterButton_Click(object sender, EventArgs e) {
            try {
                if (imageBeforeFilter.Image == null) return;

                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                using (Bitmap originalImage = new Bitmap(imageBeforeFilter.Image))
                using (Bitmap newImage = new Bitmap(originalImage)) {
                    const int MAX_FRAGMENTS = 10000;
                    int tileSize = mosaicPower.Value;

                    // Obliczamy początkową liczbę fragmentów
                    int fragmentsX = (int)Math.Ceiling((double)originalImage.Width / tileSize);
                    int fragmentsY = (int)Math.Ceiling((double)originalImage.Height / tileSize);
                    int totalFragments = fragmentsX * fragmentsY;

                    // Jeśli liczba fragmentów przekracza limit, zwiększamy rozmiar fragmentu
                    int fragmentSize = tileSize;
                    while (totalFragments > MAX_FRAGMENTS) {
                        fragmentSize += tileSize * 4; // Zwiększamy o 4 kafelki
                        fragmentsX = (int)Math.Ceiling((double)originalImage.Width / fragmentSize);
                        fragmentsY = (int)Math.Ceiling((double)originalImage.Height / fragmentSize);
                        totalFragments = fragmentsX * fragmentsY;
                    }

                    Bitmap[] fragments = new Bitmap[totalFragments];

                    // Dzielimy obraz na fragmenty
                    for (int y = 0; y < fragmentsY; y++) {
                        for (int x = 0; x < fragmentsX; x++) {
                            int index = y * fragmentsX + x;
                            int currentWidth = Math.Min(fragmentSize, originalImage.Width - (x * fragmentSize));
                            int currentHeight = Math.Min(fragmentSize, originalImage.Height - (y * fragmentSize));

                            Rectangle cropRect = new Rectangle(
                                x * fragmentSize,
                                y * fragmentSize,
                                currentWidth,
                                currentHeight
                            );

                            fragments[index] = new Bitmap(currentWidth, currentHeight);
                            using (Graphics g = Graphics.FromImage(fragments[index])) {
                                g.DrawImage(originalImage,
                                    new Rectangle(0, 0, currentWidth, currentHeight),
                                    cropRect,
                                    GraphicsUnit.Pixel);
                            }
                        }
                    }

                    // Przetwarzanie równoległe - każdy wątek przetwarza jeden fragment
                    Parallel.For(0, totalFragments, i => {
                        if (radioCSharp.Checked) {
                            int x = i % fragmentsX;
                            int y = i / fragmentsX;
                            Point fragmentPosition = new Point(x * fragmentSize, y * fragmentSize);

                            fragments[i] = ImageFilterCS.ApplyMosaic(
                                fragments[i],
                                tileSize, // Używamy oryginalnego rozmiaru kafelka do mozaikowania
                                fragmentPosition
                            );
                        } else {
                            // Implementacja ASM...
                        }
                    });

                    // Składanie obrazu
                    using (Graphics g = Graphics.FromImage(newImage)) {
                        for (int y = 0; y < fragmentsY; y++) {
                            for (int x = 0; x < fragmentsX; x++) {
                                int index = y * fragmentsX + x;
                                g.DrawImage(fragments[index], x * fragmentSize, y * fragmentSize);
                                fragments[index].Dispose();
                            }
                        }
                    }

                    imageAfterFilter.Image?.Dispose();
                    imageAfterFilter.Image = (Bitmap)newImage.Clone();

                    stopwatch.Stop();
                    MessageBox.Show($"Gotowe\nCzas wykonania: {stopwatch.ElapsedMilliseconds} ms\n" +
                                  $"Liczba fragmentów: {totalFragments}\n" +
                                  $"Rozmiar fragmentu: {fragmentSize}px\n" +
                                  $"Rozmiar kafelka: {tileSize}px",
                        "Gotowe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
        }

        private void bottomPanel_Paint(object sender, PaintEventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
            threadNumberText.Text = threadNumber.Value.ToString();

        }

        private void mosaicPower_Scroll(object sender, EventArgs e) {
            mosaicPowerThread.Text = mosaicPower.Value.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void clearImage_Click(object sender, EventArgs e) {
            imageBeforeFilter.Image = null;
            imageAfterFilter.Image = null;

            locationTextBox.Clear();

            selectedImageLocation = string.Empty;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e) {

        }

        private void threadNumberText_Click(object sender, EventArgs e) {
        }

        private void mosaicPowerText_Click(object sender, EventArgs e) {
        }

        private void imageUpload_Click(object sender, EventArgs e) {
            try {
                // Tworzymy obiekt dialogu do wyboru pliku
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png|JPEG files(*.jpeg)|*.jpeg|JPG files(*.jpg)|*.jpg|All files(*.*)|*.*";  //PNG i JPEG

                // Sprawdzamy, czy użytkownik wybrał plik
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Bitmap originalImage = new Bitmap(dialog.FileName);

                    imageBeforeFilter.Image = originalImage;

                    selectedImageLocation = dialog.FileName;
                    locationTextBox.Text = selectedImageLocation;

                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
