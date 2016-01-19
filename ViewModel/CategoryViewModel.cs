using System.Drawing;
using Todo.UI.Model;
using Todo.UI.Tools.Model;

namespace Todo.UI.ViewModel
{
    /// <summary>
    /// ViewModel of category.
    /// </summary>
    public sealed class CategoryViewModel : BaseViewModel
    {
        public Category Model { get; }

        /// <summary>
        /// Name of category.
        /// </summary>
        public string Name
        {
            get { return Model.Name; }
            set { SetField(() => Model.Name, v => Model.Name = v, value);}
        }

        /// <summary>
        /// Color of category.
        /// </summary>
        public Color Color
        {
            get { return Model.Color; }
            set { SetField(() => Model.Color, v => Model.Color = v, value); }
        }

        /// <summary>
        /// Priority of category.
        /// </summary>
        public int Order
        {
            get { return Model.Order; }
            set { SetField(() => Model.Order, v => Model.Order = v, value); }
        }

        public CategoryViewModel(Category model)
        {
            Model = model;
        }
    }
}
