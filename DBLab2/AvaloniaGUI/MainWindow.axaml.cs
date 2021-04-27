using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;

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

        private void enableControls() {
            List<string> buttonsToEnable = new List<string> { "DeleteButton", "AddButton", "EditButton", "FilterButton" };
            foreach (var button in buttonsToEnable) {
                this.FindControl<Button>(button).IsEnabled = true;
            }
            this.FindControl<ComboBox>("Tables").IsEnabled = true;
        }

        public async void OpenDBClick(object sender, RoutedEventArgs e) {
            var cal1 = this.FindControl<Button>("OpenButton");
            var fileDialog = new OpenFileDialog();
            fileDialog.Filters.Add(new FileDialogFilter() { Name = "Text", Extensions = { "db" } });
            var res = await fileDialog.ShowAsync(this);
            if (res is null || res.Length < 1 || res[0].Length == 0) {
                return;
            }

            var isDBSet = DBLab2.DBLogic.SqliteAdapter.TrySetDatabase(res[0]);
            if (isDBSet) {
                enableControls();
                var elem = this.FindControl<ComboBox>("Tables");
                elem.Items = from table in DBLab2.Common.GlobalContainer.Tables where table != "Tables" select table;
            }
        }

        public void SpawnWindow(object sender, RoutedEventArgs e) {
            var wnd = new AvaloniaGUI.LogInWindow();
            wnd.Show();
        }
    }
}
