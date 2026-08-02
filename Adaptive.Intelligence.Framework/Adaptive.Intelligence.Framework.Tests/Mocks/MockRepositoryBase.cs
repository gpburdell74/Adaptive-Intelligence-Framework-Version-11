using Adaptive.Intelligence.Abstractions.Repository;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Mocks
{
    /// <summary>
    /// Provides a testable wrapper for <see cref="RepositoryBase"/> protected members.
    /// </summary>
    public sealed class MockRepositoryBase : RepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockRepositoryBase"/> class.
        /// </summary>
        /// <param name="logger">
        /// The optional logger instance.
        /// </param>
        public MockRepositoryBase(ILogger? logger = null)
            : base(logger)
        {
        }

        /// <summary>
        /// Invokes <see cref="RepositoryBase.OnAsyncQueryStarted(string)"/>.
        /// </summary>
        /// <param name="methodName">
        /// The operation name.
        /// </param>
        public void InvokeOnAsyncQueryStarted(string methodName)
        {
            OnAsyncQueryStarted(methodName);
        }

        /// <summary>
        /// Invokes <see cref="RepositoryBase.OnAsyncQueryCompleted(string)"/>.
        /// </summary>
        /// <param name="methodName">
        /// The operation name.
        /// </param>
        public void InvokeOnAsyncQueryCompleted(string methodName)
        {
            OnAsyncQueryCompleted(methodName);
        }
    }
}