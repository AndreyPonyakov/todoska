using System.Windows;
using TodoSystem.UI.Model;
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
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainView = new MainWindow();
            mainView.Show();
            mainView.DataContext = new WorkspaceViewModel(
                new CommandFactory(),
                new TodoService());
        }
    }
}
