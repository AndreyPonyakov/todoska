using System;
using System.Data.Common;
using System.Drawing;
using System.Linq;

using Effort;
using Effort.Provider;

using NUnit.Framework;
using TodoSystem.Model.SqlCe;
using TodoSystem.Service.Model.Interface.Exceptions;

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

        private static void ApplyConfiguration2(TodoDbContext context)
        {
            context.Categories.Add(
                new Category() { Name = "First", Color = -1245, Order = 1 });
            context.Categories.Add(
                new Category() { Name = "Second", Color = -12, Order = 2 });
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullContext_ArgumentNullException()
        {
            var repository = new SqlCeCategoryRepository(null, new TodoMapperFactory().CreateMapper());
        }

        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullMapper_ArgumentNullException()
        {
            var context = new TodoDbContext(Connection.Value);
            var repository = new SqlCeCategoryRepository(context, null);
        }

        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_RightItemCount()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var all = repository.GetAll();

            Assert.That(all.Count(), Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_RightNameOfFirstItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var all = repository.GetAll();

            Assert.That(all.Single(c => c.Id == 1).Name, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndFirstItemId_RightNameOfFirstIdItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = repository.Get(1);

            Assert.That(category.Name, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndSecondItemId_RightOrderOfSecondItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = repository.Get(2);

            Assert.That(category.Order, Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndNonExistItemId_ItemIsNull()
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
        [ExpectedException(typeof(DataValidationException))]
        public void Save_Configuration1AndNewItemWithSoLongName_DataValidationException()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = default(int),
                Name = new string('t',300),
                Color = Color.Aqua,
                Order = 4
            };
            repository.Save(category);
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndNewItem_TargetId()
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
        [ExpectedException(typeof(DataValidationException))]
        public void Save_Configuration1AndExistItemWithSoLongName_DataValidationException()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = 1,
                Name = new string('t', 300),
                Color = Color.Aqua,
                Order = 4
            };
            repository.Save(category);
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration1AndExistItemWithoutTodoes_DecreaseItemCount()
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
        public void Delete_Configuration1AndExistItemWithoutTodoes_RecordDeleted()
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
        [ExpectedException(typeof(ForeignKeyConstraintException))]
        [Category("Repository")]
        public void Delete_Configuration2AndExistItemWithTodoes_ForeignKeyConstraintException()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration2(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = 1,
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };

            repository.Delete(category);
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration1AndNonExistItem_NoException()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration2(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = new Interface.Category
            {
                Id = 5,
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };

            Assert.DoesNotThrow(() => repository.Delete(category));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndNameOfFirstItem_ListWithFirstItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var categories = repository.Find("First");

            Assert.That(categories.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndAbsentName_NoItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var categories = repository.Find("Third");

            Assert.That(categories.Any(), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1_RightIdOfLastItem()
        {
            var context = new TodoDbContext(Connection.Value);
            ApplyConfiguration1(context);
            var repository = new SqlCeCategoryRepository(context, new TodoMapperFactory().CreateMapper());
            var category = repository.FindLast();

            Assert.That(category.Id, Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1AndDeleteAllItem_IsNull()
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
        public void FindLast_Configuration1AndChangeOrder_RightIdOfListItem()
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
