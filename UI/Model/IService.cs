namespace TodoSystem.UI.Model
{
    /// <summary>
    /// Abstract service.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Gets exceptions' resolver.
        /// </summary>
        IFaultExceptionManager FaultExceptionManager { get; }
    }
}
