using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;
using System.Reflection;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Represents a property that can be serialized.
    /// </summary>
    /// <seealso cref="BinarySerializationReader{T}"/>
    public sealed class SerializationProperty : DisposableObjectBase
    {
        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationProperty"/> class.
        /// </summary>
        /// <param name="data">
        /// A string containing the data content to be deserialized.
        /// </param>
        public SerializationProperty(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                ParsePropertyDefinition(data);
            }
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            DataType = null;
            ExistingProperty = null;
            PropertyName = null;
            PropertyType = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the name of the data type.
        /// </summary>
        /// <value>
        /// A string specifying the type of data for the property.
        /// </value>
        public string? DataType { get; private set; }
        /// <summary>
        /// Gets or sets the reference to the existing property.
        /// </summary>
        /// <value>
        /// A <see cref="PropertyInfo"/> instance representing an existing
        /// reflected property.
        /// </value>
        public PropertyInfo? ExistingProperty { get; set; }
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// A string specifying the name of the property.
        /// </value>
        public string? PropertyName { get; set; }
        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the property.
        /// </value>
        public Type? PropertyType { get; private set; }
        #endregion

        #region Private Methods / Functions		
        /// <summary>
        /// Parses the provided string into a property definition.
        /// </summary>
        /// <param name="data">
        /// A string containing the property definition in the format [name]:[type].
        /// </param>
        private void ParsePropertyDefinition(string data)
        {
            // Format: <property name>:<data type name>
            int index = data.IndexOf(CharacterConstants.ColonChar, StringComparison.Ordinal);
            if (index > -1)
            {
                // Read the property name value.
                int nextIndex = index + 1;
                PropertyName = data[..index];

                // Read the data type name.
                DataType = data[nextIndex..];

                // Get the .NET data type.
                try
                {
                    PropertyType = Type.GetType(DataType);
                    if (PropertyType != null)
                    {
                        DataType = PropertyType.Name;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
        }
        #endregion
    }
}
