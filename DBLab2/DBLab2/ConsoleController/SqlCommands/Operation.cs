using System;

namespace DBLab2.ConsoleController.SqlCommands {
    public enum Operation {
        Equal,
        Less,
        Greater,
        EqualOrLess,
        EqualOrGreater,
        NonEqual,
    }

    public static class Extension {
        public static Operation ToOperation(this string @value) => @value switch {
            "=" => Operation.Equal,
            "<" => Operation.Less,
            ">" => Operation.Greater,
            "<=" => Operation.EqualOrLess,
            ">=" => Operation.EqualOrGreater,
            "<>" => Operation.NonEqual,
            _ => throw new NotSupportedException()
        };

        public static string ToString(this Operation @op) => @op switch {
            Operation.Equal => "=",
            Operation.Less => "<",
            Operation.Greater => ">",
            Operation.EqualOrLess => "<=",
            Operation.EqualOrGreater => ">=",
            Operation.NonEqual => "<>",
            _ => throw new NotSupportedException()
        };
    }
}