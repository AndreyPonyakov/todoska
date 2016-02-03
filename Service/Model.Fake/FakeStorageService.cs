using System;
using System.Collections.Generic;
using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Storage service for Service model.
    /// </summary>
    public class FakeStorageService : IStorageService
    {
        /// <summary>
        /// Initializes static members of the <see cref="FakeStorageService"/> class.
        /// </summary>
        static FakeStorageService()
        {
            InstaceLazy = new Lazy<IStorageService>(() => new FakeStorageService());
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FakeStorageService"/> class from being created.
        /// </summary>
        private FakeStorageService()
        {
            CategoryList = new List<Category>();
            TodoList = new List<Todo>();
        }

        /// <summary>
        /// Gets singleton instance of <see cref="IStorageService" />.
        /// </summary>
        public static IStorageService Instance => InstaceLazy.Value;

        /// <summary>
        /// Gets list of <see cref="Category"/> face objects.
        /// </summary>
        public IList<Category> CategoryList { get; }

        /// <summary>
        /// Gets list of <see cref="Todo"/> fake objects.
        /// </summary>
        public IList<Todo> TodoList { get; }

        /// <summary>
        /// Gets backed field of <see cref="IStorageService"/> instance.
        /// Implement lazy singleton pattern.
        /// </summary>
        private static Lazy<IStorageService> InstaceLazy { get; }
    }
}
