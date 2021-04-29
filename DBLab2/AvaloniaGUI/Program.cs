using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Common;
using System.Collections.Generic;

namespace AvaloniaGUI {
    public class Program {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) {
            if (!Common.SqlCommands.SqliteAdapter.TrySetDatabase("Library.db")) {
                return;
            }
            //var t = new AvaloniaGUI.AskForDataWindow();
            //t.SetupAskForDataWindow(new List<string> { "FirstName", "LastName" });
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
