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
        [DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")]//laptop
                                                                                                                  //[DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]//komputer
        private static extern void ApplyMosaicASM(
            IntPtr InBuffer,          // Wskaźnik do bufora wejściowego
            IntPtr OutBuffer,         // Wskaźnik do bufora wyjściowego
            int height,               // Wysokość obrazu
            int width,                // Szerokość obrazu
            int start,                // Początkowy indeks dla wątku
            int end,                  // Końcowy indeks dla wątku
            int tileSize              // Rozmiar kafelka
        );


        //static extern int imageFilterAsm(int a, int b);
        //[DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]//komputer
        //static extern int subtracting(int a, int b);




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
            int threadCount = threadNumber.Value;
            int tileSize = mosaicPower.Value;

            // Start pomiaru czasu
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try {

                using (Bitmap sourceBitmap = new Bitmap(originalImage))//Kopia obrazu
                {
                    int width = sourceBitmap.Width;
                    int height = sourceBitmap.Height;
                    //MessageBox.Show($"Szer: {width}, wys: {height}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (height <= 0 || width <= 0) {
                        MessageBox.Show("Niepoprawne wymiary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (Bitmap filteredBitmap = new Bitmap(sourceBitmap)) {
                        BitmapData sourceData = null;
                        BitmapData filteredData = null;

                        try {

                            //tylko odczyt dla źródła
                            sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                            //tylko zapis dla wyniku
                            filteredData = filteredBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);//(4 bajty na piksel: RGB i puste)

                            int stride = sourceData.Stride; //Liczba bajtów w jednym wierszu obrazu
                            int bytes = Math.Abs(stride) * height; //Liczba bajtów w obrazie

                            byte[] sourceBuffer = new byte[bytes]; //Bufor dla obrazu źródłowego i wynikowego
                            byte[] resultBuffer = new byte[bytes]; //Kopia obrazu źródłowego
                            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, bytes);//Kopiowanie danych z obrazu źródłowego do bufora

                            int alignedBytes = (bytes + 15) & ~15;
                            IntPtr sourcePtr = Marshal.AllocHGlobal(alignedBytes); //Alokacja pamięci dla wskaźnika do obrazu źródłowego
                            IntPtr resultPtr = Marshal.AllocHGlobal(alignedBytes); //Alokacja pamięci dla wskaźnika do obrazu wynikowego

                            try {
                                Marshal.Copy(sourceBuffer, 0, sourcePtr, bytes); //Kopiowanie danych z bufora do wskaźnika
                                Marshal.Copy(sourceBuffer, 0, resultPtr, bytes); //Kopiowanie danych z bufora do wskaźnika

                                // Podzial po wierszach obrazu
                                int rowsPerThread = height / threadCount;
                                int[] startRows = new int[threadCount];
                                int[] endRows = new int[threadCount];

                                for (int i = 0; i < threadCount; i++) {
                                    startRows[i] = i * rowsPerThread;
                                    endRows[i] = (i == threadCount - 1) ? height : (i + 1) * rowsPerThread;
                                }
                                // Konwertuje wiersze na bajty
                                int[] start = startRows.Select(row => row * width * 4).ToArray();
                                int[] end = endRows.Select(row => row * width * 4).ToArray();
                                Parallel.For(0, threadCount, i => {
                                    if (radioCSharp.Checked) {
                                        ImageFilterCS.ApplyMosaic(sourcePtr, resultPtr, height, width, start[i], end[i], tileSize);
                                    } else if (radioASM.Checked) {
                                        ApplyMosaicASM(sourcePtr, resultPtr, height, width, start[i], end[i], tileSize);
                                    }
                                });
                                Marshal.Copy(resultPtr, resultBuffer, 0, bytes); //Kopiowanie danych z wskaźnika do bufora
                                Marshal.Copy(resultBuffer, 0, filteredData.Scan0, bytes); //Kopiowanie danych z bufora do obrazu wynikowego

                            }
                            finally {

                                // Zwalnia pamięc
                                Marshal.FreeHGlobal(sourcePtr);
                                Marshal.FreeHGlobal(resultPtr);
                            }
                        }
                        finally {

                            if (sourceData != null) sourceBitmap.UnlockBits(sourceData);
                            if (filteredData != null) filteredBitmap.UnlockBits(filteredData);
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

                // Konfiguracja okna dialogowego zapisu
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

                    // Wybierz odpowiedni format na podstawie rozszerzenia
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

        private void testBtn_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(selectedImageLocation) || imageBeforeFilter.Image == null) {
                MessageBox.Show("Proszę najpierw załadować obraz.",
                    "Brak obrazu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                const int TEST_RUNS = 5;
                int[] threadCounts = { 1, 2, 4, 8, 16, 32, 64 };

                using (Bitmap sourceBitmap = new Bitmap(imageBeforeFilter.Image)) {
                    int width = sourceBitmap.Width;
                    int height = sourceBitmap.Height;

                    StringBuilder results = new StringBuilder();
                    results.AppendLine($"Test wydajności - {DateTime.Now:yyyy-MM-dd HH:mm:ss} (UTC)");
                    results.AppendLine($"Wymiary obrazu: {width}x{height}");
                    results.AppendLine($"Rozmiar kafelka: {mosaicPower.Value}px"); 
                    results.AppendLine($"Liczba powtórzeń testu: {TEST_RUNS}");
                    results.AppendLine(new string('-', 50));
                    results.AppendLine("\nWyniki testów:");
                    results.AppendLine("Wątki\tASM[ms]\tC#[ms]\tPrzyspieszenie");
                    results.AppendLine(new string('-', 50));

                    using (Bitmap filteredBitmap = new Bitmap(sourceBitmap)) {
                        BitmapData sourceData = null;
                        BitmapData filteredData = null;

                        try {
                            sourceData = sourceBitmap.LockBits(
                                new Rectangle(0, 0, width, height),
                                ImageLockMode.ReadOnly,
                                PixelFormat.Format32bppRgb);

                            filteredData = filteredBitmap.LockBits(
                                new Rectangle(0, 0, width, height),
                                ImageLockMode.WriteOnly,
                                PixelFormat.Format32bppRgb);

                            int stride = sourceData.Stride;
                            int bytes = Math.Abs(stride) * height;
                            byte[] sourceBuffer = new byte[bytes];
                            byte[] resultBuffer = new byte[bytes];
                            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, bytes);

                            IntPtr sourcePtr = Marshal.AllocHGlobal(bytes);
                            IntPtr resultPtr = Marshal.AllocHGlobal(bytes);

                            try {
                                int tileSize = mosaicPower.Value;

                                foreach (int threadCount in threadCounts) {
                                    long[] asmTimes = new long[TEST_RUNS];
                                    long[] csharpTimes = new long[TEST_RUNS];

                                    int rowsPerThread = height / threadCount;
                                    int[] startRows = new int[threadCount];
                                    int[] endRows = new int[threadCount];

                                    for (int i = 0; i < threadCount; i++) {
                                        startRows[i] = i * rowsPerThread;
                                        endRows[i] = (i == threadCount - 1) ? height : (i + 1) * rowsPerThread;
                                    }

                                    int[] start = startRows.Select(row => row * width * 4).ToArray();
                                    int[] end = endRows.Select(row => row * width * 4).ToArray();

                                    // Testy ASM
                                    for (int i = 0; i < TEST_RUNS; i++) {
                                        Marshal.Copy(sourceBuffer, 0, sourcePtr, bytes);
                                        Marshal.Copy(sourceBuffer, 0, resultPtr, bytes);

                                        var sw = Stopwatch.StartNew();
                                        Parallel.For(0, threadCount, new ParallelOptions {
                                            MaxDegreeOfParallelism = threadCount
                                        }, j =>
                                        {
                                            ApplyMosaicASM(sourcePtr, resultPtr, height, width,
                                                start[j], end[j], tileSize);
                                        });
                                        sw.Stop();
                                        asmTimes[i] = sw.ElapsedMilliseconds;
                                    }

                                    // Testy C#
                                    for (int i = 0; i < TEST_RUNS; i++) {
                                        Marshal.Copy(sourceBuffer, 0, sourcePtr, bytes);
                                        Marshal.Copy(sourceBuffer, 0, resultPtr, bytes);

                                        var sw = Stopwatch.StartNew();
                                        Parallel.For(0, threadCount, new ParallelOptions {
                                            MaxDegreeOfParallelism = threadCount
                                        }, j =>
                                        {
                                            ImageFilterCS.ApplyMosaic(sourcePtr, resultPtr, height, width,
                                                start[j], end[j], tileSize);
                                        });
                                        sw.Stop();
                                        csharpTimes[i] = sw.ElapsedMilliseconds;
                                    }

                                    double asmAvg = asmTimes.Average();
                                    double csharpAvg = csharpTimes.Average();
                                    double speedup = csharpAvg / asmAvg;

                                    results.AppendLine(
                                        $"{threadCount,2}\t{asmAvg,6:F1}\t{csharpAvg,6:F1}\t{speedup,7:F2}x");
                                }

                                results.AppendLine(new string('-', 50));

                                // Wyświetl wyniki
                                MessageBox.Show(results.ToString(), "Wyniki testów",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Zapisz wyniki do pliku
                                string logFileName = $"benchmark_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                                File.WriteAllText(logFileName, results.ToString());
                            }
                            finally {
                                Marshal.FreeHGlobal(sourcePtr);
                                Marshal.FreeHGlobal(resultPtr);
                            }
                        }
                        finally {
                            if (sourceData != null)
                                sourceBitmap.UnlockBits(sourceData);
                            if (filteredData != null)
                                filteredBitmap.UnlockBits(filteredData);
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Wystąpił błąd podczas testów:\n{ex.Message}",
                    "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Metoda pomocnicza do obliczania odchylenia standardowego
        private double CalculateStdDev(long[] values, double mean) {
            double sumOfSquaresOfDifferences = values.Select(val =>
                (val - mean) * (val - mean)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / values.Length);
        }

    }




}
