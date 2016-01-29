using System.Linq;
using System.ServiceModel;

using TodoSystem.UI.Model;
using TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel of main todo controller
    /// </summary>
    public sealed class CategoryControllerViewModel : BaseOrderableControllerViewModel<ITodoService, Category, CategoryViewModel>
    {
        /// <summary>
        /// ViewModel of main workspace.
        /// </summary>
        private readonly WorkspaceViewModel _workspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryControllerViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand instance. </param>
        /// <param name="workspace">Workspace with controllers. </param>
        public CategoryControllerViewModel(ICommandFactory commandFactory, WorkspaceViewModel workspace)
            : base(commandFactory)
        {
            _workspace = workspace;
        }

        /// <summary>
        /// Undo create new item.
        /// </summary>
        /// <returns>A new <see cref="CategoryViewModel"/> instance. </returns>
        public override CategoryViewModel CreateItem()
        {
            var category = new CategoryViewModel(CommandFactory, Service);
            AppendItem(category);            
            return category;
        }

        /// <summary>
        /// Updates at the service.
        /// </summary>
        /// TODO: implement pull model of notifying errors.
        public override void Refresh()
        {
            try
            {
                List.Clear();
                Service.CategoryController
                    .SelectAll()
                    .OrderBy(t => t.Order)
                    .ToList()
                    .ForEach(item => CreateItem().Refresh(item));
            }
            catch (CommunicationException)
            {
                _workspace.AppendErrors(nameof(_workspace.Address), "There was no category endpoint.");
            }
        }
    }
}
