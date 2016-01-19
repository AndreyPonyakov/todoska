﻿using System.Drawing;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of category.
    /// </summary>
    public sealed class CategoryViewModel : BaseViewModel
    {
        /// <summary>
        /// Todo service
        /// </summary>
        private readonly ITodoService _service;

        /// <summary>
        /// ICategory of current category
        /// </summary>
        public ICategory Model { get; set; }

        /// <summary>
        /// Behind field of Name.
        /// </summary>
        private string _name;
        /// <summary>
        /// Name of category.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value);}
        }

        /// <summary>
        /// Behind field of Color.
        /// </summary>
        private Color _color;
        /// <summary>
        /// Color of category.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }

        /// <summary>
        /// Behind field of Appended
        /// </summary>
        private bool _appended;
        /// <summary>
        /// True if category is committed.
        /// </summary>
        public bool Appended
        {
            get { return _appended; }
            set { SetField(ref _appended, value); }
        }

        public bool CreateVisibility => !Appended;

        /// <summary>
        /// Behind field of Order
        /// </summary>
        private int _order;
        /// <summary>
        /// Priority of category.
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { SetField(ref _order, value); }
        }

        /// <summary>
        /// Finish of creating category.
        /// </summary>
        public void Create()
        {
            Model =_service.CategoryController.Create(Name, Color, 0);
            Appended = true;
        }

        /// <summary>
        /// Create category command
        /// </summary>
        public ICommand CreateCommand { get; }

        /// <summary>
        /// Constructor of CategoryViewModel
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        public CategoryViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            _service = service;
            CreateCommand = commandFactory.CreateCommand(Create);
            this.SetPropertyChanged(
                nameof(Appended), 
                () => OnPropertyChanged(nameof(CreateVisibility)));
        }
    }
}
