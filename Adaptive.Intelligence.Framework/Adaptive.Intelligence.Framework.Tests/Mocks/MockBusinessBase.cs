using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Validation;

namespace Adaptive.Intelligence.Framework.Tests.Mocks
{
    /// <summary>
    /// Provides a testable wrapper for the <see cref="BusinessBase"/> abstract class.
    /// </summary>
    public sealed class MockBusinessBase : BusinessBase
    {
        /// <summary>
        /// Gets or sets the value returned by <see cref="PerformDelete"/>.
        /// </summary>
        public bool DeleteResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by <see cref="PerformDeleteAsync"/>.
        /// </summary>
        public bool DeleteAsyncResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by synchronous load operations.
        /// </summary>
        public bool LoadResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by asynchronous load operations.
        /// </summary>
        public bool LoadAsyncResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by <see cref="PerformSave"/>.
        /// </summary>
        public bool SaveResult { get; set; } = true;

        /// <summary>
        /// Gets or sets the value returned by <see cref="PerformSaveAsync"/>.
        /// </summary>
        public bool SaveAsyncResult { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="PerformDelete"/> throws.
        /// </summary>
        public bool ThrowOnDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="PerformDeleteAsync"/> throws.
        /// </summary>
        public bool ThrowOnDeleteAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether synchronous load operations throw.
        /// </summary>
        public bool ThrowOnLoad { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether asynchronous load operations throw.
        /// </summary>
        public bool ThrowOnLoadAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="PerformSave"/> throws.
        /// </summary>
        public bool ThrowOnSave { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="PerformSaveAsync"/> throws.
        /// </summary>
        public bool ThrowOnSaveAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="PerformValidation"/> throws.
        /// </summary>
        public bool ThrowOnValidation { get; set; }

        /// <summary>
        /// Gets the number of times <see cref="PerformValidation"/> has been called.
        /// </summary>
        public int PerformValidationCallCount { get; private set; }

        /// <summary>
        /// Gets or sets the validation messages returned by <see cref="PerformValidation"/>.
        /// </summary>
        public ValidationMessageCollection ValidationToReturn { get; set; } = [];

        /// <summary>
        /// Performs the mock delete operation.
        /// </summary>
        /// <returns>
        /// The configured <see cref="DeleteResult"/> value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="ThrowOnDelete"/> is <see langword="true"/>.
        /// </exception>
        public override bool PerformDelete()
        {
            if (ThrowOnDelete)
            {
                throw new InvalidOperationException("Delete failed.");
            }

            return DeleteResult;
        }

        /// <summary>
        /// Performs the mock asynchronous delete operation.
        /// </summary>
        /// <returns>
        /// A task containing the configured <see cref="DeleteAsyncResult"/> value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="ThrowOnDeleteAsync"/> is <see langword="true"/>.
        /// </exception>
        public override Task<bool> PerformDeleteAsync()
        {
            if (ThrowOnDeleteAsync)
            {
                throw new InvalidOperationException("Delete async failed.");
            }

            return Task.FromResult(DeleteAsyncResult);
        }

        /// <summary>
        /// Registers child business object events for forwarding tests.
        /// </summary>
        /// <param name="child">
        /// The child instance to register.
        /// </param>
        public void RegisterChild(BusinessBase? child)
        {
            RegisterEvents(child);
        }

        /// <summary>
        /// Unregisters child business object events for forwarding tests.
        /// </summary>
        /// <param name="child">
        /// The child instance to unregister.
        /// </param>
        public void UnregisterChild(BusinessBase? child)
        {
            UnRegisterEvents(child);
        }

        /// <summary>
        /// Raises a property changed notification for testing.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed.
        /// </param>
        public void RaisePropertyChangedForTest(string? propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises a property validation changed notification for testing.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property whose validation state changed.
        /// </param>
        public void RaisePropertyValidationChangedForTest(string propertyName)
        {
            OnPropertyValidationChanged(propertyName);
        }

        protected override TResult PerformLoad<TId, TResult>(TId? id) where TId : default
        {
            _ = id;

            if (ThrowOnLoad)
            {
                throw new InvalidOperationException("Load failed.");
            }

            if (typeof(TResult) == typeof(bool))
            {
                return (TResult)(object)LoadResult;
            }

            return default!;
        }

        protected override Task<TResult> PerformLoadAsync<TId, TResult>(TId? id)
            where TId : default
        {
            _ = id;

            if (ThrowOnLoadAsync)
            {
                throw new InvalidOperationException("Load async failed.");
            }

            if (typeof(TResult) == typeof(bool))
            {
                return Task.FromResult((TResult)(object)LoadAsyncResult);
            }

            return Task.FromResult(default(TResult)!);
        }

        /// <summary>
        /// Gets the definition for PerformSave.
        /// </summary>
        protected override bool PerformSave()
        {
            if (ThrowOnSave)
            {
                throw new InvalidOperationException("Save failed.");
            }

            return SaveResult;
        }

        /// <summary>
        /// Gets the definition for PerformSaveAsync.
        /// </summary>
        protected override Task<bool> PerformSaveAsync()
        {
            if (ThrowOnSaveAsync)
            {
                throw new InvalidOperationException("Save async failed.");
            }

            return Task.FromResult(SaveAsyncResult);
        }

        /// <summary>
        /// Gets the definition for PerformValidation.
        /// </summary>
        protected override ValidationMessageCollection PerformValidation()
        {
            PerformValidationCallCount++;

            if (ThrowOnValidation)
            {
                throw new InvalidOperationException("Validation failed.");
            }

            return ValidationToReturn;
        }


    }
}