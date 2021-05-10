using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Common;
using DBLab2.DBLogic;

namespace DBLab2.ConsoleController {
    public static class ConsoleApp {

        private const string HELP_TEXT
            = "Welcome to our basic SQL Application!\n\n" +
              "To execute supported SQL commands (SELECT, INSERT, UPDATE, DELETE), " +
              "just type the command into the command prompt.\n" +
              "Available commands:\n" +
              "load <path>\t- load another DB-file;\n" +
              "list\t\t- prints names of all available tables;\n" +
              "show <table>\t- prints names of fields in the table;\n" +
              "help\t\t- print this message;\n" +
              "exit\t\t- close the application.\n\n" +
              "Command syntax:\n\n" +
              "- SELECT:\n" +
              "\tYou will be asked to enter the table, fields to print and conditions the feilds must correspond." +
              "\n- INSERT:\n" +
              "\tYou will be asked to enter the table, fields and their values needed for insertion." +
              "\n- UPDATE:\n" +
              "\tYou will be asked to enter the table, fields to modify and then condition, the fields to be modified must correspond." +
              "\n- DELETE:\n" +
              "\tYou will be asked to enter the table and the condition, the stuff you're deleting must correspond.\n";

        private static readonly ImmutableDictionary<string, Action> Actions =
            new Dictionary<string, Action> {
            ["SELECT"] = SqlConstructor.CreateSelect,
            ["UPDATE"] = SqlConstructor.CreateUpdate,
            ["INSERT"] = SqlConstructor.CreateInsert,
            ["DELETE"] = SqlConstructor.CreateDelete,
            [ "load" ] = () => { LoadTable(_input!); },
            [ "list" ] = ListTables,
            [ "show" ] = () => { ShowFields(_input!); },
            [ "help" ] = () => { Console.WriteLine(HELP_TEXT); },
            [ "exit" ] = () => { _exit = true; }
        }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

        private static bool _exit;
        private static string _input = "";

        private const StringComparison COMPARISON = StringComparison.OrdinalIgnoreCase;

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
                        if (input.Contains("select", COMPARISON) ||
                            input.Contains("update", COMPARISON) ||
                            input.Contains("insert", COMPARISON) ||
                            input.Contains("delete", COMPARISON)) {
                            Printer.Info("Don't enter full SQL request. This command will be interpreted as \"{0}\"",
                                input.ToUpper());
                        }
                    }
                    else {
                        input = _input;
                    }
                }
                catch {
                    input = string.Empty;
                }

                if (Actions.ContainsKey(input)) {
                    try {
                        Actions[input].Invoke();
                    }
                    catch (Exception ex) {
                        Printer.Error("Failed to execute command:\n{0}", ex.Message);
                    }
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
            string actualName = name[(name.IndexOf(' ') + 1)..];
            if (!GlobalContainer.TableExists(actualName)) {
                Printer.Error("No such table: {0}", actualName);
                return;
            }
            Console.Write("\n\t");
            Console.WriteLine(string.Join("\n\t", GlobalContainer.Fields(actualName)));
            Console.WriteLine();
        }

        private static void LoadTable(string input) {
            if (SqliteAdapter.TrySetDatabase(input)) {
                Printer.Success("New database selected: {0}", Path.GetFileName(input));
            }
            else {
                Printer.Error("No new database was selected.");
            }
        }
    }
}
