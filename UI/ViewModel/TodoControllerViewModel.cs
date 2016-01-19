using System.Collections.ObjectModel;
using System.Windows.Input;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of main todo controller
    /// </summary>
    public class TodoControllerViewModel : BaseViewModel
    {
        /// <summary>
        /// Category list.
        /// </summary>
        public ObservableCollection<CategoryViewModel> Categories { get; }

        /// <summary>
        /// Create category command.
        /// </summary>
        public ICommand CreateCategoryCommand { get; set; }

        /// <summary>
        /// Create insatance if <see cref="TodoControllerViewModel"/>.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand instance. </param>
        public TodoControllerViewModel(ICommandFactory commandFactory)
        {
            Categories = new ObservableCollection<CategoryViewModel>();
        }
    }
}
