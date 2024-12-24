using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static ImageFilterCS;  // Klasa imageFilterCS


namespace MosaicFilterProject {
    public partial class Form1 : Form {
        [DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")]//laptop
                                                                                                                  //[DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]//komputer
        private static extern void ApplyMosaicASM(
            IntPtr sourcePtr,   // wskaźnik do danych obrazu
            int stride,         // szerokość linii w bajtach
            int width,          // szerokość obrazu
            int height,         // wysokość obrazu
            int tileSize        // rozmiar kafelka
        );


        //static extern int imageFilterAsm(int a, int b);
        //[DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]//komputer
        //static extern int subtracting(int a, int b);




        private string selectedImageLocation;
        public Form1() {
            InitializeComponent();
            threadNumber.Value = Environment.ProcessorCount;
            threadNumberText.Text = threadNumber.Value.ToString();
            mosaicPowerThread.Text = mosaicPower.Value.ToString();

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
                    int tileSize = mosaicPower.Value;

                    if (radioCSharp.Checked) {
                        // Wersja C#
                        ImageFilterCS.ApplyMosaic(newImage, tileSize, new Point(0, 0));
                    } else {
                        // Wersja ASM
                        BitmapData bmpData = newImage.LockBits(
                            new Rectangle(0, 0, newImage.Width, newImage.Height),
                            ImageLockMode.ReadWrite,
                            PixelFormat.Format32bppArgb);
                        try {
                            ApplyMosaicASM(
                                bmpData.Scan0,          // wskaźnik do danych obrazu
                                bmpData.Stride,         // szerokość linii w bajtach
                                newImage.Width,         // szerokość obrazu
                                newImage.Height,        // wysokość obrazu
                                tileSize               // rozmiar kafelka
                            );
                        }
                        finally {
                            newImage.UnlockBits(bmpData);
                        }
                    }

                    imageAfterFilter.Image?.Dispose();
                    imageAfterFilter.Image = (Bitmap)newImage.Clone();

                    stopwatch.Stop();
                    MessageBox.Show($"Gotowe\nCzas wykonania: {stopwatch.ElapsedMilliseconds} ms\n" +
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
