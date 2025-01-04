using System;
using System.CodeDom;
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

                    using (Bitmap filteredBitmap = new Bitmap(sourceBitmap)) 
                    {
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

                            IntPtr sourcePtr = Marshal.AllocHGlobal(bytes); //Alokacja pamięci dla wskaźnika do obrazu źródłowego
                            IntPtr resultPtr = Marshal.AllocHGlobal(bytes); //Alokacja pamięci dla wskaźnika do obrazu wynikowego

                            try {
                                Marshal.Copy(sourceBuffer, 0, sourcePtr, bytes); //Kopiowanie danych z bufora do wskaźnika
                                Marshal.Copy(sourceBuffer, 0, resultPtr, bytes); //Kopiowanie danych z bufora do wskaźnika

                                int bytesPerThread = bytes / threadCount; //Liczba bajtów na wątek
                                int[] start = new int[threadCount];
                                int[] end = new int[threadCount];
                                for (int i = 0; i < threadCount; i++) {
                                    start[i] = i * bytesPerThread;
                                    end[i] = (i == threadCount - 1) ? bytes : (i + 1) * bytesPerThread;
                                }

                                //Parallel.For(0, threadCount, i =>
                                //{
                                if (radioCSharp.Checked) {
                                    ImageFilterCS.ApplyMosaic(sourcePtr, resultPtr,height,width,0,0,tileSize);
                                } else if(radioASM.Checked) {
                                    ApplyMosaicASM(sourcePtr, resultPtr, height, width, 0, 0, tileSize);
                                }
                                // });
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

    }
}
