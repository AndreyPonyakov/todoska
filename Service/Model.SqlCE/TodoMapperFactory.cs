using System.Drawing;

using AutoMapper;

namespace TodoSystem.Service.Model.SqlCe
{
    /// <summary>
    /// Mapper factory class for todo and category.
    /// </summary>
    public class TodoMapperFactory
    {
        /// <summary>
        /// Initializes category objects in Mapper class.
        /// </summary>
        /// <param name="config">Mapper configuration. </param>
        public static void MapCategory(IMapperConfiguration config)
        {
            config.CreateMap<Category, Interface.Category>()
                .ForMember(
                    dest => dest.Color,
                    opt =>
                    opt.MapFrom(
                        src =>
                        src.Color != null
                            ? (Color?)Color.FromArgb(src.Color.Value)
                            : null));
            config.CreateMap<Interface.Category, Category>()
                .ForMember(
                    dest => dest.Color,
                    opt =>
                    opt.MapFrom(
                        src =>
                        src.Color != null
                            ? (int?)src.Color.Value.ToArgb()
                            : null));
        }

        /// <summary>
        /// Initializes todo objects in Mapper class.
        /// </summary>
        /// <param name="config">Mapper configuration. </param>
        public static void MapTodo(IMapperConfiguration config)
        {
            config.CreateMap<Todo, Interface.Todo>();
            config.CreateMap<Interface.Todo, Todo>();
        }

        /// <summary>
        /// Creates new instance of the <see cref="IMapper"/> class.
        /// </summary>
        /// <returns><see cref="IMapper"/> object. </returns>
        public IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                MapCategory(cfg);
                MapTodo(cfg);
            }).CreateMapper();
        }
    }
}
