using System;
using DBLab2.Common;
using DBLab2.ConsoleController;

namespace DBLab2 {
    internal static class Program {
        internal static void Main(string[] args) {
            if (args.Length < 1 || args[0].Equals("console", StringComparison.InvariantCultureIgnoreCase)) {
                ConsoleApp.Run();
                Printer.Debug("Exiting...");
                Console.Read();
            }
            else {
                ;
            }
        }
    }
}
