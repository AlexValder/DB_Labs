using System;
using System.Collections.Generic;
using System.Linq;

namespace DBLab2.ConsoleController.SqlCommands {
    
    /// <summary>
    /// Represents SQL command UPDATE %table_name% SET field1 = value1, ... (WHERE %condition%) 
    /// </summary>
    public class SqlUpdate : SqlCommand {
        public IEnumerable<(string, string)> Set { get; }
        public IEnumerable<(string, Operation, string)>? Conditions { get; }

        public SqlUpdate(in string table, in IEnumerable<(string, string)> set) : base(table) {
            Set = set ?? throw new ArgumentNullException(nameof(set));
        }

        public SqlUpdate(in string table,
            in IEnumerable<(string, string)> set,
            in IEnumerable<(string, Operation, string)> cond)
            : this(table, set) {
            Conditions = cond;
        }

        public override string ToString() =>
            $"UPDATE {Table}\n" +
            $"SET {GetFieldValuePairs()}\n" +
            (Conditions != null ? $"WHERE {GetConditions()}" : "");

        private string GetFieldValuePairs() => string.Join(", ",
            Set.Select(pair => $"\"{pair.Item1}\" = \"{pair.Item2}\""));

        private string GetConditions() => string.Join(", ",
            Conditions!.Select(pair
                => $"\"{pair.Item1}\" {Extension.ToString(pair.Item2)} \"{pair.Item3}\""));
    }
}