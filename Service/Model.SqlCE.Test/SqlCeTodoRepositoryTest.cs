using System;
using System.Data.Common;
using System.Linq;

using Effort;
using Effort.Provider;

using NUnit.Framework;

using TodoSystem.Service.Model.Interface;
using TodoSystem.Service.Model.Interface.Exceptions;
using TodoSystem.Service.Model.SqlCe;

using Category = TodoSystem.Service.Model.SqlCe.Category;
using Todo = TodoSystem.Service.Model.SqlCe.Todo;

using static TodoSystem.Service.Model.SqlCE.Test.SqlCeTestHelper;

namespace TodoSystem.Service.Model.SqlCE.Test
{
    [TestFixture]
    public sealed class SqlCeTodoRepositoryTest
    {
        private ITodoRepository CreateRepository()
        {
            return new SqlCeTodoRepository(CreateFakeContextFactory(), new TodoMapperFactory().CreateMapper());
        }

        
        [Test]
        [Category("Repository")]
        public void Constructor_NotNullContext_NoException()
        {
            Assert.DoesNotThrow(
                () =>
                    {
                        var config = ApplyConfiguration3();
                        var repository = CreateRepository();
                    });
        }


        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullContext_ArgumentNullException()
        {
            var repository = new SqlCeTodoRepository(null, new TodoMapperFactory().CreateMapper());
        }

        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullMapper_ArgumentNullException()
        {
            var repository = new SqlCeTodoRepository(CreateFakeContextFactory(), null);
        }

        [Test]
        [Category("Repository")]
        public void GetAll_Configuration3_RightItemCount()
        {
            ApplyConfiguration3();
            var repository = CreateRepository();
            var all = repository.GetAll();

            Assert.That(all.Count(), Is.EqualTo(3));
        }

        
        [Test]
        [Category("Repository")]
        public void GetAll_Configuration3_RightTitleOfFirstItem()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            var all = repository.GetAll();

            Assert.That(all.Single(t => t.Id == config.Todo.Item1.Id).Title, Is.EqualTo("First"));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration3AndFirstItemId_RightTitleOfFirstItem()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item1.Id;
            var todo = repository.Get(id);

            Assert.That(todo.Title, Is.EqualTo("First"));
        }

        
        [Test]
        [Category("Repository")]
        public void Get_Configuration3AndSecondItemId_RightName()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item2.Id;
            var todo = repository.Get(id);

            Assert.That(todo.Order, Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void Get_Configuration3AndNonExistItemId_ItemIsNull()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item3.Id;
            var todo = repository.Get(id + 1);

            Assert.That(todo, Is.Null);
        }
        
        [Test]
        [Category("Repository")]
        public void Save_Configuration3AndNewItem_IncreaseItemCount()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            var todo = new Interface.Todo
                           {
                               Id = default(int),
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                               CategoryId = config.Category.Item1.Id
            };
            repository.Save(todo);

