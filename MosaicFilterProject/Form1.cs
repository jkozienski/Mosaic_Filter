using System;
using System.CodeDom;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static ImageFilterCS;  // Klasa imageFilterCS


namespace MosaicFilterProject {
    public partial class Form1 : Form {
        //[DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Release\FilterAsm.dll")]//laptop
        [DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Release\FilterAsm.dll")]//komputer
        private static extern void ApplyMosaicASM(
          IntPtr InBuffer,          // Wskaźnik do bufora wejściowego
          IntPtr OutBuffer,         // Wskaźnik do bufora wyjściowego
          int height,               // Wysokość obrazu
          int width,                // Szerokość obrazu
          int start,                // Początkowa pozycja dla wątku
          int end,                  // Końcowa pozycja dla wątku
          int tileSize              // Rozmiar kafelka
      );



        private string selectedImageLocation;
        private Image originalImage;

        public Form1() {
            InitializeComponent();

            // Domyślnie liczba watkow CPU
            threadNumber.Value = Environment.ProcessorCount;
            threadNumberText.Text = threadNumber.Value.ToString();
            mosaicPowerThread.Text = mosaicPower.Value.ToString();

        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void label1_Click(object sender, EventArgs e) {
        }


        private void filterButton_Click(object sender, EventArgs e) {
            if (originalImage == null) {
                MessageBox.Show("Nie wybrano obrazu", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Pomiar czasu
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try {

                using (Bitmap sourceBitmap = new Bitmap(originalImage))//Kopia obrazu
                {
                    int width = sourceBitmap.Width;
                    int height = sourceBitmap.Height;
                    int threadCount = threadNumber.Value;
                    int tileSize = mosaicPower.Value;


                    if (height <= 0 || width <= 0) {
                        MessageBox.Show("Niepoprawne wymiary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (Bitmap filteredBitmap = new Bitmap(sourceBitmap)) {
                        BitmapData srcImage = null;
                        BitmapData filteredImage = null;

                        try {

                            //tylko odczyt dla źródła
                            srcImage = sourceBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                            //tylko zapis dla wyniku
                            filteredImage = filteredBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);//(4 bajty na piksel: RGB i puste)

                            int stride = srcImage.Stride; //Liczba bajtów w jednym wierszu obrazu
                            int bytes = Math.Abs(stride) * height; // bajty w obrazie

                            byte[] srcBuffer = new byte[bytes]; //Bufor dla obrazu źródłowego i wynikowego
                            byte[] resultBuffer = new byte[bytes]; //Kopia obrazu źródłowego
                            Marshal.Copy(srcImage.Scan0, srcBuffer, 0, bytes);

                            int alignedBytes = (bytes + 15) & ~15;
                            IntPtr sourcePtr = Marshal.AllocHGlobal(alignedBytes); 
                            IntPtr resultPtr = Marshal.AllocHGlobal(alignedBytes); 

                            try {
                                Marshal.Copy(srcBuffer, 0, sourcePtr, bytes); 
                                Marshal.Copy(srcBuffer, 0, resultPtr, bytes); 

                                // Podzial obrazu na równe cześci dla watkow
                                int[] startIndex = new int[threadCount];
                                int[] finishIndex = new int[threadCount];

                                // Oblicza liczbę pełnych kafelków w pionie
                                int totalTiles = height / tileSize + (height % tileSize == 0 ? 0 : 1);
                                int tilesPerThread = totalTiles / threadCount;

                                for (int i = 0; i < threadCount; i++) {
                                    // Początek zakresu w pikselach
                                    startIndex[i] = i * tilesPerThread * tileSize * width * 4;

                                    if (i == threadCount - 1) {
                                        finishIndex[i] = height * width * 4;
                                    } else {
                                        finishIndex[i] = (i + 1) * tilesPerThread * tileSize * width * 4;
                                    }
                                }

                                Parallel.For(0, threadCount, i => {
                                    if (radioCSharp.Checked) {
                                        ImageFilterCS.ApplyMosaic(sourcePtr, resultPtr, height, width, startIndex[i], finishIndex[i], tileSize);
                                    } else if (radioASM.Checked) {
                                        ApplyMosaicASM(sourcePtr, resultPtr, height, width, startIndex[i], finishIndex[i], tileSize);
                                    }
                                });
                                Marshal.Copy(resultPtr, resultBuffer, 0, bytes); //Kopiowanie ze wskaźnika do bufora
                                Marshal.Copy(resultBuffer, 0, filteredImage.Scan0, bytes); //Kopiowanie z bufora do obrazu wynikowego

                            }
                            finally {

                                // Zwalnia pamięc
                                Marshal.FreeHGlobal(sourcePtr);
                                Marshal.FreeHGlobal(resultPtr);
                            }
                        }
                        finally {

                            if (srcImage != null) sourceBitmap.UnlockBits(srcImage);
                            if (filteredImage != null) filteredBitmap.UnlockBits(filteredImage);
                        }
                        stopwatch.Stop();
                        if (imageAfterFilter.Image != null) {
                            imageAfterFilter.Image?.Dispose();

                        }
                        imageAfterFilter.Image = new Bitmap(filteredBitmap);

                        MessageBox.Show($"Gotowe\nCzas wykonania: {stopwatch.ElapsedMilliseconds} ms\n" +
                                      $"Rozmiar kafelka: {tileSize}px \nSzer: {width}, wys: {height}",
                            "Gotowe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
            originalImage = null;
            imageAfterFilter.Image = null;
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

        private void imageUpload_Click(object sender, EventArgs e) {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";  //jpg jpeg png

            if (dialog.ShowDialog() == DialogResult.OK) {
                try {
                    originalImage?.Dispose();
                    originalImage = Image.FromFile(dialog.FileName);
                    imageBeforeFilter.Image?.Dispose();
                    imageBeforeFilter.Image = new Bitmap(originalImage);
                    selectedImageLocation = dialog.FileName;
                    locationTextBox.Text = selectedImageLocation;
                }
                catch (Exception ex) {
                    MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveImage_Click(object sender, EventArgs e) {
            try {
                // Sprawdź czy jest jakieś zdjęcie do zapisania
                if (imageAfterFilter.Image == null) {
                    MessageBox.Show("Brak zdjęcia do zapisania.", "Błąd",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SaveFileDialog saveDialog = new SaveFileDialog {
                    Filter = "Pliki PNG (*.png)|*.png|Pliki JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Pliki BMP (*.bmp)|*.bmp|Wszystkie pliki (*.*)|*.*",
                    FilterIndex = 1,
                    Title = "Zapisz przetworzone zdjęcie",
                    AddExtension = true,
                    DefaultExt = "png"
                };

                // Pokaż okno dialogowe zapisu
                if (saveDialog.ShowDialog() == DialogResult.OK) {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    ImageFormat format = ImageFormat.Png; // domyślny format

                    switch (extension) {
                        case ".jpg":
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                    }

                    // Zapisz obraz
                    using (Bitmap bmp = new Bitmap(imageAfterFilter.Image)) {
                        bmp.Save(saveDialog.FileName, format);
                    }

                    MessageBox.Show("Zdjęcie zostało zapisane pomyślnie.", "Sukces",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania pliku:\n{ex.Message}",
                    "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
