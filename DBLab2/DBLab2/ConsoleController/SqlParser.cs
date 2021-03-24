using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

                    if (words.Length > 5 && "where".Equals(words[4], STR_COMPARISON)) {
                        var cond = words[5];
                        
                        var condList = new List<(string, Operation, string)>();
                        var conditions = cond.Split(',', OPTIONS);
                        
                        foreach (var c in conditions) {
                            var opInd = c.IndexOfAny(Ops);
                            var lastOpInd = c.LastIndexOfAny(Ops);
                            
                            if (!(0 < opInd && opInd <= lastOpInd && lastOpInd < c.Length)) {
                                Printer.Error("Invalid set statement");
                                return;
                            }

                            var col = c[..opInd] ?? string.Empty;
                            var op = Extension.ToOperation(c[opInd..(lastOpInd + 1)]);
                            var val = c[(lastOpInd + 1)..] ?? string.Empty;

                            if (!GlobalContainer.FieldExists(words[1], col)) {
                                Printer.Error("Invalid field: {0}", col);
                                return;
                            }

                            condList.Add((col, op, val));
                        }
                        
                        command = new SqlUpdate(words[1], setList, condList);
                    }
                    else {
                        command = new SqlUpdate(words[1], setList);
                    }

                    break;
            }
            
            Printer.Success(command.Execute());
        }
        
        public static void ParseInsert(in string input) {
            //WIP
        }

        public static void ParseDelete(in string input) {
            //WIP
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
            tmp.Replace(", ", ",");

            betterInput = tmp.ToString();
        }
    }
}
