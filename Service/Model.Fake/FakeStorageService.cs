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
        /// List of <see cref="Category"/> face objects.
        /// </summary>
        public IList<Category> CategoryList { get; }

        /// <summary>
        /// List of <see cref="Todo"/> fake objects.
        /// </summary>
        public IList<Todo> TodoList { get; }

        /// <summary>
        /// Backed field of <see cref="IStorageService"/> instance.
        /// Implement lazy singleton pattern. 
        /// </summary>
        private static readonly Lazy<IStorageService> InstaceLazy;

        /// <summary>
        /// Singleton instance of <see cref="IStorageService" />.
        /// </summary>
        public static IStorageService Instance => InstaceLazy.Value;

        /// <summary>
        /// Static constructor of <see cref="FakeStorageService"/>.
        /// </summary>
        static FakeStorageService()
        {
            InstaceLazy = new Lazy<IStorageService>(() => new FakeStorageService());
        }

        /// <summary>
        /// Create instance of <see cref="FakeStorageService"/>.
        /// </summary>
        private FakeStorageService()
        {
            CategoryList = new List<Category>();
            TodoList = new List<Todo>();
        }

    }
}
