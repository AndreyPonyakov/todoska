using System;
using System.Collections.Generic;
using System.Drawing;

using NUnit.Framework;
using Rhino.Mocks;

using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.Implementation.Test
{
    [TestFixture]
    public class CategoryControllerTest
    {
        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullArguments_ArgumentNullException()
        {
            var controller = new CategoryController(null);        
        }

        [Test]
        [Category("Controller")]
        public void Constructor_NotNullArguments_NoException()
        {
            Assert.DoesNotThrow(
                () =>
                    {
                        var repository =
                            MockRepository.GenerateStub<ICategoryRepository>();
                        var controller = new CategoryController(repository);
                    });
        }

        [Test]
        [Category("Controller")]
        public void SelectAll_List_SameList()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var list = MockRepository.GenerateStub<IEnumerable<Category>>();
            repository.Stub(r => r.GetAll()).Return(list);
            var controller = new CategoryController(repository);

            Assert.That(controller.SelectAll(), Is.SameAs(list));
        }

        [Test]
        [Category("Controller")]
        public void SelectAll_List_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var list = MockRepository.GenerateStub<IEnumerable<Category>>();
            repository.Expect(r => r.GetAll()).Return(list);
            var controller = new CategoryController(repository);

            controller.SelectAll();

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SelectById_Item_SameItem()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var id = 1;
            var category = new Category();
            repository.Stub(r => r.Get(id)).Return(category);
            var controller = new CategoryController(repository);

            Assert.That(controller.SelectById(id), Is.SameAs(category));
        }

        [Test]
        [Category("Controller")]
        public void SelectById_Item_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var category = new Category();
            repository.Expect(r => r.Get(id)).Return(category);
            var controller = new CategoryController(repository);

            controller.SelectById(id);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SelectByName_Name_RightList()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var name = "some name";
            var list = MockRepository.GenerateStub<IEnumerable<Category>>();
            repository.Stub(r => r.Find(name)).Return(list);
            var controller = new CategoryController(repository);

            Assert.That(controller.SelectByName(name), Is.SameAs(list));
        }

        [Test]
        [Category("Controller")]
        public void SelectByName_Name_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var name = "some name";
            var list = MockRepository.GenerateStub<IEnumerable<Category>>();
            repository.Expect(r => r.Find(name)).Return(list);
            var controller = new CategoryController(repository);

            controller.SelectByName(name);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Create_ArgumentList_Category()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var last = new Category
            {
                Id = 10,
                Name = "last name",
                Color = null,
                Order = 14
            };
            var name = "some name";
            var category = new Category();
            repository.Stub(r => r.FindLast()).Return(last);
            repository
                .Stub(r => r.Save(
                    Arg<Category>.Matches(
                        c => c.Name == name && c.Color == null 
                        && c.Order == last.Order + 1 && c.Id == default(int))))
                .Return(category);
            var controller = new CategoryController(repository);

            Assert.That(
                controller.Create(name),
                Is.SameAs(category));
        }

        [Test]
        [Category("Controller")]
        public void Create_FirstItem_CategoryWithZeroOrder()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var name = "some name";
            var category = new Category();
            repository.Stub(r => r.FindLast()).Return(null);
            repository
                .Stub(r => r.Save(
                    Arg<Category>.Matches(
                        c => c.Name == name && c.Color == null
                        && c.Order == default(int) && c.Id == default(int))))
                .Return(category);
            var controller = new CategoryController(repository);

            Assert.That(
                controller.Create(name),
                Is.SameAs(category));
        }

        [Test]
        [Category("Controller")]
        public void Create_FirstItem_Category()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var name = "some name";
            var category = new Category();
            repository.Stub(r => r.FindLast()).Return(null);
            repository
                .Stub(r => r.Save(
                    Arg<Category>.Matches(
                        c => c.Name == name && c.Color == null
                        && c.Order == default(int) && c.Id == default(int))))
                .Return(category);
            var controller = new CategoryController(repository);

            Assert.That(
                controller.Create(name),
                Is.SameAs(category));
        }

        [Test]
        [Category("Controller")]
        public void Create_ArgumentList_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var last = new Category
            {
                Id = 10,
                Name = "last name",
                Color = null,
                Order = 14
            };

            var name = "some name";
            var category = new Category();
            repository.Expect(r => r.FindLast()).Return(last);
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(
                        c => c.Name == name && c.Color == null
                        && c.Order == last.Order + 1 && c.Id == default(int))))
                .Return(category);
            var controller = new CategoryController(repository);

            controller.Create(name);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Delete_RightId_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var name = "some name";
            var color = Color.Chartreuse;
            var order = 100;
            var category = new Category
                               {
                                   Id = id,
                                   Name = name,
                                   Color = color,
                                   Order = order
                               };

            repository.Expect(r => r.Get(id)).Return(category);
            repository.Expect(r => r.Delete(category));
            var controller = new CategoryController(repository);

            controller.Delete(id);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_RightIdAndNewName_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var name = "some name";
            var color = Color.Chartreuse;
            var order = 100;
            var newName = "new name";
            var category = new Category
            {
                Id = id,
                Name = name,
                Color = color,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(category);
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(c => c.Id == id && c.Name == newName)))
                .Return(category);
            var controller = new CategoryController(repository);

            controller.ChangeText(id, newName);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_RightIdAndNewOrder_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var name = "some name";
            var color = Color.Chartreuse;
            var order = 100;
            var newOrder = 200;
            var category = new Category
                               {
                                   Id = id,
                                   Name = name,
                                   Color = color,
                                   Order = order
                               };
            repository.Expect(r => r.Get(id)).Return(category);
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(c => c.Id == id && c.Order == newOrder)))
                .Return(category);
            var controller = new CategoryController(repository);

            controller.ChangeOrder(id, newOrder);

            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        [Category("Controller")]
        public void ChangeName_RightIdAndNullName_ArgumentNullException()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var id = 1;
            var controller = new CategoryController(repository);

            controller.ChangeText(id, null);
        }

        [Test]
        [Category("Controller")]
        public void ChangeColor_RightIdAndNewColor_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var name = "some name";
            var color = Color.Chartreuse;
            var order = 100;
            var newColor = Color.Turquoise;
            var category = new Category
            {
                Id = id,
                Name = name,
                Color = color,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(category);
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(c => c.Id == id && c.Color == newColor)))
                .Return(category);
            var controller = new CategoryController(repository);

            controller.ChangeColor(id, newColor);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeColor_RightIdAndNullColor_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var category = new Category
            {
                Id = id,
                Name = "some name",
                Color = Color.Chartreuse,
                Order = 100
            };
            repository.Expect(r => r.Get(id)).Return(category);
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(c => c.Id == id && c.Color == null)))
                .Return(category);
            var controller = new CategoryController(repository);

            controller.ChangeColor(id, null);

            repository.VerifyAllExpectations();
        }
    }
}
