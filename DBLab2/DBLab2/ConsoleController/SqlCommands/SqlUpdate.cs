using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DBLab2.ConsoleController.SqlCommands { 
        public class SqlUpdate : SqlCommand {
        public List<(string, string)> Set { get; }
        public (string, Operation, string)? Condition { get; }

        public SqlUpdate(in string table, in List<(string, string)> set) : base(table) {
            Set = set;
        }

        public SqlUpdate(in string table, in List<(string, string)> set, in (string, Operation, string) cond)
            : this(table, set) {
            Condition = cond;
        }

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