using System;

namespace DBLab2.ConsoleController.SqlCommands {
    
    /// <summary>
    /// Used for Condition field in <see cref="SqlUpdate"/> and <see cref="SqlDelete"/> classes.
    /// </summary>
    public enum Operation {
        Equal,
        Less,
        Greater,
        EqualOrLess,
        EqualOrGreater,
        NonEqual,
    }

    /// <summary>
    /// Extension methods to quickly convert from enum to string representation and vice versa
    /// </summary>
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