namespace Adaptive.Intelligence.Common;

/// <summary>
/// Contains a simple container for a list of <see cref="Exception"/> instances.
/// </summary>
/// <seealso cref="List{T}" />
/// <seealso cref="Exception"/>
public class ExceptionCollection : List<Exception>, IDisposable, ICloneable
{
    #region Constructor / Dispose Methods		
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionCollection"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public ExceptionCollection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionCollection"/> class.
    /// </summary>
    /// <param name="capacity">
    /// An integer specifying the number of elements that the new list can initially store.
    /// </param>
    public ExceptionCollection(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionCollection"/> class.
    /// </summary>
    /// <param name="sourceList">
    /// An <see cref="IEnumerable{T}"/> list of <see cref="Exception"/> to add to this collection instance.
    /// </param>
    public ExceptionCollection(IEnumerable<Exception> sourceList)
    {
        AddRange(sourceList);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        Clear();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

    #region Public Methods / Functions		
    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new <see cref="ExceptionCollection"/> instance that contains the same <see cref="Exception"/>
    /// instances as the current instance.
    /// </returns>
    public ExceptionCollection Clone()
    {
        return new ExceptionCollection(this);
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    object ICloneable.Clone()
    {
        return Clone();
    }
    #endregion
}

