﻿using System.Collections.Generic;
using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Storage interface for fake model.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// List of <see cref="Category"/> face objects.
        /// </summary>
        IList<Category> CategoryList { get; }

        /// <summary>
        /// List of <see cref="Todo"/> fake objects.
        /// </summary>
        IList<Todo> TodoList { get; }


    }
}