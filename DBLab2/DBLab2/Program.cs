using System;
using System.IO;
using DBLab2.Common;
using DBLab2.ConsoleController;
using DBLab2.DBLogic;

namespace DBLab2 {
    internal static class Program {
        private static readonly string DefaultLibraryDb = AppDomain.CurrentDomain.BaseDirectory + "Library.db";

        internal static void Main(string[] args) {

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            if (args.Length >= 1 && File.Exists(args[0])) {
                SqliteAdapter.TrySetDatabase(args[0]);
            }
            else {
                SqliteAdapter.TrySetDatabase(DefaultLibraryDb);
            }

            if (args.Length < 2 || args[1].Equals("console", StringComparison.InvariantCultureIgnoreCase)) {
                Printer.Info("DB selected: {0}", GlobalContainer.BdSelected);
                ConsoleApp.Run();
                Printer.Debug("Press any button to exit...");
                Console.Read();
            }
            else {
                // TBA: GUI Application
            }
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Printer.Error("Application unexpectedly terminated due to unhandled exception");
            Printer.Error((Exception)e.ExceptionObject, "The exception in question:");
            Environment.Exit(-1);
        }
    }
}
