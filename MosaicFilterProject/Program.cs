using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MosaicFilterProject {
    internal static class Program {
        [STAThread] // Atrybut powinien by� przed metod� Main
        static void Main() {
            // Uruchomienie aplikacji Windows Forms
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}