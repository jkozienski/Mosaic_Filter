using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MosaicFilterProject {
    internal static class Program {
        [STAThread] // Atrybut powinien byæ przed metod¹ Main
        static void Main() {
            // Uruchomienie aplikacji Windows Forms
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}