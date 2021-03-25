using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DBLab2.Common;
using DBLab2.ConsoleController.SqlCommands;

namespace DBLab2.ConsoleController {
    internal static class SqlParser {

        private const StringSplitOptions OPTIONS =
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries;

        private const StringComparison STR_COMPARISON = StringComparison.InvariantCultureIgnoreCase;

        private static readonly char[] Ops = {'=', '>', '<'};

        /// <summary>
        /// Parses string in next format:
        /// `SELECT field1, field2 FROM table`
        /// `SELECT * FROM table`
        /// </summary>
        /// <param name="input">input string</param>
        public static void ParseSelect(in string input) {
            PrepareInput(input, out var betterInput);

            var words = betterInput.Split(' ', OPTIONS);

            if (words.Length != 4) {
                Printer.Error("Invalid number of words");
                return;
            }

            if (!"from".Equals(words[2], STR_COMPARISON)) {
                Printer.Error("Invalid string format");
                return;
            }

            if (!GlobalContainer.TableExists(words[3])) {
                Printer.Error("No such table: {0}", words[3]);
                return;
            }

            SqlSelect command;
            var fields = words[1].Split(',', OPTIONS);

            switch (fields.Length) {
                case 0:
                    Printer.Error("Failed to read fields");
                    return;
                case 1 when fields[0] == "*":
                    command = new SqlSelect(words[3]);
                    break;
                default: {
                    if (GlobalContainer.FieldsExist(words[3], fields)) {
                        command = new SqlSelect(words[3], fields);
                    }
                    else {
                        Printer.Error("Invalid fields.");
                        return;
                    }

                    break;
                }
            }

            Printer.Success(command.Execute());
        }

        /// <summary>
        /// Parses string in next format:
        /// `UPDATE table SET col1=val1, col2=val2,...`
        /// `UPDATE table SET col1=val1, col2=val2,... WHERE condition`
        /// </summary>
        /// <param name="input">input string</param>
        public static void ParseUpdate(in string input) {
            PrepareInput(input, out var betterInput);
            Printer.Debug("INPUT: {0}", betterInput);

            var words = betterInput.Split(' ', OPTIONS);

            if (!"set".Equals(words[2], STR_COMPARISON)) {
                Printer.Error("Invalid string format");
                return;
            }

            if (!GlobalContainer.TableExists(words[1])) {
                Printer.Error("No such table: {0}", words[3]);
                return;
            }

            SqlUpdate command;
            var sets = words[3].Split(',', OPTIONS);

            switch (sets.Length) {
                case 0:
                    Printer.Error("Failed to read new values");
                    return;
                default:
                    var setList = new List<(string, string)>();
                    foreach (var set in sets) {
                        var opInd = set.IndexOf('=');
                        if (opInd < 0 || opInd >= set.Length) {
                            Printer.Error("Invalid set statement");
                            return;
                        }

                        var col = set[..opInd] ?? string.Empty;
                        var val = set[(opInd + 1)..] ?? string.Empty;

                        if (!GlobalContainer.FieldExists(words[1], col)) {
                            Printer.Error("Invalid field: {0}", col);
                            return;
                        }

                        setList.Add((col, val));
                    }

                    if (words.Length > 5 && "where".Equals(words[4], STR_COMPARISON))
                    {
                        if (TryParseCondition(words[1], words[5], out var condList)) {
                            command = new SqlUpdate(words[1], setList, condList);
                        }
                        else {
                            Printer.Error("Invalid conditions.");
                            return;
                        }
                    }
                    else {
                        command = new SqlUpdate(words[1], setList);
                    }

                    break;
            }

            Printer.Success(command.Execute());
        }

        /// <summary>
        /// Parses string in next format:
        /// `INSERT INTO table (col1, col2, ...) VALUES (val1, val2, ...)`
        /// `INSERT INTO table VALUES (val1, val2, ...)`
        /// </summary>
        /// <param name="input">input string</param>
        public static void ParseInsert(in string input) {
            PrepareInput(input, out var betterInput);

            var words = betterInput.Split(' ', OPTIONS);

            switch (words.Length) {
                case <= 4:
                case > 6:
                    Printer.Error("Invalid command format");
                    return;
                default:
                    if (!"into".Equals(words[1], STR_COMPARISON)) {
                        Printer.Error("Invalid command format");
                        return;
                    }

                    if (!GlobalContainer.TableExists(words[2])) {
                        Printer.Error("No such table: {0}", words[2]);
                        return;
                    }

                    if (!"values".Equals(words[^2], STR_COMPARISON)) {
                        Printer.Error("Invalid command format");
                        return;
                    }

                    SqlInsertInto command;
                    if (!TryParseTuple(words[2], words[^1], out var vals, false)) {
                        Printer.Error("Failed to parse new values");
                        return;
                    }

                    if (words.Length == 6) {
                        if (!TryParseTuple(words[2], words[3], out var cols, true)) {
                            Printer.Error("Failed to parse columns");
                            return;
                        }

                        if (vals.Count != cols.Count) {
                            Printer.Error("Number of values and columns don't match");
                            return;
                        }

                        command = new SqlInsertInto(words[2], vals, cols);
                    }
                    else {
                        if (GlobalContainer.FieldCount(words[2]) != vals.Count) {
                            Printer.Error("Not all columns are covered");
                            return;
                        }

                        command = new SqlInsertInto(words[2], vals);
                    }
                    Printer.Success(command.Execute());
                    break;
            }
        }

