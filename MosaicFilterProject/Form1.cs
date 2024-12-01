using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FilterCSharp;  // Dodaj przestrze� nazw dla klasy imageFilterCS

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
        // Wywo�anie funkcji z CSharp
        //int res12 = FilterCSharp.ImageFilterCS.test();
        //MessageBox.Show($"CSLIB {res12}", "Wynik oblicze�");
        // Wywo�anie funkcji z ASM
        //int result = imageFilterAsm(12, 3);
        // MessageBox.Show($"Wynik z MyProc1: {result}", "Wynik oblicze�");

        private void button1_Click(object sender, EventArgs e) {
            try {
                // Tworze obiekt dialogu do wyboru pliku
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png";  // Tylko pliki PNG

                // Sprawdzay, czy u�ytkownik wybra� plik
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Wczytanie obrazu
                    Bitmap originalImage = new Bitmap(dialog.FileName);

                    // Ustawiam wczytany obraz w PictureBox
                    imageBeforeFilter.Image = originalImage;
                   
                    selectedImageLocation = dialog.FileName;
                    selectedImageName = System.IO.Path.GetFileName(dialog.FileName);
                }
            }
            catch (Exception ex) {
                
                MessageBox.Show($"Wyst�pi� b��d: {ex.Message}", "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void filterButton_Click(object sender, EventArgs e) {
            try {
                // Sprawdzam, czy obraz jest za�adowany w PictureBox
                if (imageBeforeFilter.Image == null) {
                    MessageBox.Show("Prosz� za�adowa� obraz przed przetwarzaniem.", "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Pobieram obraz z PictureBox
                Bitmap originalImage = (Bitmap)imageBeforeFilter.Image;

                // Pobieram warto�� z suwaka mosaicPower
                int tileSize = mosaicPower.Value; 

                Bitmap mosaicImage = ImageFilterCS.ApplyMosaic(originalImage, tileSize);

                // Ustawiam zmodyfikowany obraz w PictureBox
                imageAfterFilter.Image = mosaicImage;

                // Wy�wietlam komunikat o zako�czeniu pracy 
                MessageBox.Show($"Obraz zosta� przekszta�cony w mozaik� z kafelkami o rozmiarze {tileSize}x{tileSize}.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                MessageBox.Show($"Wyst�pi� b��d: {ex.Message}", "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
