using System.Collections.Generic;

namespace DBLab2.ConsoleController.SqlCommands {

    /// <summary>
    /// Represents SQL command INSERT INTO %table_name% (field1, field2, ...) VALUES (value1, value2, ...)
    /// </summary>
    public class SqlInsertInto : SqlCommand {
        public IList<string> Values { get; }
        public IList<string>? Fields { get; }

        public SqlInsertInto(in string table, in IList<string> values) : base(table) {
            Values = values;
        }

        public SqlInsertInto(in string table, in IList<string> values, in IList<string> fields)
            : this(table, values) {
            Fields = fields;
        }

        public override string ToString() =>
            $"INSERT INTO {Table} {GetFields()}\n" +
            $"VALUES {GetValues()}";

        private string GetFields() =>
            Fields != null ? $"(\"{string.Join("\", \"", Fields)}\")" : "";

        private string GetValues() => $"(\"{string.Join("\", \"", Values)}\")";
    }
}