        /// <summary>
        /// Parses string in next format:
        /// `DELETE FROM table WHERE condition`
        /// `DELETE FROM table`
        /// </summary>
        /// <param name="input">input string</param>
        public static void ParseDelete(in string input) {
            PrepareInput(input, out var betterInput);

            var words = betterInput.Split(' ', OPTIONS);

            switch (words.Length) {
                case <= 2:
                case 4:
                case >= 6:
                    Printer.Error("Invalid command format");
                    return;
                case 3:
                case 5:
                    if (!"from".Equals(words[1], STR_COMPARISON)) {
                        Printer.Error("Invalid command format");
                        return;
                    }

                    if (!GlobalContainer.TableExists(words[2])) {
                        Printer.Error("No such table: {0}", words[2]);
                        return;
                    }

                    SqlDelete command;
                    if (words.Length == 3) {
                        Printer.Info("WARNING! You're going to delete all entries in {0}. Are you sure? (Y/N)",
                            words[2]);
                        while (true) {
                            var key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.N) {
                                Printer.Info("Command discarded");
                                return;
                            }

                            if (key == ConsoleKey.Y) {
                                Printer.Info("Proceeded...");
                                break;
                            }
                        }

                        command = new SqlDelete(words[2]);
                    }
                    else {
                        if (!"where".Equals(words[3], STR_COMPARISON)) {
                            Printer.Error("Invalid command format");
                            return;
                        }

                        if (!TryParseCondition(words[2], words[4], out var conds)) {
                            Printer.Error("Failed to parse conditions");
                            return;
                        }

                        command = new SqlDelete(words[2], conds);
                    }

                    Printer.Success(command.Execute());
                    break;
            }
        }

        private static void PrepareInput(in string input, out string betterInput) {
            var regex = new Regex("[ ]{2,}", RegexOptions.None);
            var tmp = new StringBuilder(regex.Replace(input, " "));
            tmp.Replace(" =", "=");
            tmp.Replace("= ", "=");
            tmp.Replace(" >", ">");
            tmp.Replace("> ", ">");
            tmp.Replace(" <", "<");
            tmp.Replace("< ", "<");
            tmp.Replace(" ,", ",");
            tmp.Replace(", ", ",");
            tmp.Replace("( ", "(");
            tmp.Replace(" )", ")");

            betterInput = tmp.ToString();
        }

        private static bool TryParseCondition(
            in string tableName, in string raw, out List<(string, Operation, string)> conds) {

            conds = new List<(string, Operation, string)>();
            var conditions = raw.Split(',', OPTIONS);

            foreach (var c in conditions) {
                var opInd = c.IndexOfAny(Ops);
                var lastOpInd = c.LastIndexOfAny(Ops);

                if (!(0 < opInd && opInd <= lastOpInd && lastOpInd < c.Length)) {
                    Printer.Error("Invalid set statement");
                    return false;
                }

                var col = c[..opInd] ?? string.Empty;
                var op = Extension.ToOperation(c[opInd..(lastOpInd + 1)]);
                var val = c[(lastOpInd + 1)..] ?? string.Empty;

                if (!GlobalContainer.FieldExists(tableName, col)) {
                    Printer.Error("Invalid field: {0}", col);
                    return false;
                }

                conds.Add((col, op, val));
            }

            return true;
        }

        private static bool TryParseTuple(
            in string tableName, in string raw, out List<string> names, bool check) {

            if (raw[0] != '(' || raw[^1] != ')') {
                Printer.Error("Not a tuple");
                names = new List<string>();
                return false;
            }

            if (raw.Length <= 2) {
                Printer.Error("Empty tuple");
                names = new List<string>();
                return false;
            }

            var rawNames = raw[1..^1].Split(',', OPTIONS).ToList();

            if (check && !GlobalContainer.FieldsExist(tableName, rawNames)) {
                names = new List<string>();
                return false;
            }

            names = rawNames.ToList();
            return true;
        }
    }
}
