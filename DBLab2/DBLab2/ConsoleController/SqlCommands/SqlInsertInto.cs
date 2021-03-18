using System.Collections.Generic;

namespace DBLab2.ConsoleController.SqlCommands {
    public class SqlInsertInto : ISqlCommand {
        public string Table { get; set; }
        public List<string> Values { get; set; }
        public List<string>? Fields { get; set; }

        public SqlInsertInto(in string table, in List<string> values) {
            Table = table;
            Values = values;
        }

        public SqlInsertInto(in string table, in List<string> values, in List<string> fields)
            : this(table, values) {
            Fields = fields;
        }

        public string Execute() => ToString();

        public override string ToString() =>
            $"INSERT INTO {Table} {GetFields()}\n" +
            $"VALUES {GetValues()}";

        private string GetFields() =>
            Fields != null ? $"({string.Join(',', Fields)})" : "";

        private string GetValues() => $"({string.Join(',', Values)})";
    }
}