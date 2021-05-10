using System.Collections.Generic;

namespace Common.SqlCommands {

    /// <summary>
    /// Represents SQL command SELECT %fields% from %table_name%
    /// </summary>
    public sealed class SqlSelect : SqlCommand {
        public IList<string>? Fields { get; }

        public SqlSelect(in string table) : base(table) {}

        public SqlSelect(in string table, in IList<string> fields) : base(table) {
            Fields = fields;
        }

        public SqlSelect(in string table,
            in IList<string>? fields,
            in IList<(string, Operation, string)>? conditions) : base(table, conditions) {
            Fields = fields;
        }

        public override string ToString()
            => $"SELECT {GetSqlFields()} FROM {Table}" +
               (Conditions != null ? $"\nWHERE {GetConditions()}" : "");

        private string GetSqlFields() => Fields != null ? string.Join(",", Fields) : "*";
    }
}
