using System;
using System.Collections.Generic;

namespace DBLab2.ConsoleController.SqlCommands {
    
    /// <summary>
    /// Represents SQL command SELECT %fields% from %table_name%
    /// </summary>
    public sealed class SqlSelect : SqlCommand {
        public List<string>? Fields { get; }

        public SqlSelect(in string table) : base(table) {}

        public SqlSelect(in string table, in List<string> fields) : this(table) {
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public override string ToString() => $"SELECT {GetSqlFields()} FROM {Table}";

        private string GetSqlFields() => Fields != null ? string.Join(",", Fields) : "*";
    }
}
