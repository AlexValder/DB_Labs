using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
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

        public Action<List<string>> onSubmit { get; set; } = _ => { };

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
            onSubmit(values);
            Close();
        }
    }
}
