using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;

using NUnit.Framework;

using Rhino.Mocks;

using TodoSystem.Service.Model.Interface;
using TodoSystem.Service.Model.Interface.Exceptions;
using TodoSystem.Service.Model.Interface.Faults;

namespace TodoSystem.Service.Model.Implementation.Test
{
    [TestFixture]
    public class CategoryControllerTest
    {
        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullRepository_ArgumentNullException()
        {
            var controller = new CategoryController(null);        
        }

        [Test]
        [Category("Controller")]
        public void Constructor_NotNullRepository_NoException()
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
        public void Create_CorrectName_NewCategory()
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
        public void Create_CorrectName_RightBehavior()
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
        public void Delete_AnyItem_RightBehavior()
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
        [ExpectedException(typeof(FaultException<ForeignKeyConstraintFault>))]
        public void Delete_ForeignConstraint_ForeignKeyFault()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
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
            var exception = new ForeignKeyConstraintException("Todo");

            repository.Stub(r => r.Get(id)).Return(category);
            repository.Stub(r => r.Delete(category)).Throw(exception);
            var controller = new CategoryController(repository);

            controller.Delete(id);
        }

        [Test]
        [Category("Controller")]
        public void Delete_ForeignConstraint_RightBehavior()
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
            var exception = new ForeignKeyConstraintException("Todo");

            repository.Expect(r => r.Get(id)).Return(category);
            repository.Expect(r => r.Delete(category)).Throw(exception);
            var controller = new CategoryController(repository);

            try
            {
                controller.Delete(id);
            }
            catch (FaultException<ForeignKeyConstraintFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_ExistItemAndNewName_RightBehavior()
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
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        public void ChangeText_NonExistItemAndNewName_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
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
            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new CategoryController(repository);

            controller.ChangeText(id, newName);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_NonExistItemAndNewName_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var newName = "new name";

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new CategoryController(repository);

            try
            {
                controller.ChangeText(id, newName);
            }
            catch (FaultException<ItemNotFoundFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(FaultException<DataValidationFault>))]
        public void ChangeText_ExistItemAndInvalidNewName_DataValidationFault()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
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
            repository.Stub(r => r.Get(id)).Return(category);
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(c => c.Id == id && c.Name == newName)))
                .Throw(new DataValidationException());
            var controller = new CategoryController(repository);

            controller.ChangeText(id, newName);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_ExistItemAndInvalidNewName_RightBehavior()
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
                .Throw(new DataValidationException());
            var controller = new CategoryController(repository);

            try
            {
                controller.ChangeText(id, newName);
            }
            catch (FaultException<DataValidationFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(FaultException<DataValidationFault>))]
        [Category("Controller")]
        public void ChangeName_AnyItemAndNullName_DataValidationFault()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var id = 1;
            var controller = new CategoryController(repository);

            controller.ChangeText(id, null);
        }

        [Test]
        [Category("Controller")]
        public void ChangeOrder_ExistItemAndNewOrder_RightBehavior()
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
        [Category("Controller")]
        public void ChangeOrder_NonExistItemAndNewOrder_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var newOrder = 200;

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new CategoryController(repository);

            try
            {
                controller.ChangeOrder(id, newOrder);
            }
            catch (FaultException<ItemNotFoundFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        public void ChangeOrder_NonExistItemAndNewOrder_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var id = 1;
            var newOrder = 200;
            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new CategoryController(repository);

            controller.ChangeOrder(id, newOrder);
        }

        [Test]
        [Category("Controller")]
        public void ChangeColor_ExistItenAndNewColor_RightBehavior()
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
        public void ChangeColor_ExistItemAndNullColor_NoExeption()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var name = "some name";
            var order = 100;
            var category = new Category
            {
                Id = id,
                Name = name,
                Color = null,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(category);
            repository
                .Expect(r => r.Save(
                    Arg<Category>.Matches(c => c.Id == id && c.Color == null)))
                .Return(category);
            var controller = new CategoryController(repository);

            Assert.DoesNotThrow(() => controller.ChangeColor(id, null));
        }

        [Test]
        [Category("Controller")]
        public void ChangeColor_ExistItemAndNullColor_RightBehavior()
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

        [Test]
        [Category("Controller")]
        public void ChangeColor_NonExistItenAndNewColor_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ICategoryRepository>();
            var id = 1;
            var newColor = Color.Turquoise;

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new CategoryController(repository);

            try
            {
                controller.ChangeColor(id, newColor);
            }
            catch (FaultException<ItemNotFoundFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        public void ChangeColor_NonExistItenAndNewColor_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ICategoryRepository>();
            var id = 1;
            var newColor = Color.Turquoise;

            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new CategoryController(repository);

            controller.ChangeColor(id, newColor);

            repository.VerifyAllExpectations();
        }
    }
}
