using AutoMapper;

using TodoSystem.Model.Implementation;
using TodoSystem.Model.SqlCe;

namespace Host
{
    /// <summary>
    /// Service for Category Controller.
    /// </summary>
    public class CategoryController : BaseCategoryController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        public CategoryController()
            : base(
                new SqlCeCategoryRepository(
                    new TodoDbContext(),
                    new MapperConfiguration(cfg => cfg.MapCategory()).CreateMapper()))
        {
        }
    }
}
