using System.Collections.Generic;

namespace DBLab2.ConsoleController.SqlCommands {
    public class SqlInsertInto : SqlCommand {
        public List<string> Values { get; }
        public List<string>? Fields { get; }

        public SqlInsertInto(in string table, in List<string> values) : base(table) {
            Values = values;
        }

        public SqlInsertInto(in string table, in List<string> values, in List<string> fields)
            : this(table, values) {
            Fields = fields;
        }

        public override string ToString() =>
            $"INSERT INTO {Table} {GetFields()}\n" +
            $"VALUES {GetValues()}";

        private string GetFields() =>
            Fields != null ? $"({string.Join(',', Fields)})" : "";

        private string GetValues() => $"({string.Join(',', Values)})";
    }
}