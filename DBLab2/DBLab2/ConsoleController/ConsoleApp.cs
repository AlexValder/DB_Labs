using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DBLab2.Common;

namespace DBLab2.ConsoleController {
    public static class ConsoleApp {
        private static readonly ImmutableDictionary<string, Action> Actions =
            new Dictionary<string, Action> {
            ["SELECT"] = () => { SqlParser.ParseSelect(_input); },
            ["UPDATE"] = () => { SqlParser.ParseUpdate(_input); },
            ["INSERT"] = () => { SqlParser.ParseInsert(_input); },
            ["DELETE"] = () => { SqlParser.ParseDelete(_input); },
            ["help"]   = () => { Printer.Info("We're sorry :(");},
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
    }
}
