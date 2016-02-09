using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;

using AutoMapper;

using Microsoft.Practices.Unity;

using TodoSystem.Model.Implementation;
using TodoSystem.Model.SqlCe;
using TodoSystem.Service.Model.Interface;

namespace Host
{
    /// <summary>
    /// Class with entry point of wcf host.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point of wcf host
        /// </summary>
        private static void Main()
        {
            var container =
                new UnityContainer()
                    .RegisterType<IMapper>(
                        new ContainerControlledLifetimeManager(),
                        new InjectionFactory(uc => new TodoMapperFactory().CreateMapper()))
                    .RegisterType<TodoDbContext>(new InjectionFactory(uc => new TodoDbContext()))
                    .RegisterType<ICategoryRepository, SqlCeCategoryRepository>()
                    .RegisterType<ITodoRepository, SqlCeTodoRepository>()
                    .RegisterType<ICategoryController, CategoryController>()
                    .RegisterType<ITodoController, TodoController>();

            using (var categoryHost = new ServiceHost(typeof(CategoryController)))
            using (var todoHost = new ServiceHost(typeof(TodoController)))
            {
                new InstanceProviderBehavior<ICategoryController>(() => container.Resolve<ICategoryController>()).AddToAllContracts(categoryHost);
                new InstanceProviderBehavior<ITodoController>(() => container.Resolve<ITodoController>()).AddToAllContracts(todoHost);

                categoryHost.Open();
                todoHost.Open();
                Console.Write($"TodoSystem service started at {DateTime.Now}...");
                Console.ReadLine();
            }
        }
    }
}
