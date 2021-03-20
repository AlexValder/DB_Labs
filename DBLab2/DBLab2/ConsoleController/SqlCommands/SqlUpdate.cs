using System;
using System.Collections.Generic;
using System.Linq;

namespace DBLab2.ConsoleController.SqlCommands {
    
    /// <summary>
    /// Represents SQL command UPDATE %table_name% SET field1 = value1, ... (WHERE %condition%) 
    /// </summary>
    public class SqlUpdate : SqlCommand {
        public List<(string, string)> Set { get; }
        public (string, Operation, string)? Condition { get; }

        public SqlUpdate(in string table, in List<(string, string)> set) : base(table) {
            Set = set ?? throw new ArgumentNullException(nameof(set));
        }

        public SqlUpdate(in string table, in List<(string, string)> set, in (string, Operation, string) cond)
            : this(table, set) {
            Condition = cond;
        }

        public override string ToString() =>
            $"UPDATE {Table}\n" +
            $"SET {GetFieldValuePairs()}\n" +
            (Condition != null ?
                $"WHERE \"{Condition.Value.Item1}\" " +
                $"{Extension.ToString(Condition.Value.Item2)} " +
                $"\"{Condition.Value.Item3}\"" :
                ""
            );

        private string GetFieldValuePairs() => string.Join(", ",
            Set.Select(pair => $"\"{pair.Item1}\" = \"{pair.Item2}\""));
    }
}