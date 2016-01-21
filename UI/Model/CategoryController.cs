using Todo.Service.Model.Interface;

namespace Todo.UI.Model
{
    /// <summary>
    /// Category controller class.
    /// </summary>
    public sealed class CategoryController
    {
        /// <summary>
        /// Remore category controller
        /// </summary>
        private readonly ICategoryController _controller;

        /// <summary>
        /// Create new category contro
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Category Create(Category category)
        {
            var remoteCategory = _controller.Create(category.Name, category.Color, category.Order);
            return new Category(remoteCategory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Category Update(Category category)
        {
            if (category.Id == null)
            {
                return null;
            }

            var id = category.Id.Value;
            _controller.Update(category.Remote);
            return new Category(_controller.SelectById(id));
        }

        /// <summary>
        /// Delete category.
        /// </summary>
        /// <param name="category"></param>
        public void Delete(Category category)
        {
            if (category.Id == null)
            {
                return;
            }

            var id = category.Id.Value;
            _controller.Delete(id);
        }

        public CategoryController(ICategoryController controller)
        {
            _controller = controller;
        }
    }
}
