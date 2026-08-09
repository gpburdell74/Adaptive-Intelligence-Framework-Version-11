using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;
using System.Globalization;
using System.Reflection;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides a writer instance for serializing objects to a stream.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the object being serialized.
    /// </typeparam>
    public sealed class BinarySerializationWriter<T> : DisposableObjectBase
    {
        #region Private Member Declarations
        /// <summary>
        /// The writer instance.
        /// </summary>
        private BinaryWriter? _writer;
        /// <summary>
        /// The destination stream.
        /// </summary>
        private Stream? _destinationStream;
        /// <summary>
        /// The list of properties defined on type T.
        /// </summary>
        private PropertyInfo[]? _propList;
        /// <summary>
        /// The type definition for T.
        /// </summary>
        private Type? _itemType;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializationWriter{T}"/> class.
        /// </summary>
        /// <param name="destinationStream">
        /// The destination <see cref="Stream"/> instance to which the object will
        /// be serialized.
        /// </param>
        /// <exception cref="ArgumentNullException">destinationStream</exception>
        public BinarySerializationWriter(Stream destinationStream)
        {
            _destinationStream = destinationStream ?? throw new ArgumentNullException(nameof(destinationStream));
            _writer = new BinaryWriter(_destinationStream);

            Type[] arguments = GetType().GetGenericArguments();
            if (arguments.Length > 0)
            {
                _itemType = arguments[0];
            }

            if (_itemType != null)
            {
                _propList = _itemType.GetProperties();
            }
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _writer?.Dispose();
            }

            _propList = null;
            _itemType = null;
            _writer = null;
            _destinationStream = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Closes this instance, but does not close the underlying stream.
        /// </summary>
        public void Close()
        {
            _writer = null;
        }
        /// <summary>
        /// Writes the list of objects to the underlying stream.
        /// </summary>
        /// <param name="list">
        /// A <see cref="List{T}"/> of instances to be written.
        /// </param>
        public void WriteList(List<T> list)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException("Could not write to the specified stream.");
            }
            ArgumentNullException.ThrowIfNull(list);

            // Write the property list.
            WritePropertyList();

            // Write the count, and each instance.
            _writer.Write(list.Count);
            foreach (T instance in list)
            {
                Write(instance);
            }

            _writer.Flush();

        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Writes the property list to the start of the stream.
        /// </summary>
        /// <remarks>
        /// This writes the names of the properties that were serialized.
        /// </remarks>
        private void WritePropertyList()
        {
            if ((_writer != null) && (_propList != null))
            {
                // Write the property name list.
                _writer.Write(_propList.Length);

                foreach (PropertyInfo property in _propList)
                {
                    _writer.Write(property.Name + CharacterConstants.ColonChar + property.PropertyType.FullName);
                }
                _writer.Flush();
            }
        }
        /// <summary>
        /// Serializes the specified instance to the underlying stream.
        /// </summary>
        /// <param name="currentInstance">
        /// The current instance of <typeparamref name="T"/> being written to the
        /// stream.
        /// </param>
        private void Write(T currentInstance)
        {
            if (_propList != null)
            {
                foreach (PropertyInfo prop in _propList)
                {
                    WritePropertyValue(currentInstance, prop);
                }
            }
        }
        /// <summary>
        /// Serializes the property value to the underlying stream.
        /// </summary>
        /// <param name="currentInstance">
        /// The current instance of <typeparamref name="T"/> being written to the
        /// stream.
        /// </param>
        /// <param name="propertyDefinition">
        /// A <see cref="PropertyInfo"/> instance containing the definition of the
        /// property.
        /// </param>
        public void WritePropertyValue(T currentInstance, PropertyInfo? propertyDefinition)
        {
            if (propertyDefinition != null)
            {
                Type dataType = propertyDefinition.PropertyType;

                if (dataType.GetNullableUnderlyingType() != null)
                {
                    // Handle nullable types.
                    WriteForNullableType(currentInstance, propertyDefinition);
                }
                else
                {
                    // Handle regular (non-object) types.
                    WriteForType(currentInstance, propertyDefinition);
                }
            }
        }
        /// <summary>
        /// Writes the null-able data type content to the underlying stream.
        /// </summary>
        /// <param name="currentInstance">
        /// The current instance of <typeparamref name="T"/> being written to the
        /// stream.
        /// </param>
        /// <param name="propertyDefinition">
        /// A <see cref="PropertyInfo"/> instance containing the definition of the
        /// property.
        /// </param>
        private void WriteForNullableType(T currentInstance, PropertyInfo? propertyDefinition)
        {
            if (propertyDefinition != null)
            {
                Type[] typeList = propertyDefinition.PropertyType.GetGenericArguments();

                if (typeList.Length > 0)
                {
                    // Find the actual data type.
                    Type actualType = typeList[0];
                    object? value = propertyDefinition.GetValue(currentInstance);

                    // Write null/non-null indicator.
                    WriteIsNull(value);
                    // If not null, write the value.
                    if (value != null)
                    {
                        WriteByType(actualType, value);
                    }
                }
            }
        }
        /// <summary>
        /// Writes the content for the data type of the property.
        /// </summary>
        /// <param name="currentInstance">
        /// The current instance of <typeparamref name="T"/> being written to the
        /// stream.
        /// </param>
        /// <param name="propertyDefinition">
        /// A <see cref="PropertyInfo"/> instance containing the definition of the
        /// property.
        /// </param>
        private void WriteForType(T currentInstance, PropertyInfo? propertyDefinition)
        {
            if (propertyDefinition != null)
            {
                object? value = propertyDefinition.GetValue(currentInstance);
                if (value != null)
                {
                    WriteByType(propertyDefinition.PropertyType, value);
                }
            }
        }
        /// <summary>
        /// Writes the null/is not null indicator to the stream.
        /// </summary>
        /// <param name="value">The possibly null-able value.</param>
        private void WriteIsNull(object? value)
        {
            if (_writer != null)
            {
                if (value == null)
                {
                    _writer.Write((byte)0);
                }
                else
                {
                    _writer.Write((byte)1);
                }
            }
        }
        /// <summary>
        /// Writes the data of the property to the serialization stream based
        /// on the type of data being written.
        /// </summary>
        /// <param name="propertyType">
        /// The data <see cref="Type"/> of the property.
        /// </param>
        /// <param name="value">
        /// The current value of the property.
        /// </param>
        private void WriteByType(Type? propertyType, object value)
        {
            if (propertyType != null && _writer != null)
            {
                // Handle arrays specially.
                if (propertyType.IsArray)
                {
                    WriteArrayByType(propertyType, value);
                }
                else
                {
                    TypeCode typeCode = Type.GetTypeCode(propertyType);
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            _writer.Write((bool)value);
                            break;

                        case TypeCode.Byte:
                            _writer.Write((byte)value);
                            break;

                        case TypeCode.Char:
                            _writer.Write((char)value);
                            break;

                        case TypeCode.DateTime:
                            _writer.Write(((DateTime)value).ToFileTime());
                            break;

                        case TypeCode.Decimal:
                            _writer.Write((decimal)value);
                            break;

                        case TypeCode.Double:
                            _writer.Write((double)value);
                            break;

                        case TypeCode.Int16:
                            _writer.Write((short)value);
                            break;

                        case TypeCode.Int32:
                            _writer.Write((int)value);
                            break;

                        case TypeCode.Int64:
                            _writer.Write((long)value);
                            break;

                        case TypeCode.SByte:
                            _writer.Write((sbyte)value);
                            break;

                        case TypeCode.Single:
                            _writer.Write((float)value);
                            break;

                        case TypeCode.UInt16:
                            _writer.Write((ushort)value);
                            break;

                        case TypeCode.UInt32:
                            _writer.Write((uint)value);
                            break;

                        case TypeCode.UInt64:
                            _writer.Write((UInt64)value);
                            break;

                        case TypeCode.String:
                            string? itemToWrite = value as string;
                            WriteIsNull(itemToWrite);
                            if (itemToWrite != null)
                            {
                                _writer.Write(itemToWrite);
                            }

                            break;

                        case TypeCode.Object:
                            string name = propertyType.Name.ToLower(CultureInfo.InvariantCulture);
                            if (name == AiConstants.DataTypeDateTimeOffsetLower)
                            {
                                string stringRepresentation = ((DateTimeOffset)value).ToString(CultureInfo.InvariantCulture);
                                _writer.Write(stringRepresentation);
                            }
                            else if (name == AiConstants.DataTypeGuidLower)
                            {
                                _writer.Write(((Guid)value).ToString());
                            }
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Writes the data of the array property to the serialization stream based
        /// on the type of data being written.
        /// </summary>
        /// <param name="propertyType">
        /// The data <see cref="Type"/> of the property.
        /// </param>
        /// <param name="value">
        /// The current value of the property.
        /// </param>
        private void WriteArrayByType(Type? propertyType, object? value)
        {
            if (propertyType != null && _writer != null)
            {
                TypeCode typeCode = Type.GetTypeCode(propertyType.GetElementType());
                switch (typeCode)
                {
                    case TypeCode.Byte:
                        // May be null...
                        WriteIsNull(value);
                        if (value != null)
                        {
                            // Write length, then data...
                            _writer.Write(((byte[])value).Length);
                            _writer.Write((byte[])value);
                        }
                        break;

                    case TypeCode.Char:
                        // May be null...
                        WriteIsNull(value);
                        if (value != null)
                        {
                            // Write length, then data...
                            _writer.Write(((char[])value).Length);
                            _writer.Write((char[])value);
                        }
                        break;

                    case TypeCode.String:
                        // May be null...
                        WriteIsNull(value);
                        if (value != null)
                        {
                            // Write length, then data...
                            string[] data = (string[])value;
                            _writer.Write(data.Length);
                            for (int count = 0; count < data.Length; count++)
                            {
                                _writer.Write(data[count]);
                            }
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
