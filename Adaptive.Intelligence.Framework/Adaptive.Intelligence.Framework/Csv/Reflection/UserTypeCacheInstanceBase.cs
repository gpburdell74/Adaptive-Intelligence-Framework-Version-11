using Adaptive.Intelligence.Abstractions;
using System.Reflection;

namespace Adaptive.Intelligence.Csv.Reflection;

/// <summary>
/// Provides a base definition for a user-type cache instance.
/// </summary>
/// <remarks>
/// The purpose of this class is to provide a base definition to each insance of this type can be
/// referenced regardless of the generic type parameter.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="UserTypeCacheInstance{T}"/> class.
/// </remarks>
/// <param name="nonIndexedFieldsLast">
/// An optional parameter indicating how to place the non-indexed property list when creating
/// the ordered list of properties.  If <b>true</b>, these properties are apppended to the 
/// end of the list; otherwise, they are pre-pended to the start of the list.
/// </param>
/// <param name="flags">
/// An opertional parameter of <see cref="BindingFlags"/> that determines which properties are 
/// read from the specified data source.
/// </param>
public abstract class UserTypeCacheInstance(bool nonIndexedFieldsLast = true, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance) : DisposableObjectBase
{
    #region Private Member Declarations
    /// <summary>
    /// A flag indicating whether to add the non-indexed list of properties at the end of the list
    /// when ordered, or not.
    /// </summary>
    private bool _nonIndexedFieldsLast = nonIndexedFieldsLast;

    /// <summary>
    /// The binding flags that are used to query the data type for hte list of properties.
    /// </summary>
    private BindingFlags _flags = flags;

    #endregion
    #region Constructor / Dispose Methods
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets the binding flags used to query for the list of properties on the specified data type.
    /// </summary>
    /// <value>
    /// A <see cref="BindingFlags"/> enumerated value.  The default is <see cref="BindingFlags.Public"/> and
    /// <see cref="BindingFlags.Instance"/>.
    /// 
    /// </value>
    public BindingFlags Flags { get => _flags; set => _flags = value; }

    /// <summary>
    /// Gets or sets a value indicating whether to place the non-indexed properties at the
    /// end of the ordered list.
    /// </summary>
    /// <value>
    ///   <c>true</c> if to place the non-indexed properties at the end when sorting; otherwise, <c>false</c>
    ///   to place them at the start when sorting.
    /// </value>
    public bool NonIndexedFieldsLast { get => _nonIndexedFieldsLast; set => _nonIndexedFieldsLast = value; }
    #endregion
}