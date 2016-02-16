using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;

using TodoSystem.Service.Model.Interface;
using TodoSystem.Service.Model.Interface.Exceptions;
using TodoSystem.Service.Model.Interface.Faults;

namespace TodoSystem.Model.Implementation
{
    /// <summary>
    /// Storage implementation for <see cref="ICategoryController"/>
    /// </summary>
    public sealed class CategoryController : ICategoryController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="repository">Category repository. </param>
        /// <exception cref="ArgumentNullException">Throws if the repository is null. </exception>
        public CategoryController(ICategoryRepository repository)
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
        /// <returns>Category instance. </returns>
        public Category Create(string name)
        {
            if (name == null)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }

            try
            {
                var category = 
                    new Category
                        {
                            Name = name,
                            Color = null,
                            Order = (Repository.FindLast()?.Order ?? default(int) - 1) + 1
                        };
                return Repository.Save(category);
            }
            catch (DataValidationException)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }
        }

        /// <summary>
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        public void Delete(int id)
        {
            var category = Repository.Get(id);
            if (category == null)
            {
                return;
            }

            try
            {
                Repository.Delete(category);
            }
            catch (ForeignKeyConstraintException ex)
            {
                throw new FaultException<ForeignKeyConstraintFault>(
                    new ForeignKeyConstraintFault { ForeignKey = ex.ForeignKey });
            }
        }

        /// <summary>
        /// Change name of item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="name">Target name. </param>
        public void ChangeText(int id, string name)
        {
            if (name == null)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }

            var category = Repository.Get(id);
            if (category == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                new ItemNotFoundFault());
            }

            category.Name = name;

            try
            {
                Repository.Save(category);
            }
            catch (DataValidationException)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }
        }

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Target priority in list. </param>
        public void ChangeOrder(int id, int order)
        {
            var category = Repository.Get(id);
            if (category == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                    new ItemNotFoundFault());
            }

            category.Order = order;
            Repository.Save(category);
        }

        /// <summary>
        /// Change color of item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="color">Target color. </param>
        public void ChangeColor(int id, Color? color)
        {

            var category = Repository.Get(id);
            if (category == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                    new ItemNotFoundFault());
            }

            category.Color = color;
            Repository.Save(category);
        }
    }
}
