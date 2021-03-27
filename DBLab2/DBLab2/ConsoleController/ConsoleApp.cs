using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using DBLab2.Common;

namespace DBLab2.ConsoleController {
    public static class ConsoleApp {

        private const string HELP_TEXT = "Welcome to our basic SQL Application!\n\n" +
                                         "To execute supported SQL commands (SELECT, INSERT INTO, UPDATE, DELETE), " +
                                         "just type the command into the command prompt.\n" +
                                         "Available commands:\n" +
                                         "load <path> - load another DB-file;" +
                                         "list - prints names of all available tables;\n" +
                                         "list <table> - prints names of fields in the table;\n" +
                                         "help - you're here!\n" +
                                         "exit - close the application.\n\n" +
                                         "Command syntax:\n\n" +
                                         "- SELECT:\n" +
                                         "\tSELECT <fields> FROM <table>\n" +
                                         "\tSELECT * FROM <table>\n" +
                                         "- INSERT INTO:\n" +
                                         "\tINSERT INTO <table> (column1, column2, ...) VALUES (value1, value2, " +
                                         "...)\n" +
                                         "\tINSERT INTO <table> VALUES (value1, value2, ...)\n" +
                                         "- UPDATE:\n" +
                                         "\tUPDATE <table> SET field1=value1, field2=value2, ... WHERE <condition>\n" +
                                         "\tUPDATE <table> SET field1=value1, field2=value2\n" +
                                         "- DELETE:\n" +
                                         "\tDELETE FROM <table>\n" +
                                         "\tDELETE FROM <table> WHERE <condition>\n\n";

        private static readonly ImmutableDictionary<string, Action> Actions =
            new Dictionary<string, Action> {
            ["SELECT"] = () => { SqlParser.ParseSelect(_input!); },
            ["UPDATE"] = () => { SqlParser.ParseUpdate(_input!); },
            ["INSERT"] = () => { SqlParser.ParseInsert(_input!); },
            ["DELETE"] = () => { SqlParser.ParseDelete(_input!); },
            ["load"]   = () => { LoadTable(_input); },
            ["list"]   = ListTables,
            ["show"]   = () => { ShowFields(_input); },
            ["help"]   = () => { Console.WriteLine(HELP_TEXT); },
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
                if (_input == string.Empty) {
                    continue;
                }

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

        private static void ShowFields(string name) {
            Console.WriteLine();
            if (!GlobalContainer.TableExists(name)) {
                Printer.Error("No such table: {0}", name);
                return;
            }
            Console.Write("\n\t");
            Console.WriteLine(string.Join("\n\t", GlobalContainer.Fields(name)));
            Console.WriteLine();
        }

        private static void LoadTable(string input) {
            // TODO:
            // 1. Validate input (can it be null?)
            // 2. Load table into GlobalContainer

            Printer.Info("DB selected: {0}", GlobalContainer.BdSelected);
        }
    }
}
