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

        // Sprawdzamy, czy u�ytkownik wybra� plik
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            imageLocation = dialog.FileName;

            // Przypisujemy �cie�k� do obrazu w PictureBox
            imageBeforeFilter.ImageLocation = imageLocation;
        }
    }
    catch (Exception ex)
    {
        // W przypadku b��du wy�wietlamy komunikat
        MessageBox.Show($"Wyst�pi� b��d: {ex.Message}", "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
    }
}
