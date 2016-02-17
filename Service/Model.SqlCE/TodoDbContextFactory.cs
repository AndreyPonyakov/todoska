namespace TodoSystem.Service.Model.SqlCe
{
    /// <summary>
    /// Factory to create a new instance of the <see cref="TodoDbContext"/> class.
    /// </summary>
    public sealed class TodoDbContextFactory : ITodoDbContextFactory
    {
        /// <summary>
        ///  Creates a new instance of the <see cref="TodoDbContext"/> class.
        /// </summary>
        /// <returns>A new context instance</returns>
        public TodoDbContext CreateContext() => new TodoDbContext();
    }
}
