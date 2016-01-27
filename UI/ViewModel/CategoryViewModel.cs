using System.Drawing;
using System.Windows.Input;
using TodoSystem.UI.Model;
using TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel of category.
    /// TODO State Pattern
    /// </summary>
    public sealed class CategoryViewModel : BaseOrderedItemViewModel<ITodoService, Category, CategoryViewModel>
    {
        private string _name;
        private Color _color;

        private bool _dataModified;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for <see cref="ICommand"/> instance. </param>
        /// <param name="service">Todo service. </param>
        public CategoryViewModel(ICommandFactory commandFactory, ITodoService service)
            : base(service, commandFactory)
        {
            this.SetPropertyChanged(
                new[] { nameof(Name), nameof(Color) },
                () => DataModified = true)
                .SetPropertyChanged(
                    new[] { nameof(OrderModified), nameof(DataModified) },
                    () =>
                        {
                            Modified = DataModified || OrderModified;
                        });
        }

        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        /// <summary>
        /// Gets or sets category color.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of name or color.
        /// </summary>
        public bool DataModified
        {
            get { return _dataModified; }
            set { SetField(ref _dataModified, value); }
        }

        /// <summary>
        /// Create at service.
        /// </summary>
        /// <returns>True in case of operation successfulness. </returns>
        public override bool Create()
        {
            Model = Service.CategoryController.Create(Name, Color, Order);
            return true;
        }

        /// <summary>
        /// Create at service.
        /// </summary>
        public override void Update()
        {
            if (DataModified)
            {
                Model.Name = Name;
                Model.Color = Color;
                Service.CategoryController.Update(Model);
                DataModified = false;
            }

            if (OrderModified)
            {
                Service.CategoryController.ChangeOrder(Model.Id, Order);
                Model.Order = Order;
                OrderModified = false;
            }
        }

        /// <summary>
        /// Delete action.
        /// </summary>
        /// <returns>True in case of operation successfulness. </returns>
        public override bool Delete()
        {
            Service.CategoryController.Delete(Model.Id);
            return Service.CategoryController.SelectById(Model.Id) != null;
        }

        /// <summary>
        /// Update from service.
        /// </summary>
        /// <param name="model">Back-end DTO category. </param>
        public override void Refresh(Category model)
        {
            Model = Service.CategoryController.SelectById(model.Id);
            Name = Model.Name;
            Color = Model.Color;
            Order = Model.Order;
            Service.CategoryController.Update(Model);
            base.Refresh(model);
        }

        /// <summary>
        /// Set false for all modified properties. 
        /// </summary>
        public override void ClearMofidied()
        {
            base.ClearMofidied();
            DataModified = false;
        }
    }
}
