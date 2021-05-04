using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using Common.SqlCommands;
using System;

namespace AvaloniaGUI {
    public class FilterWindow : Window {
        public Action<List<string>, List<(string, Operation, string)>> onSubmit { get; set; } = (_1, _2) => { };

        public FilterWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void onSubmitPressed(object sender, RoutedEventArgs args) {
            var conditions = new List<(string, Operation, string)>();
            var fields = new List<string>();
            for (int c = 0; c < 8; c++) {
                var field = this.FindControl<Label>($"Label{c}").Content as string;
                var operation = this.FindControl<ComboBox>($"ComboBox{c}").SelectedItem as Operation?;
                var value = this.FindControl<TextBox>($"TextBox{c}").Text;
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(field) && operation is not null) {
                    fields.Add(field);
                    conditions.Add((field, (Operation)operation, value));
                } else {
                    fields.Add(field);
                }
            }
            onSubmit(fields, conditions);
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
