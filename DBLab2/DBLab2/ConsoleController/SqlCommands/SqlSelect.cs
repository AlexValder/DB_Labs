using System;
using System.Collections.Generic;

namespace DBLab2.ConsoleController.SqlCommands {

    /// <summary>
    /// Represents SQL command SELECT %fields% from %table_name%
    /// </summary>
    public sealed class SqlSelect : SqlCommand {
        public IList<string>? Fields { get; }

        public SqlSelect(in string table,
            in IList<string>? fields = null,
            in IList<(string, Operation, string)>? conditions = null) : base(table, conditions) {
            Fields = fields;
        }

        public override string ToString()
            => $"SELECT {GetSqlFields()} FROM {Table}" +
               (Conditions != null ? $"\nWHERE {GetConditions()}" : "");

        private string GetSqlFields() => Fields != null ? string.Join(",", Fields) : "*";
    }
}
