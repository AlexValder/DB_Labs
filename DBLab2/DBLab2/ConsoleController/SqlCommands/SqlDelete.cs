using System;
using System.Data;

namespace DBLab2.ConsoleController.SqlCommands {
    
    /// <summary>
    /// Represents SQL command DELETE FROM %table_name% (WHERE %condition%) command 
    /// </summary>
    public class SqlDelete : SqlCommand {
        public (string, Operation, string)? Condition { get; }

        public SqlDelete(in string table) : base(table) {}

        public SqlDelete(in string table, in (string, Operation, string) condition) : this(table) {
            if (condition.Item1 == null || condition.Item3 == null) {
                throw new ArgumentNullException(nameof(condition));
            }

            if (condition.Item1 == string.Empty || condition.Item3 == string.Empty) {
                throw new ArgumentException("Empty SQL statements", nameof(condition));
            }
            
            Condition = condition;
        }

        public override string ToString() =>
            $"DELETE FROM {Table}" +
            (Condition != null
                ? $" WHERE \"{Condition.Value.Item1}\" " +
                  $"{Extension.ToString(Condition.Value.Item2)} " +
                  $"\"{Condition.Value.Item3}\""
                : "");
    }
}