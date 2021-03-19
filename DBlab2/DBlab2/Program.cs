using System;
using DBLab2.Common;

namespace DBLab2 {
    internal static class Program {
        internal static void Main(string[] args) {
            SetupConsole();

            Printer.Debug("Debug-only info");
            Printer.Info("Press any key to exit...");
            Printer.Error(new Exception("Oh fuck"), ">:(");
            Console.ReadKey();
        }

        private static void SetupConsole() {
            // TODO?
        }
    }
}