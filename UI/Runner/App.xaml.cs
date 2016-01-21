using System;
using System.Diagnostics;
using System.Windows;
using Todo.Service.Model.Fake;
using Todo.UI.Tools.View;
using Todo.UI.ViewModel;

namespace Todo.UI.Runner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var mainView = new MainWindow();
                mainView.Show();
                mainView.DataContext = new WorkspaceViewModel(
                    new CommandFactory(), 
                    FakeTodoService.Instance);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}
