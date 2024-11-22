namespace MosaicFilterProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e)
{
    string imageLocation = "";
    try
    {
        // Tworzymy obiekt dialogu do wyboru pliku
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "PNG files(*.png)|*.png";  // Tylko pliki PNG

        // Sprawdzamy, czy u¿ytkownik wybra³ plik
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            imageLocation = dialog.FileName;

            // Przypisujemy œcie¿kê do obrazu w PictureBox
            imageBeforeFilter.ImageLocation = imageLocation;
        }
    }
    catch (Exception ex)
    {
        // W przypadku b³êdu wyœwietlamy komunikat
        MessageBox.Show($"Wyst¹pi³ b³¹d: {ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
    }
}
