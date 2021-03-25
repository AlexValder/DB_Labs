using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DBLab2.Common;

namespace DBLab2.ConsoleController {
    public static class ConsoleApp {
        private static readonly ImmutableDictionary<string, Action> Actions =
            new Dictionary<string, Action> {
            ["SELECT"] = () => { SqlParser.ParseSelect(_input); },
            ["UPDATE"] = () => { SqlParser.ParseUpdate(_input); },
            ["INSERT"] = () => { SqlParser.ParseInsert(_input); },
            ["DELETE"] = () => { SqlParser.ParseDelete(_input); },
            ["list"]   = ListTables,
            ["show"]   = ShowFields,
            ["help"]   = PrintHelp,
            ["exit"]   = () => { _exit = true; }
        }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

        private static bool _exit;
        private static string _input = "";

        public static void Run() {
            MainLoop();
        }

        private static void MainLoop() {
            while (!_exit) {
                Printer.Info("Enter command: ");
                _input = Console.ReadLine() ?? string.Empty;
                if (_input == string.Empty) continue;

                string input;
                try {
                    if (_input.Contains(' ')) {
                        input = _input[0.._input.IndexOf(' ')] ?? string.Empty;
                    }
                    else {
                        input = _input;
                    }
                }
                catch {
                    input = string.Empty;
                }

                if (Actions.ContainsKey(input)) {
                    Actions[input].Invoke();
                }
                else {
                    Printer.Error("Invalid command");
                }
            }
        }

        private static void ListTables() {
            Console.WriteLine();
            Printer.Info("AVAILABLE TABLES:");
            Console.WriteLine(string.Join("\n", GlobalContainer.Tables));
            Console.WriteLine();
        }

        private static void ShowFields() {
            Console.WriteLine();
            Printer.Info("Enter table name:");
            var name = Console.ReadLine() ?? string.Empty;
            if (!GlobalContainer.TableExists(name)) {
                Printer.Error("No such table");
                return;
            }
            Console.Write("\n\t");
            Console.WriteLine(string.Join("\n\t", GlobalContainer.Fields(name)));
            Console.WriteLine();
        }

        private static void PrintHelp() {
            Console.WriteLine("We're (not) sorry");
        }
    }
}
