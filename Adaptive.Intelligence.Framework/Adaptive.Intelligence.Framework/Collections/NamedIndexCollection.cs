using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Collections
{
    /// <summary>
    /// Provides a base class definition for storing elements in a list that can be referenced by name.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the items being stored in the list.
    /// </typeparam>
    public abstract class NamedIndexCollection<T> : AdaptiveCollectionBase<T> where T : class
    {
        #region Private Member Declarations
        /// <summary>
        /// Storage by name dictionary.
        /// </summary>
        private readonly Dictionary<string, T> _items;
        #endregion

        #region Constructor / Destructor Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedIndexCollection{T}"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        protected NamedIndexCollection()
        {
            _items = [];
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedIndexCollection{T}"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        protected NamedIndexCollection(int capacity)
            : base(capacity)
        {
            _items = new Dictionary<string, T>(capacity);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the <typeparamref name="T"/> with the specified name.
        /// </summary>
        /// <remarks>
        /// Setting this property will replace the original <typeparamref name="T"/> instance with the 
        /// new one.  If the instance with the specified name does not exist, nothing happens.
        /// </remarks>
        /// <value>
        /// The <typeparamref name="T"/> instance in the collection with the specified key value.
        /// </value>
        /// <param name="name">
        /// The name value used as the key to locate the instance.
        /// </param>
        public T? this[string name]
        {
            get => !string.IsNullOrEmpty(name) && _items.TryGetValue(name, out T? returnValue) ? returnValue : null;
            set
            {
                if (value != null && !string.IsNullOrEmpty(name))
                {
                    if (_items.ContainsKey(name))
                    {
                        _items[name] = value;
                    }
                }
            }
        }
        #endregion

        #region Protected Abstract Methods / Functions
        /// <summary>
        /// Gets the name / key value of the specified instance.
        /// </summary>
        /// <remarks>
        /// This is called from several methods, including the Add() method, to identify the instance
        /// being added.
        /// </remarks>
        /// <param name="item">
        /// The <typeparamref name="T"/> item to be stored in the collection.
        /// </param>
        /// <returns>
        /// The name / key value of the specified instance.
        /// </returns>
        protected abstract string GetName(T item);
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Adds an object to the end of the <see cref="NamedIndexCollection{T}" />.
        /// </summary>
        /// <param name="item">
        /// The object to be added to the end of the <see cref="NamedIndexCollection{T}" />.
        /// </param>
        public new void Add(T item)
        {
            string name = GetName(item);
            if (!string.IsNullOrEmpty(name) && !_items.ContainsKey(name))
            {
                _items.Add(name, item);
                base.Add(item);
            }
        }
        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="NamedIndexCollection{T}"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection whose elements should be added to the end of the <see cref="NamedIndexCollection{T}"/>.
        /// </param>
        public new void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                Add(item);
            }
        }
        /// <summary>
        /// Determines whether the collection contains an instance that is identified with the specified key value.
        /// </summary>
        /// <param name="name">
        /// The name / key value used to identify the instance.
        /// </param>
        /// <returns>
        /// <b>true</b> if an instance with the specified key is present in the collection; otherwise, returns <b>false</b>.
        /// </returns>
        public bool Contains(string name)
        {
            return !string.IsNullOrEmpty(name) && _items.ContainsKey(name);
        }
        /// <summary>
        /// Removes all elements from the <see cref="NamedIndexCollection{T}" />.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            _items.Clear();
        }
        /// <summary>
        /// Inserts an element into the <see cref="NamedIndexCollection{T}" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        public new void Insert(int index, T item)
        {
            string name = GetName(item);
            if (!string.IsNullOrEmpty(name) && !_items.ContainsKey(name))
            {
                base.Insert(index, item);
                _items.Add(name, item);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific instance from the collection.
        /// </summary>
        /// <param name="item">
        /// A reference to the instance to be removed.
        /// </param>
        public new void Remove(T? item)
        {
            if (item != null)
            {
                string name = GetName(item);
                Remove(name);
            }
        }
        /// <summary>
        /// Removes the first occurrence of a specific instance using the specified name/key value from the collection.
        /// </summary>
        /// <param name="name">
        /// The name / key value used to identify the instance.
        /// </param>
        public void Remove(string name)
        {
            if (!string.IsNullOrEmpty(name) && _items.TryGetValue(name, out T? value))
            {
                _ = _items.Remove(name);
                base.Remove(value);
            }
        }
        /// <summary>
        /// Removes the element at the specified index of the <see cref="NamedIndexCollection{T}" />.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public new void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                T item = this[index];
                string name = GetName(item);
                Remove(name);
            }
        }
        #endregion
    }
}