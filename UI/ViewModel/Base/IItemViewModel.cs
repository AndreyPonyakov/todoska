using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace TodoSystem.UI.ViewModel.Base
{
    /// <summary>
    /// Interface of functionality of item ViewModel in controller.
    /// </summary>
    /// <typeparam name="TModel">Model type. </typeparam>
    public interface IItemViewModel<TModel> : INotifyPropertyChanged
        where TModel : class
    {
        /// <summary>
        /// Indicates append notification.
        /// </summary>
        event EventHandler<EventArgs> Appended;

        /// <summary>
        /// Indicates delete notification.
        /// </summary>
        event EventHandler<EventArgs> Deleted;

        /// <summary>
        /// Gets DTO back-end todo.
        /// </summary>
        TModel Model { get; }

        /// <summary>
        /// Gets or sets a value indicating whether change notification flag of any property.
        /// </summary>
        bool Modified { get; set; }

        /// <summary>
        /// Gets a value indicating whether ability of command execute (apply).
        /// </summary>
        bool CanApply { get; }

        /// <summary>
        /// Gets a value indicating whether ability of command execute (undo).
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Gets a value indicating whether ability of command execute (delete).
        /// </summary>
        bool CanDelete { get; }

        /// <summary>
        /// Gets a value indicating whether service error.
        /// </summary>
        bool HasServiceError { get; }

        /// <summary>
        /// Gets create category command
        /// </summary>
        ICommand ApplyCommand { get; }

        /// <summary>
        /// Gets undo command.
        /// </summary>
        ICommand UndoCommand { get; }

        /// <summary>
        /// Gets delete  command.
        /// </summary>
        ICommand TryDeleteCommand { get; }

        /// <summary>
        /// Gets list of attribute properties.
        /// </summary>
        IEnumerable<string> Attributes { get; }

        /// <summary>
        /// Apply action.
        /// </summary>
        void Apply();

        /// <summary>
        /// Undo action.
        /// </summary>
        void Undo();

        /// <summary>
        /// Attempt complete delete action.
        /// </summary>
        void TryDelete();

        /// <summary>
        /// Delete action.
        /// </summary>
        /// <returns>True if records doesn't exist already. </returns>
        bool Delete();

        /// <summary>
        /// Refresh from service.
        /// </summary>
        /// <param name="model">DTO back-end of current todo. </param>
        void Refresh(TModel model);

        /// <summary>
        /// Update at service.
        /// </summary>
        void Update();

        /// <summary>
        /// Create at service.
        /// </summary>
        /// <returns>True in case of operation successfulness. </returns>
        bool Create();

        /// <summary>
        /// Set false for all modified properties.
        /// </summary>
        void ClearMofidied();

        /// <summary>
        /// Checks validation of content properties.
        /// </summary>
        /// <returns>True if content passed validation. </returns>
        bool ContentValidate();
    }
}