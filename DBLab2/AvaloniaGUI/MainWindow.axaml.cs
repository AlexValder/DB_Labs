using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.SqlCommands;
using System;
using System.Collections.ObjectModel;

namespace AvaloniaGUI {
    public static class TempStorage {
        public static Data Data { get; set; } = new();
    }

    public class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _printer = this.FindControl<DataGrid>("TablePrinter");
            _tables = this.FindControl<ComboBox>("Tables");
        }

        private ObservableCollection<Data> Elements { get; } = new();

        public static int CurrentLibrarian { get; private set; } = -1;
        private readonly DataGrid _printer;
        private readonly ComboBox _tables;

        public void SelectionChangedHandler(object sender, SelectionChangedEventArgs args) {
            UpdateTablePrinter();
        }

        private void OnFilterSubmit(List<string> fields, List<(string, Operation, string)> list) {
            if (_tables.SelectedItem is not string table)
                return;

            var command = list.Count != 0 ? new SqlSelect(table, fields, list) : new SqlSelect(table, fields);
            var selected = SqliteAdapter.Select(command);
            _printer.Items = new List<string>();
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
                                _printer.Columns[u].Header = u < element.Count ? element[u] : string.Empty;
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
            _printer.Items = Elements;
            _printer.IsReadOnly = Elements.Count < GlobalContainer.FieldCount(table);
        }

        public void OnFilterButton(object sender, RoutedEventArgs args) {
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
                    comboBox.Items = new List<Operation>() { Operation.Equal, Operation.EqualOrGreater, Operation.EqualOrLess, Operation.Greater, Operation.Less, Operation.NonEqual };
                    label.Content = headers[c];

                } else {
                    label.IsVisible = false;
                    comboBox.IsVisible = false;
                    textBox.IsVisible = false;
                    checkBox.IsVisible = false;
                }
            }
            wnd.OnSubmit = OnFilterSubmit;
            wnd.Show();
        }

        public void OnCellEditBegining(object sender, DataGridBeginningEditEventArgs args) {
            var index = args.Row.GetIndex();
            TempStorage.Data = new Data(Elements[index]);
        }

        public void OnCellEditEnded(object sender, DataGridCellEditEndedEventArgs args) {
            if (_tables.SelectedItem is not string table) return;
            var index = args.Row.GetIndex();
            if (Elements[index][0] != TempStorage.Data[0]) {
                Elements[index] = new Data(TempStorage.Data);
                var errorWindow = new WarningErrorWindow();
                errorWindow.FindControl<TextBlock>("ErrorWarningMessage").Text = "Do whatever you want, but I won't allow you to change Id!";
                errorWindow.FindControl<Label>("ErrorWarningTitle").Content = "⚠ ERROR";
                errorWindow.Show();
                return;
            }
            var headers = GlobalContainer.Fields(table);
            var updatedData = headers.Select((_, c) => (headers.ElementAt(c), Elements[index][c])).ToList();

            var hasSecondEmployee = false;
            foreach (var (key, value) in updatedData) {
                if (key != "LibraryEmployee2Id" || !string.IsNullOrEmpty(value)) continue;
                updatedData.Remove((key, value));
                updatedData.Add(("LibraryEmployee2Id", CurrentLibrarian.ToString()));
                hasSecondEmployee = true;
                break;
            }

            if (!hasSecondEmployee) {
                updatedData.Add(("LibraryEmployee2Id", CurrentLibrarian.ToString()));
            }

            var command = new SqlUpdate(
                table,
                updatedData,
                new List<(string, Operation, string)> {
                    ("Id", Operation.Equal, TempStorage.Data[0])
                }
            );
            try {
                SqliteAdapter.Update(command);
            } catch (Exception ex) {
                var errorWindow = new WarningErrorWindow();
                Elements[index] = new Data(TempStorage.Data);
                errorWindow.FindControl<Label>("ErrorWarningTitle").Content = "⚠ ERROR";
                errorWindow.FindControl<TextBlock>("ErrorWarningMessage").Text = ex.Message;
                errorWindow.Show();
            }
        }

        private void SetupAskForDataWindow(Control wnd, ICollection<string> labels) {
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

        public void ReloadDb(object sender, RoutedEventArgs args) {
            _printer.IsReadOnly = false;
            UpdateTablePrinter();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void EnableControls() {
            List<string> buttonsToEnable = new() { "AddTeacherButton", "AddStudentButton", "FilterButton", "UpdateButton", "GodModeButton" };
            foreach (var button in buttonsToEnable) {
                this.FindControl<Button>(button).IsEnabled = true;
            }
            _tables.IsEnabled = true;
        }

        private void UpdateTablePrinter() {
            Elements.Clear();
            _printer.Items = null;
            if (_tables.SelectedItem is not string currentTable) {
                return;
            }

            var listoflists = SqliteAdapter.Select(
                new SqlSelect(currentTable)
            );
            var fields = GlobalContainer.Fields(currentTable);
            foreach (var sublist in listoflists) {
                if (sublist[0] == "Id") {
                    for (int i = 0; i < 8; i++) {
                        _printer.Columns[i].Header = i < fields.Count ? sublist[i] : "";
                    }
                    continue;
                }
                var data = new Data();
                for (int i = 0; i < fields.Count; i++) {
                    data[i] = sublist.ElementAt(i);
                }
                Elements.Add(data);
            }
            _printer.Items = Elements;
        }

        public async void OpenDbClick(object sender, RoutedEventArgs e) {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filters.Add(new FileDialogFilter() { Name = "Text", Extensions = { "db" } });
            var res = await fileDialog.ShowAsync(this);
            if (res is null || res.Length < 1 || res[0].Length == 0) {
                return;
            }

            var isDbSet = SqliteAdapter.TrySetDatabase(res[0]);
            if (isDbSet) {
                this.FindControl<Button>("LoginButton").IsEnabled = true;
                _tables.Items = from table in GlobalContainer.Tables where table is "StudentCards" or "TeacherCards" select table;
            }
        }

        public void OnLoginPressed(object sender, RoutedEventArgs e) {
            var wnd = new AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string> { "First name", "Last name" });
            wnd.OnSubmit = OnLoginSubmit;
            wnd.Show();
        }

        private void OnLoginSubmit(List<string> fields) {
            var request = new SqlSelect("LibraryEmployees");
            var employees = SqliteAdapter.Select(request);
            foreach (var employee in employees.Where(employee => employee.Contains(fields[0]) && employee.Contains(fields[1]))) {
                EnableControls();
                CurrentLibrarian = int.Parse(employee[0]);
                return;
            }
            var wnd = new WarningErrorWindow();
            wnd.FindControl<Label>("ErrorWarningTitle").Content = "BAD CREDENTIALS";
            wnd.FindControl<TextBlock>("ErrorWarningMessage").Text = "This library employee was not found.";
            wnd.Show();
        }

        private void OnSubmitLogic(IList<string> values, IList<string> fields) {
            if (_tables.SelectedItem is not string table) {
                return;
            }
            values.Add(CurrentLibrarian.ToString());
            var request = new SqlInsertInto(table, values, fields);
            SqliteAdapter.InsertInto(request);
            UpdateTablePrinter();
        }

        private void OnTeacherSubmit(List<string> values) {//BookId, DueDate, TeacherId, TakenDate
            OnSubmitLogic(values, new List<string> { "BookId", "TeacherId", "TakenDate", "LibraryEmployeeId" });
        }

        private void OnStudentSubmit(List<string> values) {//BookId, DueDate, StudentId, TakenDate
            OnSubmitLogic(values, new List<string> { "BookId", "DueDate", "StudentId", "TakenDate", "LibraryEmployeeId" });
        }

        public void AddStudentEntry(object sender, RoutedEventArgs e) {
            var wnd = new AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string> { "BookId", "DueDate", "StudentId", "TakenDate" });
            wnd.OnSubmit = OnStudentSubmit;
            wnd.Show();
        }

        public void AddTeacherEntry(object sender, RoutedEventArgs e) {
            var wnd = new AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string>() { "BookId", "TeacherId", "TakenDate" });
            wnd.OnSubmit = OnTeacherSubmit;
            wnd.Show();
        }

        public void SwitchToGodMode(object sender, RoutedEventArgs e) {
            var wnd = new GodModWindow();
            wnd.Closed += (_, _) =>
            {
                Show();
                Focus();
            };
            wnd.Show();
            Hide();
        }

        public void ShowCredits(object? sender, RoutedEventArgs e) {
            // ReSharper disable StringLiteralTypo
            WarningErrorWindow.ShowCredits("Credits", "MADE BY", "@ArthurMamedov\n@AlexValder", "Ok");
            // ReSharper restore StringLiteralTypo
        }
    }
}
