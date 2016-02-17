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
    /// Class for plain database category table management.
    /// </summary>
    public sealed class SqlCeCategoryRepository : Interface.ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeCategoryRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">Database context factory. </param>
        /// <param name="mapper">Transform object from DTO to Entity and conversely.</param>
        public SqlCeCategoryRepository(ITodoDbContextFactory contextFactory, IMapper mapper)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException(nameof(contextFactory));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(contextFactory));
            }

            ContextFactory = contextFactory;
            Mapper = mapper;
        }

        /// <summary>
        /// Gets database context factory.
        /// </summary>
        private ITodoDbContextFactory ContextFactory { get; }

        /// <summary>
        /// Gets mapper for transform from DTO to Entity and conversely.
        /// </summary>
        private IMapper Mapper { get; }

        /// <summary>
        /// Gets full item list.
        /// </summary>
        /// <returns>Full item list. </returns>
        public IEnumerable<Interface.Category> GetAll()
        {
            using (var context = ContextFactory.CreateContext())
            {
                return
                    context.Categories.ToList()
                        .Select(c => Mapper.Map<Interface.Category>(c));
            }
        }

        /// <summary>
        /// Gets item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Todo instance. </returns>
        public Interface.Category Get(int id)
        {
            using (var context = ContextFactory.CreateContext())
            {
                return
                    context.Categories.Where(c => c.Id == id)
                        .ToList()
                        .Select(c => Mapper.Map<Interface.Category>(c))
                        .FirstOrDefault();
            }
        }

        /// <summary>
        /// Saves todo into the repository.
        /// </summary>
        /// <param name="item">Saving item. </param>
        /// <returns>Saved item. </returns>
        public Interface.Category Save(Interface.Category item)
        {
            using (var context = ContextFactory.CreateContext())
            {
                var entity = Mapper.Map<Category>(item);
                if (entity.Id == default(int))
                {
                    context.Categories.Add(entity);
                }
                else
                {
                    var entry = context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        var attachedEntity =
                            context.Categories.Local.SingleOrDefault(
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

                return Mapper.Map<Interface.Category>(entity);
            }
        }

        /// <summary>
        /// Deletes item from the repository.
        /// </summary>
        /// <param name="item">Deleting item. </param>
        public void Delete(Interface.Category item)
        {
            using (var context = ContextFactory.CreateContext())
            {
                context.Categories.Where(c => c.Id == item.Id)
                    .ToList()
                    .ForEach(
                        c =>
                            {
                                if (c.Todoes.Any())
                                {
                                    throw new ForeignKeyConstraintException(
                                        $"{nameof(Todo)} - {nameof(Category)}");
                                }

                                context.Categories.Remove(c);
                            });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Filters category list by category.
        /// </summary>
        /// <param name="name">Filtering category. </param>
        /// <returns>Filtered item list. </returns>
        public IEnumerable<Interface.Category> Find(string name)
        {
            using (var context = ContextFactory.CreateContext())
            {
                return
                    context.Categories.Where(c => c.Name == name)
                        .ToList()
                        .Select(c => Mapper.Map<Interface.Category>(c));
            }
        }

        /// <summary>
        /// Finds item item by order attribute.
        /// </summary>
        /// <returns>Last item by priority. </returns>
        public Interface.Category FindLast()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var last =
                    context.Categories.OrderByDescending(t => t.Order)
                        .FirstOrDefault();
                return last != null
                           ? Mapper.Map<Interface.Category>(last)
                           : null;
            }
        }
    }
}
