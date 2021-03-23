using System;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DBLab2.Common;

namespace DBLab2.ConsoleController {
    internal static class SqlParser {

        private enum CommandType {
            Select,
            Add,
            Insert,
            Delete,
        }

        private const string SELECT_PATTERN = @"SELECT ";
        private const string ADD_PATTERN = @"ADD ";
        private const string INSERT_PATTERN = @"INSERT INTO ";
        private const string DELETE_PATTERN = @"DELETE ";

        private const RegexOptions OPTIONS = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase;

        public static void ParseSelect(in string input) => Parse(input, CommandType.Select);

        public static void ParseAdd(in string input) => Parse(input, CommandType.Add);

        public static void ParseInsert(in string input) => Parse(input, CommandType.Insert);

        public static void ParseDelete(in string input) => Parse(input, CommandType.Delete);

        private static void Parse(in string input, in CommandType type) {
            var regex = new Regex(GetPattern(type), OPTIONS);
            var match = regex.Match(input);
            if (match.Success) {
                Printer.Success("Command is regex-correct: {0}", input);
            }
            else {
                Printer.Error("Command in bullshit: {0}", input);
            }

            static string GetPattern(in CommandType type) => type switch {
                CommandType.Select => SELECT_PATTERN,
                CommandType.Insert => INSERT_PATTERN,
                CommandType.Add    => ADD_PATTERN,
                CommandType.Delete => DELETE_PATTERN,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
