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
        public static Data data { get; set; } = new Data();
    }

    public class Data {
        public string Field0 { get; set; } = "";
        public string Field1 { get; set; } = "";
        public string Field2 { get; set; } = "";
        public string Field3 { get; set; } = "";
        public string Field4 { get; set; } = "";
        public string Field5 { get; set; } = "";
        public string Field6 { get; set; } = "";
        public string Field7 { get; set; } = "";

        public Data() { }

        public Data(Data another) {
            for (int c = 0; c < 8; c++) {
                this[c] = another[c];
            }
        }

        public override string ToString() {
            return $"{Field0} {Field1} {Field2} {Field3} {Field4} {Field5} {Field6} {Field7}";
        }

        public string this[int index] {
            get {
                switch (index) {
                    case 0: return Field0;
                    case 1: return Field1;
                    case 2: return Field2;
                    case 3: return Field3;
                    case 4: return Field4;
                    case 5: return Field5;
                    case 6: return Field6;
                    case 7: return Field7;
                    default:
                        throw new IndexOutOfRangeException("Wrong index");
                }
            }
            set {
                switch (index) {
                    case 0: Field0 = value; break;
                    case 1: Field1 = value; break;
                    case 2: Field2 = value; break;
                    case 3: Field3 = value; break;
                    case 4: Field4 = value; break;
                    case 5: Field5 = value; break;
                    case 6: Field6 = value; break;
                    case 7: Field7 = value; break;
                    default:
                        throw new IndexOutOfRangeException("Wrong index");
                }
            }
        }
    }

    public class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public ObservableCollection<Data> Elements { get; set; } = new ObservableCollection<Data>() { };

        private int currentLibrarian = -1;

        public void SelectionChangedHandler(object sender, SelectionChangedEventArgs args) {
            updateTablePrinter();
        }

        public void onFilterSubmit(List<string> fields, List<(string, Operation, string)> list) {
            var table = this.FindControl<ComboBox>("Tables").SelectedItem as string;
            if (table is null)
                return;
#region GOVNOCODE
            var command = list.Count != 0 ? new SqlSelect(table, fields, list) : new SqlSelect(table, fields);
            var selected = SqliteAdapter.Select(command);
            var tablePrinter = this.FindControl<DataGrid>("TablePrinter");
            tablePrinter.Items = new List<string>();
            Elements.Clear();
            bool first = true;
            foreach (var element in selected) {
                var data = new Data();
                int p = 0;
                for (int c = 0; c < fields.Count(); c++) {
                    if (fields[c] != string.Empty) {
                        if (first) {
                            first = false;
                            for (int u = 0; u < 8; u++)
                                tablePrinter.Columns[u].Header = u < element.Count() ? element[u] : string.Empty; 
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
            tablePrinter.Items = Elements;
#endregion
        }

        public void onFilterButton(object sender, RoutedEventArgs args) {
            var table = this.FindControl<ComboBox>("Tables").SelectedItem as string;
            if (table is null)
                return;
            var headers = GlobalContainer.Fields(table).ToList();
            var wnd = new FilterWindow();
            for (int c = 0; c < 8; c++) {
                var label = wnd.FindControl<Label>($"Label{c}");
                var comboBox = wnd.FindControl<ComboBox>($"ComboBox{c}");
                var textBox = wnd.FindControl<TextBox>($"TextBox{c}");
                var checkBox = wnd.FindControl<CheckBox>($"CheckBox{c}");
                if (c < headers.Count()) {
                    comboBox.Items = new List<Operation>() { Operation.Equal, Operation.EqualOrGreater, Operation.EqualOrLess, Operation.Greater, Operation.Less, Operation.NonEqual };
                    label.Content = headers[c];

                } else {
                    label.IsVisible = false;
                    comboBox.IsVisible = false;
                    textBox.IsVisible = false;
                    checkBox.IsVisible = false;
                }
            }
            wnd.onSubmit = onFilterSubmit;
            wnd.Show();
        }

        public void onCellEditBegining(object sender, DataGridBeginningEditEventArgs args) {
            var index = args.Row.GetIndex();
            TempStorage.data = new Data(Elements[index]);
        }

        public void onCellEditEnded(object sender, DataGridCellEditEndedEventArgs args) {
            var table = this.FindControl<ComboBox>("Tables").SelectedItem as string;
            if (table is null) return;
            var index = args.Row.GetIndex();
            if (Elements[index][0] != TempStorage.data[0]) {
                Elements[index] = new Data(TempStorage.data);
                var errorWindow = new WarningErrorWindow();
                errorWindow.FindControl<Label>("ErrorWarningMessage").Content = "Do whatever you want, but I won't allow you to change Id! Cocksucker...";
                errorWindow.Show();
                return;
            }
            var headers = GlobalContainer.Fields(table);
            var updatedData = new List<(string, string)>();
            for (int c = 0; c < headers.Count(); c++) {
                updatedData.Add((headers.ElementAt(c), Elements[index][c]));
            }
            var command = new SqlUpdate(
                (string)table,
                updatedData,
                new List<(string, Operation, string)>() {
                    ("Id", Operation.Equal, TempStorage.data[0])
                }
            );
            try {
                Common.SqlCommands.SqliteAdapter.Update(command);
            } catch (Exception ex) {
                var errorWindow = new WarningErrorWindow();
                Elements[index] = new Data(TempStorage.data);
                errorWindow.FindControl<Label>("ErrorWarningMessage").Content = ex.Message;
                errorWindow.Show();
            }
        }

        public void SetupAskForDataWindow(AskForDataWindow wnd, IEnumerable<string> labels) {
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

        public void ReloadDb(object sender, RoutedEventArgs args) {
            updateTablePrinter();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void EnableControls() {
            List<string> buttonsToEnable = new() { "AddTeacherButton", "AddStudentButton", "FilterButton", "UpdateButton", "GodModeButton" };
            foreach (var button in buttonsToEnable) {
                this.FindControl<Button>(button).IsEnabled = true;
            }
            this.FindControl<ComboBox>("Tables").IsEnabled = true;
        }

        public void updateTablePrinter() {
            Elements.Clear();
            var tables = this.FindControl<DataGrid>("TablePrinter");
            tables.Items = null;
            var currentTable = this.FindControl<ComboBox>("Tables").SelectedItem as string;
            if (currentTable is null) {
                return;
            }
            
            var listoflists = Common.SqlCommands.SqliteAdapter.Select(
                new Common.SqlCommands.SqlSelect(currentTable)
            );
            var fields = Common.GlobalContainer.Fields(currentTable);
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
                var elem = this.FindControl<ComboBox>("Tables");
                elem.Items = from table in GlobalContainer.Tables where table == "StudentCards" || table == "TeacherCards" select table;
            }
        }

        public void onLoginPressed(object sender, RoutedEventArgs e) {
            var wnd = new AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string>() { "First name", "Last name" });
            wnd.onSubmit = onLoginSubmit;
            wnd.Show();
        }

        public void onLoginSubmit(List<string> fields) {
            var request = new SqlSelect("LibraryEmployees");
            var employees = Common.SqlCommands.SqliteAdapter.Select(request);
            foreach (var employee in employees) {
                if (employee.Contains(fields[0]) && employee.Contains(fields[1])) {
                    EnableControls();
                    currentLibrarian = int.Parse(employee[0]);
                    break;
                }
            }
        }

        private void onSubmitLogic(List<string> values, List<string> fields) {
            var table = this.FindControl<ComboBox>("Tables").SelectedItem as string;
            if (table is null) {
                return;
            }
            values.Add(currentLibrarian.ToString());
            var request = new Common.SqlCommands.SqlInsertInto(table, values, fields);
            SqliteAdapter.InsertInto(request);
            updateTablePrinter();
        }

        public void onTeacherSubmit(List<string> values) {//BookId, DueDate, TeacherId, TakenDate
            onSubmitLogic(values, new List<string>() { "BookId", "TeacherId", "TakenDate", "LibraryEmployeeId" });
        }

        public void onStudentSubmit(List<string> values) {//BookId, DueDate, StudentId, TakenDate
            onSubmitLogic(values, new List<string>() { "BookId", "DueDate", "StudentId", "TakenDate", "LibraryEmployeeId" });
        }

        public void AddStudentEntry(object sender, RoutedEventArgs e) {
            var wnd = new AvaloniaGUI.AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string>() { "BookId", "DueDate", "StudentId", "TakenDate" });
            wnd.onSubmit = onStudentSubmit;
            wnd.Show();
        }

        public void AddTeacherEntry(object sender, RoutedEventArgs e) {
            var wnd = new AvaloniaGUI.AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string>() { "BookId", "TeacherId", "TakenDate" });
            wnd.onSubmit = onTeacherSubmit;
            wnd.Show();
        }

        public void SwitchToGodMode(object sender, RoutedEventArgs e) {
            var wnd = new GodModWindow();
            wnd.Closed += (_, _2) =>
            {
                Show();
                Focus();
            };
            wnd.Show();
            Hide();
        }

        public void SpawnWindow(object sender, RoutedEventArgs e) {
            var wnd = new AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string> { "First", "Second", "Third" });
            wnd.Show();
        }
    }
}
