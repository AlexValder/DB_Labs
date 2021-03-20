using System.Collections.Generic;

namespace DBLab2.ConsoleController.SqlCommands {
    public sealed class SqlSelect : SqlCommand {
        public List<string>? Fields { get; }
        public string Table { get; }

        public SqlSelect(in string table) : base(table) {}

        public SqlSelect(in string table, in List<string> fields) : this(table) {
            Fields = fields;
        }

        public override string ToString() => $"SELECT {GetSqlFields()} FROM {Table}";

        private string GetSqlFields() => Fields != null ? string.Join(",", Fields) : "*";
    }
}
