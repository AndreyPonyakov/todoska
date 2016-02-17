using System;
using System.Data;
using System.Linq;

using Effort;
using Effort.Provider;

using TodoSystem.Service.Model.SqlCe;

using Category = TodoSystem.Service.Model.SqlCe.Category;
using Todo = TodoSystem.Service.Model.SqlCe.Todo;

namespace TodoSystem.Service.Model.SqlCE.Test
{
    internal static class SqlCeTestHelper
    {
        private static bool _register;
        public static TodoDbContext CreateFakeContext()
        {
            if (!_register)
            {
                EffortProviderConfiguration.RegisterProvider();
                _register = true;
            }
            var connection =
                EntityConnectionFactory.CreatePersistent("name=TodoDbContext");
            return new TodoDbContext(connection);
        }

        private class FakeTodoDbContextFactory : ITodoDbContextFactory
        {
            private Func<TodoDbContext> ContextBuilder { get; }

            public FakeTodoDbContextFactory(Func<TodoDbContext> contextBuilder)
            {
                ContextBuilder = contextBuilder;
            }
            public TodoDbContext CreateContext()
            {
                return ContextBuilder();
            }
        }

        public static ITodoDbContextFactory CreateFakeContextFactory() => new FakeTodoDbContextFactory(CreateFakeContext);

        public static dynamic ApplyConfiguration1()
        {

            using (var context = CreateFakeContext())
            {
                context.Todoes.ToList()
                    .ForEach(t => context.Entry(t).State = EntityState.Deleted);
                context.Categories.ToList()
                    .ForEach(c => context.Entry(c).State = EntityState.Deleted);
                context.SaveChanges();
            }

            using (var context = CreateFakeContext())
            {
                var category1 = new Category { Name = "First", Color = -1245, Order = 1 };
                var category2 = new Category { Name = "Second", Color = -12, Order = 2 };
                context.Categories.Add(category1);
                context.Categories.Add(category2);
                context.SaveChanges();

                return new
                {
                    Category = new { Item1 = category1, Item2 = category2 }
                };
            }
        }

        public static dynamic ApplyConfiguration2()
        {
            using (var context = CreateFakeContext())
            {
                context.Todoes.ToList()
                    .ForEach(t => context.Entry(t).State = EntityState.Deleted);
                context.Categories.ToList()
                    .ForEach(c => context.Entry(c).State = EntityState.Deleted);
                context.SaveChanges();
            }

            using (var context = CreateFakeContext())
            {
                var category1 = new Category { Name = "First", Color = -1245, Order = 1 };
                var category2 = new Category { Name = "Second", Color = -12, Order = 2 };

                context.Categories.Add(category1);
                context.Categories.Add(category2);
                context.SaveChanges();

                var todo1 = new Todo
                                {
                                    Title = "First",
                                    Desc = null,
                                    CategoryId = category1.Id,
                                    Order = 1,
                                    Deadline = DateTime.Now,
                                    Checked = false
                                };
                context.Todoes.Add(todo1);
                context.SaveChanges();

                return new
                {
                    Category = new { Item1 = category1, Item2 = category2 },
                    Todo = new { Item1 = todo1 }
                };
            }
        }

        public static dynamic ApplyConfiguration3()
        {
            using (var context = CreateFakeContext())
            {
                context.Todoes.ToList()
                    .ForEach(t => context.Entry(t).State = EntityState.Deleted);
                context.Categories.ToList()
                    .ForEach(c => context.Entry(c).State = EntityState.Deleted);
                context.SaveChanges();
            }

            using (var context = CreateFakeContext())
            {
                var category1 = new Category { Name = "First", Color = -1245, Order = 1 };
                var category2 = new Category { Name = "Second", Color = -12, Order = 2 };

                context.Categories.Add(category1);
                context.Categories.Add(category2);
                context.SaveChanges();

                var todo1 = new Todo
                                {
                                    Title = "First",
                                    Desc = null,
                                    CategoryId = category1.Id,
                                    Order = 1,
                                    Deadline = DateTime.Now,
                                    Checked = false
                                };
                var todo2 = new Todo
                                {
                                    Title = "Second",
                                    Desc = "Description",
                                    CategoryId = category1.Id,
                                    Order = 2,
                                    Deadline = null,
                                    Checked = false
                                };
                var todo3 = new Todo
                                {
                                    Title = "Third",
                                    Desc = null,
                                    CategoryId = category2.Id,
                                    Order = 3,
                                    Deadline = DateTime.Now,
                                    Checked = true
                                };
                context.Todoes.Add(todo1);
                context.Todoes.Add(todo2);
                context.Todoes.Add(todo3);
                context.SaveChanges();

                return new
                {
                    Category = new { Item1 = category1, Item2 = category2 },
                    Todo = new { Item1 = todo1, Item2 = todo2, Item3 = todo3 }
                };
            }
        }

    }
}
