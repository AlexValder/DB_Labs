using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaGUI {
    public class GodModWindow : Window {
        public GodModWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupAskForDataWindow(AskForDataWindow wnd, IEnumerable<string> labels) {
            if (wnd is null) {
                return;
            }
            labels.Count();
            for (int c = 0; c < labels.Count(); c++) {
                var ctr = wnd.FindControl<Label>("Label" + c);
                ctr.Content = labels.ElementAt(c);
            }
            for (int c = labels.Count(); c < 8; c++) {
                var ctr = wnd.FindControl<Label>("Label" + c);
                ctr.IsVisible = false;
                var txb = wnd.FindControl<TextBox>("TextBox" + c);
                txb.IsVisible = false;
            }
            wnd.Height = wnd.Height / 7 * labels.Count();
        }

        public void AddEntry(object sender, RoutedEventArgs e) {
            var wnd = new AvaloniaGUI.AskForDataWindow();
            var godTables = this.FindControl<ComboBox>("GodTables");
            var selectedTable = godTables.SelectedItem;
            //Then we take fields names from DB, setting them up (throwing to SetupAskForDataWindow as a second parameter) and taking the response.
            //SetupAskForDataWindow(wnd, new List<string> {field1, field2, ..., fieldN});
            
            wnd.Show();
        }
    }
}
