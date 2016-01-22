using System;
using System.ServiceModel;

namespace TodoSystem.Service.Model.Interface
{
    /// <summary>
    /// Todo class.
    /// </summary>
    [ServiceContract]
    public interface ITodo
    {
        /// <summary>
        /// Make checked.
        /// </summary>
        [OperationContract]
        void Check(bool isChecked);

        /// <summary>
        /// Change category.
        /// </summary>
        /// <param name="categoryId">Primary key of new category. </param>
        [OperationContract]
        void SetCategory(int categoryId);

        /// <summary>
        /// Set deadline time.
        /// </summary>
        /// <param name="deadline">Deadline time. </param>
        [OperationContract]
        void SetDeadline(DateTime deadline);
    }
}
