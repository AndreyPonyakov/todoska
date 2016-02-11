using System;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;

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
        /// Initializes a new instance of the <see cref="CategoryControllerViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand instance. </param>
        public CategoryControllerViewModel(ICommandFactory commandFactory)
            : base(commandFactory)
        {
            SortByNameAscCommand = CommandFactory.CreateCommand(() => Sort(c => c.Name));
        }

        /// <summary>
        /// Gets sorting by name command.
        /// </summary>
        public ICommand SortByNameAscCommand { get; }

        /// <summary>
        /// Undo create new item.
        /// </summary>
        /// <returns>A new instance of the <see cref="CategoryViewModel"/> class. </returns>
        public override CategoryViewModel CreateItem()
        {
            var category = new CategoryViewModel(CommandFactory, Service);
            AppendItem(category);
            return category;
        }

        /// <summary>
        /// Updates at the service.
        /// </summary>
        public override void Open()
        {
            ClearErrors(nameof(Service));
            try
            {
                Service.CategoryController
                    .SelectAll()
                    .OrderBy(t => t.Order)
                    .ToList()
                    .ForEach(item => CreateItem().Refresh(item));
            }
            catch (CommunicationException)
            {
                AppendErrors(nameof(Service), "There was no category endpoint.");
            }
        }
    }
}
