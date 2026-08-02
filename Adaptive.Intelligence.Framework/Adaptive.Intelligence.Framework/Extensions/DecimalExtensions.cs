namespace Adaptive.Intelligence.Extensions
{
    /// <summary>
    /// Provides extensions to the <see cref="System.Decimal"/> structure.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Converts the current value to a byte array.
        /// </summary>
        /// <param name="value">
        /// The current <see cref="decimal"/> instance.
        /// </param>
        /// <returns>
        /// A byte array representing the decimal structure content.
        /// </returns>
        public static byte[] GetBytes(this decimal value)
        {
            MemoryStream ms = new(64);
            BinaryWriter writer = new(ms);
            writer.Write(value);
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            byte[] asArray = ms.ToArray();
            writer.Dispose();
            ms.Dispose();

            return asArray;
        }
    }

}
