using System;
using System.Collections.Generic;
using System.Linq;
using Interface = TodoSystem.Service.Model.Interface;

namespace Model.SqlCe
{
    using Interface;

    /// <summary>
    /// Storage implementation for <see cref="ITodoController"/>
    /// </summary>
    public class SqlCeTodoController : ITodoController
    {
        /// <summary>
        /// Select full list of todo from. 
        /// </summary>
        /// <returns>List of Todo. </returns>
        public IEnumerable<Interface.Todo> SelectAll()
        {
            using (var context = new TodoEntities())
            {
                return context.Todoes
                    .ToList()
                    .Select(t => t.ToDto());
            }
        }

        /// <summary>
        /// Select single todo instance by id.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Todo instance. </returns>
        public Interface.Todo SelectById(int id)
        {
            using (var context = new TodoEntities())
            {
                return context.Todoes
                    .Where(c => c.Id == id)
                    .ToList()
                    .Select(c => c.ToDto())
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Select list of todo with target title.
        /// </summary>
        /// <param name="title">Target title. </param>
        /// <returns>List of Todo. </returns>
        public IEnumerable<Interface.Todo> SelectByTitle(string title)
        {
            using (var context = new TodoEntities())
            {
                return context.Todoes
                    .Where(t => t.Title == title)
                    .ToList()
                    .Select(t => t.ToDto());
            }
        }

        /// <summary>
        /// Selects list of todo with target category.
        /// </summary>
        /// <param name="categoryId">Target category primary key</param>
        /// <returns>List of Todo. </returns>
        public IEnumerable<Interface.Todo> SelectByCategory(int categoryId)
        {
            using (var context = new TodoEntities())
            {
                return context.Todoes
                    .Where(t => t.CategoryId == categoryId)
                    .ToList()
                    .Select(t => t.ToDto());
            }
        }

        /// <summary>
        /// Creates a new Todo and appends in the controller.
        /// </summary>
        /// <param name="title">New title value. </param>
        /// <param name="desc">New description value. </param>
        /// <param name="deadline">New deadline value. </param>
        /// <param name="categoryId">New category primary key. </param>
        /// <param name="order">New priority value. </param>
        /// <returns>Created todo. </returns>
        public Interface.Todo Create(string title, string desc, DateTime deadline, int categoryId, int order)
        {
            using (var context = new TodoEntities())
            {
                var entity = new Todo()
                {
                    Title = title,
                    Desc = desc,
                    Deadline = deadline,
                    CategoryId = categoryId,
                    Order = order
                };
                var result = context.Todoes.Add(entity);
                context.SaveChanges();
                return
                    context.Todoes.Where(c => c.Id == result.Id)
                        .ToList()
                        .Select(c => c.ToDto())
                        .FirstOrDefault();
            }
        }

        /// <summary>
        /// Updates from other DTO todo instance. 
        /// </summary>
        /// <param name="todo">Updated todo. </param>
        public void Update(Interface.Todo todo)
        {
            using (var context = new TodoEntities())
            {
                context.Todoes
                    .Where(c => c.Id == todo.Id)
                    .ToList()
                    .ForEach(c =>
                    {
                        c.Title = todo.Title;
                        c.Desc = todo.Desc;
                    });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete todo by its id
        /// </summary>
        /// <param name="id">Primary key. </param>
        public void Delete(int id)
        {
            using (var context = new TodoEntities())
            {
                context.Todoes
                    .Where(t => t.Id == id)
                    .ToList()
                    .ForEach(t => context.Todoes.Remove(t));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Priority in the controller. </param>
        public void ChangeOrder(int id, int order)
        {
            using (var context = new TodoEntities())
            {
                context.Todoes
                    .Where(t => t.Id == id)
                    .ToList()
                    .ForEach(t => t.Order = order);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Make checked.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="isChecked">True if the todo is checked. </param>
        public void Check(int id, bool isChecked)
        {
            using (var context = new TodoEntities())
            {
                context.Todoes
                    .Where(t => t.Id == id)
                    .ToList()
                    .ForEach(t => t.Checked = isChecked);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Change category.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="categoryId">Primary key of new category. </param>
        public void SetCategory(int id, int categoryId)
        {
            using (var context = new TodoEntities())
            {
                context.Todoes.Where(t => t.Id == id)
                    .ToList()
                    .ForEach(t => t.CategoryId = categoryId);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Set deadline time.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="deadline">Deadline time. </param>
        public void SetDeadline(int id, DateTime deadline)
        {
            using (var context = new TodoEntities())
            {
                context.Todoes.Where(t => t.Id == id)
                    .ToList()
                    .ForEach(t => t.Deadline = deadline);
                context.SaveChanges();
            }
        }
    }
}
