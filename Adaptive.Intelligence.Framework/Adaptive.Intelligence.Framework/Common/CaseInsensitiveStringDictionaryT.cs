using Adaptive.Intelligence.Abstractions;
using System.Collections;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Provides a Dictionary implementation using a string as the key value where the
    /// key value is case insensitive.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the values being stored in the dictionary.
    /// </typeparam>
    public class CaseInsensitiveStringDictionary<T> : DisposableObjectBase, IDictionary<string, T>
    {
        #region Private Member Declarations
        /// <summary>
        /// The main dictionary container.
        /// </summary>
        private Dictionary<string, T>? _container;
        /// <summary>
        /// The original key list.
        /// </summary>
        private List<string>? _originalKeys;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseInsensitiveStringDictionary{T}"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public CaseInsensitiveStringDictionary()
        {
            _container = [];
            _originalKeys = [];
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _originalKeys?.Clear();
                _container?.Clear();
            }

            _originalKeys = null;
            _container = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the number of elements contained in the <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </returns>
        public int Count => _container == null ? 0 : _container.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="CaseInsensitiveStringDictionary{T}"/> is read-only.
        /// </summary>
        /// <returns>
        /// Always <see langword="false" />.
        /// </returns>
        public bool IsReadOnly => false;
        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">
        /// A string containing the key of the element to get or set.
        /// </param>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        public T this[string key]
        {
            get
            {
                T? itemValue = default;
                if (_container != null && !string.IsNullOrEmpty(key))
                {
                    string actualKey = key.ToLowerInvariant();
                    _container.TryGetValue(actualKey, out itemValue);
                }

                return itemValue!;
            }
            set
            {
                if (_container != null && !string.IsNullOrEmpty(key))
                {
                    string actualKey = key.ToLowerInvariant();
                    if (_container.ContainsKey(actualKey))
                    {
                        _container[actualKey] = value;
                    }
                    else
                    {
                        _originalKeys?.Add(key);
                        _container.TryAdd(actualKey, value);
                    }
                }
            }
        }
        /// <summary>
        /// Gets an <see cref="ICollection{K}"/> containing the keys of the <see cref="CaseInsensitiveStringDictionary{T}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{K}"/> containing the keys of the object that implements
        /// <see cref="CaseInsensitiveStringDictionary{T}" />.
        /// </returns>
        public ICollection<string> Keys => _originalKeys == null ? [] : (ICollection<string>)_originalKeys;

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the values in the <see cref="CaseInsensitiveStringDictionary{T}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{T}"/> containing the values in the object that implements <see cref="CaseInsensitiveStringDictionary{T}" />.
        /// </returns>
        public ICollection<T> Values => _container == null ? [] : _container.Values;

        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="CaseInsensitiveStringDictionary{T}" />.
        /// </summary>
        /// <param name="key">
        /// A string containing the key value of the element to add.
        /// </param>
        /// <param name="value">
        /// The object to use as the value of the element to add.
        /// </param>
        public void Add(string key, T value)
        {
            if (!string.IsNullOrEmpty(key) && _originalKeys != null && _container != null)
            {
                _originalKeys.Add(key);
                string storageKey = key.ToLowerInvariant();
                _container.Add(storageKey, value);
            }
        }
        /// <summary>
        /// Adds an item to the <see cref="CaseInsensitiveStringDictionary{T}" />.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="CaseInsensitiveStringDictionary{T}"/>
        /// </param>
        public void Add(KeyValuePair<string, T> item)
        {
            if (!string.IsNullOrEmpty(item.Key) && _originalKeys != null && _container != null)
            {
                string key = item.Key.ToLowerInvariant();
                _container.Add(key, item.Value);
                _originalKeys.Add(item.Key);
            }
        }
        /// <summary>
        /// Removes all items from the <see cref="CaseInsensitiveStringDictionary{T}" />.
        /// </summary>
        public void Clear()
        {
            _container?.Clear();
            _originalKeys?.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="CaseInsensitiveStringDictionary{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="CaseInsensitiveStringDictionary{T}"/>; otherwise, <see langword="false" />.
        /// </returns>
        public bool Contains(KeyValuePair<string, T> item)
        {
            bool containsKey = false;
            if (!string.IsNullOrEmpty(item.Key) && _originalKeys != null && _container != null)
            {
                string key = item.Key.ToLowerInvariant();
                containsKey = _container.ContainsKey(key);
            }

            return containsKey;
        }
        /// <summary>
        /// Determines whether the <see cref="CaseInsensitiveStringDictionary{T}"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key">
        /// A string containing the key to locate in the <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the <see cref="CaseInsensitiveStringDictionary{T}"/> contains an element with the key; otherwise, <see langword="false" />.
        /// </returns>
        public bool ContainsKey(string key)
        {
            bool containsKey = false;
            if (!string.IsNullOrEmpty(key) && _container != null)
            {
                string compareKey = key.ToLowerInvariant();
                containsKey = _container.ContainsKey(compareKey);
            }

            return containsKey;
        }
        /// <summary>
        /// Copies the elements of the <see cref="CaseInsensitiveStringDictionary{T}"/> to an
        /// <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array" /> that is the destination of the elements copied from
        /// <see cref="CaseInsensitiveStringDictionary{T}"/>. The <see cref="System.Array" /> must have zero-based
        /// indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// An integer specifying the zero-based index in <paramref name="array" /> at which copying begins.
        /// </param>
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            if (_container != null)
            {
                ((IDictionary<string, T>)_container).CopyTo(array, arrayIndex);
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _container == null ? new EmptyEnumerator<KeyValuePair<string, T>>() : _container.GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the
        /// <see cref="CaseInsensitiveStringDictionary{T}"/>; otherwise, <see langword="false" />.
        /// This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the
        /// original <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </returns>
        public bool Remove(KeyValuePair<string, T> item)
        {
            return Remove(item.Key);
        }
        /// <summary>
        /// Removes the element with the specified key from the <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </summary>
        /// <param name="key">
        /// A string containing the key of the element to remove.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.
        /// This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the
        /// original <see cref="CaseInsensitiveStringDictionary{T}"/>.
        /// </returns>
        public bool Remove(string key)
        {
            bool success = false;

            if (!string.IsNullOrEmpty(key) && _originalKeys != null && _container != null)
            {
                string compareKey = key.ToLowerInvariant();
                _originalKeys.Remove(key);
                _originalKeys.Remove(compareKey);
                if (_container.ContainsKey(compareKey))
                {
                    success = _container.Remove(compareKey);
                }
            }

            return success;
        }
        /// <summary>
        /// Gets the value associated with the specified key.</summary>
        /// <param name="key">
        /// A string specifying the key whose value is to be retrieved.
        /// </param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the <see cref="CaseInsensitiveStringDictionary{T}"/> contains an element with
        /// the specified key; otherwise, <see langword="false" />.
        /// </returns>
        public bool TryGetValue(string key, out T value)
        {
            bool success = false;

            value = default!;
            if (!string.IsNullOrEmpty(key) && _container != null)
            {
                string compareKey = key.ToLowerInvariant();
                success = _container.TryGetValue(compareKey, out value!);
            }

            return success;
        }
        #endregion
    }
}