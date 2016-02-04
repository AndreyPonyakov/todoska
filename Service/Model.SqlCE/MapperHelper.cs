using System.Drawing;
using AutoMapper;
using Interface = TodoSystem.Service.Model.Interface;

namespace TodoSystem.Model.SqlCe
{
    /// <summary>
    /// Mapper initialization helper class
    /// </summary>
    public static class MapperHelper
    {
        /// <summary>
        /// Initializes category objects in Mapper class.
        /// </summary>
        /// <param name="config">Mapper configuration. </param>
        public static void MapCategory(this IMapperConfiguration config)
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
        public static void MapTodo(this IMapperConfiguration config)
        {
            config.CreateMap<Todo, Interface.Todo>();
            config.CreateMap<Interface.Todo, Todo>();
        }
    }
}
