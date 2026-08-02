namespace Adaptive.Intelligence.Abstractions.Repository
{
    /// <summary>
    /// Provides the signature definition for standard data access / repository methods and functions to
    /// support data access operations.
    /// </summary>
    /// <typeparam name="TIdType">
    /// The data type of the identity column or field for the entity instances.
    /// </typeparam>
    /// <typeparam name="TEntityType">
    /// The data type of the entity instances managed by the repository.
    /// </typeparam>
    public interface IRepository<TIdType, TEntityType> : IRepository
    {
        /// <summary>
        /// Creates and adds a new instance of <typeparamref name="TEntityType"/> to the repository.
        /// </summary>
        /// <param name="item">
        /// The new instance to be added.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        bool Add(TEntityType? item);

        /// <summary>
        /// Creates and adds a new instance of <typeparamref name="TEntityType"/> to the repository.
        /// </summary>
        /// <param name="item">
        /// The new instance to be added.
        /// </param>
        /// <param name="token">
        /// The <see cref="CancellationToken"/> instance used to cancel the operation.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        Task<bool> AddAsync(TEntityType? item, CancellationToken token);

        /// <summary>
        /// Deletes the specified business object definition from the data store.
        /// </summary>
        /// <param name="item">
        /// The <typeparamref name="TEntityType"/> instance to be deleted.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        bool Delete(TEntityType? item);

        /// <summary>
        /// Deletes the specified business object definition from the data store.
        /// </summary>
        /// <param name="item">
        /// The <typeparamref name="TEntityType"/> instance to be deleted.
        /// </param>
        /// <param name="token">
        /// The <see cref="CancellationToken"/> instance used to cancel the operation.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        Task<bool> DeleteAsync(TEntityType? item, CancellationToken token);

        /// <summary>
        /// Gets the instance from data source.
        /// </summary>
        /// <param name="id">
        /// A value serving as the identity value used to load the instance.
        /// </param>
        /// <returns>
        /// A <typeparamref name="TEntityType"/> instance, if successful; otherwise, returns <b>null</b>.
        /// </returns>
        TEntityType? LoadItem(TIdType id);

        /// <summary>
        /// Gets the instance from the data source.
        /// </summary>
        /// <param name="id">
        /// A value serving as the identity value used to load the instance.
        /// </param>
        /// <param name="token">
        /// The <see cref="CancellationToken"/> instance used to cancel the operation.
        /// </param>
        /// <returns>
        /// A <typeparamref name="TEntityType"/> instance, if successful; otherwise, returns <b>null</b>.
        /// </returns>
        Task<TEntityType?> LoadItemAsync(TIdType id, CancellationToken token);

        /// <summary>
        /// Saves the specified item to the data store.
        /// </summary>
        /// <param name="item">
        /// The business object instance to be saved.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation completes successfully; otherwise, returns <b>false</b>.
        /// </returns>
        bool Save(TEntityType? item);

        /// <summary>
        /// Saves the specified item to the data store.
        /// </summary>
        /// <param name="item">
        /// The business object instance to be saved.
        /// </param>
        /// <param name="token">
        /// The <see cref="CancellationToken"/> instance used to cancel the operation.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation completes successfully; otherwise, returns <b>false</b>.
        /// </returns>
        Task<bool> SaveAsync(TEntityType? item, CancellationToken token);
    }
}