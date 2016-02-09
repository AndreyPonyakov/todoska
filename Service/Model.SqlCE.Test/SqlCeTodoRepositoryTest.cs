using System;
using System.Data.Common;
using System.Linq;

using Effort;
using Effort.Provider;
using NUnit.Framework;

using TodoSystem.Model.SqlCe;
using Interface = TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.SqlCE.Test
{
    [TestFixture]
    public sealed class SqlCeTodoRepositoryTest
    {
        public Lazy<DbConnection> Connection { get; set; }

        [SetUp]
        public void Init()
        {
            Connection = new Lazy<DbConnection>(
                () =>
                    {
                        EffortProviderConfiguration.RegisterProvider();
                        return
                            EntityConnectionFactory.CreateTransient(
                                "name=TodoDbContext");
                    });
        }

        private static void ApplyConfiguration1(TodoDbContext context)
        {
            context.Categories.Add(
                new Category { Name = "First", Color = -1245, Order = 1 });
            context.Categories.Add(
                new Category { Name = "Second", Color = -12, Order = 2 });
            context.SaveChanges();
            context.Todoes.Add(
                new Todo
                    {
                        Title = "First",
                        Desc = null,
                        CategoryId = 1,
                        Order = 1,
                        Deadline = DateTime.Now,
                        Checked = false
                    });
            context.Todoes.Add(
                new Todo
                    {
                        Title = "Second",
                        Desc = "Description",
                        CategoryId = 1,
                        Order = 2,
                        Deadline = null,
                        Checked = false
                    });
            context.Todoes.Add(
                new Todo
                    {
                        Title = "Third",
                        Desc = null,
                        CategoryId = 2,
                        Order = 3,
                        Deadline = DateTime.Now,
                        Checked = true
                    });
            context.SaveChanges();
        }

        [Test]
        [Category("Repository")]
        public void Constructor_NotNullContext_NoException()
        {
            Assert.DoesNotThrow(
                () =>
                    {
                        var context = new TodoDbContext(Connection.Value);
                        var repository = new SqlCeTodoRepository(
                            context,
                            new TodoMapperFactory().CreateMapper());
                    });
        }

        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_ItemCount()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var all = repository.GetAll();

            Assert.That(all.Count(), Is.EqualTo(3));
        }

        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_ExistTitleWithIdIsOne()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var all = repository.GetAll();

            Assert.That(all.Single(t => t.Id == 1).Title, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndIdIsOne_TitleIsFirst()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = repository.Get(1);

            Assert.That(todo.Title, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndIdIsTwo_OrderIsTwo()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = repository.Get(2);

            Assert.That(todo.Order, Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndIdsFour_IsNull()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = repository.Get(4);

            Assert.That(todo, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndNewItem_IncreaseItemCount()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = new Interface.Todo
                           {
                               Id = default(int),
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                               CategoryId = 1
                           };
            repository.Save(todo);

            Assert.That(context.Todoes.Count(), Is.EqualTo(4));
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndNewItem_IdIsFour()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = new Interface.Todo
                           {
                               Id = default(int),
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                               CategoryId = 1
                           };
            var result = repository.Save(todo);

            Assert.That(result.Id, Is.EqualTo(4));
        }


        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndExistItem_RightContent()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = new Interface.Todo
                           {
                               Id = 3,
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                               CategoryId = 1
                           };
            repository.Save(todo);
            var result = repository.Get(3);

            Assert.That(result.Title, Is.EqualTo(todo.Title));
            Assert.That(result.Desc, Is.EqualTo(todo.Desc));
            Assert.That(result.Checked, Is.EqualTo(todo.Checked));
            Assert.That(result.Order, Is.EqualTo(todo.Order));
            Assert.That(result.CategoryId, Is.EqualTo(todo.CategoryId));
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration1AndNewItem_DecreaseItemCount()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = new Interface.Todo
                           {
                               Id = 3,
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                           };
            repository.Delete(todo);

            Assert.That(context.Categories.Count(), Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration1AndNewItem_NoExistsItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var todo = new Interface.Todo
                           {
                               Id = 3,
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                           };
            repository.Delete(todo);

            Assert.That(context.Categories.Any(t => t.Id == 3), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndTitleIsFirst_OneItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var categories = repository.Find("First");

            Assert.That(categories.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndTitleIsFourth_NoItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var categories = repository.Find("Fourth");

            Assert.That(categories.Any(), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndCategoreIsOne_TwoItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
                               {
                                   Id = 1,
                                   Name = "Fourth",
                                   Color = null,
                                   Order = 4
                               };
            var categories = repository.Find(category);

            Assert.That(categories.Count(), Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndCategoreIsThree_NoItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
                               {
                                   Id = 3,
                                   Name = "Fourth",
                                   Color = null,
                                   Order = 4
                               };
            var categories = repository.Find(category);

            Assert.That(categories.Any(), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1_IdIsThree()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());

            Assert.That(repository.FindLast().Id, Is.EqualTo(3));
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1AndClear_IsNull()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());
            while (repository.GetAll().Any())
            {
                repository.Delete(repository.GetAll().Last());
            }

            var category = repository.FindLast();

            Assert.That(category, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1AndChangeOrder_IdIsOne()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeTodoRepository(
                context,
                new TodoMapperFactory().CreateMapper());

            var todo = repository.Get(1);
            todo.Order = 4;
            repository.Save(todo);

            Assert.That(repository.FindLast().Id, Is.EqualTo(1));
        }
    }
}
