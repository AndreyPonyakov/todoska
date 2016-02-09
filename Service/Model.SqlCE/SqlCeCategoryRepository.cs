using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;

using Interface = TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.SqlCe
{
    /// <summary>
    /// Class for plain database category table management.
    /// </summary>
    public sealed class SqlCeCategoryRepository : Interface.ICategoryRepository, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeCategoryRepository"/> class.
        /// </summary>
        /// <param name="context">Database context. </param>
        /// <param name="mapper">Transform object from DTO to Entity and conversely.</param>
        public SqlCeCategoryRepository(TodoDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        /// <summary>
        /// Gets database context.
        /// </summary>
        private TodoDbContext Context { get; }

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
            return
                Context.Categories.ToList()
                    .Select(c => Mapper.Map<Interface.Category>(c));
        }

        /// <summary>
        /// Gets item by primary key.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <returns>Todo instance. </returns>
        public Interface.Category Get(int id)
        {
            return
                Context.Categories.Where(c => c.Id == id)
                    .ToList()
                    .Select(c => Mapper.Map<Interface.Category>(c))
                    .FirstOrDefault();
        }

        /// <summary>
        /// Saves todo into the repository.
        /// </summary>
        /// <param name="item">Saving item. </param>
        /// <returns>Saved item. </returns>
        public Interface.Category Save(Interface.Category item)
        {
            var entity = Mapper.Map<Category>(item);
            if (entity.Id == default(int))
            {
                Context.Categories.Add(entity);
            }
            else
            {
                var entry = Context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    var attachedEntity = Context.Categories.Local
                        .SingleOrDefault(e => e.Id == entity.Id);
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

            Context.SaveChanges();
            return Mapper.Map<Interface.Category>(entity);
        }

        /// <summary>
        /// Deletes item from the repository.
        /// </summary>
        /// <param name="item">Deleting item. </param>
        public void Delete(Interface.Category item)
        {
            Context.Categories
                .Where(c => c.Id == item.Id)
                .ToList()
                .ForEach(c => Context.Categories.Remove(c));
            Context.SaveChanges();
        }

        /// <summary>
        /// Filters category list by category.
        /// </summary>
        /// <param name="name">Filtering category. </param>
        /// <returns>Filtered item list. </returns>
        public IEnumerable<Interface.Category> Find(string name)
        {
            return Context.Categories
                .Where(c => c.Name == name)
                .ToList()
                .Select(c => Mapper.Map<Interface.Category>(c));
        }

        /// <summary>
        /// Finds item item by order attribute.
        /// </summary>
        /// <returns>Last item by priority. </returns>
        public Interface.Category FindLast()
        {
            var last = Context.Categories
                    .OrderByDescending(t => t.Order)
                    .FirstOrDefault();
            return last != null ? Mapper.Map<Interface.Category>(last) : null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => Context?.Dispose();
    }
}
