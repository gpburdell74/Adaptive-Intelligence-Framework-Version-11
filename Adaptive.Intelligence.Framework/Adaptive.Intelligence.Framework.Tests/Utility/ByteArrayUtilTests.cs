using Adaptive.Intelligence.Utility;

namespace Adaptive.Intelligence.Framework.Tests.Utility
{
    /// <summary>
    /// Gets the definition for ByteArrayUtilTests.
    /// </summary>
    public class ByteArrayUtilTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Create_Pinned_Array_Works.
        /// </summary>
        public void Create_Pinned_Array_Works()
        {
            byte[] data = ByteArrayUtil.CreatePinnedArray(100);
            Assert.NotNull(data);
            Assert.Equal(100, data.Length);

            ByteArrayUtil.Clear(data);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Clear_Works.
        /// </summary>
        public void Clear_Works()
        {
            byte[] data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
            ByteArrayUtil.Clear(data);
            for (int index = 0; index < 10; index++)
            {
                Assert.Equal(0, data[index]);
            }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CreateRandomArray_Works.
        /// </summary>
        public void CreateRandomArray_Works()
        {
            byte[] data = ByteArrayUtil.CreateRandomArray(100);
            Assert.NotNull(data);
            Assert.Equal(100, data.Length);

        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyToNewArray_Works.
        /// </summary>
        public void CopyToNewArray_Works()
        {
            byte[] data = ByteArrayUtil.CreateRandomArray(100);
            byte[]? newData = ByteArrayUtil.CopyToNewArray(data);
            Assert.NotNull(newData);
            Assert.Equal(data.Length, newData.Length);
            for (int index = 0; index < data.Length; index++)
            {
                Assert.Equal(data[index], newData[index]);
            }
            ByteArrayUtil.Clear(data);
            ByteArrayUtil.Clear(newData);

        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyToNewArray_Does_Not_Throw_When_Null.
        /// </summary>
        public void CopyToNewArray_Does_Not_Throw_When_Null()
        {
            byte[]? data = null;
            byte[]? newData = ByteArrayUtil.CopyToNewArray(data);
            Assert.Null(newData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ConcatenateArrays_Acutally_Concatenates.
        /// </summary>
        public void ConcatenateArrays_Acutally_Concatenates()
        {
            byte[] data1 = ByteArrayUtil.CreateRandomArray(100);
            byte[] data2 = ByteArrayUtil.CreateRandomArray(50);
            byte[]? concatenatedData = ByteArrayUtil.ConcatenateArrays(data1, data2);
            Assert.NotNull(concatenatedData);
            Assert.Equal(data1.Length + data2.Length, concatenatedData.Length);
            for (int index = 0; index < data1.Length; index++)
            {
                Assert.Equal(data1[index], concatenatedData[index]);
            }
            for (int index = 0; index < data2.Length; index++)
            {
                Assert.Equal(data2[index], concatenatedData[data1.Length + index]);
            }
            ByteArrayUtil.Clear(data2);
            ByteArrayUtil.Clear(data1);
            ByteArrayUtil.Clear(concatenatedData);

        }

        [Fact]
        /// <summary>
        /// Gets the definition for IsNullOrEmpty_Returns_True_When_Null_Or_Empty.
        /// </summary>
        public void IsNullOrEmpty_Returns_True_When_Null_Or_Empty()
        {
            byte[]? data1 = null;
            byte[] data2 = [];
            Assert.True(ByteArrayUtil.IsNullOrEmpty(data1));
            Assert.True(ByteArrayUtil.IsNullOrEmpty(data2));

            ByteArrayUtil.Clear(data2);
            ByteArrayUtil.Clear(data1);

        }

        [Fact]
        /// <summary>
        /// Gets the definition for IsNullOrEmpty_Returns_False_When_Not_Null_Or_Empty.
        /// </summary>
        public void IsNullOrEmpty_Returns_False_When_Not_Null_Or_Empty()
        {
            byte[] data1 = [1, 2, 3];
            byte[] data2 = [4, 5, 6, 7, 8, 9, 10, 11, 12, 13];
            byte[] data3 = [1];
            Assert.False(ByteArrayUtil.IsNullOrEmpty(data1));
            Assert.False(ByteArrayUtil.IsNullOrEmpty(data2));
            Assert.False(ByteArrayUtil.IsNullOrEmpty(data3));

            byte[] data4 = ByteArrayUtil.CreateRandomArray(100);
            Assert.False(ByteArrayUtil.IsNullOrEmpty(data4));

            ByteArrayUtil.Clear(data4);
            ByteArrayUtil.Clear(data3);
            ByteArrayUtil.Clear(data2);
            ByteArrayUtil.Clear(data1);
        }
    }
}