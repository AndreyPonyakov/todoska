using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;

using AutoMapper;

using TodoSystem.Service.Model.Interface.Exceptions;

namespace TodoSystem.Service.Model.SqlCe
{
    /// <summary>
    /// Interface for plain database todo table management.
    /// </summary>
    public sealed class SqlCeTodoRepository : Interface.ITodoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeTodoRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">Database context factory. </param>
        /// <param name="mapper">Transform object from DTO to Entity and conversely.</param>
        /// <exception cref="ArgumentNullException">Throws if any argument is null. </exception>
        public SqlCeTodoRepository(ITodoDbContextFactory contextFactory, IMapper mapper)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException(nameof(contextFactory));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            ContextFactory = contextFactory;
            Mapper = mapper;
        }

        /// <summary>
        /// Gets database context.
        /// </summary>
        public ITodoDbContextFactory ContextFactory { get; }

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
            using (var context = ContextFactory.CreateContext())
            {
                return
                    context.Todoes.ToList()
                        .Select(t => Mapper.Map<Interface.Todo>(t));
            }
        }

        /// <summary>
        /// Gets item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Item instance. </returns>
        public Interface.Todo Get(int id)
        {
            using (var context = ContextFactory.CreateContext())
            {
                return
                    context.Todoes.Where(t => t.Id == id)
                        .ToList()
                        .Select(t => Mapper.Map<Interface.Todo>(t))
                        .FirstOrDefault();
            }
        }

        /// <summary>
        /// Saves todo into the repository.
        /// </summary>
        /// <param name="item">Saving item. </param>
        /// <returns>Saved item. </returns>
        public Interface.Todo Save(Interface.Todo item)
        {
            using (var context = ContextFactory.CreateContext())
            {
                if (item.CategoryId != null
                    && !context.Categories.Any(c => c.Id == item.CategoryId))
                {
                    throw new ForeignKeyConstraintException(
                        $"{nameof(Todo)} - {nameof(Category)}");
                }

                var entity = Mapper.Map<Todo>(item);
                if (entity.Id == default(int))
                {
                    context.Todoes.Add(entity);
                }
                else
                {
                    var entry = context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        var attachedEntity =
                            context.Todoes.Local.SingleOrDefault(
                                e => e.Id == entity.Id);
                        if (attachedEntity != null)
                        {
                            var attachedEntry = context.Entry(attachedEntity);
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
                    context.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    throw new DataValidationException();
                }

                return Mapper.Map<Interface.Todo>(entity);
            }
        }

        /// <summary>
        /// Deletes item from the repository.
        /// </summary>
        /// <param name="item">Deleting item. </param>
        public void Delete(Interface.Todo item)
        {
            using (var context = ContextFactory.CreateContext())
            {
                context.Todoes.Where(t => t.Id == item.Id)
                    .ToList()
                    .ForEach(t => context.Entry(t).State = EntityState.Deleted);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Filters todo list by title.
        /// </summary>
        /// <param name="title">Filtering title. </param>
        /// <returns>Filtered item list. </returns>
        public IEnumerable<Interface.Todo> Find(string title)
        {
            using (var context = ContextFactory.CreateContext())
            {
                return
                    context.Todoes.Where(t => t.Title == title)
                        .ToList()
                        .Select(t => Mapper.Map<Interface.Todo>(t));
            }
        }

        /// <summary>
        /// Filters todo list by category.
        /// </summary>
        /// <param name="category">Filtering category. </param>
        /// <returns>Filtered item list. </returns>
        public IEnumerable<Interface.Todo> Find(Interface.Category category)
        {
            using (var context = ContextFactory.CreateContext())
            {
                return
                    context.Todoes.Where(t => t.CategoryId == category.Id)
                        .ToList()
                        .Select(t => Mapper.Map<Interface.Todo>(t));
            }
        }

        /// <summary>
        /// Finds item item by order attribute.
        /// </summary>
        /// <returns>Last item by priority. </returns>
        public Interface.Todo FindLast()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var last =
                    context.Todoes.OrderByDescending(t => t.Order)
                        .FirstOrDefault();
                return last != null ? Mapper.Map<Interface.Todo>(last) : null;
            }
        }
    }
}
