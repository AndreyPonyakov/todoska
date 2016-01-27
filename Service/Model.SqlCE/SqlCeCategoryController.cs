using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interface = TodoSystem.Service.Model.Interface;

namespace Model.SqlCe
{
    /// <summary>
    /// Storage implementation for <see cref="Interface.ICategoryController"/>
    /// </summary>
    public class SqlCeCategoryController : Interface.ICategoryController
    {
        /// <summary>
        /// Gets full list if category.
        /// </summary>
        /// <returns>Category list. </returns>
        public IEnumerable<Interface.Category> SelectAll()
        {
            using (var context = new TodoEntities())
            {
                return context.Categories
                    .ToList()
                    .Select(c => c.ToDto());
            }
        }

        /// <summary>
        /// Fetches single category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        /// <returns>Category instance. </returns>
        public Interface.Category SelectById(int id)
        {
            using (var context = new TodoEntities())
            {
                return context.Categories
                    .Where(c => c.Id == id)
                    .ToList()
                    .Select(c => c.ToDto())
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Fetches category list with target name. 
        /// </summary>
        /// <param name="name">Target name. </param>
        /// <returns>Category list. </returns>
        public IEnumerable<Interface.Category> SelectByName(string name)
        {
            using (var context = new TodoEntities())
            {
                return context.Categories
                    .Where(c => c.Name == name).ToList()
                    .Select(c => c.ToDto());
            }
        }

        /// <summary>
        /// Creates new category with target attributes.
        /// </summary>
        /// <param name="name">Short name. </param>
        /// <param name="color">Preferable color. </param>
        /// <param name="order">New priority. </param>
        /// <returns>Category instance. </returns>
        public Interface.Category Create(string name, Color color, int order)
        {
            using (var context = new TodoEntities())
            {
                var entity = new Category
                                         {
                                             Order = order,
                                             Name = name,
                                             Color = color.ToArgb()
                                         };
                var result = context.Categories.Add(entity);
                context.SaveChanges();
                return
                    context.Categories.Where(c => c.Id == result.Id)
                        .ToList()
                        .Select(c => c.ToDto())
                        .FirstOrDefault();
            }
        }

        /// <summary>
        /// Update category with target attributes.
        /// </summary>
        /// <param name="category">Update category local instance.</param>
        public void Update(Interface.Category category)
        {
            using (var context = new TodoEntities())
            {
                context.Categories
                    .Where(c => c.Id == category.Id)
                    .ToList()
                    .ForEach(c => 
                            {
                                c.Name = category.Name;
                                c.Color = category.Color.ToArgb();
                            });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete category by primary key.
        /// </summary>
        /// <param name="id">Primary key of category. </param>
        public void Delete(int id)
        {
            using (var context = new TodoEntities())
            {
                context.Categories
                    .Where(c => c.Id == id)
                    .ToList()
                    .ForEach(c => context.Categories.Remove(c));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Change priority of order.
        /// </summary>
        /// <param name="id">Primary key. </param>
        /// <param name="order">Target priority in list. </param>
        public void ChangeOrder(int id, int order)
        {
            using (var context = new TodoEntities())
            {
                context.Categories.Where(c => c.Id == id)
                    .ToList()
                    .ForEach(c => c.Order = order);
                context.SaveChanges();
            }
        }
    }
}
