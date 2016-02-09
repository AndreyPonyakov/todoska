using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using TodoSystem.UI.Tools.Model;
using TodoSystem.UI.ViewModel.Event;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Implement common functionality of controller ViewModel.
    /// </summary>
    /// <typeparam name="TService">Back-end service. </typeparam>
    /// <typeparam name="TItemModel">Type of model item element. </typeparam>
    /// <typeparam name="TItemViewModel">Type of item element. </typeparam>
    public abstract class BaseOrderableControllerViewModel<TService, TItemModel, TItemViewModel> : BaseViewModel, IServiceable<TService>
        where TItemModel : class
        where TItemViewModel : BaseViewModel, IOrderableItemViewModel<TItemModel, TItemViewModel>
    {
        /// <summary>
        /// Todo Service
        /// </summary>
        private TService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseOrderableControllerViewModel{TService,TItemModel,TItemViewModel}"/> class.
        /// </summary>
        /// <param name="commandFactory">Factory for create ICommand instance. </param>
        protected BaseOrderableControllerViewModel(ICommandFactory commandFactory)
        {
            CommandFactory = commandFactory;
            List = new ObservableCollection<TItemViewModel>();
            CreateItemCommand = commandFactory.CreateCommand(
                () => CreateItem(),
                () => Service != null && !this.HasErrors(nameof(Service)));
        }

        /// <summary>
        /// Gets category list.
        /// </summary>
        public ObservableCollection<TItemViewModel> List { get; }

        /// <summary>
        /// Gets or sets back-end service.
        /// </summary>
        public TService Service
        {
            get { return _service; }
            set { SetField(ref _service, value); }
        }

        /// <summary>
        /// Gets or sets create category command.
        /// </summary>
        public ICommand CreateItemCommand { get; set; }

        /// <summary>
        /// Gets factory for create ICommand instance.
        /// </summary>
        protected ICommandFactory CommandFactory { get; }

        /// <summary>
        /// Undo create new item.
        /// </summary>
        /// <returns>A new item instance. </returns>
        public abstract TItemViewModel CreateItem();

        /// <summary>
        /// Retrieves data from the service and fill form.
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// Closes all opened data.
        /// </summary>
        public void Close()
        {
            List.Clear();
        }

        /// <summary>
        /// Commit all uncommitted changes.
        /// </summary>
        public void Apply()
        {
            List
                .Where(item => item.CanApply)
                .ToList()
                .ForEach(item => item.Apply());
        }

        /// <summary>
        /// Append item into list new item.
        /// </summary>
        /// <param name="item">Appending item. </param>
        public void AppendItem(TItemViewModel item)
        {
            List.Add(item);

            item.Deleted += (sender, args) => List.Remove(sender as TItemViewModel);
            item.Moved += (sender, args) =>
            {
                List.MoveTo(args.DataTransition);
                List.Select((val, i) => new { Value = val, Index = i })
                    .ToList()
                    .ForEach(rec => rec.Value.Order = rec.Index);
            };
        }
    }
}
