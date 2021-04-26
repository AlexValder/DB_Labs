using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.SqlCommands;
using Conditions = System.ValueTuple<string, Common.SqlCommands.Operation, string>;
using SqliteAdapter = DBLab2.DBLogic.SqliteAdapter;

namespace DBLab2.ConsoleController {
    public static class SqlConstructor {

        private const StringSplitOptions OPTIONS =
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries;

        private const StringComparison STR_COMPARISON = StringComparison.OrdinalIgnoreCase;
        private const string RETURN = "@back";

        private static readonly char[] Ops = {'=', '>', '<'};

        private static void PrintHint() => Printer.Debug($"You can return to main menu anytime by typing '{RETURN}'");

        public static void CreateSelect() {
            PrintHint();

           if (!ParseTableName(out string table)) {
                return;
            }

            if (!ParseFields(table, out var fields)) {
                return;
            }

            if (!ParseConditions(table, out var where)) {
                return;
            }

            var command = new SqlSelect(
                table,
                fields,
                where == null || where.Count == 0 ? null : where
            );

            Printer.Debug("Resulting command:");
            Printer.Success(command.Execute());
            Printer.PrintTable(SqliteAdapter.Select(command));
        }

        public static void CreateUpdate() {
            PrintHint();

            if (!ParseTableName(out var table)) {
                return;
            }

            if (!ParseFieldValuePairs(table, out var set)) {
                return;
            }

            if (!ParseConditions(table, out var where)) {
                return;
            }

            var command = new SqlUpdate(
                table,
                set!.Select(pair => (pair.Key, pair.Value)).ToList(),
                where
            );

            Printer.Debug("Resulting command:");
            Printer.Success(command.Execute());
            SqliteAdapter.Update(command);
        }

        public static void CreateInsert() {
            PrintHint();

            if (!ParseTableName(out var table)) {
                return;
            }

            if (!ParseFieldValuePairs(table, out var set, true)) {
                return;
            }

            var command = new SqlInsertInto(
                table,
                set?.Values.ToList() ?? new List<string>(),
                set?.Keys.ToList() ?? new List<string>()
            );

            Printer.Debug("Resulting command:");
            Printer.Success(command.Execute());
            SqliteAdapter.InsertInto(command);
        }

        public static void CreateDelete() {
            PrintHint();

            if (!ParseTableName(out var table)) {
                return;
            }

            if (!ParseConditions(table, out var where)) {
                return;
            }

            if (where == null || where.Count == 0) {
                Console.WriteLine();
                Printer.Info("WARNING! You are going to delete all entries in the table. " +
                             "Please confirm [default N] (Y/N)");
                if (Console.ReadKey(true).Key != ConsoleKey.Y) {
                    Printer.Error("Canceled");
                    return;
                }
            }

            var command = new SqlDelete(
                table,
                where
            );

            Printer.Debug("Resulting command:");
            Printer.Success(command.Execute());
            SqliteAdapter.Delete(command);
        }

        private static bool ParseTableName(out string table) {
            var availableTables = GlobalContainer.Tables;
            Console.WriteLine();
            Printer.Debug("Available tables: {0}", string.Join(", ", availableTables));

            Printer.Info("Enter the table name: ");
            while (true) {
                string input = Console.ReadLine() ?? "";
                table = input.Replace(" ", "");

                if (RETURN.Equals(table, STR_COMPARISON)) {
                    Printer.Error("Canceled");
                    return false;
                }

                if (GlobalContainer.TableExists(table)) {
                    break;
                }

                Printer.Error("No such table: {0}", table);
            }
            return true;
        }

