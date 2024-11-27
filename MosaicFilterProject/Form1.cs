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
                // Tworze obiekt dialogu do wyboru pliku
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "PNG files(*.png)|*.png";  // Tylko pliki PNG

                // Sprawdzay, czy u¿ytkownik wybra³ plik
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
                
                MessageBox.Show($"Wyst¹pi³ b³¹d: {ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void filterButton_Click(object sender, EventArgs e) {
            try {
                // Sprawdzam, czy obraz jest za³adowany w PictureBox
                if (imageBeforeFilter.Image == null) {
                    MessageBox.Show("Proszê za³adowaæ obraz przed przetwarzaniem.", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Pobieram obraz z PictureBox
                Bitmap originalImage = (Bitmap)imageBeforeFilter.Image;

                // Pobieram wartoœæ z suwaka mosaicPower
                int tileSize = mosaicPower.Value; 

                Bitmap mosaicImage = ImageFilterCS.ApplyMosaic(originalImage, tileSize);

                // Ustawiam zmodyfikowany obraz w PictureBox
                imageAfterFilter.Image = mosaicImage;

                // Wyœwietlam komunikat o zakoñczeniu pracy 
                MessageBox.Show($"Obraz zosta³ przekszta³cony w mozaikê z kafelkami o rozmiarze {tileSize}x{tileSize}.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
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
