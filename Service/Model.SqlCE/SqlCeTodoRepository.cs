using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;

using AutoMapper;

using TodoSystem.Service.Model.Interface.Exceptions;

using Interface = TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.SqlCe
{
    /// <summary>
    /// Interface for plain database todo table management.
    /// </summary>
    public sealed class SqlCeTodoRepository : Interface.ITodoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeTodoRepository"/> class.
        /// </summary>
        /// <param name="context">Database context. </param>
        /// <param name="mapper">Transform object from DTO to Entity and conversely.</param>
        /// <exception cref="ArgumentNullException">Throws if any argument is null. </exception>
        public SqlCeTodoRepository(TodoDbContext context, IMapper mapper)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            Context = context;
            Mapper = mapper;
        }

        /// <summary>
        /// Gets database context.
        /// </summary>
        public TodoDbContext Context { get; }

        /// <summary>
        /// Gets mapper for transform from DTO to Entity and conversely.
        /// </summary>
        private IMapper Mapper { get; }

        /// <summary>
        /// Gets full item list.
        /// </summary>
        /// <returns>Full item list. </returns>
        public IEnumerable<Interface.Todo> GetAll()
        {
            return
                Context.Todoes.ToList()
                    .Select(t => Mapper.Map<Interface.Todo>(t));
        }

        /// <summary>
        /// Gets item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Item instance. </returns>
        public Interface.Todo Get(int id)
        {
            return
                Context.Todoes.Where(t => t.Id == id)
                    .ToList()
                    .Select(t => Mapper.Map<Interface.Todo>(t))
                    .FirstOrDefault();
        }

        /// <summary>
        /// Saves todo into the repository.
        /// </summary>
        /// <param name="item">Saving item. </param>
        /// <returns>Saved item. </returns>
        public Interface.Todo Save(Interface.Todo item)
        {
            if (item.CategoryId != null
                && !Context.Categories.Any(c => c.Id == item.CategoryId))
            {
                throw new ForeignKeyConstraintException(
                    $"{nameof(Todo)} - {nameof(Category)}");
            }

            var entity = Mapper.Map<Todo>(item);
            if (entity.Id == default(int))
            {
                Context.Todoes.Add(entity);
            }
            else
            {
                var entry = Context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    var attachedEntity =
                        Context.Todoes.Local.SingleOrDefault(
                            e => e.Id == entity.Id);
                    if (attachedEntity != null)
                    {
                        var attachedEntry = Context.Entry(attachedEntity);
                        attachedEntry.CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                    }
                }
            }

            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                throw new DataValidationException();
            }

            return Mapper.Map<Interface.Todo>(entity);
        }

        /// <summary>
        /// Deletes item from the repository.
        /// </summary>
        /// <param name="item">Deleting item. </param>
        public void Delete(Interface.Todo item)
        {
            Context.Todoes.Where(t => t.Id == item.Id)
                .ToList()
                .ForEach(t => Context.Entry(t).State = EntityState.Deleted);
            Context.SaveChanges();
        }

        /// <summary>
        /// Filters todo list by title.
        /// </summary>
        /// <param name="title">Filtering title. </param>
        /// <returns>Filtered item list. </returns>
        public IEnumerable<Interface.Todo> Find(string title)
        {
            return
                Context.Todoes.Where(t => t.Title == title)
                    .ToList()
                    .Select(t => Mapper.Map<Interface.Todo>(t));
        }

        /// <summary>
        /// Filters todo list by category.
        /// </summary>
        /// <param name="category">Filtering category. </param>
        /// <returns>Filtered item list. </returns>
        public IEnumerable<Interface.Todo> Find(Interface.Category category)
        {
            return
                Context.Todoes.Where(t => t.CategoryId == category.Id)
                    .ToList()
                    .Select(t => Mapper.Map<Interface.Todo>(t));
        }

        /// <summary>
        /// Finds item item by order attribute.
        /// </summary>
        /// <returns>Last item by priority. </returns>
        public Interface.Todo FindLast()
        {
            var last = Context.Todoes
                    .OrderByDescending(t => t.Order)
                    .FirstOrDefault();
            return last != null ? Mapper.Map<Interface.Todo>(last) : null;
        }
    }
}
