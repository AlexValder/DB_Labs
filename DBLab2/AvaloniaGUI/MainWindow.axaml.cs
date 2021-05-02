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
    public class Data {
        public string Field0 { get; set; } = "";
        public string Field1 { get; set; } = "";
        public string Field2 { get; set; } = "";
        public string Field3 { get; set; } = "";
        public string Field4 { get; set; } = "";
        public string Field5 { get; set; } = "";
        public string Field6 { get; set; } = "";
        public string Field7 { get; set; } = "";

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
            updateListBox();
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

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void EnableControls() {
            List<string> buttonsToEnable = new() { "AddTeacherButton", "AddStudentButton", "EditButton", "FilterButton", "GodModeButton" };
            foreach (var button in buttonsToEnable) {
                this.FindControl<Button>(button).IsEnabled = true;
            }
            this.FindControl<ComboBox>("Tables").IsEnabled = true;
        }

        public void updateListBox() {
            Elements.Clear();
            var tables = this.FindControl<DataGrid>("TablePrinter");
            tables.Items = null;
            var currentTable = this.FindControl<ComboBox>("Tables").SelectedItem as string;
            if (currentTable is null) {
                return;
            }
            var command = new Common.SqlCommands.SqlSelect(currentTable);
            var listoflists = Common.SqlCommands.SqliteAdapter.Select(command);
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
            updateListBox();
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
