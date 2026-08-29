using Adaptive.Intelligence.Csv.Attributes;
using Adaptive.Intelligence.Csv.Metadata;
using System.Reflection;

namespace Adaptive.Intelligence.Csv.Reflection;

/// <summary>
/// Contains the metadata for a specified user/generic data type in order to speed up
/// Reflection operations.
/// </summary>
/// <typeparam name="T">
/// The original data type whose metadata is being cached.
/// </typeparam>
public sealed class UserTypeCacheInstance<T> : UserTypeCacheInstance
{
    #region Private Member Declarations
    /// <summary>
    /// The data type reference.
    /// </summary>
    private Type? _userType;

    /// <summary>
    /// The original list of property information objects from the original data type.
    /// </summary>
    private List<PropertyInfo>? _originalPropertyList;

    /// <summary>
    /// The list of properties in order.
    /// </summary>
    private List<PropertyInfo>? _orderedList;

    /// <summary>
    /// The list of properties that have <see cref="IndexAttribute"/> decorations.
    /// </summary>
    private List<PropertyInfo>? _indexedProperties;

    /// <summary>
    /// The list of properties that do not have <see cref="IndexAttribute"/> decorations.
    /// </summary>
    private List<PropertyInfo>? _nonIndexedProperties;
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="UserTypeCacheInstance{T}"/> class.
    /// </summary>
    /// <param name="nonIndexedFieldsLast">
    /// An optional parameter indicating how to place the non-indexed property list when creating
    /// the ordered list of properties.  If <b>true</b>, these properties are apppended to the 
    /// end of the list; otherwise, they are pre-pended to the start of the list.
    /// </param>
    /// <param name="flags">
    /// An opertional parameter of <see cref="BindingFlags"/> that determines which properties are 
    /// read from the specified data source.
    /// </param>
    public UserTypeCacheInstance(bool nonIndexedFieldsLast = true, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        : base(nonIndexedFieldsLast, flags)
    {
        // Store the data type.
        _userType = typeof(T);

        // Get the list of properties.
        ReadOriginalPropertyList();

        // Sort and store the property data accordingly.
        IndexProperties();
    }
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
    /// <b>false</b> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            _originalPropertyList?.Clear();
            _orderedList?.Clear();
            _indexedProperties?.Clear();
            _nonIndexedProperties?.Clear();
        }

        _userType = null;
        _originalPropertyList = null;
        _orderedList = null;
        _indexedProperties = null;
        _nonIndexedProperties = null;

        base.Dispose(disposing);
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets the reference to the list of property meta data objects that have been decorated
    /// with <see cref="IndexAttribute"/>s.
    /// </summary>
    /// <value>
    /// A <see cref="List{T}"/> of <see cref="PropertyInfo"/> instances, or <b>null</b>.
    /// </value>
    public List<PropertyInfo>? IndexedProperties => _indexedProperties;

    /// <summary>
    /// Gets the reference to the list of property meta data objects that have been NOT decorated
    /// with <see cref="IndexAttribute"/>s.
    /// </summary>
    /// <value>
    /// A <see cref="List{T}"/> of <see cref="PropertyInfo"/> instances, or <b>null</b>.
    /// </value>
    public List<PropertyInfo>? NonIndexedProperties => _nonIndexedProperties;

    /// <summary>
    /// Gets the reference to the list of properties after the sorting operation was performed.
    /// </summary>
    /// <value>
    /// A <see cref="List{T}"/> of <see cref="PropertyInfo"/> instances sorted by index, or <b>null</b>.
    /// The location of non-indexed properties is determined by the value of <see cref="UserTypeCacheInstance.NonIndexedFieldsLast"/>.
    /// </value>
    public List<PropertyInfo>? OrderedProperties => _orderedList;

    /// <summary>
    /// Gets the reference to the original list of property meta data objects read from the provided data type.
    /// </summary>
    /// <value>
    /// A <see cref="List{T}"/> of <see cref="PropertyInfo"/> instances, or <b>null</b>.
    /// </value>
    public List<PropertyInfo>? OriginalProperties => _originalPropertyList;
    #endregion

    #region Public Methods / Functions    
    /// <summary>
    /// Updates the list of <see cref="CsvColumnInfo"/> instances with the latest information
    /// from the sorted list of properties.
    /// </summary>
    /// <param name="columnsList">
    /// The <see cref="List{T}"/> of <see cref="CsvColumnInfo"/> instances to be updated.
    /// </param>
    public void UpdateCsvColumnInfo(List<CsvColumnInfo> columnsList)
    {
        if (_orderedList != null)
        {
            foreach (CsvColumnInfo columnInfo in columnsList)
            {
                // Find the property metadata for the column with a matching name value, or
                // a matching header text value.
                PropertyInfo? propInfo = FindPropertyByName(columnInfo.ColumnName, columnInfo.HeaderText);

                // If the property is not found, update the info object to show a missing property.
                if (propInfo == null)
                {
                    columnInfo.PropertyMissing = true;
                    columnInfo.PropertyData = null;
                }
                else
                {
                    // Update the column information.  Because of sorting operations and possible duplicate 
                    // indices, the actual position of the object may not match the index value of the 
                    // attribute on the column.
                    columnInfo.Index = _orderedList.IndexOf(propInfo);

                    columnInfo.ColumnName = propInfo.Name;
                    columnInfo.PropertyData = propInfo;
                    columnInfo.PropertyMissing = false;
                }
            }
        }
    }
    #endregion

