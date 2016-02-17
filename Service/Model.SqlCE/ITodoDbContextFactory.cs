namespace TodoSystem.Service.Model.SqlCe
{
    /// <summary>
    /// Factory interface to create a new instance of the <see cref="TodoDbContext"/> class.
    /// </summary>
    public interface ITodoDbContextFactory
    {
        /// <summary>
        ///  Creates a new instance of the <see cref="TodoDbContext"/> class.
        /// </summary>
        /// <returns>A new context instance</returns>
        TodoDbContext CreateContext();
    }
}