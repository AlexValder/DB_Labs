using System;
using System.IO;
using Avalonia.Controls;
using AvaloniaGUI;
using DBLab2.Common;
using DBLab2.ConsoleController;
using DBLab2.DBLogic;

namespace DBLab2 {
    internal static class Program {
        private static readonly string DefaultLibraryDb = AppDomain.CurrentDomain.BaseDirectory + "Library.db";

        [STAThread]
        internal static void Main(string[] args) {

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            bool success;
            if (args.Length >= 1 && File.Exists(args[0])) {
                success = SqliteAdapter.TrySetDatabase(args[0]);
            }
            else {
                success = SqliteAdapter.TrySetDatabase(DefaultLibraryDb);
            }

            if (!success) {
                Printer.Error("Failed to load DB.");
            }

            if (args.Length < 1 || args[0].Equals("console", StringComparison.OrdinalIgnoreCase)) {
                Printer.Info("DB selected: {0}", GlobalContainer.BdSelected);
                ConsoleApp.Run();
                Printer.Debug("Press any button to exit...");
                Console.Read();
            }
            else if (args.Length >= 1 && args[0].Equals("gui", StringComparison.OrdinalIgnoreCase)) {
                AvaloniaGUI.Program.Main(Array.Empty<string>());
            }
            else {
                Printer.Error("Invalid arguments. Acceptable arguments: none, 'console' or 'gui'.");
                Environment.Exit(-2);
            }
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Printer.Error("Application unexpectedly terminated due to unhandled exception");
            Printer.Error((Exception)e.ExceptionObject, "The exception in question:");
            Environment.Exit(-1);
        }
    }
}
