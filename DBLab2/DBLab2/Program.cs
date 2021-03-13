using System;
using DBLab2.Common;

namespace DBLab2 {
    internal static class Program {
        internal static void Main(string[] args) {
            SetupConsole();

            Printer.Info("Press any key to exit...");
            Console.ReadKey();
        }

        private static void SetupConsole() {
            // TODO?
        }
    }
}