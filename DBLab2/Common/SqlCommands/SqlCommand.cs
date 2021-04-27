using System.Collections.Generic;
using System.Linq;

namespace Common.SqlCommands {

    /// <summary>
    /// Base abstract class for SQL command classes
    /// </summary>
    /// <remarks>
    /// Neither <see cref="SqlCommand"/> not its children check for validity.
    /// This is delegated to <see cref="SqlParser"/>
    /// </remarks>
    public abstract class SqlCommand {
        public string Table { get; }
        public IList<(string, Operation, string)>? Conditions { get; }
        public string Execute() => ToString() ?? "";

        protected SqlCommand(in string table) {
            Table = table;
        }

        protected SqlCommand(in string table, in IList<(string, Operation, string)>? conditions) {
            Table = table;
            Conditions = conditions;
        }

        protected string GetConditions() => string.Join(", ",
            Conditions!.Select(pair
                => $"\"{pair.Item1}\" {Extension.ToString(pair.Item2)} \"{pair.Item3}\""));
    }
}
