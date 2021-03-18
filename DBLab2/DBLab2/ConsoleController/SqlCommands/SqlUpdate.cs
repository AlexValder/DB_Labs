using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

    public class SqlUpdate : ISqlCommand {
        public string Table { get; set; }
        public List<(string, string)> Set { get; set; }
        public (string, Operation, string)? Condition { get; set; }

        public SqlUpdate(in string table, in List<(string, string)> set) {
            Table = table;
            Set = set;
        }

        public SqlUpdate(in string table, in List<(string, string)> set, in (string, Operation, string) cond)
            : this(table, set) {
            Condition = cond;
        }

        public string Execute() => ToString();

        public override string ToString() =>
            $"UPDATE {Table}\n" +
            $"SET {GetFieldValuePairs()}\n" +
            (Condition != null ?
                $"WHERE {Condition.Value.Item1} {Condition.Value.Item2.ToString()} {Condition.Value.Item3}" :
                ""
            );

        private string GetFieldValuePairs() => string.Join(", ",
                Set.Select(pair => $"{pair.Item1} = {pair.Item2}"));
    }
}