using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DBLab2.Common
{
    public static class Printer
    {
        private enum MessageTypes
        {
            Info,
            Success,
            Error,
        }

        private static readonly ImmutableDictionary<MessageTypes, ConsoleColor> Foregrounds =
            new Dictionary<MessageTypes, ConsoleColor>
            {
                [MessageTypes.Info] = ConsoleColor.White,
                [MessageTypes.Success] = ConsoleColor.Green,
                [MessageTypes.Error] = ConsoleColor.Red,
            }.ToImmutableDictionary();
        
        private static readonly ImmutableDictionary<MessageTypes, ConsoleColor> Backgrounds =
            new Dictionary<MessageTypes, ConsoleColor>
            {
                [MessageTypes.Info] = ConsoleColor.Black,
                [MessageTypes.Success] = ConsoleColor.Black,
                [MessageTypes.Error] = ConsoleColor.Black,
            }.ToImmutableDictionary();

        public static void Info(string template, params object[] args)
            => Print(MessageTypes.Info, template, args);

        public static void Success(string template, params object[] args)
            => Print(MessageTypes.Success, template, args);

        public static void Error(string template, params object[] args)
            => Print(MessageTypes.Error, template, args);

        public static void Error(Exception ex, string template)
        {
            var temp = string.Concat(template, ex.Message, ex.StackTrace);
            Print(MessageTypes.Error, temp);
        }

        private static void Print(MessageTypes type, string template, params object[] args)
        {
            var foreTmp = Console.ForegroundColor;
            var backTmp = Console.BackgroundColor;
            try
            {
                Console.ForegroundColor = Foregrounds[type];
                Console.BackgroundColor = Backgrounds[type];
                Console.WriteLine(template, args);
            }
            finally
            {
                Console.ForegroundColor = foreTmp;
                Console.BackgroundColor = backTmp;
            }
        }
    }
}