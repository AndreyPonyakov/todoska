using System;
using System.Data.Common;
using System.Drawing;
using System.Linq;

using Effort;
using Effort.Provider;

using NUnit.Framework;
using TodoSystem.Model.SqlCe;

using Category = TodoSystem.Model.SqlCe.Category;
using Interface = TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.SqlCE.Test
{
    [TestFixture]
    class SqlCeCategoryRepositoryTest
    {
        public Lazy<DbConnection> Connection { get; set; }

        [SetUp]
        public void Init()
        {
            Connection = new Lazy<DbConnection>(
                () =>
                    {
                        EffortProviderConfiguration.RegisterProvider();
                        return EntityConnectionFactory.CreateTransient("name=TodoDbContext");
                    });
        }

        private static void ApplyConfiguration1(TodoDbContext context)
        {
            context.Categories.Add(
                new Category() { Name = "First", Color = -1245, Order = 1 });
            context.Categories.Add(
                new Category() { Name = "Second", Color = -12, Order = 2 });
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
                        var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
                    });
        }

        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_ItemCount()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var all = repository.GetAll();

            Assert.That(all.Count(), Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_ExistNameWithIdIsOne()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var all = repository.GetAll();

            Assert.That(all.Single(c => c.Id == 1).Name, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndIdIsOne_NameIsFirst()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = repository.Get(1);

            Assert.That(category.Name, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndIdIsTwo_OrderIsTwo()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = repository.Get(2);

            Assert.That(category.Order, Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndIdIsThree_IsNull()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = repository.Get(3);

            Assert.That(category, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndNewItem_IncreaseItemCount()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category{
                Id = default(int),
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            repository.Save(category);

            Assert.That(context.Categories.Count(), Is.EqualTo(3));
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndNewItem_IdIsThree()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = default(int),
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            var result = repository.Save(category);

            Assert.That(result.Id, Is.EqualTo(3));
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndExistItem_RightContent()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = 2,
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            var result = repository.Save(category);

            Assert.That(result.Name, Is.EqualTo(category.Name));
            Assert.That(result.Order, Is.EqualTo(category.Order));
            Assert.That(result.Color.Value.R, Is.EqualTo(category.Color.Value.R));
            Assert.That(result.Color.Value.G, Is.EqualTo(category.Color.Value.G));
            Assert.That(result.Color.Value.B, Is.EqualTo(category.Color.Value.B));
            Assert.That(result.Color.Value.A, Is.EqualTo(category.Color.Value.A));
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration1AndNewItem_DecreaseItemCount()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = 1,
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            repository.Delete(category);

            Assert.That(context.Categories.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration1AndNewItem_NoExistsItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = 1,
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            repository.Delete(category);

            Assert.That(context.Categories.Any(t => t.Id == 1), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndNameIsFirst_OneItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var categories = repository.Find("First");

            Assert.That(categories.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndNameIsThird_NoItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var categories = repository.Find("Third");

            Assert.That(categories.Any(), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1_IdIsTwo()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = repository.FindLast();

            Assert.That(category.Id, Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1AndClear_IsNull()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
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
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());

            var category = repository.Get(1);
            category.Order = 4;
            repository.Save(category);

            Assert.That(repository.FindLast().Id, Is.EqualTo(1));
        }

    }
}