    #region Private Methods / Functions    
    /// <summary>
    /// Attempts to find the metadata for the property with the specified name or header text
    /// from the ordered list.
    /// </summary>
    /// <param name="columnName">
    /// A string containing the name of the column/property to find.
    /// </param>
    /// <param name="headerText">
    /// A string containing the CSV column header, if present.
    /// </param>
    /// <returns>
    /// The <see cref="PropertyInfo"/> instance if found; otherwise, returns <b>null</b>.
    /// </returns>
    private PropertyInfo? FindPropertyByName(string? columnName, string? headerText)
    {
        PropertyInfo? propInfo = null;

        if (_orderedList != null && columnName != null)
        {
            propInfo = _orderedList.FirstOrDefault(
                property => property.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

            // Search by header text if not found.
            if (propInfo == null && headerText != null)
            {
                propInfo = _orderedList.FirstOrDefault(
                    property => property.Name.Equals(headerText, StringComparison.OrdinalIgnoreCase));
            }
        }

        return propInfo;
    }

    /// <summary>
    /// Sorts the list of property information objects from the provided data type by 
    /// the values of thier <see cref="IndexAttribute.Index"/> values, and places the non-indexed
    /// fields in the appropriate location, based on the value of <see cref="UserTypeCacheInstance.NonIndexedFieldsLast"/>.
    /// </summary>
    private void IndexProperties()
    {
        if (_originalPropertyList != null)
        {
            // Read the list of properties from the original data type and separate the list into
            // ones that have been decorated, with the <see cref="IndexAttribute"/>, and ones that 
            // have not.
            ReadIndexedProperties();
            ReadNonIndexedProperties();

            _orderedList?.Clear();
            _orderedList ??= new List<PropertyInfo>(_originalPropertyList.Count);

            // If the <see cref="NonIndexedFieldsLast"/> property is false,
            // add the list of non-indexed fields first.
            if (!NonIndexedFieldsLast)
            {
                AddNonIndexedProperties(_orderedList, _nonIndexedProperties);
            }

            // Sort the indexed properties by thier <see cref="IndexAttribute.Index"/> values, 
            // and add them to the ordered list.
            IOrderedEnumerable<PropertyInfo>? sortedList = SortIndexedProperties();
            if (sortedList != null)
            {
                _orderedList.AddRange(sortedList);
            }

            // If the <see cref="NonIndexedFieldsLast"/> property is true,
            // add the list of non-indexed fields first.
            if (NonIndexedFieldsLast)
            {
                AddNonIndexedProperties(_orderedList, _nonIndexedProperties);
            }
        }
    }

    /// <summary>
    /// Adds the non indexed properties to the list
    /// </summary>
    /// <param name="targetList">
    /// The <see cref="List{T}"/> of <see cref="PropertyInfo"/> to add items to.
    /// </param>
    /// <param name="sourceList">
    /// The source list <see cref="List{T}"/> of <see cref="PropertyInfo"/> instances to be copied
    /// after being sorted by name.
    /// </param>
    private static void AddNonIndexedProperties(List<PropertyInfo>? targetList, List<PropertyInfo>? sourceList)
    {
        if (targetList != null)
        {
            if (sourceList != null && sourceList.Count > 0)
            {
                IOrderedEnumerable<PropertyInfo> nameOrderList = sourceList.OrderBy(property => property.Name);
                targetList.AddRange(nameOrderList);
            }
        }
    }

    /// <summary>
    /// Get the list of indexed properties from the original list.
    /// </summary>
    private void ReadIndexedProperties()
    {
        if (_originalPropertyList != null)
        {
            _indexedProperties?.Clear();
            _indexedProperties = null;
            // Read the list of properties from the original data type that have been decorated
            // with the <see cref="IndexAttribute"/>.
            _indexedProperties = [.. _originalPropertyList
                .Where(property => property.GetCustomAttribute<IndexAttribute>() != null)];
        }
    }

    /// <summary>
    /// Get the list of non-indexed properties from the original list.
    /// </summary>
    private void ReadNonIndexedProperties()
    {
        if (_originalPropertyList != null)
        {
            _nonIndexedProperties?.Clear();
            _nonIndexedProperties = null;
            // Read the list of properties from the original data type that have been decorated
            // with the <see cref="IndexAttribute"/>.
            _nonIndexedProperties = [.. _originalPropertyList
                .Where(property => property.GetCustomAttribute<IndexAttribute>() == null)
                .OrderBy(property => property.Name)];
        }
    }

    /// <summary>
    /// Sorts the indexed properties.
    /// </summary>
    /// <returns>
    /// An <see cref="IOrderedEnumerable{T}"/> of <see cref="PropertyInfo"/> instances if successful;
    /// otherwise, returns <b>null</b>.
    /// </returns>
    private IOrderedEnumerable<PropertyInfo>? SortIndexedProperties()
    {
        IOrderedEnumerable<PropertyInfo>? sortedList = null;

        if (_indexedProperties != null)
        {
            sortedList = _indexedProperties.OrderBy(property =>
                {
                    var attribute = property.GetCustomAttribute<IndexAttribute>();
                    if (attribute is null)
                    {
                        return 0;
                    }
                    else
                    {
                        return attribute.Index;
                    }
                });
        }
        return sortedList;
    }

    /// <summary>
    /// Reads and caches the list of properties and metadata from the data source.
    /// </summary>
    private void ReadOriginalPropertyList()
    {
        _originalPropertyList?.Clear();
        _originalPropertyList = null;

        if (_userType != null)
        {
            try
            {
                _originalPropertyList = [.. _userType.GetProperties(Flags)];
            }
            catch
            {
                _originalPropertyList = null;
            }
        }
    }
    #endregion
}