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
            var name = "some name";
            var color = Color.Chartreuse;
            var order = 100;
            var category = new Category();
            repository
                .Stub(r => r.Save(
                    Arg<Category>.Matches(
                        c => c.Name == name && c.Color == color 
                        && c.Order == order && c.Id == default(int))))
                .Return(category);
            var controller = new CategoryController(repository);

            Assert.That(
                controller.Create(name, color, order),
                Is.SameAs(category));
        }

        [Test]
        [Category("Controller")]
        public void Create_ArgumentList_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var name = "some name";
            var color = Color.Chartreuse;
            var order = 100;
            var category = new Category();
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(
                        c => c.Name == name && c.Color == color
                        && c.Order == order && c.Id == default(int))))
                .Return(category);
            var controller = new CategoryController(repository);

            controller.Create(name, color, order);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Update_DefaultId_NoActionBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var name = "some name";
            var color = Color.Chartreuse;
            var order = 100;
            var category = new Category {
                                   Id = default(int),
                                   Name = name,
                                   Color = color,
                                   Order = order
                               };
            var controller = new CategoryController(repository);

            controller.Update(category);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Update_RightId_RightBehavior()
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
            var result = new Category();
            repository.Expect(r => r.Save(category)).Return(result);
            var controller = new CategoryController(repository);

            controller.Update(category);

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
        public void ChangeOrder_RightIdAndNewOrder_RightBehavior()
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
                .Expect(r =>r.Save(
                    Arg<Category>.Matches(c => c.Id == id && c.Order == newOrder)))
                .Return(category);
            var controller = new CategoryController(repository);

            controller.ChangeOrder(id, newOrder);

            repository.VerifyAllExpectations();
        }
    }
}
