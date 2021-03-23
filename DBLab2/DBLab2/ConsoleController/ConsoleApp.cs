using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DBLab2.Common;

namespace DBLab2.ConsoleController {
    public static class ConsoleApp {
        private static readonly SqlParser Parser = new SqlParser();
        private static readonly ImmutableDictionary<string, Action> Actions =
            new Dictionary<string, Action> {
            ["SELECT"] = () => { Parser.ParseSelect(_input); },
            ["ADD"]    = () => { Parser.ParseAdd(_input); },
            ["INSERT"] = () => { Parser.ParseInsert(_input); },
            ["DELETE"] = () => { Parser.ParseDelete(_input); },
            ["Exit"]   = () => { _exit = true; }
        }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

        public static void Run() {
            MainLoop();
        }

        
        private static bool _exit = false;
        private static string _input = "";
        private static void MainLoop() {
            while (!_exit) {
                Printer.Info("Enter command: ");
                _input = Console.ReadLine() ?? string.Empty;
                if (_input == string.Empty) continue;

                var input = _input[0.._input.IndexOf(' ')] ?? string.Empty;
                
                if (Actions.ContainsKey(input)) {
                    Actions[input].Invoke();
                }
                else {
                    Printer.Error("Invalid command");
                }
            }
        }
    }
}
