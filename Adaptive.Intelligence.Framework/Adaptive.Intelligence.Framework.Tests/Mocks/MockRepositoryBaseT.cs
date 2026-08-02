using Adaptive.Intelligence.Abstractions.Repository;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Mocks
{
    /// <summary>
    /// Provides a testable wrapper for <see cref="RepositoryBase{TIdType, TEntityType}"/>.
    /// </summary>
    public sealed class MockRepositoryBaseT : RepositoryBase<int, MockEntityBase>
    {
        /// <summary>
        /// Gets or sets the value returned by add operations.
        /// </summary>
        public bool AddResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by delete operations.
        /// </summary>
        public bool DeleteResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by save operations.
        /// </summary>
        public bool SaveResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by asynchronous add operations.
        /// </summary>
        public bool AddAsyncResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by asynchronous delete operations.
        /// </summary>
        public bool DeleteAsyncResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by asynchronous save operations.
        /// </summary>
        public bool SaveAsyncResult { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether add operations throw.
        /// </summary>
        public bool ThrowOnAdd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether asynchronous add operations throw.
        /// </summary>
        public bool ThrowOnAddAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether delete operations throw.
        /// </summary>
        public bool ThrowOnDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether asynchronous delete operations throw.
        /// </summary>
        public bool ThrowOnDeleteAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether load operations throw.
        /// </summary>
        public bool ThrowOnLoad { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether asynchronous load operations throw.
        /// </summary>
        public bool ThrowOnLoadAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether save operations throw.
        /// </summary>
        public bool ThrowOnSave { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether asynchronous save operations throw.
        /// </summary>
        public bool ThrowOnSaveAsync { get; set; }

        /// <summary>
        /// Gets or sets the value returned by load operations.
        /// </summary>
        public MockEntityBase? LoadByIdResult { get; set; }

        /// <summary>
        /// Gets or sets the value returned by asynchronous load operations.
        /// </summary>
        public MockEntityBase? LoadAsyncResult { get; set; }

        /// <summary>
        /// Gets the last cancellation token passed to an async add operation.
        /// </summary>
        public CancellationToken? LastAddAsyncToken { get; private set; }

        /// <summary>
        /// Gets the last cancellation token passed to an async delete operation.
        /// </summary>
        public CancellationToken? LastDeleteAsyncToken { get; private set; }

        /// <summary>
        /// Gets the last cancellation token passed to an async load operation.
        /// </summary>
        public CancellationToken? LastLoadAsyncToken { get; private set; }

        /// <summary>
        /// Gets the last cancellation token passed to an async save operation.
        /// </summary>
        public CancellationToken? LastSaveAsyncToken { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRepositoryBaseT"/> class.
        /// </summary>
        /// <param name="logger">
        /// The optional logger instance.
        /// </param>
        public MockRepositoryBaseT(ILogger? logger = null)
            : base(logger)
        {
        }

        /// <summary>
        /// Performs add operation for the repository base implementation.
        /// </summary>
        /// <param name="instance">
        /// The instance to add.
        /// </param>
        /// <returns>
        /// The configured add result.
        /// </returns>
        protected override bool PerformAdd(MockEntityBase instance)
        {
            _ = instance;

            if (ThrowOnAdd)
            {
                throw new InvalidOperationException("Add failed.");
            }

            return AddResult;
        }

        /// <summary>
        /// Performs asynchronous add operation for the repository base implementation.
        /// </summary>
        /// <param name="instance">
        /// The instance to add.
        /// </param>
        /// <param name="token">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The configured asynchronous add result.
        /// </returns>
        protected override Task<bool> PerformAddAsync(MockEntityBase instance, CancellationToken token)
        {
            _ = instance;
            LastAddAsyncToken = token;

            if (ThrowOnAddAsync)
            {
                throw new InvalidOperationException("Add async failed.");
            }

            return Task.FromResult(AddAsyncResult);
        }

        /// <summary>
        /// Performs delete operation for the repository base implementation.
        /// </summary>
        /// <param name="instance">
        /// The instance to delete.
        /// </param>
        /// <returns>
        /// The configured delete result.
        /// </returns>
        protected override bool PerformDelete(MockEntityBase instance)
        {
            _ = instance;

            if (ThrowOnDelete)
            {
                throw new InvalidOperationException("Delete failed.");
            }

            return DeleteResult;
        }

        /// <summary>
        /// Performs asynchronous delete operation for the repository base implementation.
        /// </summary>
        /// <param name="instance">
        /// The instance to delete.
        /// </param>
        /// <param name="token">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The configured asynchronous delete result.
        /// </returns>
        protected override Task<bool> PerformDeleteAsync(MockEntityBase instance, CancellationToken token)
        {
            _ = instance;
            LastDeleteAsyncToken = token;

            if (ThrowOnDeleteAsync)
            {
                throw new InvalidOperationException("Delete async failed.");
            }

            return Task.FromResult(DeleteAsyncResult);
        }

        /// <summary>
        /// Performs the load operation.
        /// </summary>
        /// <param name="idType">
        /// The identity value.
        /// </param>
        /// <returns>
        /// The configured load result.
        /// </returns>
        protected override MockEntityBase? PerformLoadById(int idType)
        {
            _ = idType;

            if (ThrowOnLoad)
            {
                throw new InvalidOperationException("Load failed.");
            }

            return LoadByIdResult;
        }

        /// <summary>
        /// Performs the asynchronous load operation.
        /// </summary>
        /// <param name="idType">
        /// The identity value.
        /// </param>
        /// <param name="token">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The configured asynchronous load result.
        /// </returns>
        protected override Task<MockEntityBase?> PerformLoadAsync(int idType, CancellationToken token)
        {
            _ = idType;
            LastLoadAsyncToken = token;

            if (ThrowOnLoadAsync)
            {
                throw new InvalidOperationException("Load async failed.");
            }

            return Task.FromResult(LoadAsyncResult);
        }

        /// <summary>
        /// Performs save operation for the repository base implementation.
        /// </summary>
        /// <param name="instance">
        /// The instance to save.
        /// </param>
        /// <returns>
        /// The configured save result.
        /// </returns>
        protected override bool PerformSave(MockEntityBase instance)
        {
            _ = instance;

            if (ThrowOnSave)
            {
                throw new InvalidOperationException("Save failed.");
            }

            return SaveResult;
        }

        /// <summary>
        /// Performs asynchronous save operation for the repository base implementation.
        /// </summary>
        /// <param name="instance">
        /// The instance to save.
        /// </param>
        /// <param name="token">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The configured asynchronous save result.
        /// </returns>
        protected override Task<bool> PerformSaveAsync(MockEntityBase instance, CancellationToken token)
        {
            _ = instance;
            LastSaveAsyncToken = token;

            if (ThrowOnSaveAsync)
            {
                throw new InvalidOperationException("Save async failed.");
            }

            return Task.FromResult(SaveAsyncResult);
        }
    }
}