using TodoSystem.Service.Model.Interface;

namespace TodoSystem.Service.Model.Fake
{
    /// <summary>
    /// Special help class for create new instance DTO.
    /// </summary>
    public static class FakeModelHelper
    {
        /// <summary>
        /// Clone Todo from origin.
        /// </summary>
        /// <param name="origin">Original todo. </param>
        /// <returns>Cloned todo. </returns>
        public static Todo Clone(this Todo origin) =>
            new Todo()
            {
                Id = origin.Id,
                Title = origin.Title,
                Desc = origin.Desc,
                CategoryId = origin.CategoryId,
                Deadline = origin.Deadline,
                Checked = origin.Checked,
                Order = origin.Order
            };

        /// <summary>
        /// Clone category from origin.
        /// </summary>
        /// <param name="origin">Original category. </param>
        /// <returns>Cloned category. </returns>
        public static Category Clone(this Category origin) =>
            new Category()
            {
                Id = origin.Id,
                Name = origin.Name,
                Color = origin.Color,
                Order = origin.Order
            };
    }
}
