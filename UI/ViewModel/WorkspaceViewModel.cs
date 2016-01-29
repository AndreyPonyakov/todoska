﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using TodoSystem.UI.Model;
using TodoSystem.UI.Tools.Model;

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
        private INotifyPropertyChanged _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="serviceFactory">Todo service factory. </param>
        public WorkspaceViewModel(ICommandFactory commandFactory, Func<string, TodoService> serviceFactory)
        {
            _serviceFactory = serviceFactory;

            RefreshCommand = commandFactory.CreateCommand(Refresh);
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
        /// Gets refresh command.
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Gets apply command.
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Gets or sets current controller.
        /// </summary>
        public INotifyPropertyChanged Controller
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
        /// Update from service.
        /// </summary>
        public void Refresh()
        {
            CategoryController.Refresh();
            TodoController.Refresh();
        }

        /// <summary>
        /// Commit all uncommitted changes.
        /// </summary>
        public void Apply()
        {
            CategoryController.Apply();
            TodoController.Apply();
        }

        /// <summary>
        /// Apply new address and create new service.
        /// </summary>
        public void ApplyAddress()
        {
            Service = _serviceFactory(Address);
        }
    }
}