using System;
using System.ServiceModel;

using AutoMapper;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using TodoSystem.Model.SqlCe;
using TodoSystem.Service.Model.Implementation;
using TodoSystem.Service.Model.Interface;
using TodoSystem.Service.Model.SqlCe;
using TodoSystem.Service.Tools.Aspects;
using TodoSystem.Service.Tools.UnityExtension;

namespace TodoSystem.Service.Host
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
            using (var container = new UnityContainer())
            {
                container
                    .AddNewExtension<DisposableStrategyExtension>()
                    .AddNewExtension<Interception>()
                    .RegisterType<ITimeProvider, TimeProvider>(new ContainerControlledLifetimeManager())
                    .RegisterType<ILogger, ConsoleLogger>(new ContainerControlledLifetimeManager())
                    .RegisterType<IMapper>(
                        new ContainerControlledLifetimeManager(),
                        new InjectionFactory(uc => new TodoMapperFactory().CreateMapper()))
                    .RegisterType<TodoDbContext>(
                        new DisposingTransientLifetimeManager(),
                        new InjectionFactory(uc => new TodoDbContext()))
                    .RegisterType<ICategoryRepository, SqlCeCategoryRepository>()
                    .RegisterType<ITodoRepository, SqlCeTodoRepository>()
                    .RegisterType<ICategoryController, CategoryController>()
                    .RegisterType<ITodoController, TodoController>();

                container.Configure<Interception>()
                    .SetInterceptorFor<ICategoryController>(new InterfaceInterceptor())
                    .SetInterceptorFor<ITodoController>(new InterfaceInterceptor());

                using (var categoryHost = new ServiceHost(typeof(CategoryController)))
                using (var todoHost = new ServiceHost(typeof(TodoController)))
                {
                    categoryHost.SetFactory<ICategoryController>(container);
                    todoHost.SetFactory<ITodoController>(container);

                    categoryHost.Open();
                    todoHost.Open();

                    Console.WriteLine($"TodoSystem service started at {DateTime.Now}...");
                    Console.WriteLine(string.Empty);
                    Console.ReadLine();
                }
            }
        }
    }
}
