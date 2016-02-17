using System;
using System.Collections.Generic;
using System.ServiceModel;

using NUnit.Framework;

using Rhino.Mocks;

using TodoSystem.Service.Model.Interface;
using TodoSystem.Service.Model.Interface.Exceptions;
using TodoSystem.Service.Model.Interface.Faults;

namespace TodoSystem.Service.Model.Implementation.Test
{
    [TestFixture]
    public class TodoControllerTest
    {
        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullRepository_ArgumentNullException()
        {
            var controller = new TodoController(null);
        }

        [Test]
        [Category("Controller")]
        public void Constructor_NotNullRepository_NoException()
        {
            Assert.DoesNotThrow(
                () =>
                {
                    var repository = MockRepository.GenerateStub<ITodoRepository>();
                    var controller = new TodoController(repository);
                });
        }

        [Test]
        [Category("Controller")]
        public void SelectAll_List_SameList()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var list = MockRepository.GenerateStub<IEnumerable<Todo>>();
            repository.Stub(r => r.GetAll()).Return(list);
            var controller = new TodoController(repository);

            Assert.That(controller.SelectAll(), Is.SameAs(list));
        }

        [Test]
        [Category("Controller")]
        public void SelectAll_List_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var list = MockRepository.GenerateStub<IEnumerable<Todo>>();
            repository.Expect(r => r.GetAll()).Return(list);
            var controller = new TodoController(repository);

            controller.SelectAll();

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SelectById_Item_SameItem()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var todo = new Todo();
            repository.Stub(r => r.Get(id)).Return(todo);
            var controller = new TodoController(repository);

