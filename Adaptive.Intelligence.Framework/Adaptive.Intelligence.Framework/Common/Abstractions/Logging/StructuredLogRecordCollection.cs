namespace Adaptive.Intelligence.Common.Abstractions.Logging
{
    /// <summary>
    /// Provides a method for storing, reading, and managing a collection of structured log records in a logging system.
    /// </summary>
    public class StructuredLogRecordCollection : AdaptiveCollectionBase<StructuredLogRecordBase>
    {
        #region Private Member Declarations
        /// <summary>
        /// The synchronization root object used to synchronize access to the collection.
        /// </summary>
        private readonly Lock _syncRoot = new();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredLogRecordCollection"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public StructuredLogRecordCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredLogRecordCollection"/> class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the collection can contain.</param>
        public StructuredLogRecordCollection(int capacity) : base(capacity)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredLogRecordCollection"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new collection.</param>
        public StructuredLogRecordCollection(IEnumerable<StructuredLogRecordBase> collection) : base(collection)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the structured log record at the specified index in a thread-safe manner.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the structured log record to get or set.
        /// </param>
        /// <returns>
        /// The <see cref="StructuredLogRecordBase"/> at the specified index, or <b>null</b> if the index is out of range.
        /// </returns>
        public new StructuredLogRecordBase? this[int index]
        {
            get
            {
                lock (_syncRoot)
                {
                    return base[index];
                }
            }
            set
            {
                if (value != null)
                {
                    lock (_syncRoot)
                    {
                        base[index] = value;
                    }
                }
            }
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Adds a structured log record to the collection in a thread-safe manner.
        /// </summary>
        /// <param name="instance">
        /// The structured log record to add to the collection.
        /// </param>
        public override void Add(StructuredLogRecordBase instance)
        {
            lock(_syncRoot)
            {
                base.Add(instance);
            }
        }

        /// <summary>
        /// Adds a range of structured log records to the collection in a thread-safe manner.
        /// </summary>
        /// <param name="collection">
        /// The collection of structured log records to add to the collection.
        /// </param>
        public override void AddRange(IEnumerable<StructuredLogRecordBase> collection)
        {
            lock (_syncRoot)
            {
                base.AddRange(collection);
            }
        }

        /// <summary>
        /// Removes all structured log records from the collection in a thread-safe manner.
        /// </summary>
        public override void Clear()
        {
            lock (_syncRoot)
            {
                base.Clear();
            }
        }

        /// <summary>
        /// Removes a structured log record from the collection in a thread-safe manner.
        /// </summary>
        /// <param name="instance">
        /// The structured log record to remove from the collection.
        /// </param>
        public override void Remove(StructuredLogRecordBase instance)
        {
            lock (_syncRoot)
            {
                base.Remove(instance);
            }
        }

        /// <summary>
        /// Removes all structured log records that match the conditions defined by the specified predicate from the collection in a thread-safe manner.
        /// </summary>
        /// <param name="matchingFunction">
        /// A <see cref="Predicate{T}"/> delegate that defines the conditions of the structured log records to remove.
        /// </param>
        public override void RemoveAll(Predicate<StructuredLogRecordBase> matchingFunction)
        {
            lock (_syncRoot)
            {
                base.RemoveAll(matchingFunction);
            }
        }

        /// <summary>
        /// Removes the structured log record at the specified index from the collection in a thread-safe manner.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the structured log record to remove.
        /// </param>
        public override void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                base.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes a range of structured log records from the collection in a thread-safe manner.
        /// </summary>
        /// <param name="index">
        /// The zero-based starting index of the range of structured log records to remove.
        /// </param>
        /// <param name="count">
        /// The number of items to remove.
        /// </param>
        public override void RemoveRange(int index, int count)
        {
            lock (_syncRoot)
            {
                base.RemoveRange(index, count);
            }
        }

        /// <summary>
        /// Reverses the order of the structured log records in the collection in a thread-safe manner.
        /// </summary>
        public override void Reverse()
        {
            lock (_syncRoot)
            {
                base.Reverse();
            }
        }
        #endregion
    }
}
