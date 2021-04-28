using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace AvaloniaGUI {
    public class AskForDataWindow : Window {
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
            List<string> fields = new List<string>();
            for (int c = 0; c < 8; c++) {
                var tbctr = this.FindControl<TextBox>("TextBox" + c);
                var lctr = this.FindControl<Label>("Label" + c);
                if (lctr.IsVisible) {
                    fields.Add(tbctr.Text);
                }
            }
            //Writing to DB...
        }
    }
}
