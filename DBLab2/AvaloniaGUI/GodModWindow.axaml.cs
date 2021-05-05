using System;
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
        private readonly DataGrid _grid;
        private readonly ComboBox _tables;
        private string? _tableName = "";

        public ObservableCollection<Data> Elements { get; set; } = new();

        private readonly ImmutableList<string> _buttonNames = new List<string> {
            "AddButton",
            "RemoveButton",
            "ReloadButton",
            "FilterButton",
        }.ToImmutableList();

        public GodModWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _grid = this.FindControl<DataGrid>("GodGrid");
            _tables = this.FindControl<ComboBox>("GodTables");
            if (!string.IsNullOrEmpty(GlobalContainer.BdSelected)) {
                EnableControls();
                _tables.Items = from table in GlobalContainer.Tables where table != "Tables" select table;
            }
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void UpdateList() {
            Elements.Clear();
            var tables = this.FindControl<DataGrid>("GodGrid");
            tables.Items = null;
            var currentTable = this.FindControl<ComboBox>("GodTables").SelectedItem as string;
            if (currentTable is null) {
                return;
            }

            var listoflists = SqliteAdapter.Select(
                new SqlSelect(currentTable)
            );
            var fields = GlobalContainer.Fields(currentTable);
            foreach (var sublist in listoflists) {
                if (sublist[0] == "Id") {
                    for (int i = 0; i < 8; i++)
                        if (i < fields.Count())
                            tables.Columns[i].Header = sublist[i];
                        else
                            tables.Columns[i].Header = "";
                    continue;
                }
                var data = new Data();
                for (int i = 0; i < fields.Count(); i++) {
                    data[i] = sublist.ElementAt(i);
                }
                Elements.Add(data);
            }
            tables.Items = Elements;
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

            if (selectedTable is null) {
                return;
            }

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
            if (_tables.SelectedItem is not string table)
                return;
            var headers = GlobalContainer.Fields(table).ToList();
            var wnd = new FilterWindow();
            for (int c = 0; c < 8; c++) {
                var label = wnd.FindControl<Label>($"Label{c}");
                var comboBox = wnd.FindControl<ComboBox>($"ComboBox{c}");
                var textBox = wnd.FindControl<TextBox>($"TextBox{c}");
                var checkBox = wnd.FindControl<CheckBox>($"CheckBox{c}");
                if (c < headers.Count) {
                    comboBox.Items = new List<Operation> { Operation.Equal, Operation.EqualOrGreater, Operation.EqualOrLess, Operation.Greater, Operation.Less, Operation.NonEqual };
                    label.Content = headers[c];

                } else {
                    label.IsVisible = false;
                    comboBox.IsVisible = false;
                    textBox.IsVisible = false;
                    checkBox.IsVisible = false;
                }
            }
            wnd.onSubmit = OnDeleteSubmit;
            wnd.Show();
        }

        private void OnDeleteSubmit(List<string> fields, List<(string, Operation, string)> list) {
            if (_tables.SelectedItem is not string table)
                return;
            var command = new SqlDelete(table, list);
            SqliteAdapter.Delete(command);
            UpdateList();
        }

        public void OnCellEditEnded(object sender, DataGridCellEditEndedEventArgs args) {
            if (this.FindControl<ComboBox>("GodTables").SelectedItem is not string table) {
                return;
            }

            var index = args.Row.GetIndex();
            if (Elements[index][0] != TempStorage.data[0]) {
                Elements[index] = new Data(TempStorage.data);
                var errorWindow = new WarningErrorWindow();
                errorWindow.FindControl<Label>("ErrorWarningMessage").Content = "Do whatever you want, but I won't allow you to change Id!";
                errorWindow.Show();
                return;
            }
            var headers = GlobalContainer.Fields(table);
            var updatedData = headers.Select((t, i) => (headers.ElementAt(i), Elements[index][i])).ToList();
            var (item1, item2) = updatedData[^1];
            if (item1.EndsWith("Id") && string.IsNullOrEmpty(item2)) {
                updatedData[^1] = (item1, MainWindow.CurrentLibrarian.ToString());
            }
            var command = new SqlUpdate(
                table,
                updatedData,
                new List<(string, Operation, string)> {
                    ("Id", Operation.Equal, TempStorage.data[0])
                }
            );
            try {
                SqliteAdapter.Update(command);
            } catch (Exception ex) {

                var errorWindow = new WarningErrorWindow();
                Elements[index] = new Data(TempStorage.data);
                errorWindow.FindControl<Label>("ErrorWarningMessage").Content = ex.Message;
                errorWindow.Show();
                //updateTablePrinter();
            }
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
            _tables.Items = from table in GlobalContainer.Tables where table != "Tables" select table;
        }

        private void GodTables_OnSelectionChanged(object? _, SelectionChangedEventArgs _2) {
            if (string.IsNullOrEmpty(_tableName)) {
                _tableName = _grid.SelectedItem as string;
            }
            UpdateList();
        }

        private void OnCellEditBegining(object? sender, DataGridBeginningEditEventArgs e) {
            var index = e.Row.GetIndex();
            TempStorage.data = new Data(Elements[index]);
        }

        private void OnFilterButton(object? sender, RoutedEventArgs e) {
            if (_tables.SelectedItem is not string table)
                return;
            var headers = GlobalContainer.Fields(table).ToList();
            var wnd = new FilterWindow();
            for (int c = 0; c < 8; c++) {
                var label = wnd.FindControl<Label>($"Label{c}");
                var comboBox = wnd.FindControl<ComboBox>($"ComboBox{c}");
                var textBox = wnd.FindControl<TextBox>($"TextBox{c}");
                var checkBox = wnd.FindControl<CheckBox>($"CheckBox{c}");
                if (c < headers.Count) {
                    comboBox.Items = new List<Operation> { Operation.Equal, Operation.EqualOrGreater, Operation.EqualOrLess, Operation.Greater, Operation.Less, Operation.NonEqual };
                    label.Content = headers[c];

                } else {
                    label.IsVisible = false;
                    comboBox.IsVisible = false;
                    textBox.IsVisible = false;
                    checkBox.IsVisible = false;
                }
            }
            wnd.onSubmit = OnFilterSubmit;
            wnd.Show();
        }

        private void OnFilterSubmit(List<string> fields, List<(string, Operation, string)> list) {
            if (_tables.SelectedItem is not string table)
                return;
            #region GOVNOCODE2
            var command = list.Count != 0 ? new SqlSelect(table, fields, list) : new SqlSelect(table, fields);
            var selected = SqliteAdapter.Select(command);
            _grid.Items = new List<string>();
            Elements.Clear();
            bool first = true;
            foreach (var element in selected) {
                var data = new Data();
                int p = 0;
                for (int c = 0; c < fields.Count; c++) {
                    if (fields[c] != string.Empty) {
                        if (first) {
                            first = false;
                            for (int u = 0; u < 8; u++)
                                _grid.Columns[u].Header = u < element.Count ? element[u] : string.Empty;
                            break;
                        }
                        data[c] = element[p];
                        p++;
                    } else {
                        data[c] = string.Empty;
                    }
                }
                Elements.Add(data);
            }
            _grid.Items = Elements;
            #endregion
        }

        private void ReloadButton_OnClick(object? sender, RoutedEventArgs e) {
            UpdateList();
        }

        private void OnCloseWorldButton(object? sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }
    }
}
