namespace Adaptive.Intelligence.Common.Abstractions
{
    /// <summary>
    /// Provides a base implementation for creating collections of data entity instances.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the entity instance.
    /// </typeparam>
    /// <typeparam name="TIdType">
    /// The data type of the ID value of the entity instance.
    /// </typeparam>
    public abstract class EntityCollectionBase<T, TIdType> : AdaptiveCollectionBase<T> 
        where T : EntityBase<TIdType>
        where TIdType : struct
    {
        #region Constructor / Destructor Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollectionBase{T, TIdType}"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        protected EntityCollectionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollectionBase{T, TIdType}"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The number of elements that the new list can initially store.
        /// </param>
        protected EntityCollectionBase(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollectionBase{T, TIdType}"/> class.
        /// </summary>
        /// <param name="sourceList">
        /// An <see cref="IEnumerable{T}"/> instance containing the objects used to
        /// populate the collection.
        /// </param>
        protected EntityCollectionBase(IEnumerable<T>? sourceList)
        {
            if (sourceList != null)
            {
                AddRange(sourceList);
            }
        }
        #endregion

    }
}
