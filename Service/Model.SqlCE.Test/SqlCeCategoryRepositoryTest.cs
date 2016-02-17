using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

using TodoSystem.Service.Model.Interface;
using TodoSystem.Service.Model.Interface.Exceptions;
using TodoSystem.Service.Model.SqlCe;

using static TodoSystem.Service.Model.SqlCE.Test.SqlCeTestHelper;

namespace TodoSystem.Service.Model.SqlCE.Test
{
    [TestFixture]
    class SqlCeCategoryRepositoryTest
    {
        private ICategoryRepository CreateRepository()
        {
            return new SqlCeCategoryRepository(CreateFakeContextFactory(), new TodoMapperFactory().CreateMapper());
        }


        [Test]
        [Category("Repository")]
        public void Constructor_NotNullContext_NoException()
        {
            Assert.DoesNotThrow(
                () =>
                    {
                        ApplyConfiguration1();
                        CreateRepository();
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
            var repository = new SqlCeCategoryRepository(CreateFakeContextFactory(), null);
        }

        
        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_RightItemCount()
        {
            ApplyConfiguration1();
            var repository = CreateRepository();
            var all = repository.GetAll();

            Assert.That(all.Count(), Is.EqualTo(2));
        }

        
        [Test]
        [Category("Repository")]
        public void GetAll_Configuration1_RightNameOfFirstItem()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();

            var all = repository.GetAll();

            Assert.That(all.Single(c => c.Id == config.Category.Item1.Id).Name, Is.EqualTo("First"));
        }

        
        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndFirstItemId_RightNameOfFirstIdItem()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            int id = config.Category.Item1.Id;
            var category = repository.Get(id);

            Assert.That(category.Name, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndSecondItemId_RightOrderOfSecondItem()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            int id = config.Category.Item2.Id;
            var category = repository.Get(id);

            Assert.That(category.Order, Is.EqualTo(config.Category.Item2.Order));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration1AndNonExistItemId_ItemIsNull()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            int id = config.Category.Item2.Id + 1;
            var category = repository.Get(id);

            Assert.That(category, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndNewItem_IncreaseItemCount()
        {
            ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category{
                Id = default(int),
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            repository.Save(category);

            using(var context = CreateFakeContext())
            {
                Assert.That(context.Categories.Count(), Is.EqualTo(3));
            }
        }
        
        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(DataValidationException))]
        public void Save_Configuration1AndNewItemWithSoLongName_DataValidationException()
        {
            ApplyConfiguration1();
            var repository = CreateRepository();
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
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category
            {
                Id = default(int),
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            var result = repository.Save(category);

            Assert.That(result.Id, Is.EqualTo(config.Category.Item2.Id + 1));
        }

        [Test]
        [Category("Repository")]
        public void Save_Configuration1AndExistItem_RightContent()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category
            {
                Id = config.Category.Item2.Id,
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
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category
            {
                Id = config.Category.Item1.Id,
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
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category
            {
                Id = config.Category.Item1.Id,
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            repository.Delete(category);

            using (var context = CreateFakeContext())
            {
                Assert.That(context.Categories.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration1AndExistItemWithoutTodoes_RecordDeleted()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category
            {
                Id = config.Category.Item1.Id,
                Name = "Fourth",
                Color = Color.Aqua,
                Order = 4
            };
            repository.Delete(category);

            using (var context = CreateFakeContext())
            {
                Assert.That(context.Categories.ToList().Any(t => t.Id == config.Category.Item1.Id), Is.False);
            }
        }

        [Test]
        [ExpectedException(typeof(ForeignKeyConstraintException))]
        [Category("Repository")]
        public void Delete_Configuration2AndExistItemWithTodoes_ForeignKeyConstraintException()
        {
            var config = ApplyConfiguration2();
            var repository = CreateRepository();

            var category = new Interface.Category
            {
                Id = config.Category.Item1.Id,
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
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category
            {
                Id = config.Category.Item2.Id + 3,
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
            ApplyConfiguration1();
            var repository = CreateRepository();
            var categories = repository.Find("First");

            Assert.That(categories.Count(), Is.EqualTo(1));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndAbsentName_NoItem()
        {
            ApplyConfiguration1();
            var repository = CreateRepository();
            var categories = repository.Find("Third");

            Assert.That(categories.Any(), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1_RightIdOfLastItem()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = repository.FindLast();

            Assert.That(category.Id, Is.EqualTo(config.Category.Item2.Id));
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration1AndDeleteAllItem_IsNull()
        {
            ApplyConfiguration1();
            var repository = CreateRepository();

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
            var config = ApplyConfiguration1();

            var repository = CreateRepository();
            int id = config.Category.Item1.Id;
            var category = repository.Get(id);
            category.Order = 4;
            repository.Save(category);

            Assert.That(repository.FindLast().Id, Is.EqualTo(config.Category.Item1.Id));
        }
    }
}
