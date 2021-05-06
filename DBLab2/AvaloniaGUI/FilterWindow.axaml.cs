using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using Common.SqlCommands;
using System;

namespace AvaloniaGUI {
    public class FilterWindow : Window {
        public Action<List<string>, List<(string, Operation, string)>> OnSubmit { get; set; } = (_, _) => { };

        public FilterWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void OnSubmitPressed(object sender, RoutedEventArgs args) {
            var conditions = new List<(string, Operation, string)>();
            var fields = new List<string>();
            for (int c = 0; c < 8; c++) {
                var field = this.FindControl<Label>($"Label{c}").Content as string;
                var operation = this.FindControl<ComboBox>($"ComboBox{c}").SelectedItem as Operation?;
                var value = this.FindControl<TextBox>($"TextBox{c}").Text;
                var shallBeConsidered = this.FindControl<CheckBox>($"CheckBox{c}").IsChecked;
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(field) && operation is not null) {
                    conditions.Add((field, (Operation)operation, value));
                }
                if (shallBeConsidered is not null && (bool)shallBeConsidered && field is not null) {
                    fields.Add(field);
                }
            }
            OnSubmit(fields, conditions);
            Close();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