            using (var context = CreateFakeContext())
            {
                Assert.That(context.Todoes.Count(), Is.EqualTo(4));
            }
        }

        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(DataValidationException))]
        public void Save_Configuration3AndNewItemWithSoTitle_DataValidationException()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            var todo = new Interface.Todo
            {
                Id = default(int),
                Title = new string('t', 300),
                Desc = "new descition",
                Checked = false,
                Order = 5,
                CategoryId = config.Category.Item1.Id
            };
            repository.Save(todo);
        }
        
        [Test]
        [Category("Repository")]
        public void Save_Configuration3AndNewItem_TargetId()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            var todo = new Interface.Todo
                           {
                               Id = default(int),
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 3,
                               CategoryId = config.Category.Item1.Id
            };
            var result = repository.Save(todo);

            Assert.That(result.Id, Is.EqualTo(config.Todo.Item3.Id + 1));
        }


        [Test]
        [Category("Repository")]
        public void Save_Configuration3AndExistItem_RightContent()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item3.Id;
            var todo = new Interface.Todo
                           {
                               Id = id,
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                               CategoryId = config.Category.Item1.Id
            };
            repository.Save(todo);
            
            var result = repository.Get(id);

            Assert.That(result.Title, Is.EqualTo(todo.Title));
            Assert.That(result.Desc, Is.EqualTo(todo.Desc));
            Assert.That(result.Checked, Is.EqualTo(todo.Checked));
            Assert.That(result.Order, Is.EqualTo(todo.Order));
            Assert.That(result.CategoryId, Is.EqualTo(todo.CategoryId));
        }
        
        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(DataValidationException))]
        public void Save_Configuration2AndExistItemWithSoLongName_DataValidationException()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item3.Id;
            var todo = new Interface.Todo
            {
                Id = id,
                Title = new string('t', 300),
                Desc = "new descition",
                Checked = false,
                Order = 5,
                CategoryId = config.Category.Item1.Id
            };
            repository.Save(todo);
        }

        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(DataValidationException))]
        public void Save_Configuration3AndExistItemWithSoLongDesc_DataValidationException()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item1.Id;

            var todo = new Interface.Todo
            {
                Id = id,
                Title = "some name",
                Desc = new string('t', 300),
                Checked = false,
                Order = 5,
                CategoryId = config.Category.Item1.Id
            };
            repository.Save(todo);
        }
        
        [Test]
        [Category("Repository")]
        public void Save_Configuration3AndExistItemAndNullCategory_RightContent()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item3.Id;

            var todo = new Interface.Todo
            {
                Id = id,
                Title = "Fourth",
                Desc = "new descition",
                Checked = false,
                Order = 5,
                CategoryId = null
            };
            repository.Save(todo);
            var result = repository.Get(id);

            Assert.That(result.Title, Is.EqualTo(todo.Title));
            Assert.That(result.Desc, Is.EqualTo(todo.Desc));
            Assert.That(result.Checked, Is.EqualTo(todo.Checked));
            Assert.That(result.Order, Is.EqualTo(todo.Order));
            Assert.That(result.CategoryId, Is.EqualTo(todo.CategoryId));
        }

        [Test]
        [Category("Repository")]
        [ExpectedException(typeof(ForeignKeyConstraintException))]
        public void Save_Configuration3AndExistItemAndWrongCategory_ForeignKeyConstraintException()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item1.Id;
            var todo = new Interface.Todo
            {
                Id = id,
                Title = "Fourth",
                Desc = "new descition",
                Checked = false,
                Order = 5,
                CategoryId = config.Category.Item2.Id + 2
            };
            repository.Save(todo);
            var result = repository.Get(id);

            Assert.That(result.Title, Is.EqualTo(todo.Title));
            Assert.That(result.Desc, Is.EqualTo(todo.Desc));
            Assert.That(result.Checked, Is.EqualTo(todo.Checked));
            Assert.That(result.Order, Is.EqualTo(todo.Order));
            Assert.That(result.CategoryId, Is.EqualTo(todo.CategoryId));
        }
        
        [Test]
        [Category("Repository")]
        public void Delete_Configuration3AndExistItem_DecreaseItemCount()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item3.Id;

            var todo = new Interface.Todo
                           {
                               Id = id,
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                           };
            repository.Delete(todo);

            using (var context = CreateFakeContext())
            {
                Assert.That(context.Categories.Count(), Is.EqualTo(2));
            }
        }

        [Test]
        [Category("Repository")]
        public void Delete_Configuration3AndExistItem_NoExistsItem()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item1.Id;
            var todo = new Interface.Todo
                           {
                               Id = id,
                               Title = "Fourth",
                               Desc = "new descition",
                               Checked = false,
                               Order = 5,
                           };
            repository.Delete(todo);

            using (var context = CreateFakeContext())
            {
                Assert.That(context.Todoes.Any(t => t.Id == id), Is.False);
            }
        }
        
        [Test]
        [Category("Repository")]
        public void Delete_Configuration3AndNonExistItem_NoException()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item3.Id;
            var todo = new Interface.Todo
            {
                Id = id,
                Title = "Fourth",
                Desc = "new descition",
                Checked = false,
                Order = 5,
            };

            Assert.DoesNotThrow(() => repository.Delete(todo));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration3AndTitleofFirstItem_ListWithFirstItem()
        {
            ApplyConfiguration3();
            var repository = CreateRepository();
            var categories = repository.Find("First");

            Assert.That(categories.Count(), Is.EqualTo(1));
        }
        
        [Test]
        [Category("Repository")]
        public void Find_Configuration3AndAbsentTitle_NoItem()
        {
            ApplyConfiguration3();
            var repository = CreateRepository();
            var categories = repository.Find("Fourth");

            Assert.That(categories.Any(), Is.False);
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration3AndCategoryOfFirst_ListOfTargetCategory()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            var category = new Interface.Category
                               {
                                   Id = config.Category.Item1.Id,
                                   Name = "Fourth",
                                   Color = null,
                                   Order = 4
                               };
            var categories = repository.Find(category);

            Assert.That(categories.Count(), Is.EqualTo(2));
        }

        [Test]
        [Category("Repository")]
        public void Find_Configuration1AndCategoryWothoutTodo_NoItem()
        {
            var config = ApplyConfiguration1();
            var repository = CreateRepository();
            var category = new Interface.Category
                               {
                                   Id = config.Category.Item1.Id,
                                   Name = "Fourth",
                                   Color = null,
                                   Order = 4
                               };
            var categories = repository.Find(category);

            Assert.That(categories.Any(), Is.False);
        }
        
        [Test]
        [Category("Repository")]
        public void FindLast_Configuration3_RightIdOfLastItem()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item3.Id;

            Assert.That(repository.FindLast().Id, Is.EqualTo(id));
        }

        [Test]
        [Category("Repository")]
        public void FindLast_Configuration3AndDeleteAllItem_IsNull()
        {
            ApplyConfiguration3();
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
        public void FindLast_Configuration3AndChangeOrder_RightIdOfLastItem()
        {
            var config = ApplyConfiguration3();
            var repository = CreateRepository();
            int id = config.Todo.Item1.Id;

            var todo = repository.Get(id);
            todo.Order = 4;
            repository.Save(todo);

            Assert.That(repository.FindLast().Id, Is.EqualTo(id));
        }
    }
}
