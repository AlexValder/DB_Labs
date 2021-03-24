using System.Data;

namespace DBLab2.ConsoleController.SqlCommands {
    public class SqlDelete : SqlCommand {
        public (string, Operation, string)? Condition { get; }

        public SqlDelete(in string table) : base(table) {}

        public SqlDelete(in string table, in (string, Operation, string) condition) : this(table) {
            Condition = condition;
        }

        public override string ToString() =>
            $"DELETE FROM {Table}" +
            (Condition != null
                ? $" WHERE {Condition.Value.Item1} {Condition.Value.Item2} {Condition.Value.Item3}"
                : "");
    }
}