using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MosaicFilterProject {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")]
        static extern int MyProc1(int a, int b);

        [STAThread] // Atrybut powinien byæ przed metod¹ Main
        static void Main() {
            // Najpierw wywo³ujemy funkcjê MyProc1
            int result = MyProc1(12, 3);
            Console.WriteLine(result);  // Wypisuje wynik w konsoli (tylko w wersji konsolowej)
            MessageBox.Show($"Wynik: {result}", "Wynik obliczeñ");

            // Uruchomienie aplikacji Windows Forms
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}