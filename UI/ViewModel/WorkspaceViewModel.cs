using System;
using System.Collections.Generic;
using System.Windows.Input;
using TodoSystem.UI.Model;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// Root ViewModel.
    /// </summary>
    public sealed class WorkspaceViewModel : BaseViewModel
    {
        private readonly Func<string, ITodoService> _serviceFactory;

        private ITodoService _service;
        private string _address;

        /// <summary>
        /// Current controller.
        /// </summary>
        private IController<ITodoService> _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="serviceFactory">Todo service factory. </param>
        public WorkspaceViewModel(ICommandFactory commandFactory, Func<string, TodoService> serviceFactory)
        {
            _serviceFactory = serviceFactory;

            ApplyCommand = commandFactory.CreateCommand(Apply);
            ApplyAddressCommand = commandFactory.CreateCommand(ApplyAddress);

            CategoryController = new CategoryControllerViewModel(commandFactory);
            TodoController = new TodoControllerViewModel(commandFactory, this);

            Controller = TodoController;
            this.SetPropertyChanged(
                nameof(Service),
                () =>
                    {
                        CategoryController.Service = Service;
                        TodoController.Service = Service;
                    });

            RetrieveErrors(
                nameof(Address),
                new Dictionary<IController<ITodoService>, string>
                    {
                        [CategoryController] = nameof(CategoryController.Service),
                        [TodoController] = nameof(TodoController.Service)
                    });
        }

        /// <summary>
        /// Gets or sets back-end service.
        /// </summary>
        public ITodoService Service
        {
            get
            {
                return _service;
            }

            set
            {
                var last = _service;
                try
                {
                    SetField(ref _service, value);
                }
                finally
                {
                    if (last != _service)
                    {
                        (last as IDisposable)?.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets service address.
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { SetField(ref _address, value); }
        }

        /// <summary>
        /// Gets update command.
        /// </summary>
        public ICommand ApplyAddressCommand { get; }

        /// <summary>
        /// Gets apply command.
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Gets or sets current controller.
        /// </summary>
        public IController<ITodoService> Controller
        {
            get { return _controller; }
            set { SetField(ref _controller, value); }
        }

        /// <summary>
        /// Gets category controller.
        /// </summary>
        public CategoryControllerViewModel CategoryController { get; }

        /// <summary>
        /// Gets todo controller.
        /// </summary>
        public TodoControllerViewModel TodoController { get; }

        /// <summary>
        /// Commits all uncommitted changes.
        /// </summary>
        public void Apply()
        {
            CategoryController.Apply();
            TodoController.Apply();
        }

        /// <summary>
        /// Applies new address and create new service.
        /// </summary>
        public void ApplyAddress()
        {
            ClearErrors(nameof(Address));
            Service = _serviceFactory(Address);

            TodoController.Close();
            CategoryController.Close();

            CategoryController.Open();
            TodoController.Open();
        }
    }
}