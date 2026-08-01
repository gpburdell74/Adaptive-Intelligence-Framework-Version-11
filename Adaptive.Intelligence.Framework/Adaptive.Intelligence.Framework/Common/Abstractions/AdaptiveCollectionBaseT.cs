using System.Collections.Specialized;

namespace Adaptive.Intelligence.Common.Abstractions;

/// <summary>
/// Provides the base implementation for a collection that contains a list of business objects.
/// </summary>
/// <typeparam name="T">
/// The data type being stored in the collection instance.
/// </typeparam>
/// <seealso cref="List{T}"/>
public class AdaptiveCollectionBase<T> : List<T>, INotifyCollectionChanged
{
    #region Public Events
    /// <summary>
    /// Occurs when the collection changes.
    /// </summary>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    #endregion

    #region Constructor / Destructor Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="AdaptiveCollectionBase{T}"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    protected AdaptiveCollectionBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdaptiveCollectionBase{T}"/> class.
    /// </summary>
    /// <param name="capacity">
    /// The number of elements that the new list can initially store.
    /// </param>
    protected AdaptiveCollectionBase(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdaptiveCollectionBase{T}"/> class.
    /// </summary>
    /// <param name="sourceList">
    /// An <see cref="IEnumerable{T}"/> instance containing the objects used to
    /// populate the collection.
    /// </param>
    protected AdaptiveCollectionBase(IEnumerable<T>? sourceList)
    {
        if (sourceList != null)
        {
            AddRange(sourceList);
        }
    }
    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Adds an object to the end of the <see cref="AdaptiveCollectionBase{T}"/> collection.
    /// </summary>
    /// <param name="instance">
    /// The instance to add to the collection.
    /// </param>
    public virtual new void Add(T instance)
    {
        base.Add(instance);
        OnCollectionChanged(NotifyCollectionChangedAction.Add);
    }

    /// <summary>
    /// Adds the elements of the specified collection to the end of the <see cref="AdaptiveCollectionBase{T}"/> collection.
    /// </summary>
    /// <param name="collection">
    /// The <see cref="IEnumerable{T}"/> instance containing the list of items to be added.
    /// </param>
    public virtual new void AddRange(IEnumerable<T> collection)
    {
        base.AddRange(collection);
        OnCollectionChanged(NotifyCollectionChangedAction.Add);
    }

    /// <summary>
    /// Clears the content of the collection.
    /// </summary>
    public virtual new void Clear()
    {
        base.Clear();
        OnCollectionChanged(NotifyCollectionChangedAction.Reset);
    }

    /// <summary>
    /// Removes the object at the specified index from the <see cref="AdaptiveCollectionBase{T}"/> collection.
    /// </summary>
    /// <param name="index">
    /// An integer specifying the ordinal index.
    /// </param>
    public virtual new void RemoveAt(int index)
    {
        if (index >= 0 && index < Count)
        {
            base.RemoveAt(index);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove);
        }
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="AdaptiveCollectionBase{T}"/> collection.
    /// </summary>
    /// <param name="instance">
    /// The reference to the instance of <typeparamref name="T"/> to be removed.
    /// </param>
    public virtual new void Remove(T instance)
    {
        if (instance != null && Contains(instance))
        {
            base.Remove(instance);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove);
        }
    }

    /// <summary>
    /// Removes all the elements that match the conditions defined by the specified predicate from the <see cref="AdaptiveCollectionBase{T}"/> collection.
    /// </summary>
    /// <param name="matchingFunction">
    /// The predicate delegate that defines the conditions of the elements to remove.
    /// </param>
    public virtual new void RemoveAll(Predicate<T> matchingFunction)
    {
        base.RemoveAll(matchingFunction);
        OnCollectionChanged(NotifyCollectionChangedAction.Remove);  
    }
    /// <summary>
    /// Removes a range of elements from the <see cref="AdaptiveCollectionBase{T}"/> collection.
    /// </summary>
    /// <param name="index">
    /// The zero-based starting index of the range of elements to remove.
    /// </param>
    /// <param name="count">
    /// The number of elements to remove.
    /// </param>
    public virtual new void RemoveRange(int index, int count)
    {
        base.RemoveRange(index, count);
        OnCollectionChanged(NotifyCollectionChangedAction.Remove);
    }

    /// <summary>
    /// Reverses the order of the elements in the entire <see cref="AdaptiveCollectionBase{T}"/> collection.
    /// </summary>
    public virtual new void Reverse()
    {
        base.Reverse();
        OnCollectionChanged(NotifyCollectionChangedAction.Move);
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="AdaptiveCollectionBase{T}"/> collection using the default comparer.
    /// </summary>
    public virtual new void Sort()
    {
        base.Sort();
        OnCollectionChanged(NotifyCollectionChangedAction.Move);
    }

    /// <summary>
    /// Sorts the elements in the entire <see cref="AdaptiveCollectionBase{T}"/> collection using the specified comparison function.
    /// </summary>
    /// <param name="comparisonFunction">
    /// The comparison function to use for sorting.
    /// </param>
    public virtual new void Sort(Comparison<T> comparisonFunction)
    {
        base.Sort(comparisonFunction);
        OnCollectionChanged(NotifyCollectionChangedAction.Move);
    }

    /// <summary>
    /// Sorts the elements in a range of elements in the <see cref="AdaptiveCollectionBase{T}"/> collection using the specified comparer.
    /// </summary>
    /// <param name="index">
    /// The zero-based starting index of the range of elements to sort.
    /// </param>
    /// <param name="count">
    /// The number of elements to sort.
    /// </param>
    /// <param name="comparer">
    /// The comparer to use for sorting.
    /// </param>
    public virtual new void Sort(int index, int count, IComparer<T> comparer)
    {
        base.Sort(index, count, comparer);
        OnCollectionChanged(NotifyCollectionChangedAction.Move);
    }

    #endregion

    #region Event Methods
    /// <summary>
    /// Raises the <see cref="CollectionChanged"/> event.
    /// </summary>
    /// <param name="action">
    /// A <see cref="NotifyCollectionChangedAction"/> enumerated value indicating how the collection has changed.
    /// </param>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action)
    {
        if (action == NotifyCollectionChangedAction.Reset)
        {
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(action));
        }
        else
        {
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(action, this));
        }
    }
    #endregion

}