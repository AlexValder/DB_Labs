using System;
using System.Collections.Generic;
using System.Linq;

namespace DBLab2.ConsoleController.SqlCommands {

    /// <summary>
    /// Represents SQL command UPDATE %table_name% SET field1 = value1, ... (WHERE %condition%)
    /// </summary>
    public class SqlUpdate : SqlCommand {
        public IList<(string, string)> Set { get; }

        public SqlUpdate(in string table, in IList<(string, string)> set) : base(table) {
            Set = set ?? throw new ArgumentNullException(nameof(set));
        }

        public SqlUpdate(in string table,
            in IList<(string, string)> set,
            in IList<(string, Operation, string)> cond)
            : base(table, cond) {
            Set = set ?? throw new ArgumentNullException(nameof(set));
        }

        public override string ToString() =>
            $"UPDATE {Table}\n" +
            $"SET {GetFieldValuePairs()}\n" +
            (Conditions != null ? $"WHERE {GetConditions()}" : "");

        private string GetFieldValuePairs() => string.Join(", ",
            Set.Select(pair => $"\"{pair.Item1}\" = \"{pair.Item2}\""));
    }
}
