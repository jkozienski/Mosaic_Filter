using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FilterCSharp;  // Dodaj przestrzeñ nazw dla klasy imageFilterCS

namespace MosaicFilterProject {
    public partial class Form1 : Form {
        // Deklaracja funkcji z DLL ASM
        [DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")]
        static extern int imageFilterAsm(int a, int b);


        private string selectedImageLocation;
        private string selectedImageName;
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void label1_Click(object sender, EventArgs e) {
        }
        // Wywo³anie funkcji z CSharp
        //int res12 = FilterCSharp.ImageFilterCS.test();
        //MessageBox.Show($"CSLIB {res12}", "Wynik obliczeñ");
        // Wywo³anie funkcji z ASM
        //int result = imageFilterAsm(12, 3);
        // MessageBox.Show($"Wynik z MyProc1: {result}", "Wynik obliczeñ");

        private void button1_Click(object sender, EventArgs e) {
            try {
                // Tworzymy obiekt dialogu do wyboru pliku
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png";  // Tylko pliki PNG

                // Sprawdzamy, czy u¿ytkownik wybra³ plik
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Wczytanie obrazu
                    Bitmap originalImage = new Bitmap(dialog.FileName);

                    // Ustawiamy wczytany obraz w PictureBox
                    imageBeforeFilter.Image = originalImage;
                    // Mo¿esz zapisaæ lokalizacjê i nazwê pliku, jeœli potrzebujesz
                    selectedImageLocation = dialog.FileName;
                    selectedImageName = System.IO.Path.GetFileName(dialog.FileName);
                }
            }
            catch (Exception ex) {
                // W przypadku b³êdu, wyœwietl komunikat
                MessageBox.Show($"Wyst¹pi³ b³¹d: {ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void filterButton_Click(object sender, EventArgs e) {
            try {
                // Pobieramy obraz z PictureBox (zak³adaj¹c, ¿e obraz jest ju¿ za³adowany)
                Bitmap originalImage = (Bitmap)imageBeforeFilter.Image;

                // Wywo³anie metody z imageFilterCS, która zmienia obraz na odcienie czerwone
                Bitmap redScaleImage = ImageFilterCS.ApplyRedScale(originalImage);

                // Ustawiamy zmodyfikowany obraz w PictureBox
                imageAfterFilter.Image = redScaleImage;

                // Wyœwietlamy komunikat o zakoñczeniu przetwarzania
                MessageBox.Show("Obraz zosta³ przekszta³cony na odcienie czerwone.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                // Obs³uga b³êdów, gdy obraz nie jest za³adowany lub wyst¹pi inny problem
                MessageBox.Show($"Wyst¹pi³ b³¹d: {ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
        }

        private void bottomPanel_Paint(object sender, PaintEventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }
    }
}
