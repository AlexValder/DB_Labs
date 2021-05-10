using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace AvaloniaGUI {
    public class AskForDataWindow : Window {

        public string Table { get; init; } = "";
        public AskForDataWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        public Action<List<string>> OnSubmit { get; set; } = _ => { };

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
            OnSubmit(values);
            Close();
        }
    }
}
