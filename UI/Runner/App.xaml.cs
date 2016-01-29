using System;
using System.Windows;
using TodoSystem.UI.Model;
using TodoSystem.UI.Runner.Properties;
using TodoSystem.UI.Tools.View;
using TodoSystem.UI.ViewModel;

namespace TodoSystem.UI.Runner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Startup implement.
        /// </summary>
        /// <param name="e">Startup arguments (command line parameters). </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainView = new MainWindow();
            mainView.Show();
            var context = new WorkspaceViewModel(
                new CommandFactory(),
                address => new TodoService(address))
                              {
                                  Address = Settings.Default.Address
                              };

            mainView.DataContext = context;
            mainView.Dispatcher.ShutdownStarted +=
                (sender, args) => (context.Service as IDisposable)?.Dispose();
        }
    }
}
