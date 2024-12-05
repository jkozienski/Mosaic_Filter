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
        private static extern void ApplyRedFilter(
            IntPtr sourcePtr,
            int width,
            int height
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

        private void button1_Click(object sender, EventArgs e) {
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


        private void filterButton_Click(object sender, EventArgs e) {
            try {
                if (imageBeforeFilter.Image == null) {
                    MessageBox.Show("Proszę załadować obraz przed przetwarzaniem.", "Błąd",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                using (Bitmap originalImage = new Bitmap(imageBeforeFilter.Image))
                using (Bitmap newImage = new Bitmap(originalImage)) {
                    int tileSize = mosaicPower.Value;

                    // Obliczamy liczbę fragmentów bazując na rozmiarze kafelka
                    int fragmentsX = (int)Math.Ceiling((double)originalImage.Width / tileSize);
                    int fragmentsY = (int)Math.Ceiling((double)originalImage.Height / tileSize);
                    int totalFragments = fragmentsX * fragmentsY;

                    Bitmap[] fragments = new Bitmap[totalFragments];

                    // Podział obrazu głownego na fragmenty 
                    for (int y = 0; y < fragmentsY; y++) {
                        for (int x = 0; x < fragmentsX; x++) {
                            int index = y * fragmentsX + x;
                            int currentWidth = Math.Min(tileSize, originalImage.Width - (x * tileSize));
                            int currentHeight = Math.Min(tileSize, originalImage.Height - (y * tileSize));

                            Rectangle cropRect = new Rectangle(
                                x * tileSize,
                                y * tileSize,
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

                    // Przetwarzanie równoległe
                    Parallel.For(0, totalFragments, i => {
                        if (radioCSharp.Checked) {
                            int x = i % fragmentsX;
                            int y = i / fragmentsX;
                            Point fragmentPosition = new Point(x * tileSize, y * tileSize);

                            fragments[i] = ImageFilterCS.ApplyMosaic(fragments[i], tileSize, fragmentPosition);
                        } else {
                           //ASM
                        }
                    });

                    // Składanie obrazu
                    using (Graphics g = Graphics.FromImage(newImage)) {
                        for (int y = 0; y < fragmentsY; y++) {
                            for (int x = 0; x < fragmentsX; x++) {
                                int index = y * fragmentsX + x;
                                g.DrawImage(fragments[index], x * tileSize, y * tileSize);
                                fragments[index].Dispose();
                            }
                        }
                    }

                    imageAfterFilter.Image?.Dispose();
                    imageAfterFilter.Image = (Bitmap)newImage.Clone();

                    stopwatch.Stop();

                    MessageBox.Show($"Gotowe\nCzas wykonania: {stopwatch.ElapsedMilliseconds} ms",
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

            locationTextBox.Clear();

            selectedImageLocation = string.Empty;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e) {

        }

        private void threadNumberText_Click(object sender, EventArgs e) {
        }

        private void mosaicPowerText_Click(object sender, EventArgs e) {
        }
    }
}
