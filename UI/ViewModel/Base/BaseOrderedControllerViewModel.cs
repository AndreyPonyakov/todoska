using TodoSystem.UI.Tools.Model;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Implement common functionality of controller ViewModel.
    /// </summary>
    /// <typeparam name="TM">Model type. </typeparam>
    public abstract class BaseOrderedControllerViewModel<TM> : BaseViewModel
        where TM : class
    {
    }
}
