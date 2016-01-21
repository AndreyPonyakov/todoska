using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;
using Todo.UI.ViewModel.Event;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of main todo controller
    /// </summary>
    public class CategoryControllerViewModel : BaseViewModel
    {
        /// <summary>
        /// Todo Service
        /// </summary>
        private readonly ITodoService _service;

        /// <summary>
        /// Factory for create ICommand instance
        /// </summary>
        private readonly ICommandFactory _commandFactory;

        /// <summary>
        /// Category list.
        /// </summary>
        public ObservableCollection<CategoryViewModel> List { get; }

        /// <summary>
        /// Create category command.
        /// </summary>
        public ICommand CreateCategoryCommand { get; set; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public CategoryViewModel CreateItem()
        {
            var category = new CategoryViewModel(_commandFactory, _service);
            List.Add(category);

            category
                .SetPropertyChanged(
                    nameof(category.Appended),
                    () =>
                    {
                        if (category.Appended)
                        {
                            category.Appended = false;
                        }
                    })
                .SetPropertyChanged(
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

            category.MoveToEvent += (sender, args) => List.MoveTo(args.DataTransition);
            
            return category;
        }

        /// <summary>
        /// Create insatance if <see cref="CategoryControllerViewModel"/>.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand instance. </param>
        /// <param name="service">Todo service</param>
        public CategoryControllerViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            _service = service;
            _commandFactory = commandFactory;
            List = new ObservableCollection<CategoryViewModel>();
            CreateCategoryCommand = commandFactory.CreateCommand(() => CreateItem());
        }

        /// <summary>
        /// Update from serveice.
        /// </summary>
        /// <param name="model">Model. </param>
        public void Update(ICategoryController model)
        {
            List.Clear();
            model.SelectAll()
                .ToList()
                .ForEach(item => CreateItem().Update(item));
        }
    }
}
