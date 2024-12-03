using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FilterCSharp;  // Dodaj przestrzeń nazw dla klasy imageFilterCS

namespace MosaicFilterProject {
    public partial class Form1 : Form {
        // Deklaracja funkcji z DLL ASM
        //[DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")]//laptop
        [DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]//komputer

        static extern int imageFilterAsm(int a, int b);
        //private static extern int filterProc2(int x, int y);



        private string selectedImageLocation;
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void label1_Click(object sender, EventArgs e) {
        }
        // Wywołanie funkcji z CSharp
        //int res12 = FilterCSharp.ImageFilterCS.test();
        //MessageBox.Show($"CSLIB {res12}", "Wynik obliczeń");
        // Wywołanie funkcji z ASM
        //int result = imageFilterAsm(12, 3);
        // MessageBox.Show($"Wynik z MyProc1: {result}", "Wynik obliczeń");
        // Wywołanie funkcji z ASM

        private void button1_Click(object sender, EventArgs e) {
            try {
                // Tworzymy obiekt dialogu do wyboru pliku
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png|JPEG files(*.jpeg)|*.jpeg|JPG files(*.jpg)|*.jpg|All files(*.*)|*.*";  // Filtry dla różnych typów plików

                // Sprawdzamy, czy użytkownik wybrał plik
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Bitmap originalImage = new Bitmap(dialog.FileName);

                    imageBeforeFilter.Image = originalImage;

                    selectedImageLocation = dialog.FileName;
                    locationTextBox.Text = selectedImageLocation; 

                }
            }
            catch (Exception ex) {
                // W przypadku błędu, wyświetl komunikat
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void filterButton_Click(object sender, EventArgs e) {
            try {
                if (imageBeforeFilter.Image == null) {
                    MessageBox.Show("Proszę załadować obraz przed przetwarzaniem.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                Bitmap originalImage = (Bitmap)imageBeforeFilter.Image;

                int tileSize = mosaicPower.Value; 
                if (radioCSharp.Checked) {
                    Bitmap mosaicImage = ImageFilterCS.ApplyMosaic(originalImage, tileSize);
                    imageAfterFilter.Image = mosaicImage;
                    MessageBox.Show($"Obraz został przekształcony w mozaikę z kafelkami o rozmiarze {tileSize}x{tileSize}.", "Wybrano C#", MessageBoxButtons.OK, MessageBoxIcon.Information);

                } else {
                        int result = imageFilterAsm(12, 3); 
                        MessageBox.Show($"Użyto biblioteki ASM, wynik: {result}", "Wybrano Asm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex) {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
        }

        private void bottomPanel_Paint(object sender, PaintEventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
        }

        private void mosaicPower_Scroll(object sender, EventArgs e) {

        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void clearImage_Click(object sender, EventArgs e) {
            imageBeforeFilter.Image = null;

            locationTextBox.Clear();

            selectedImageLocation = string.Empty;
        }
    }
}
