namespace DBLab2.ConsoleController.SqlCommands {
    
    /// <summary>
    /// Base abstract class for SQL command classes
    /// </summary>
    /// <remarks>
    /// Neither <see cref="SqlCommand"/> not its children check for validity.
    /// This is delegated to <see cref="SqlParser"/> 
    /// </remarks>
    public abstract class SqlCommand {
        public string Table { get; }
        public string Execute() => ToString() ?? "";

        protected SqlCommand(in string table) {
            Table = table;
        }
    }
}
