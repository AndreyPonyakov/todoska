using System.Windows.Media;

namespace Todo.Service.Model.Interface
{
    /// <summary>
    /// Category interface
    /// </summary>
    public interface ICategory
    {
        int Id { get; }
        string Name { get; }
        Color Color { get; }
        int Order { get; }
    }
}
