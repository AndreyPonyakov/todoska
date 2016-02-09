using System.Drawing;
using AutoMapper;
using TodoSystem.Model.SqlCe;

using Interface = TodoSystem.Service.Model.Interface;

namespace Host
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
        /// <returns>Mapper configuration for fluent style. </returns>
        public static IMapperConfiguration MapCategory(this IMapperConfiguration config)
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
            return config;
        }

        /// <summary>
        /// Initializes todo objects in Mapper class.
        /// </summary>
        /// <param name="config">Mapper configuration. </param>
        /// <returns>Mapper configuration for fluent style. </returns>
        public static IMapperConfiguration MapTodo(this IMapperConfiguration config)
        {
            config.CreateMap<Todo, Interface.Todo>();
            config.CreateMap<Interface.Todo, Todo>();
            return config;
        }
    }
}
