using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common;
using Common.SqlCommands;

namespace AvaloniaGUI {
    public class GodModWindow : Window {

        private readonly ImmutableList<string> _buttonNames = new List<string> {
            "AddButton",
            "RemoveButton",
            "EditButton",
            "FilterButton",
        }.ToImmutableList();

        public GodModWindow() {
            InitializeComponent();
            TryOpenDb("Library.db");
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupAskForDataWindow(in AskForDataWindow wnd, in IList<string> labels) {
            for (int c = 0; c < labels.Count; c++) {
                var ctr = wnd.FindControl<Label>("Label" + c);
                ctr.Content = labels.ElementAt(c);
            }
            for (int c = labels.Count; c < 8; c++) {
                var ctr = wnd.FindControl<Label>("Label" + c);
                ctr.IsVisible = false;
                var txb = wnd.FindControl<TextBox>("TextBox" + c);
                txb.IsVisible = false;
            }
            wnd.Height = wnd.Height / 7 * labels.Count;
        }

        public void AddEntry(object sender, RoutedEventArgs e) {
            var wnd = new AskForDataWindow();
            var godTables = this.FindControl<ComboBox>("GodTables");
            var selectedTable = godTables.SelectedItem as string;

            SetupAskForDataWindow(wnd, (GlobalContainer.Fields(selectedTable!) as IList<string>)!);
            wnd.CanResize = false;
            wnd.Show();
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void OnExitGodMode(object _, RoutedEventArgs _2) => Close();

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private async void OpenButton_OnClick(object? _, RoutedEventArgs _2) {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filters.Add(new FileDialogFilter { Name = "Text", Extensions = { "db" } });
            var res = await fileDialog.ShowAsync(this);
            if (res is null || res.Length < 1 || res[0].Length == 0) {
                return;
            }

            TryOpenDb(res[0]);
        }

        private void EnableControls() {
            foreach (var button in _buttonNames) {
                this.FindControl<Button>(button).IsEnabled = true;
            }
            this.FindControl<ComboBox>("GodTables").IsEnabled = true;
        }

        private void TryOpenDb(string path) {
            if (!SqliteAdapter.TrySetDatabase(path)) {
                return;
            }

            EnableControls();
            var elem = this.FindControl<ComboBox>("GodTables");
            elem.Items = from table in GlobalContainer.Tables where table != "Tables" select table;
            elem.SelectedIndex = 0;
        }
    }
}
