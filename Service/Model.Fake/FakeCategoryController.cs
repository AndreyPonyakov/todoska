using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    /// <summary>
    /// Fake implement of ICategoryController.
    /// </summary>
    public sealed class FakeCategoryController : ICategoryController
    {
        /// <summary>
        /// Service instance.
        /// </summary>
        private readonly IFakeTodoService _service;

        /// <summary>
        /// Get full list if category.
        /// </summary>
        /// <returns>Category list. </returns>
        public IEnumerable<ICategory> SelectAll()
        {
            return _service.CategoryList;
        }

        /// <summary>
        /// Get single category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        /// <returns>Category instance. </returns>
        public ICategory SelectById(int id)
        {
            return _service.CategoryList
                .FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Get category list with target name. 
        /// </summary>
        /// <param name="name">Target name. </param>
        /// <returns>Category list. </returns>
        public IEnumerable<ICategory> SelectByName(string name)
        {
            return _service.CategoryList
                .Where(c => c.Name == name)
                .ToList();
        }

        /// <summary>
        /// Create new category with target attributes.
        /// </summary>
        /// <param name="name">Short name. </param>
        /// <param name="color">Preferable color. </param>
        /// <param name="order">Priority. </param>
        /// <returns>Category instance. </returns>
        public ICategory Create(string name, Color color, int order)
        {
            var category = new FakeCategory(GeterateId())
            {
                Name = name,
                Color = color
            };
            _service.CategoryList.Add(category);
            return category;
        }

        /// <summary>
        /// Generate new primary key.
        /// </summary>
        /// <returns></returns>
        public int GeterateId()
        {
            return _service.CategoryList.Any()
                ? _service.CategoryList.Max(c => c.Id) + 1
                : 1;
        }

        /// <summary>
        /// Update category with target attributes.
        /// </summary>
        /// <param name="category">Update category local instance.</param>
        public void Update(ICategory category)
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
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        public void Delete(int id)
        {
            if(_service.TodoList.Any(t => t.CategoryId == id))
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
        /// Cahnge priority.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority. </param>        
        public void ChangeOrder(int id, int order)
        {
            _service.CategoryList
                .Where(t => t.Id == id)
                .ToList()
                .ForEach(t => t.Order = order);
        }

        /// <summary>
        /// Create new instance of <see cref="FakeCategoryController"/>.
        /// </summary>
        /// <param name="service">Service instance. </param>
        public FakeCategoryController(IFakeTodoService service)
        {
            _service = service;
        }

    }
}
