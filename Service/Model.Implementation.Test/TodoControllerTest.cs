using System;
using System.Collections.Generic;

using NUnit.Framework;
using Rhino.Mocks;

using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.Implementation.Test
{
    [TestFixture]
    public class TodoControllerTest
    {
        /// <summary>
        /// Storage implementation for <see cref="ITodoController"/>
        /// </summary>
        private sealed class TodoController : BaseTodoController
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BaseTodoController"/> class.
            /// </summary>
            /// <param name="repository">Category repository. </param>
            public TodoController(ITodoRepository repository)
                : base(repository)
            {
            }
        }

        [Test]
        [Category("Controller")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullArguments_ArgumentNullException()
        {
            var controller = new TodoController(null);
        }

        [Test]
        [Category("Controller")]
        public void Constructor_NotNullArguments_NoException()
        {
            Assert.DoesNotThrow(
                () =>
                {
                    var repository =
                        MockRepository.GenerateStub<ITodoRepository>();
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
        public void Create_ArgumentList_Todo()
        {
            var repository = MockRepository.GenerateStub<ITodoRepository>();
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var order = 100;
            var Todo = new Todo();
            repository
                .Stub(r => r.Save(
                    Arg<Todo>.Matches(
                        t => t.Title == title && t.Desc == desc
                        && t.CategoryId == categoryId && t.Deadline == deadline
                        && t.Order == order && !t.Checked && t.Id == default(int))))
                .Return(Todo);
            var controller = new TodoController(repository);

            Assert.That(
                controller.Create(title, desc, deadline, categoryId, order),
                Is.SameAs(Todo));
        }

        [Test]
        [Category("Controller")]
        public void Create_ArgumentList_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var order = 100;

            var Todo = new Todo();
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(
                        t => t.Title == title && t.Desc == desc
                        && t.CategoryId == categoryId && t.Deadline == deadline
                        && t.Order == order && !t.Checked && t.Id == default(int))))
                .Return(Todo);
            var controller = new TodoController(repository);

            controller.Create(title, desc, deadline, categoryId, order);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Update_DefaultId_NoActionBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var Todo = new Todo
            {
                Id = default(int),
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            var controller = new TodoController(repository);

            controller.Update(Todo);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Update_RightId_RightBehavior()
        {
            var repository = MockRepository.GenerateMock<ITodoRepository>();
            var title = "some name";
            var desc = "some desc";
            var deadline = DateTime.Now;
            var categoryId = 80;
            var isChecked = false;
            var order = 100;
            var Todo = new Todo
            {
                Id = default(int),
                Title = title,
                Desc = desc,
                Deadline = deadline,
                CategoryId = categoryId,
                Checked = isChecked,
                Order = order
            };
            var result = new Todo();
            repository.Expect(r => r.Save(Todo)).Return(result);
            var controller = new TodoController(repository);

            controller.Update(Todo);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Delete_RightId_RightBehavior()
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
        public void ChangeOrder_RightIdAndNewOrder_RightBehavior()
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
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.Order == newOrder)))
                .Return(Todo);
            var controller = new TodoController(repository);

            controller.ChangeOrder(id, newOrder);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SetCategory_RightIdAndNewCategoryId_RightBehavior()
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
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.CategoryId == newCategoryId)))
                .Return(Todo);
            var controller = new TodoController(repository);

            controller.SetCategory(id, newCategoryId);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void SetDeadline_RightIdAndNewDeadline_RightBehavior()
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
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.Deadline == newDeadline)))
                .Return(Todo);
            var controller = new TodoController(repository);

            controller.SetDeadline(id, newDeadline);

            repository.VerifyAllExpectations();
        }

        [Test]
        [Category("Controller")]
        public void Check_RightIdAndNewChecked_RightBehavior()
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
            repository
                .Expect(r => r.Save(
                    Arg<Todo>.Matches(c => c.Id == id && c.Checked == newChecked)))
                .Return(Todo);
            var controller = new TodoController(repository);

            controller.Check(id, newChecked);

            repository.VerifyAllExpectations();
        }

    }
}
