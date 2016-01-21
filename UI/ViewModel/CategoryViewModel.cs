﻿using System.Drawing;
using System.Windows.Input;
using Todo.Service.Model.Interface;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of category.
    /// TODO State Pattern
    /// </summary>
    public sealed class CategoryViewModel : BaseViewModel
    {
        private readonly ITodoService _service;


        /// <summary>
        /// ICategory of current category
        /// </summary>
        public ICategory Model { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }

        private int _order;
        public int Order
        {
            get { return _order; }
            set { SetField(ref _order, value); }
        }


        private bool _modified;
        public bool Modified
        {
            get { return _modified; }
            set { SetField(ref _modified, value); }
        }

        private bool _appended;
        public bool Appended
        {
            get { return _appended; }
            set { SetField(ref _appended, value); }
        }

        private bool _canceled;
        public bool Canceled
        {
            get { return _canceled; }
            set { SetField(ref _canceled, value); }
        }

        private bool _deleted;
        public bool Deleted
        {
            get { return _deleted; }
            set { SetField(ref _deleted, value); }
        }

        public bool InAction => Appended || Canceled || Deleted;
        public bool CanApply => !InAction && (Modified || Model == null);
        public bool CanUndo => !InAction && (Modified || Model == null);
        public bool CanDelete => !InAction && Model != null;

        /// <summary>
        /// Finish of creating category.
        /// </summary>
        public void Apply()
        {
            if (Model == null)
            {
                Model = _service.CategoryController.Create(Name, Color, 0);
                Appended = true;
                Modified = false;
            }
            else
            {
                _service.CategoryController.Update(Model);
                Modified = false;
            }
        }

        /// <summary>
        /// Create category command.
        /// </summary>
        public void Undo()
        {
            if (Model == null)
            {
                Canceled = true;
                Modified = false;
            }
            else
            {
                _service.CategoryController.Update(Model);
                Modified = false;
            }
        }

        /// <summary>
        /// Create category command.
        /// </summary>
        public void Delete()
        {
            _service.CategoryController.Delete(Model.Id);
            if (_service.CategoryController.SelectById(Model.Id) == null)
            {
                Deleted = true;
            }
        }

        /// <summary>
        /// Create category command
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Undo create category command.
        /// </summary>
        public ICommand UndoCommand { get; set; }
        /// <summary>
        /// Undo create category command.
        /// </summary>
        public ICommand DeleteCommand { get; set; }


        /// <summary>
        /// Constructor of CategoryViewModel
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        public CategoryViewModel(ICommandFactory commandFactory, ITodoService service)
        {
            _service = service;
            ApplyCommand = commandFactory.CreateCommand(Apply, () => CanApply);
            UndoCommand = commandFactory.CreateCommand(Undo, () => CanUndo);
            DeleteCommand = commandFactory.CreateCommand(Delete, () => CanDelete);
            this
                .SetPropertyChanged(
                    new[] {nameof(Name), nameof(Color)},
                    () => Modified = true)
                .SetPropertyChanged(
                    new[] {nameof(Appended), nameof(Canceled), nameof(Deleted)},
                    () => OnPropertyChanged(nameof(InAction)))
                .SetPropertyChanged(
                    nameof(InAction),
                    () => OnPropertyChanged(nameof(CanDelete)))
                .SetPropertyChanged(
                    new[] {nameof(InAction), nameof(Modified)},
                    () =>
                    {
                        OnPropertyChanged(nameof(CanApply));
                        OnPropertyChanged(nameof(CanUndo));
                    });
        }
    }
}
