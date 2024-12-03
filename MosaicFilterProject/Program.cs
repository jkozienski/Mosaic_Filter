using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MosaicFilterProject {
    internal static class Program {
        //[DllImport(@"C:\Users\Jakub\source\repos\Sem V\MosaicFilter\MosaicFilterProject\x64\Debug\FilterAsm.dll")] //laptop
        //[DllImport(@"C:\Users\Jakub\source\repos\Repozytorium Sem 5\JA\MosaicFilter\x64\Debug\FilterAsm.dll")]  //komputer

        static extern int imageFilterAsm(int a, int b);
                [STAThread] // Atrybut powinien byæ przed metod¹ Main
        static void Main() {
            // Uruchomienie aplikacji Windows Forms
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}