using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TodoSystem.UI.Model;
using TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;
using TodoSystem.UI.ViewModel.Event;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel of main todo controller
    /// </summary>
    /// TODO: Pull members up.
    public sealed class CategoryControllerViewModel : BaseOrderedControllerViewModel<ICategoryController>
    {
        /// <summary>
        /// Factory for create ICommand instance
        /// </summary>
        private readonly ICommandFactory _commandFactory;

        private ITodoService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryControllerViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand instance. </param>
        public CategoryControllerViewModel(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
            List = new ObservableCollection<CategoryViewModel>();
            CreateCategoryCommand = commandFactory.CreateCommand(() => CreateItem());
            this.SetPropertyChanged(nameof(Service), () => Refresh());
        }

        /// <summary>
        /// Gets or sets back-end service.
        /// </summary>
        public ITodoService Service
        {
            get { return _service; }
            set { SetField(ref _service, value); }
        }

        /// <summary>
        /// Gets category list.
        /// </summary>
        public ObservableCollection<CategoryViewModel> List { get; }

        /// <summary>
        /// Gets or sets create category command.
        /// </summary>
        public ICommand CreateCategoryCommand { get; set; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        /// <returns>A new <see cref="CategoryViewModel"/> instance. </returns>
        public CategoryViewModel CreateItem()
        {
            var category = new CategoryViewModel(_commandFactory, _service);
            category.Order = List.Any() ? List.Max(c => c.Order) + 1 : 0;
            List.Add(category);

            category.SetPropertyChanged(
                nameof(category.Appended),
                () =>
                    {
                        if (category.Appended)
                        {
                            category.Appended = false;
                        }
                    }).SetPropertyChanged(
                        nameof(category.Canceled),
                        () =>
                            {
                                if (category.Canceled)
                                {
                                    category.Canceled = false;
                                    List.Remove(category);
                                }
                            })
                .SetPropertyChanged(
                    nameof(category.Deleted),
                    () =>
                        {
                            if (category.Deleted)
                            {
                                category.Deleted = false;
                                List.Remove(category);
                            }
                        });

            category.MoveToEvent += (sender, args) =>
                {
                    List.MoveTo(args.DataTransition);
                    List.Select((val, i) => new { Value = val, Index = i })
                        .ToList()
                        .ForEach(rec => rec.Value.Order = rec.Index);
                };
            
            return category;
        }

        /// <summary>
        /// Updates at the service.
        /// </summary>
        public void Refresh()
        {
            List.Clear();
            Service.CategoryController
                .SelectAll()
                .OrderBy(t => t.Order)
                .ToList()
                .ForEach(item => CreateItem().Refresh(item));
        }

        /// <summary>
        /// Commit all uncommitted changes.
        /// </summary>
        public void Apply()
        {
            List
                .Where(item => item.CanApply)
                .ToList()
                .ForEach(item => item.Apply());
        }
    }
}