        private static bool ParseFields(in string table, out List<string>? fields) {
            var availableFields = GlobalContainer.Fields(table);
            Console.WriteLine();
            Printer.Debug("Available fields: {0}", string.Join(", ", availableFields));

            Printer.Info("Please enter the names of fields you want to select, " +
                         "separating them by comma or put `*` for all fields: ");

            while (true) {
                string input = Console.ReadLine() ?? "";
                fields = input.Replace(" ", "").Split(',', OPTIONS).ToList();

                if (fields.Count == 1 && RETURN.Equals(fields[0], STR_COMPARISON)) {
                    Printer.Error("Canceled");
                    return false;
                }

                if (fields.Count == 1 && fields[0] == "*") {
                    fields = null;
                    break;
                }

                if (fields.Count <= GlobalContainer.FieldCount(table) &&
                    GlobalContainer.FieldsExist(table, fields)) {
                    break;
                }

                Printer.Error("No such fields.");
            }
            return true;
        }

        private static bool ParseConditions(in string table, out List<Conditions>? where) {
            Console.WriteLine();
            Printer.Debug("Enter condition or hit Enter on an empty string to finish.");
            where = new List<(string, Operation, string)>();
            string input;
            while ((input = Console.ReadLine() ?? "") != "") {
                var condition = input.Replace(" ", "");

                if (RETURN.Equals(condition, STR_COMPARISON)) {
                    Printer.Error("Cancelled");
                    return false;
                }

                var opInd = condition.IndexOfAny(Ops);
                var lastOpInd = condition.LastIndexOfAny(Ops);

                if (!(0 < opInd && opInd <= lastOpInd && lastOpInd < condition.Length)) {
                    Printer.Error("Invalid condition");
                    continue;
                }

                var col = condition[..opInd] ?? string.Empty;
                var op = Extension.ToOperation(condition[opInd..(lastOpInd + 1)]);
                var val = condition[(lastOpInd + 1)..] ?? string.Empty;

                if (!GlobalContainer.FieldExists(table, col)) {
                    Printer.Error("Invalid field: {0}", col);
                    continue;
                }

                where.Add((col, op, val));
            }

            if (where.Count == 0) {
                where = null;
            }
            return true;
        }

        private static bool ParseFieldValuePairs(in string table, out Dictionary<string, string>? set, bool all = false) {
            Console.WriteLine();
            var availableFields = GlobalContainer.Fields(table).ToList();
            availableFields.Remove("id");
            availableFields.Remove("ID");
            availableFields.Remove("Id");
            availableFields.Remove("iD");
            Printer.Debug("Available fields: {0}",
                string.Join(", ", availableFields));

            var setPairs = availableFields.ToDictionary(field => field,
                _ => false,
                StringComparer.OrdinalIgnoreCase
            );

            if (all) {
                Printer.Debug("Please enter values for ALL fields.");
            }
            else {
                Printer.Debug("At any moment you can enter an empty line to indicate you're done.");
            }

            set = new Dictionary<string, string>();
            while (true) {

                if (!setPairs.Values.Contains(false)) {
                    Printer.Info("All fields are set.");
                    break;
                }

                Printer.Info("Enter name of a field");
                var field = (Console.ReadLine() ?? "").Replace(" ", "");

                if (field == "") {
                    if (!all || !setPairs.Values.Contains(false)) {
                        break;
                    }
                    Printer.Debug("You have to set all fields.");
                    continue;
                }

                if (RETURN.Equals(field, STR_COMPARISON)) {
                    Printer.Error("Cancelled");
                    return false;
                }

                if (!GlobalContainer.FieldExists(table, field)) {
                    Printer.Error("No such field");
                    continue;
                }

                setPairs[field] = true;

                var value = (Console.ReadLine() ?? "").Replace("  ", " ");

                if (value == "") {
                    if (setPairs.Values.Contains(false)) {
                        Printer.Error("Please enter a valid value");
                        continue;
                    }
                    break;
                }

                if (RETURN.Equals(value, STR_COMPARISON)) {
                    Printer.Error("Cancelled");
                    return false;
                }

                set.Add(field, value);
            }

            if (set.Count == 0) {
                Printer.Error("No fields to be set. Cancelled");
                return false;
            }

            return true;
        }
    }
}
