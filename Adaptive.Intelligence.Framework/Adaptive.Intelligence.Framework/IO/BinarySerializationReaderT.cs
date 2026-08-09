using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;
using System.Globalization;
using System.Reflection;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides a reader instance for deserializing objects from a stream.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the object being deserialized.
    /// </typeparam>
    public sealed class BinarySerializationReader<T> : DisposableObjectBase
    {
        #region Private Member Declarations
        /// <summary>
        /// The reader instance.
        /// </summary>
        private BinaryReader? _reader;
        /// <summary>
        /// The input stream.
        /// </summary>
        private Stream? _inputStream;
        /// <summary>
        /// The type definition for T.
        /// </summary>
        private Type? _itemType;
        /// <summary>
        /// The list of serialization properties.
        /// </summary>
        private List<SerializationProperty>? _serialProps;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializationReader{T}"/> class.
        /// </summary>
        /// <param name="inputStream">
        /// The destination <see cref="Stream"/> instance from which the object will
        /// be deserialized.
        /// </param>
        /// <exception cref="ArgumentNullException">destinationStream</exception>
        public BinarySerializationReader(Stream inputStream)
        {
            _inputStream = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _reader = new BinaryReader(_inputStream);

            Type[] arguments = GetType().GetGenericArguments();
            if (arguments.Length > 0)
            {
                _itemType = arguments[0];
            }

            _serialProps = [];
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _reader?.Dispose();
            }

            _itemType = null;
            _reader = null;
            _inputStream = null;
            _serialProps = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Closes this instance, but does not close the underlying stream.
        /// </summary>
        public void Close()
        {
            _reader = null;
            _inputStream = null;
        }
        /// <summary>
        /// Reads the list of objects from the underlying stream.
        /// </summary>
        /// <returns>
        /// The deserialized <see cref="List{T}"/>.
        /// </returns>
        public List<T> ReadList()
        {
            if (_reader == null)
            {
                throw new InvalidOperationException("Could not read from the specified stream.");
            }

            // Read the property list.
            ReadPropertyList();

            // Read the item count, and deserialize each instance.
            int length = _reader.ReadInt32();
            List<T> list = new(length);

            for (int count = 0; count < length; count++)
            {
                T instance = Read();
                if (instance != null)
                {
                    list.Add(instance);
                }
            }
            return list;

        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Reads the property list from the start of the stream.
        /// </summary>
        /// <remarks>
        /// This reads the names of the properties that were serialized.
        /// </remarks>
        private void ReadPropertyList()
        {
            if ((_reader != null) && (_itemType != null))
            {
                // Read the length of the property name list.
                int length = _reader.ReadInt32();

                // Create the property information instance, of the property is
                // present on the class to be deserialized.
                for (int count = 0; count < length; count++)
                {
                    string value = _reader.ReadString();
                    SerializationProperty serialProp = new(value);

                    PropertyInfo? propInstance = null;
                    try
                    {
                        if (serialProp.PropertyName != null)
                        {
                            propInstance = _itemType.GetProperty(serialProp.PropertyName);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }
                    if (propInstance != null)
                    {
                        serialProp.ExistingProperty = propInstance;
                    }
                    _serialProps ??= [];
                    _serialProps.Add(serialProp);
                }
            }
        }
        /// <summary>
        /// De-serializes the specified instance from the underlying stream.
        /// </summary>
        /// <returns>
        /// The deserialized instance of <typeparamref name="T"/>.
        /// </returns>
        private T Read()
        {
            T currentInstance = Activator.CreateInstance<T>();
            if (_serialProps != null)
            {
                foreach (SerializationProperty prop in _serialProps)
                {
                    ReadProperty(currentInstance, prop);
                }
            }
            return currentInstance;
        }
        /// <summary>
        /// De-serializes the property value from the underlying stream.
        /// </summary>
        /// <param name="currentInstance">
        /// The current instance of <typeparamref name="T"/> being read from the
        /// stream.
        /// </param>
        /// <param name="propertyDefinition">
        /// A <see cref="SerializationProperty"/> instance containing the definition of the
        /// property.
        /// </param>
        private void ReadProperty(T currentInstance, SerializationProperty? propertyDefinition)
        {
            if (propertyDefinition != null)
            {
                Type? dataType = propertyDefinition.PropertyType;

                if (dataType != null)
                {
                    if (dataType.IsGenericType)
                    {
                        // Handle nullable types.
                        ReadForNullableType(currentInstance, propertyDefinition);
                    }
                    else
                    {
                        // Handle regular (non-object) types.
                        ReadForType(currentInstance, propertyDefinition);
                    }
                }
            }
        }
        /// <summary>
        /// Reads the null-able data type content from the underlying stream.
        /// </summary>
        /// <param name="currentInstance">
        /// The current instance of <typeparamref name="T"/> being read from the
        /// stream.
        /// </param>
        /// <param name="propertyDefinition">
        /// A <see cref="PropertyInfo"/> instance containing the definition of the
        /// property.
        /// </param>
        private void ReadForNullableType(T currentInstance, SerializationProperty? propertyDefinition)
        {
            if ((propertyDefinition != null) && (propertyDefinition.PropertyType != null))
            {
                Type[] typeList = propertyDefinition.PropertyType.GetGenericArguments();
                object? value = null;

                if (typeList.Length > 0)
                {
                    // Find the actual data type.
                    Type actualType = typeList[0];

                    // Read the null/non-null indicator.
                    if (ReadIsNotNull())
                    {
                        // If not null, read the value.
                        value = ReadByType(actualType);
                    }
                    if (propertyDefinition.ExistingProperty != null)
                    {
                        if (propertyDefinition.ExistingProperty.CanWrite)
                        {
                            propertyDefinition.ExistingProperty.SetValue(currentInstance, value);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Reads the content for the data type of the property.
        /// </summary>
        /// <param name="currentInstance">
        /// The current instance of <typeparamref name="T"/> being read from the
        /// stream.
        /// </param>
        /// <param name="propertyDefinition">
        /// A <see cref="PropertyInfo"/> instance containing the definition of the
        /// property.
        /// </param>
        private void ReadForType(T currentInstance, SerializationProperty? propertyDefinition)
        {
            if (propertyDefinition != null)
            {
                object? value = null;
                if (propertyDefinition.PropertyType != null)
                {
                    value = ReadByType(propertyDefinition.PropertyType);
                }

                if (propertyDefinition.ExistingProperty != null)
                {
                    if (propertyDefinition.ExistingProperty.CanWrite)
                    {
                        propertyDefinition.ExistingProperty.SetValue(currentInstance, value);
                    }
                }
            }
        }

        /// <summary>
        /// Reads the null/is not null indicator from the stream.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the property value is not null; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        private bool ReadIsNotNull()
        {
            if (_reader == null)
            {
                return false;
            }
            else
            {
                return _reader.ReadByte() > 0;
            }
        }
        /// <summary>
        /// Reads the data of the property from the serialization stream based
        /// on the type of data being written.
        /// </summary>
        /// <param name="propertyType">
        /// The data <see cref="Type"/> of the property.
        /// </param>
        /// <returns>
        /// The deserialized value of the property.
        /// </returns>
        private object? ReadByType(Type? propertyType)
        {
            object? returnValue = null;

            if (_reader != null && propertyType != null)
            {
                // Handle arrays specially.
                if (propertyType.IsArray)
                {
                    returnValue = ReadArrayByType(propertyType);
                }
                else
                {
                    TypeCode typeCode = Type.GetTypeCode(propertyType);
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            returnValue = _reader.ReadBoolean();
                            break;

                        case TypeCode.Byte:
                            returnValue = _reader.ReadByte();
                            break;

                        case TypeCode.Char:
                            returnValue = _reader.ReadChar();
                            break;

                        case TypeCode.DateTime:
                            returnValue = DateTime.FromFileTime(_reader.ReadInt64());
                            break;

                        case TypeCode.Decimal:
                            returnValue = _reader.ReadDecimal();
                            break;

                        case TypeCode.Double:
                            returnValue = _reader.ReadDouble();
                            break;

                        case TypeCode.Int16:
                            returnValue = _reader.ReadInt16();
                            break;

                        case TypeCode.Int32:
                            returnValue = _reader.ReadInt32();
                            break;

                        case TypeCode.Int64:
                            returnValue = _reader.ReadInt64();
                            break;

                        case TypeCode.SByte:
                            returnValue = _reader.ReadSByte();
                            break;

                        case TypeCode.Single:
                            returnValue = _reader.ReadSingle();
                            break;

                        case TypeCode.UInt16:
                            returnValue = _reader.ReadUInt16();
                            break;

                        case TypeCode.UInt32:
                            returnValue = _reader.ReadUInt32();
                            break;

                        case TypeCode.UInt64:
                            returnValue = _reader.ReadUInt64();
                            break;

                        case TypeCode.String:
                            if (ReadIsNotNull())
                            {
                                try
                                {
                                    returnValue = _reader.ReadString();
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Trace.TraceError(ex.Message);
                                    returnValue = null;
                                }
                            }
                            break;

                        case TypeCode.Object:
                            string name = propertyType.Name.ToLower(CultureInfo.InvariantCulture);
                            if (name == AiConstants.DataTypeDateTimeOffsetLower)
                            {
                                try
                                {
                                    string s = _reader.ReadString();
                                    returnValue = DateTimeOffset.Parse(s, CultureInfo.InvariantCulture);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Trace.TraceError(ex.Message);
                                    returnValue = new DateTimeOffset(1900, 1, 1, 0, 0, 0,
                                        DateTimeOffset.Now.Offset);
                                }
                            }
                            else if (name == AiConstants.DataTypeGuidLower)
                            {
                                returnValue = Guid.Parse(_reader.ReadString());
                            }
                            break;
                    }
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Reads the data of the array property from the serialization stream based
        /// on the type of data being written.
        /// </summary>
        /// <param name="propertyType">
        /// The data <see cref="Type"/> of the property.
        /// </param>
        /// <returns>
        /// The current value of the property.
        /// </returns>
        private object? ReadArrayByType(Type? propertyType)
        {
            object? returnValue = null;
            if (_reader != null && propertyType != null)
            {
                TypeCode typeCode = Type.GetTypeCode(propertyType.GetElementType());

                switch (typeCode)
                {
                    case TypeCode.Byte:
                        if (ReadIsNotNull())
                        {
                            int count = _reader.ReadInt32();
                            returnValue = _reader.ReadBytes(count);
                        }
                        break;

                    case TypeCode.Char:
                        if (ReadIsNotNull())
                        {
                            int count = _reader.ReadInt32();
                            returnValue = _reader.ReadChars(count);
                        }
                        break;

                    case TypeCode.String:
                        if (ReadIsNotNull())
                        {
                            int length = _reader.ReadInt32();
                            string[] result = new string[length];
                            for (int count = 0; count < length; count++)
                            {
                                result[count] = _reader.ReadString();
                            }

                            returnValue = result;
                        }
                        break;
                    default:
                        returnValue = null;
                        break;
                }
            }
            return returnValue;
        }
        #endregion
    }
}