            Assert.That(controller.SelectById(id), Is.SameAs(todo));
        }

        [Test]
        [Category("Controller")]
        public void SelectById_Item_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var todo = new Todo();
            repository.Expect(r => r.Get(id)).Return(todo);
            var controller = new TodoController(repository);

            controller.SelectById(id);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SelectByTitle_Title_RightList()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var title = "some name";
            var list = MockRepository.GenerateStub<IEnumerable<Todo>>();
            repository.Stub(r => r.Find(title)).Return(list);
            var controller = new TodoController(repository);

            Assert.That(controller.SelectByTitle(title), Is.SameAs(list));
        }

        [Test]
        [Category("Controller")]
        public void SelectByTitle_Title_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var title = "some name";
            var list = MockRepository.GenerateStub<IEnumerable<Todo>>();
            repository.Expect(r => r.Find(title)).Return(list);
            var controller = new TodoController(repository);

            controller.SelectByTitle(title);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SelectByCategory_Category_RightList()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var categoryId = 18;
            var list = MockRepository.GenerateStub<IEnumerable<Todo>>();
            repository.Stub(r => r.Find(Arg<Category>.Matches(c => c.Id == categoryId))).Return(list);
            var controller = new TodoController(repository);

            Assert.That(controller.SelectByCategory(categoryId), Is.SameAs(list));
        }

        [Test]
        [Category("Controller")]
        public void SelectByCategory_Category_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var categoryId = 18;
            var list = MockRepository.GenerateStub<IEnumerable<Todo>>();
            repository.Expect(r => r.Find(Arg<Category>.Matches(c => c.Id == categoryId))).Return(list);
            var controller = new TodoController(repository);

            controller.SelectByCategory(categoryId);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Create_CorrectTitle_NewTodo()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var title = "some name";

            var todo = new Todo();
            var last = new Todo
            {
                Id = 100,
                Title = "some title",
                Desc = "some desc",
                Deadline = DateTime.Now,
                CategoryId = 200,
                Checked = false,
                Order = 150
            };

            repository.Stub(r => r.FindLast()).Return(last);
            repository
                .Stub(r => r.Save(
                    Arg<Todo>.Matches(
                        t => t.Title == title && t.Desc == null
                        && t.CategoryId == null && t.Deadline == null
                        && t.Order == last.Order + 1 && !t.Checked && t.Id == default(int))))
                .Return(todo);
            var controller = new TodoController(repository);

            Assert.That(controller.Create(title), Is.SameAs(todo));
        }

        [Test]
        [Category("Controller")]
        public void Create_NullTitle_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var controller = new TodoController(repository);

            try
            {
                controller.Create(null);
            }
            catch (FaultException<DataValidationFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(FaultException<DataValidationFault>))]
        [Category("Controller")]
        public void Create_NullTitle_DataValidationFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var controller = new TodoController(repository);

            controller.Create(null);

            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(FaultException<DataValidationFault>))]
        [Category("Controller")]
        public void Create_IncorrectTitle_DataValidationFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var title = "some name";

            var last = new Todo
            {
                Id = 100,
                Title = "some title",
                Desc = "some desc",
                Deadline = DateTime.Now,
                CategoryId = 200,
                Checked = false,
                Order = 150
            };

            repository.Stub(r => r.FindLast()).Return(last);
            repository
                .Stub(r => r.Save(
                    Arg<Todo>.Matches(
                        t => t.Title == title && t.Desc == null
                        && t.CategoryId == null && t.Deadline == null
                        && t.Order == last.Order + 1 && !t.Checked && t.Id == default(int))))
                .Throw(new DataValidationException());
            var controller = new TodoController(repository);

            controller.Create(title);
        }

        [Test]
        [Category("Controller")]
        public void Create_IncorrectTitle_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var title = "some name";

            var last = new Todo
            {
                Id = 100,
                Title = "some title",
                Desc = "some desc",
                Deadline = DateTime.Now,
                CategoryId = 200,
                Checked = false,
                Order = 150
            };

            repository.Expect(r => r.FindLast()).Return(last);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(
                        t => t.Title == title && t.Desc == null
                        && t.CategoryId == null && t.Deadline == null
                        && t.Order == last.Order + 1 && !t.Checked && t.Id == default(int))))
                .Throw(new DataValidationException());
            var controller = new TodoController(repository);

            try
            {
                controller.Create(title);
            }
            catch (FaultException<DataValidationFault>)
            {
            }
        }

        [Test]
        [Category("Controller")]
        public void Create_CorrectTitle_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var title = "some name";

            var todo = new Todo();
            var last = new Todo
            {
                Id = 100,
                Title = "some title",
                Desc = "some desc",
                Deadline = DateTime.Now,
                CategoryId = 200,
                Checked = false,
                Order = 150
            };

            repository.Expect(r => r.FindLast()).Return(last);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(
                        t => t.Title == title && t.Desc == null
                        && t.CategoryId == null && t.Deadline == null
                        && t.Order == last.Order + 1 && !t.Checked && t.Id == default(int))))
                .Return(todo);
            var controller = new TodoController(repository);

            controller.Create(title);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Delete_AnyItem_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var Todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };

            repository.Expect(r => r.Get(id)).Return(Todo);
            repository.Expect(r => r.Delete(Todo));
            var controller = new TodoController(repository);

            controller.Delete(id);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_ExistItemAndNewCorrectText_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newTitle = "new name";
            var newDesc = "new desc";
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(todo);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(t => t.Id == id && t.Title == newTitle && t.Desc == newDesc)))
                .Return(todo);
            var controller = new TodoController(repository);

            controller.ChangeText(id, newTitle, newDesc);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_NonExistItemAndNewCorrectText_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var newTitle = "new name";
            var newDesc = "new desc";

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            try
            {
                controller.ChangeText(id, newTitle, newDesc);
            }
            catch (FaultException<ItemNotFoundFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        [Category("Controller")]
        public void ChangeText_NonExistItemAndNewCorrectText_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var newTitle = "new name";
            var newDesc = "new desc";

            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            controller.ChangeText(id, newTitle, newDesc);

            repository.VerifyAllExpectations();
        }


        [Test]
        [Category("Controller")]
        public void ChangeText_ExistItemAndNewNullTitle_RightBehavior()
        {
            var id = 1;
            var newDesc = "new desc";
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var controller = new TodoController(repository);

            try
            {
                controller.ChangeText(id, null, newDesc);
            }
            catch (FaultException<DataValidationFault>)
            {
            }
            
            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(FaultException<DataValidationFault>))]
        [Category("Controller")]
        public void ChangeText_ExistItemAndNewNullTitle_DataValidationFault()
        {
            var id = 1;
            var newDesc = "new desc";
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var controller = new TodoController(repository);

            controller.ChangeText(id, null, newDesc);
        }

        [Test]
        [Category("Controller")]
        public void ChangeText_ExistItemAndNewIncorrect_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newTitle = "new name";
            var newDesc = "new desc";
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(todo);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(t => t.Id == id && t.Title == newTitle && t.Desc == newDesc)))
                .Throw(new DataValidationException());

            var controller = new TodoController(repository);

            try
            {
                controller.ChangeText(id, newTitle, newDesc);
            }
            catch (FaultException<DataValidationFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(FaultException<DataValidationFault>))]
        [Category("Controller")]
        public void ChangeText_ExistItemAndNewIncorrectText_DataValidationFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newTitle = "new name";
            var newDesc = "new desc";
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            repository.Stub(r => r.Get(id)).Return(todo);
            repository
                .Stub(r => r.Save(
                    Arg<Todo>.Matches(t => t.Id == id && t.Title == newTitle && t.Desc == newDesc)))
                .Throw(new DataValidationException());
            var controller = new TodoController(repository);

            controller.ChangeText(id, newTitle, newDesc);
        }


        [Test]
        [Category("Controller")]
        public void ChangeOrder_ExistItemAndNewOrder_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newOrder = 200;
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(todo);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.Order == newOrder)))
                .Return(todo);
            var controller = new TodoController(repository);

            controller.ChangeOrder(id, newOrder);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void ChangeOrder_NonExistItemAndNewOrder_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var newOrder = 200;

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

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
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        [Category("Controller")]
        public void ChangeOrder_NonExistItemAndNewOrder_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var newOrder = 200;

            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            controller.ChangeOrder(id, newOrder);
        }

        [Test]
        [Category("Controller")]
        public void SetCategory_ExistItemAndNewExistCategoryId_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newCategoryId = 200;
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(todo);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.CategoryId == newCategoryId)))
                .Return(todo);
            var controller = new TodoController(repository);

            controller.SetCategory(id, newCategoryId);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        public void SetCategory_NonExistItemAndNewExistCategoryId_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var newCategoryId = 200;

            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            controller.SetCategory(id, newCategoryId);
        }

        [Test]
        [Category("Controller")]
        public void SetCategory_NonExistItemAndNewExistCategoryId_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var newCategoryId = 200;

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            try
            {
                controller.SetCategory(id, newCategoryId);
            }
            catch (FaultException<ItemNotFoundFault>)
            {
            }

            repository.VerifyAllExpectations();
        }


        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(FaultException<ForeignKeyConstraintFault>))]
        public void SetCategory_ExistItemAndNewNonExistCategoryId_ForeignKeyConstraintFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newCategoryId = 200;
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };

            repository.Stub(r => r.Get(id)).Return(todo);
            repository
                .Stub(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.CategoryId == newCategoryId)))
                .Throw(new ForeignKeyConstraintException("constraint"));
            var controller = new TodoController(repository);

            controller.SetCategory(id, newCategoryId);
        }

        [Test]
        [Category("Controller")]
        public void SetCategory_ExistItemAndNewNonExistCategoryId_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newCategoryId = 200;
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };

            repository.Expect(r => r.Get(id)).Return(todo);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.CategoryId == newCategoryId)))
                .Throw(new ForeignKeyConstraintException("constraint"));
            var controller = new TodoController(repository);

            try
            {
                controller.SetCategory(id, newCategoryId);
            }
            catch (FaultException<ForeignKeyConstraintFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SetDeadline_ExistItemAndNewDeadline_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newDeadline = DateTime.Now.AddDays(2);
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(todo);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.Deadline == newDeadline)))
                .Return(todo);
            var controller = new TodoController(repository);

            controller.SetDeadline(id, newDeadline);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SetDeadline_NonExistItemAndNewDeadline_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var newDeadline = DateTime.Now.AddDays(2);

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            try
            {
                controller.SetDeadline(id, newDeadline);
            }
            catch (FaultException<ItemNotFoundFault>)
            {
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        public void SetDeadline_ExistItemAndNewDeadline_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var newDeadline = DateTime.Now.AddDays(2);

            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            controller.SetDeadline(id, newDeadline);
        }
        [Test]
        [Category("Controller")]
        public void Check_RightIdAndExistItemAndNewChecked_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var newChecked = true;
            var todo = new Todo
            {
                Id = id,
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            repository.Expect(r => r.Get(id)).Return(todo);
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.Checked == newChecked)))
                .Return(todo);
            var controller = new TodoController(repository);

            controller.Check(id, newChecked);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(FaultException<ItemNotFoundFault>))]
        public void Check_RightIdAndNonExistItemAndNewChecked_ItemNotFoundFault()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var id = 1;
            var newChecked = true;

            repository.Stub(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            controller.Check(id, newChecked);
        }

        [Test]
        [Category("Controller")]
        public void Check_RightIdAndNonExistItemAndNewChecked_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var id = 1;
            var newChecked = true;

            repository.Expect(r => r.Get(id)).Return(null);
            var controller = new TodoController(repository);

            try
            {
                controller.Check(id, newChecked);
            }
            catch (FaultException<ItemNotFoundFault>)
            {
            }

            repository.VerifyAllExpectations();
        }
    }
}
