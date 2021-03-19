using System.Collections.Generic;

namespace DBLab2.ConsoleController.SqlCommands {
    public sealed class SqlSelect : ISqlCommand {
        public List<string>? Fields { get; set; }
        public string Table { get; set; }

        public SqlSelect(in string table) {
            Table = table;
        }

        public SqlSelect(in string table, in List<string> fields) : this(table) {
            Fields = fields;
        }

        public string Execute() => ToString();

        public override string ToString() => $"SELECT {GetSqlFields()} FROM {Table}";

        private string GetSqlFields() => Fields != null ? string.Join(",", Fields) : "*";
    }
}
