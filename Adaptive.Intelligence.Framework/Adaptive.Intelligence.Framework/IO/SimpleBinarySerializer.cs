using System.Xml.Serialization;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides static methods and functions for simple binary serialization and de-serialization.
    /// </summary>
    public static class SimpleBinarySerializer
    {
        /// <summary>
        /// Serializes the specified instance into a byte array.
        /// </summary>
        /// <typeparam name="T">
        /// The data type of the instance being serialized.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to be serialized.
        /// </param>
        /// <returns>
        /// A byte array containing the serialized content if successful; otherwise,
        /// returns <b>null</b>.
        /// </returns>
        public static byte[]? Serialize<T>(T instance)
        {
            byte[]? returnData = null;

            // Create the serialization instances.
            XmlSerializer xmlSerializer = new(typeof(T));
            MemoryStream? stream = new(65536);

            // Attempt to serialize the instance.
            try
            {
                xmlSerializer.Serialize(stream, instance);
            }
            catch (Exception ex)
            {
                stream.Dispose();
                stream = null;
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            // Copy the data if successful.
            if (stream != null)
            {
                stream.Seek(0, SeekOrigin.Begin);
                returnData = stream.ToArray();

                stream.Dispose();
            }

            return returnData;
        }

        /// <summary>
        /// De-serializes the byte array into an object instance.
        /// </summary>
        /// <typeparam name="T">
        /// The data type of the instance being de-serialized.
        /// </typeparam>
        /// <param name="serializedData">
        /// A byte array containing the binary representation of the instance to be de-serialized.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="T"/> if successful; otherwise, returns <b>null</b>.
        /// </returns>
        public static T? Deserialize<T>(byte[]? serializedData)
        {
            T? returnInstance = default;

            if (serializedData != null && serializedData.Length > 0)
            {
                // Create the serialization objects.
                XmlSerializer xmlSerializer = new(typeof(T));
                MemoryStream sourceStream = new(serializedData);

                // Attempt to deserialize.
                try
                {
                    returnInstance = (T?)xmlSerializer.Deserialize(sourceStream);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }

                sourceStream.Dispose();
            }

            return returnInstance;
        }
    }
}
