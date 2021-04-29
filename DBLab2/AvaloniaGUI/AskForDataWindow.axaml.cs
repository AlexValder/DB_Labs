using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaGUI {
    public class AskForDataWindow : Window {
        public AskForDataWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public Action<List<string>> onSubmit { get; set; }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        public void SetupAskForDataWindow(IEnumerable<string> labels) {
            labels.Count();
            for (int c = 0; c < labels.Count(); c++) {
                var ctr = this.FindControl<Label>("Label" + c);
                ctr.Content = labels.ElementAt(c);
            }
            for (int c = labels.Count(); c < 8; c++) {
                var ctr = this.FindControl<Label>("Label" + c);
                ctr.IsVisible = false;
                var txb = this.FindControl<TextBox>("TextBox" + c);
                txb.IsVisible = false;
            }
            this.Height = this.Height / 7 * labels.Count();
        }

        public void SubmitPressed(object sender, RoutedEventArgs e) {
            List<string> fields = new List<string>();
            for (int c = 0; c < 8; c++) {
                var tbctr = this.FindControl<TextBox>("TextBox" + c);
                var lctr = this.FindControl<Label>("Label" + c);
                if (lctr.IsVisible) {
                    fields.Add(tbctr.Text);
                }
            }
            onSubmit(fields);
        }
    }
}
