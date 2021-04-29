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

        private int currentLibrarian = -1;

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
            List<string> buttonsToEnable = new() { "AddTeacherButton", "AddStudentButton", "EditButton", "FilterButton" };
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

        public void onSubmit(List<string> fields) {
               
        }

        public void AddStudentEntry(object sender, RoutedEventArgs e) {
            var wnd = new AvaloniaGUI.AskForDataWindow();
            SetupAskForDataWindow(wnd, new List<string>() { "BookId", "DueDate", "StudentId", "TakenDate" });
            wnd.onSubmit = onSubmit;
            wnd.Show();
        }

        public void AddTeacherEntry(object sender, RoutedEventArgs e) {
            //I will do it later...
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
