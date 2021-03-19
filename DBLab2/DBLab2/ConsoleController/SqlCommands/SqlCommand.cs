namespace DBLab2.ConsoleController.SqlCommands {
    public abstract class SqlCommand {
        public string Table { get; }
        public string Execute() => ToString() ?? "";

        protected SqlCommand(in string table) {
            Table = table;
        }
    }
}
