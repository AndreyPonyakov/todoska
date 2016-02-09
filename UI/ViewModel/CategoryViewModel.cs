using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using TodoSystem.UI.Model;
using TodoSystem.UI.Model.CategoryControllerServiceReference;
using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Base;

namespace TodoSystem.UI.ViewModel
{
    /// <summary>
    /// ViewModel of category.
    /// </summary>
    public sealed class CategoryViewModel : BaseOrderableItemViewModel<ITodoService, Category, CategoryViewModel>
    {
        private string _name = string.Empty;
        private Color? _color;

        private bool _nameModified;
        private bool _colorModified;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for command instance. </param>
        /// <param name="service">Todo service. </param>
        public CategoryViewModel(ICommandFactory commandFactory, ITodoService service)
            : base(service, commandFactory)
        {
            this
                .SetPropertyChanged(nameof(Name), () => NameModified = true)
                .SetPropertyChanged(nameof(Color), () => ColorModified = true)
                .SetPropertyChanged(
                    new[] { nameof(OrderModified), nameof(NameModified), nameof(ColorModified) },
                    () => { Modified = NameModified || ColorModified || OrderModified; })
                .SetPropertyChangedWithExecute(
                    nameof(Name), () => Validate(
                        Name.Length > 3 && Name.Length < 100,
                        nameof(Name),
                        "Name must be more 3 characters and less 100 characters."))
                .SetPropertyChanged(Attributes, ItemChanged);
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
        public Color? Color
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of name
        /// </summary>
        public bool NameModified
        {
            get { return _nameModified; }
            set { SetField(ref _nameModified, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of name
        /// </summary>
        public bool ColorModified
        {
            get { return _colorModified; }
            set { SetField(ref _colorModified, value); }
        }

        /// <summary>
        /// Gets list of attribute properties.
        /// </summary>
        public override IEnumerable<string> Attributes
            => base.Attributes
                .Union(new[]
                       {
                           nameof(Name), nameof(Color)
                       });

        /// <summary>
        /// Create at service.
        /// </summary>
        /// <returns>True in case of operation successfulness. </returns>
        public override bool Create()
        {
            Model = Service.CategoryController.Create(Name);
            NameModified = false;
            return true;
        }

        /// <summary>
        /// Create at service.
        /// </summary>
        public override void Update()
        {
            if (NameModified)
            {
                Model.Name = Name;
                Service.CategoryController.ChangeText(Model.Id, Name);
                NameModified = false;
            }

            if (ColorModified)
            {
                Model.Color = Color;
                Service.CategoryController.ChangeColor(Model.Id, Color);
                ColorModified = false;
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
            return Service.CategoryController.SelectById(Model.Id) == null;
        }

        /// <summary>
        /// Update from service.
        /// </summary>
        /// <param name="model">Back-end DTO category. </param>
        public override void Refresh(Category model)
        {
            if (Refreshing)
            {
                return;
            }

            Refreshing = true;
            try
            {
                Model = Service.CategoryController.SelectById(model.Id);
                Name = Model.Name;
                Color = Model.Color;
                Order = Model.Order;
                base.Refresh(model);
            }
            finally
            {
                Refreshing = false;
            }
        }

        /// <summary>
        /// Set false for all modified properties.
        /// </summary>
        public override void ClearMofidied()
        {
            base.ClearMofidied();
            NameModified = false;
            ColorModified = false;
        }
    }
}
