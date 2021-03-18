namespace DBLab2.ConsoleController.SqlCommands {
    public interface ISqlCommand {
        public string Table { get; set; }
        public string Execute();
    }
}
