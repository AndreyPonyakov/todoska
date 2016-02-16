﻿using System;
using System.Collections.Generic;
using System.ServiceModel;

using TodoSystem.Service.Model.Interface;
using TodoSystem.Service.Model.Interface.Exceptions;
using TodoSystem.Service.Model.Interface.Faults;
using TodoSystem.Service.Tools.Aspects;

namespace TodoSystem.Service.Model.Implementation
{
    /// <summary>
    /// Storage implementation for <see cref="ITodoController"/>
    /// </summary>
    public sealed class TodoController : ITodoController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoController"/> class.
        /// </summary>
        /// <param name="repository">Category repository. </param>
        public TodoController(ITodoRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            Repository = repository;
        }

        private ITodoRepository Repository { get; }

        /// <summary>
        /// Select full list of todo from.
        /// </summary>
        /// <returns>List of Todo. </returns>
        [Loggable]
        public IEnumerable<Todo> SelectAll() => Repository.GetAll();

        /// <summary>
        /// Select single todo instance by id.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Todo instance. </returns>
        [Loggable]
        public Todo SelectById(int id) => Repository.Get(id);

        /// <summary>
        /// Select list of todo with target title.
        /// </summary>
        /// <param name="title">Target title. </param>
        /// <returns>List of Todo. </returns>
        [Loggable]
        public IEnumerable<Todo> SelectByTitle(string title) => Repository.Find(title);

        /// <summary>
        /// Selects list of todo with target category.
        /// </summary>
        /// <param name="categoryId">Target category primary key</param>
        /// <returns>List of Todo. </returns>
        [Loggable]
        public IEnumerable<Todo> SelectByCategory(int categoryId)
        {
            var category = new Category { Id = categoryId };
            return Repository.Find(category);
        }

        /// <summary>
        /// Creates a new Todo and appends in the controller.
        /// </summary>
        /// <param name="title">New title value. </param>
        /// <returns>Created todo. </returns>
        [Loggable]
        public Todo Create(string title)
        {
            if (title == null)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }

            var todo = new Todo
            {
                Title = title,
                Desc = null,
                Deadline = null,
                CategoryId = null,
                Checked = false,
                Order = (Repository.FindLast()?.Order ?? default(int) - 1) + 1
            };

            try
            {
                return Repository.Save(todo);
            }
            catch (DataValidationException)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }
        }

        /// <summary>
        /// Delete todo by its id
        /// </summary>
        /// <param name="id">Primary key. </param>
        [Loggable]
        public void Delete(int id)
        {
            var todo = Repository.Get(id);
            if (todo != null)
            {
                Repository.Delete(Repository.Get(id));
            }
        }

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority in the controller. </param>
        [Loggable]
        public void ChangeOrder(int id, int order)
        {
            var todo = Repository.Get(id);
            if (todo == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                    new ItemNotFoundFault());
            }

            todo.Order = order;
            Repository.Save(todo);
        }

        /// <summary>
        /// Make checked.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="isChecked">True if the todo is checked. </param>
        [Loggable]
        public void Check(int id, bool isChecked)
        {
            var todo = Repository.Get(id);
            if (todo == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                    new ItemNotFoundFault());
            }

            todo.Checked = isChecked;

            Repository.Save(todo);
        }

        /// <summary>
        /// Change category.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="categoryId">Primary key of new category. </param>
        [Loggable]
        public void SetCategory(int id, int? categoryId)
        {
            var todo = Repository.Get(id);
            if (todo == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                    new ItemNotFoundFault());
            }

            todo.CategoryId = categoryId;

            try
            {
                Repository.Save(todo);
            }
            catch (ForeignKeyConstraintException ex)
            {
                throw new FaultException<ForeignKeyConstraintFault>(
                    new ForeignKeyConstraintFault { ForeignKey = ex.ForeignKey });
            }
        }

        /// <summary>
        /// Set deadline time.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="deadline">Deadline time. </param>
        [Loggable]
        public void SetDeadline(int id, DateTime? deadline)
        {
            var todo = Repository.Get(id);
            if (todo == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                    new ItemNotFoundFault());
            }

            todo.Deadline = deadline;
            Repository.Save(todo);
        }

        /// <summary>
        /// Set new title and description by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="title">New title. </param>
        /// <param name="desc">New description. </param>
        [Loggable]
        public void ChangeText(int id, string title, string desc)
        {
            if (title == null)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }

            var todo = Repository.Get(id);
            if (todo == null)
            {
                throw new FaultException<ItemNotFoundFault>(
                    new ItemNotFoundFault());
            }

            todo.Title = title;
            todo.Desc = desc;

            try
            {
                Repository.Save(todo);
            }
            catch (DataValidationException)
            {
                throw new FaultException<DataValidationFault>(
                    new DataValidationFault());
            }
        }
    }
}
