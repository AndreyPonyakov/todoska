using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace TodoSystem.Service.Tools.UnityExtension
{
    /// <summary>
    /// Implementing disposable maintenance unity extension class.
    /// </summary>
    public sealed class DisposableStrategyExtension : UnityContainerExtension, IDisposable
    {
        private readonly DisposingLifetimeStrategy _buildStrategy = new DisposingLifetimeStrategy();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _buildStrategy?.DisposeAllTrees();
        }

        /// <summary>
        /// Initial the container with this extension's functionality.
        /// </summary>
        protected override void Initialize()
        {
            Context.Strategies.Add(_buildStrategy, UnityBuildStage.TypeMapping);
        }
    }
}