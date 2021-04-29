using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using Common;
using Common.SqlCommands;

namespace AvaloniaGUI {
    public class GodModWindow : Window {
        private ListBox _grid;
        private ComboBox _tables;
        private string _tableName = "";

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
            _grid = this.FindControl<ListBox>("GodGrid");
            _tables = this.FindControl<ComboBox>("GodTables");
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void UpdateList() {
            _grid.Items = null;
            var command = new SqlSelect(_tableName);
            var lists = SqliteAdapter.Select(command);
            var items = new List<string> {
                string.Join(" ", GlobalContainer.Fields(_tableName))
            };
            items.AddRange(lists.Select(list => string.Join(" ", list)));
            _grid.Items = items;
        }

        private static void SetupAskForDataWindow(in AskForDataWindow wnd, in string table, in IList<string> labels) {
            wnd.Title = table;
            for (var i = 0; i < labels.Count; ++i) {
                var ctr = wnd.FindControl<Label>("Label" + i);
                ctr.Content = labels[i];
            }
            for (var i = labels.Count; i < 8; ++i) {
                var ctr = wnd.FindControl<Label>("Label" + i);
                ctr.IsVisible = false;
                var txb = wnd.FindControl<TextBox>("TextBox" + i);
                txb.IsVisible = false;
            }
            wnd.Height = wnd.Height / 7 * labels.Count + 20;
        }

        public void AddEntry(object sender, RoutedEventArgs e) {
            var godTables = this.FindControl<ComboBox>("GodTables");
            var selectedTable = godTables.SelectedItem as string;

            var fields = GlobalContainer.Fields(selectedTable!).ToList();
            fields.Remove("Id");
            var wnd = new AskForDataWindow {
                Table = selectedTable!
            };
            wnd.onSubmit = list =>
            {
                var fields = GlobalContainer.Fields(wnd.Table).ToList();
                fields.Remove("Id");
                var command = new SqlInsertInto(wnd.Table, list, fields);
                try {
                    SqliteAdapter.InsertInto(command);
                }
                catch {
                    // ????? idk what to do
                }
                UpdateList();
            };

            SetupAskForDataWindow(wnd, selectedTable!, fields);
            wnd.CanResize = false;
            wnd.Show();
        }

        private void DelEntry(object? _, RoutedEventArgs _2) {
            var godTables = this.FindControl<ComboBox>("GodTables");
            // var selectedTable
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
            _tableName = elem.SelectedItem!.ToString()!;
        }

        private void GodTables_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) {
            if (_grid == null) {
                _grid = this.FindControl<ListBox>("GodGrid");
            }
            if (_grid.SelectedItem != null) {
                _tableName = _grid.SelectedItem!.ToString()!;
                UpdateList();
            }
        }
    }
}
