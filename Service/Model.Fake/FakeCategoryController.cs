using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public class FakeCategoryController : ICategoryController
    {
        private readonly IFakeTodoService _service;

        public IEnumerable<ICategory> SelectAll()
        {
            return _service.CategoryList;
        }

        public ICategory SelectById(int id)
        {
            return _service.CategoryList
                .Single(c => c.Id == id);
        }

        public IEnumerable<ICategory> SelectByName(string name)
        {
            return _service.CategoryList
                .Where(c => c.Name == name)
                .ToList();
        }

        public ICategory Create(string name, Color color, int order)
        {
            var category = new FakeCategory(GeterateId())
            {
                Name = name,
                Order = order,
                Color = color
            };
            _service.CategoryList.Add(category);
            return category;
        }

        private int GeterateId()
        {
            return _service.CategoryList.Any()
                ? _service.CategoryList.Max(c => c.Id) + 1
                : 1;
        }

        public void Update(ICategory category)
        {
            _service.CategoryList
                .Where(c => c.Id == category.Id)
                .ToList()
                .ForEach(c =>
                {
                    c.Name = category.Name;
                    c.Order = category.Order;
                    c.Color = category.Color;
                });
        }

        public void Delete(int id)
        {
            var category = _service.CategoryList
                .FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _service.CategoryList.Remove(category);
            }
        }

        public FakeCategoryController(IFakeTodoService service)
        {
            _service = service;
        }

    }
}
