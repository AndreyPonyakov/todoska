using System.Collections.Generic;
using System.Windows.Media;

namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Category controller class.
    /// </summary>
    public interface ICategoryController
    {
        IEnumerable<ICategory> SelectAll();

        ICategory SelectById(int id);

        IEnumerable<ICategory> SelectByName(string name);

        ICategory Create(string name, Color color, int order);

        void Update(ICategory category);

        void Delete(int id);

    }
}
