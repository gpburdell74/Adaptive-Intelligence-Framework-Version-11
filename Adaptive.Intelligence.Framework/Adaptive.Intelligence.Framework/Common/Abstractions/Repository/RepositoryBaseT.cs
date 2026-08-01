using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Common.Abstractions.Repository;

/// <summary>
/// Provides a base definition for standard data access / repository methods and functions to
/// support data access operations.
/// </summary>
/// <param name="logger">
/// A reference to the <see cref="ILogger"/> instance used for logging.
/// </param>
/// <typeparam name="TIdType">
/// The data type of the identity column or field for the entity instances.
/// </typeparam>
/// <typeparam name="TEntityType">
/// The data type of the entity instances managed by the repository.
/// </typeparam>
public abstract class RepositoryBase<TIdType, TEntityType>(ILogger? logger) : 
    RepositoryBase(logger), IRepository<TIdType, TEntityType>
    where TEntityType : EntityBase<TIdType>
    where TIdType : struct
{
    #region Protected Abstract Methods		

    /// <summary>
    /// When overridden in a derived class, performs the create and insert operation on the data source.
    /// </summary>
    /// <param name="instance">
    /// The <typeparamref name="TEntityType"/> business object instance to be added.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    protected abstract bool PerformAdd(TEntityType instance);

    /// <summary>
    /// When overridden in a derived class, performs the create and insert operation on the data source.
    /// </summary>
    /// <param name="instance">
    /// The <typeparamref name="TEntityType"/> business object instance to be added.
    /// </param>
    /// <param name="token">
    /// The <see cref="CancellationToken"/> instance used to cancel the operation.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    protected abstract Task<bool> PerformAddAsync(TEntityType instance, CancellationToken token);

    /// <summary>
    /// When overridden in a derived class, performs the delete operation on the data source.
    /// </summary>
    /// <param name="instance">
    /// The <typeparamref name="TEntityType"/> business object instance to be deleted.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    protected abstract bool PerformDelete(TEntityType instance);

    /// <summary>
    /// When overridden in a derived class, performs the delete operation on the data source.
    /// </summary>
    /// <param name="instance">
    /// The <typeparamref name="TEntityType"/> business object instance to be deleted.
    /// </param>
    /// <param name="token">
    /// A <see cref="CancellationToken"/> instance.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    protected abstract Task<bool> PerformDeleteAsync(TEntityType instance, CancellationToken token);

    /// <summary>
    /// When overridden in a derived class, performs the operation to load a business object instance by ID value. 
    /// </summary>
    /// <param name="idType">
    /// The value that is the identity of the instance to be loaded.
    /// </param>
    /// <returns>
    /// An instance of <typeparamref name="TEntityType"/> if successful; otherwise, returns <b>null</b>.
    /// </returns>
    protected abstract TEntityType? PerformLoadById(TIdType idType);

    /// <summary>
    /// When overridden in a derived class, performs the operation to load a business object instance by ID value. 
    /// </summary>
    /// <param name="idType">
    /// The value that is the identity of the instance to be loaded.
    /// </param>
    /// <param name="token">
    /// The <see cref="CancellationToken"/> instance used to cancel the operation.
    /// </param>
    /// <returns>
    /// An instance of <typeparamref name="TEntityType"/> if successful; otherwise, returns <b>null</b>.
    /// </returns>
    protected abstract Task<TEntityType?> PerformLoadAsync(TIdType idType, CancellationToken token);

    /// <summary>
    /// When overridden in a derived class, performs the save/update operation on the data source.
    /// </summary>
    /// <param name="instance">
    /// The <typeparamref name="TEntityType"/> business object instance to be saved/updated.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    protected abstract bool PerformSave(TEntityType instance);

    /// <summary>
    /// When overridden in a derived class, performs the save/update operation on the data source.
    /// </summary>
    /// <param name="instance">
    /// The <typeparamref name="TEntityType"/> business object instance to be saved/updated.
    /// </param>
    /// <param name="token">
    /// The <see cref="CancellationToken"/> instance used to cancel the operation.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    protected abstract Task<bool> PerformSaveAsync(TEntityType instance, CancellationToken token);
    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Adds the specified business object definition to the data store.
    /// </summary>
    /// <param name="item">
    /// The <typeparamref name="TEntityType"/> instance to be added.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    public bool Add(TEntityType? item)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        bool success = false;
        if (item != null)
        {
            try
            {
                success = PerformAdd(item);
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
        }

        LastOperationSuccess = success;
        return success;
    }

    /// <summary>
    /// Adds the specified business object definition to the data store.
    /// </summary>
    /// <param name="item">
    /// The <typeparamref name="TEntityType"/> instance to be added.
    /// </param>
    /// <param name="token">
    /// The <see cref="CancellationToken"/> instance used to cancel the operation.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    public async Task<bool> AddAsync(TEntityType? item, CancellationToken token)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        bool success = false;
        if (item != null)
        {
            try
            {
                success = await PerformAddAsync(item, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
        }

        LastOperationSuccess = success;
        return success;
    }

    /// <summary>
    /// Deletes the specified business object definition from the data store.
    /// </summary>
    /// <param name="item">
    /// The <typeparamref name="TEntityType"/> instance to be deleted.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
    /// </returns>
    public bool Delete(TEntityType? item)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        bool success = false;
        try
        {
            if (item != null)
            {
                success = PerformDelete(item);
            }
        }
        catch (Exception ex)
        {
            RecordException(ex);
        }

        LastOperationSuccess = success;
        return success;
    }

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
    public virtual async Task<bool> DeleteAsync(TEntityType? item, CancellationToken token)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        bool success = false;
        if (item != null)
        {
            OnAsyncQueryStarted(nameof(DeleteAsync));
            try
            {

                success = await PerformDeleteAsync(item, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            OnAsyncQueryCompleted(nameof(DeleteAsync));
        }

        LastOperationSuccess = success;
        return success;
    }

    /// <summary>
    /// Gets the instance from data source.
    /// </summary>
    /// <param name="id">
    /// A value serving as the identity value used to load the instance.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TEntityType"/> instance, if successful; otherwise, returns <b>null</b>.
    /// </returns>
    public TEntityType? LoadItem(TIdType id)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        TEntityType? newItem;

        try
        {
            newItem = PerformLoadById(id);
            if (newItem != null)
            {
                LastOperationSuccess = true;
            }
        }
        catch (Exception ex)
        {
            RecordException(ex);
            newItem = null;
            LastOperationSuccess = false;
        }

        return newItem;
    }

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
    public async Task<TEntityType?> LoadItemAsync(TIdType id, CancellationToken token)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        TEntityType? newItem = default;
        OnAsyncQueryStarted(nameof(LoadItemAsync));
        try
        {
            newItem = await PerformLoadAsync(id, token).ConfigureAwait(false);
            if (newItem != null)
            {
                LastOperationSuccess = true;
            }
        }
        catch (Exception ex)
        {
            RecordException(ex);
            newItem = null;
            LastOperationSuccess = false;
        }
        OnAsyncQueryCompleted(nameof(LoadItemAsync));
        return newItem;
    }

    /// <summary>
    /// Saves the specified item to the data store.
    /// </summary>
    /// <param name="item">
    /// The business object instance to be saved.
    /// </param>
    /// <returns>
    /// <b>true</b> if the operation completes successfully; otherwise, returns <b>false</b>.
    /// </returns>
    public bool Save(TEntityType? item)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        bool success = false;
        if (item != null)
        {
            try
            {
                success = PerformSave(item);
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
        }

        LastOperationSuccess = success;
        return success;
    }
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
    public async Task<bool> SaveAsync(TEntityType? item, CancellationToken token)
    {
        LastOperationSuccess = false;
        LastOperationError = null;

        bool success = false;
        if (item != null)
        {
            OnAsyncQueryStarted(nameof(SaveAsync));
            try
            {
                success = await PerformSaveAsync(item, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            OnAsyncQueryCompleted(nameof(SaveAsync));
        }

        LastOperationSuccess = success;
        return success;
    }
    #endregion
}