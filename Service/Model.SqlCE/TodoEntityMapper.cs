using System;
using System.Drawing;
using Interface = TodoSystem.Service.Model.Interface;

namespace Model.SqlCe
{
    /// <summary>
    /// Mapper class for conversion from entity framework to dto.
    /// </summary>
    public static class TodoEntityMapper
    {
        /// <summary>
        /// Converts from category dto to category entity. 
        /// </summary>
        /// <param name="dto">Source category dto.</param>
        /// <returns>Mapped category entity. </returns>
        public static Category ToEntity(this Interface.Category dto)
        {
            return new Category()
                       {
                           Id = dto.Id,
                           Name = dto.Name,
                           Order = dto.Order,
                           Color = dto.Color.ToArgb()
                       };
        }

        /// <summary>
        /// Converts from todo dto to todo entity. 
        /// </summary>
        /// <param name="dto">Source todo dto.</param>
        /// <returns>Mapped todo entity. </returns>
        public static Todo ToEntity(this Interface.Todo dto)
        {
            return new Todo()
            {
                Id = dto.Id,
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Desc = dto.Desc,
                Deadline = dto.Deadline,
                Checked = dto.Checked,
                Order = dto.Order
            };
        }

        /// <summary>
        /// Converts from category entity to category dto. 
        /// </summary>
        /// <param name="entity">Source category entity.</param>
        /// <returns>Mapped category dto. </returns>
        public static Interface.Category ToDto(this Category entity)
        {
            return new Interface.Category()
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order,
                Color = Color.FromArgb(entity.Color.Value)
            };
        }

        /// <summary>
        /// Converts from todo entity to todo dto. 
        /// </summary>
        /// <param name="entity">Source todo entity.</param>
        /// <returns>Mapped todo dto. </returns>
        public static Interface.Todo ToDto(this Todo entity)
        {
            return new Interface.Todo()
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                Desc = entity.Desc,
                Deadline = entity.Deadline ?? DateTime.Now,
                Checked = entity.Checked ?? false,
                Order = entity.Order
            };
        }
    }
}
