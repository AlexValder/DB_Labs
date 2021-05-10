using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaGUI {
    public class WarningErrorWindow : Window {
        public WarningErrorWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void OnClick(object sender, RoutedEventArgs args) {
            Close();
        }

        public static void ShowCredits(string title, string header, string message, string button) {
            var errorWindow = new WarningErrorWindow {
                Title = title,
            };
            errorWindow.FindControl<Button>("OkButton").Content = button;
            errorWindow.FindControl<TextBlock>("ErrorWarningMessage").Text = message;
            errorWindow.FindControl<Label>("ErrorWarningTitle").Content = header;
            errorWindow.Show();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
