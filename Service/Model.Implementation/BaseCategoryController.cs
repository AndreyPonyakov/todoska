using System;
using System.Collections.Generic;
using System.Drawing;

using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.Implementation
{
    /// <summary>
    /// Storage implementation for <see cref="ICategoryController"/>
    /// </summary>
    public abstract class BaseCategoryController : ICategoryController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCategoryController"/> class.
        /// </summary>
        /// <param name="repository">Category repository. </param>
        /// <exception cref="ArgumentNullException">Throws if the repository is null. </exception>
        protected BaseCategoryController(ICategoryRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            Repository = repository;
        }

        private ICategoryRepository Repository { get; }

        /// <summary>
        /// Gets full list if category.
        /// </summary>
        /// <returns>Category list. </returns>
        public IEnumerable<Category> SelectAll() => Repository.GetAll();

        /// <summary>
        /// Fetches single category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        /// <returns>Category instance. </returns>
        public Category SelectById(int id) => Repository.Get(id);

        /// <summary>
        /// Fetches category list with target name.
        /// </summary>
        /// <param name="name">Target name. </param>
        /// <returns>Category list. </returns>
        public IEnumerable<Category> SelectByName(string name) => Repository.Find(name);

        /// <summary>
        /// Creates new category with target attributes.
        /// </summary>
        /// <param name="name">Short name. </param>
        /// <param name="color">Preferable color. </param>
        /// <param name="order">New priority. </param>
        /// <returns>Category instance. </returns>
        public Category Create(string name, Color? color, int order)
        {
            var category = new Category
                               {
                                   Order = order,
                                   Name = name,
                                   Color = color
                               };
            return Repository.Save(category);
        }

        /// <summary>
        /// Update category with target attributes.
        /// </summary>
        /// <param name="category">Update category local instance.</param>
        public void Update(Category category)
        {
            if (category.Id != default(int))
            {
                Repository.Save(category);
            }
        }

        /// <summary>
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        public void Delete(int id) => Repository.Delete(Repository.Get(id));

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Target priority in list. </param>
        public void ChangeOrder(int id, int order)
        {
            var category = Repository.Get(id);
            category.Order = order;
            Repository.Save(category);
        }
    }
}
