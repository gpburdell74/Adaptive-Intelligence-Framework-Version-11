using Adaptive.Intelligence.Csv.Attributes;
using System.Reflection;

namespace Adaptive.Intelligence.Csv.Reflection;

/// <summary>
/// Provides a thread-safe mechanism for caching the reflection metadata for data types.
/// </summary>
/// <remarks>
/// The purpose is to avoid having to call the Reflection methods and functions multiple types
/// for the same data types.
/// </remarks>
public static class UserTypeCache
{
    #region Private Member Declarations

    /// <summary>
    /// The lock for threading synchronization.
    /// </summary>
    private static readonly Lock _lock = new();

    /// <summary>
    /// The type cache container.
    /// </summary>
    private static readonly Dictionary<Type, UserTypeCacheInstance> _typeCache = [];
    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Clears the content of the cache.
    /// </summary>
    public static void ClearCache()
    {
        lock (_lock)
        {
            if (_typeCache != null)
            {
                // Dispose of each instance.
                foreach (UserTypeCacheInstance item in _typeCache.Values)
                {
                    item.Dispose();
                }
                // Clear the list.
                _typeCache.Clear();
            }
        }
    }

    /// <summary>
    /// Creates the a cache instance object for the specified data type.
    /// </summary>
    /// <typeparam name="T">
    /// The data type whose properties' metadata are to be cached.
    /// </typeparam>
    /// <param name="placenNonIndexedFieldsLast">
    /// <b>true</b> to place the properties with no <see cref="IndexAttribute"/> decorations at the end
    /// of the sorted list, otherwise <b>false</b> to place them at the start of the list.
    /// </param>
    /// <param name="flags">
    /// An optional parameter to specify the <see cref="BindingFlags"/> value used to extract the property list.
    /// </param>
    /// <returns>
    /// A <see cref="UserTypeCacheInstance{T}"/> containing the sorted property metadata for the data type.
    /// </returns>
    public static UserTypeCacheInstance<T> CreateCacheInstance<T>(
        bool placenNonIndexedFieldsLast = true,
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
    {
        return new UserTypeCacheInstance<T>(placenNonIndexedFieldsLast, flags);
    }

    /// <summary>
    /// Attempts to find an object in the cache for the specified data type.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the instance to query for.
    /// </typeparam>
    /// <returns>
    /// The reference to the <see cref="UserTypeCacheInstance{T}"/> if found;
    /// otherwise, returns <b>null</b>.
    /// </returns>
    public static UserTypeCacheInstance<T>? GetByType<T>()
    {
        UserTypeCacheInstance<T>? cachedRecord = null;

        if (_typeCache != null)
        {
            Type type = typeof(T);
            UserTypeCacheInstance? cacheInstance;
            lock (_lock)
            {
                _typeCache.TryGetValue(type, out cacheInstance);
            }
            if (cacheInstance != null)
            {
                cachedRecord = (UserTypeCacheInstance<T>)cacheInstance;
            }
        }
        return cachedRecord;
    }

    /// <summary>
    /// Gets the reference to the cached information instance if present; otherwise, creates a new 
    /// instance, adds it to the cache, and returns it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    /// The <see cref="UserTypeCacheInstance{T}"/> for the specified data type of <typeparamref name="T"/> 
    /// if successful; otherwise, returns <b>null</b>.
    /// </returns>
    public static UserTypeCacheInstance<T>? GetOrCreate<T>()
    {
        UserTypeCacheInstance<T>? cachedRecord = null;

        if (_typeCache != null)
        {
            // Look for it in the cache...
            cachedRecord = GetByType<T>();
            if (cachedRecord == null)
            {
                // If not there, create it and add it.
                cachedRecord = CreateCacheInstance<T>();
                lock (_lock)
                {
                    _typeCache[typeof(T)] = cachedRecord;
                }
            }
        }
        return cachedRecord;
    }
    #endregion
}
