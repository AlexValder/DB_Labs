using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace DBLab2.Common {
    public static class Printer {
        private enum MessageTypes {
            Info,
            Success,
            Error,
            Debug,
        }

        private static readonly ImmutableDictionary<MessageTypes, ConsoleColor> Foregrounds =
            new Dictionary<MessageTypes, ConsoleColor> {
                [MessageTypes.Info]    = ConsoleColor.White,
                [MessageTypes.Success] = ConsoleColor.Green,
                [MessageTypes.Error]   = ConsoleColor.Red,
                [MessageTypes.Debug]   = ConsoleColor.Gray,
            }.ToImmutableDictionary();

        private static readonly ImmutableDictionary<MessageTypes, ConsoleColor> Backgrounds =
            new Dictionary<MessageTypes, ConsoleColor> {
                [MessageTypes.Info]    = ConsoleColor.Black,
                [MessageTypes.Success] = ConsoleColor.Black,
                [MessageTypes.Error]   = ConsoleColor.Black,
                [MessageTypes.Debug]   = ConsoleColor.Black,
            }.ToImmutableDictionary();

        public static void Info(string template, params object[] args)
            => Print(MessageTypes.Info, template, args);

        public static void Info(IEnumerable<string> templates, params object[] args)
            => Print(MessageTypes.Info, string.Join('\n', templates), args);

        public static void Success(string template, params object[] args)
            => Print(MessageTypes.Success, template, args);

        public static void Success(IEnumerable<string> templates, params object[] args)
            => Print(MessageTypes.Success, string.Join('\n', templates), args);

        public static void Error(string template, params object[] args)
            => Print(MessageTypes.Error, template, args);

        public static void Error(IEnumerable<string> templates, params object[] args)
            => Print(MessageTypes.Error, string.Join('\n', templates), args);

        public static void Error(Exception? ex, string template, params object[] args)
            => Print(
                MessageTypes.Error,
                string.Concat(template, "\n", ex?.Message, "\n", ex?.StackTrace),
                args);

        [Conditional("DEBUG")]
        public static void Debug(string template, params object[] args)
            => Print(MessageTypes.Debug, template, args);

        [Conditional("DEBUG")]
        public static void Debug(IEnumerable<string> templates, params object[] args)
            => Print(MessageTypes.Debug, string.Join('\n', templates), args);

        private static void Print(MessageTypes type, string template, params object[] args) {
            var foreTmp = Console.ForegroundColor;
            var backTmp = Console.BackgroundColor;
            try {
                Console.ForegroundColor = Foregrounds[type];
                Console.BackgroundColor = Backgrounds[type];
                Console.WriteLine(template, args);
            }
            finally {
                Console.ForegroundColor = foreTmp;
                Console.BackgroundColor = backTmp;
            }
        }
    }
}
