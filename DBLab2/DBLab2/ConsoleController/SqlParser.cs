using DBLab2.Common;

namespace DBLab2.ConsoleController {
    class SqlParser {
        public void ParseSelect(string input) {
            Printer.Info("In Select: {0}", input);
        }

        public void ParseAdd(string input) {
            Printer.Info("In Add: {0}", input);            
        }

        public void ParseInsert(string input) {
            Printer.Info("In Insert: {0}", input);
        }

        public void ParseDelete(string input) {
            Printer.Info("In Delete: {0}", input);
        }
    }
}