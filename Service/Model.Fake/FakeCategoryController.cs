using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Fake implement of ICategoryController.
    /// </summary>
    public class FakeCategoryController : ICategoryController
    {
        /// <summary>
        /// Service instance.
        /// </summary>
        private readonly IStorageService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCategoryController"/> class.
        /// </summary>
        public FakeCategoryController()
        {
            _service = FakeStorageService.Instance;
        }

        /// <summary>
        /// Get full list if category.
        /// </summary>
        /// <returns>Category list. </returns>
        public IEnumerable<Category> SelectAll()
        {
            return _service.CategoryList
                .OrderBy(c => c.Order);
        }

        /// <summary>
        /// Get single category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        /// <returns>Category instance. </returns>
        public Category SelectById(int id)
        {
            return _service.CategoryList
                .FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Get category list with target name. 
        /// </summary>
        /// <param name="name">Target name. </param>
        /// <returns>Category list. </returns>
        public IEnumerable<Category> SelectByName(string name)
        {
            return _service.CategoryList
                .Where(c => c.Name == name)
                .ToList();
        }

        /// <summary>
        /// Creates new category with target attributes.
        /// </summary>
        /// <param name="name">Short name. </param>
        /// <param name="color">Preferable color. </param>
        /// <param name="order">Priority in the controller. </param>
        /// <returns>Category instance. </returns>
        public Category Create(string name, Color color, int order)
        {
            var category = new Category
                               {
                                   Id = GeterateId(),
                                   Name = name,
                                   Color = color,
                                   Order = order
                               };
            _service.CategoryList.Add(category);
            return category;
        }

        /// <summary>
        /// Update category with target attributes.
        /// </summary>
        /// <param name="category">Update category local instance.</param>
        public void Update(Category category)
        {
            _service.CategoryList
                .Where(c => c.Id == category.Id)
                .ToList()
                .ForEach(c =>
                {
                    c.Name = category.Name;
                    c.Order = category.Order;
                    c.Color = category.Color;
                });
        }

        /// <summary>
        /// Change priority.
        /// </summary>
        /// <param name="id">Primary key of current category. </param>
        /// <param name="order">New priority. </param>
        public void ChangeOrder(int id, int order)
        {
            _service.CategoryList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.Order = order);
        }

        /// <summary>
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        public void Delete(int id)
        {
            if (_service.TodoList.Any(t => t.CategoryId == id))
            {
                return;
            }

            var category = _service.CategoryList
                .FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _service.CategoryList.Remove(category);
            }
        }

        /// <summary>
        /// Generate new primary key.
        /// </summary>
        /// <returns>Primary key for new category.</returns>
        public int GeterateId()
        {
            return _service.CategoryList.Any()
                ? _service.CategoryList.Max(c => c.Id) + 1
                : 1;
        }
    }
}
