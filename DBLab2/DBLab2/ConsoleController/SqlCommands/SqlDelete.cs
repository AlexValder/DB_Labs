using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DBLab2.ConsoleController.SqlCommands {
    
    /// <summary>
    /// Represents SQL command DELETE FROM %table_name% (WHERE %condition%) command 
    /// </summary>
    public class SqlDelete : SqlCommand {
        public IEnumerable<(string, Operation, string)>? Conditions { get; }

        public SqlDelete(in string table) : base(table) {}

        public SqlDelete(in string table, in IEnumerable<(string, Operation, string)> conditions) : this(table) {
            foreach (var cond in conditions) {
                if (cond.Item1 == null || cond.Item3 == null) {
                    throw new ArgumentNullException(nameof(conditions));
                }

                if (cond.Item1 == string.Empty || cond.Item3 == string.Empty) {
                    throw new ArgumentException("Empty SQL statements", nameof(conditions));
                }
            }

            Conditions = conditions;
        }

        public override string ToString() =>
            $"DELETE FROM {Table}" +
            (Conditions != null ? $" WHERE {GetConditions()}" : "");

        private string GetConditions() => string.Join(", ",
            Conditions!.Select(pair
                => $"\"{pair.Item1}\" {Extension.ToString(pair.Item2)} \"{pair.Item3}\""));
    }
}