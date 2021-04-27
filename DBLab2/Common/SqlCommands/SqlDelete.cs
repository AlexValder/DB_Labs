using System.Collections.Generic;

namespace Common.SqlCommands {

    /// <summary>
    /// Represents SQL command DELETE FROM %table_name% (WHERE %condition%) command
    /// </summary>
    public class SqlDelete : SqlCommand {
        public SqlDelete(in string table) : base(table) {}

        public SqlDelete(in string table, in IList<(string, Operation, string)>? conditions)
            : base(table, conditions) {}

        public override string ToString() =>
            $"DELETE FROM {Table}" +
            (Conditions != null ? $" WHERE {GetConditions()}" : "");
    }
}
