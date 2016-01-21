﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel class of Todo Controller.
    /// </summary>
    public sealed class TodoControllerViewModel : BaseViewModel
    {
        /// <summary>
        /// Todo Service
        /// </summary>
        private readonly ITodoService _service;

        /// <summary>
        /// Factory for create ICommand instance
        /// </summary>
        private readonly ICommandFactory _commandFactory;

        private readonly CategoryControllerViewModel _categoryController;

        /// <summary>
        /// Category list.
        /// </summary>
        public ObservableCollection<TodoViewModel> List { get; }

        /// <summary>
        /// Create category command.
        /// </summary>
        public ICommand CreateCategoryCommand { get; set; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public void CreateCategory()
        {
            var todo = new TodoViewModel(_commandFactory, _service, _categoryController);
            List.Add(todo);

            todo
                .SetPropertyChanged(
                    nameof(todo.Appended),
                    () =>
                    {
                        if (todo.Appended)
                        {
                            todo.Appended = false;
                        }
                    })
                .SetPropertyChanged(
                    nameof(todo.Canceled),
                    () =>
                    {
                        if (todo.Canceled)
                        {
                            todo.Canceled = false;
                            List.Remove(todo);
                        }
                    })
                .SetPropertyChanged(
                    nameof(todo.Deleted),
                    () =>
                    {
                        if (todo.Deleted)
                        {
                            todo.Deleted = false;
                            List.Remove(todo);
                        }
                    });
        }
    
        public TodoControllerViewModel(ICommandFactory commandFactory, ITodoService service, 
            CategoryControllerViewModel categoryController)
        {
            _service = service;
            _commandFactory = commandFactory;
            List = new ObservableCollection<TodoViewModel>();

            CreateCategoryCommand = commandFactory.CreateCommand(CreateCategory);
            _categoryController = categoryController;
        }
    }
}
