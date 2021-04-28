using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.SqlCommands;

namespace AvaloniaGUI {
    public class AskForDataWindow : Window {

        public string Table { get; set; } = "";
        public AskForDataWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        public void SubmitPressed(object sender, RoutedEventArgs e) {
            var values = new List<string>();
            for (var c = 0; c < 8; c++) {
                var value = this.FindControl<TextBox>("TextBox" + c);
                var field = this.FindControl<Label>("Label" + c);
                if (!field.IsVisible) {
                    break;
                }
                values.Add(value.Text);
            }

            var fields = GlobalContainer.Fields(Table).ToList();
            fields.Remove("Id");
            var command = new SqlInsertInto(Table, values, fields);
            try {
                SqliteAdapter.InsertInto(command);
            }
            catch {
                // ????? idk what to do
            }
            Close();
        }
    }
}
