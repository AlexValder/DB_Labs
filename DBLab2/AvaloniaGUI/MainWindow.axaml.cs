using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.SqlCommands;

namespace AvaloniaGUI {
    public class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void EnableControls() {
            List<string> buttonsToEnable = new() { "DeleteButton", "AddButton", "EditButton", "FilterButton" };
            foreach (var button in buttonsToEnable) {
                this.FindControl<Button>(button).IsEnabled = true;
            }
            this.FindControl<ComboBox>("Tables").IsEnabled = true;
        }

        public async void OpenDbClick(object sender, RoutedEventArgs e) {
            var cal1 = this.FindControl<Button>("OpenButton");
            var fileDialog = new OpenFileDialog();
            fileDialog.Filters.Add(new FileDialogFilter() { Name = "Text", Extensions = { "db" } });
            var res = await fileDialog.ShowAsync(this);
            if (res is null || res.Length < 1 || res[0].Length == 0) {
                return;
            }

            var isDbSet = SqliteAdapter.TrySetDatabase(res[0]);
            if (isDbSet) {
                EnableControls();
                var elem = this.FindControl<ComboBox>("Tables");
                elem.Items = from table in GlobalContainer.Tables where table != "Tables" select table;
                elem.SelectedIndex = 0;
            }
        }

        public void SpawnWindow(object sender, RoutedEventArgs e) {
            var wnd = new AvaloniaGUI.LogInWindow();
            wnd.Show();
        }
    }
}
