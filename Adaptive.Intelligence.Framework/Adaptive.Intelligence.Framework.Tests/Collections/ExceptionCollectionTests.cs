using Adaptive.Intelligence.Collections;

namespace Adaptive.Intelligence.Framework.Tests.Collections
{
    /// <summary>
    /// Provides the tests for the <see cref="ExceptionCollection"/> class.
    /// </summary>
    public class ExceptionCollectionTests
    {
        /// <summary>
        /// Tests that the default constructor creates an empty collection.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Default_Creates_Empty_Collection.
        /// </summary>
        public void Constructor_Default_Creates_Empty_Collection()
        {
            ExceptionCollection collection = new();

            Assert.NotNull(collection);
            Assert.Empty(collection);
        }

        /// <summary>
        /// Tests that the capacity constructor creates an empty collection with at least the requested capacity.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_With_Capacity_Creates_Empty_Collection_With_Requested_Capacity.
        /// </summary>
        public void Constructor_With_Capacity_Creates_Empty_Collection_With_Requested_Capacity()
        {
            ExceptionCollection collection = new(8);

            Assert.NotNull(collection);
            Assert.Empty(collection);
            Assert.True(collection.Capacity >= 8);
        }

        /// <summary>
        /// Tests that the source-list constructor copies exceptions in order.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_With_SourceList_Copies_Exceptions_In_Order.
        /// </summary>
        public void Constructor_With_SourceList_Copies_Exceptions_In_Order()
        {
            Exception first = new InvalidOperationException("First");
            Exception second = new ApplicationException("Second");
            IEnumerable<Exception> source = [first, second];

            ExceptionCollection collection = new(source);

            Assert.Equal(2, collection.Count);
            Assert.Same(first, collection[0]);
            Assert.Same(second, collection[1]);
        }

        /// <summary>
        /// Tests that passing a null source list throws an <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_With_Null_SourceList_Throws_ArgumentNullException.
        /// </summary>
        public void Constructor_With_Null_SourceList_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ExceptionCollection((IEnumerable<Exception>)null!));
        }

        /// <summary>
        /// Tests that <see cref="ExceptionCollection.Clone"/> creates a new collection instance with matching content.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Clone_Creates_New_Collection_With_Same_Items.
        /// </summary>
        public void Clone_Creates_New_Collection_With_Same_Items()
        {
            Exception first = new InvalidOperationException("First");
            Exception second = new ApplicationException("Second");
            ExceptionCollection original = new([first, second]);

            ExceptionCollection clone = original.Clone();

            Assert.NotSame(original, clone);
            Assert.Equal(2, clone.Count);
            Assert.Same(first, clone[0]);
            Assert.Same(second, clone[1]);
        }

        /// <summary>
        /// Tests that clone and original maintain independent list structures after cloning.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Clone_Is_List_Independent_From_Original.
        /// </summary>
        public void Clone_Is_List_Independent_From_Original()
        {
            ExceptionCollection original = new([new InvalidOperationException("One")]);
            ExceptionCollection clone = original.Clone();

            original.Add(new ApplicationException("Two"));
            clone.Add(new ArgumentException("Three"));

            Assert.Equal(2, original.Count);
            Assert.Equal(2, clone.Count);
            Assert.Equal("Two", original[1].Message);
            Assert.Equal("Three", clone[1].Message);
        }

        /// <summary>
        /// Tests that explicit <see cref="ICloneable.Clone"/> returns an <see cref="ExceptionCollection"/> with the same items.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Explicit_ICloneable_Clone_Returns_ExceptionCollection_Copy.
        /// </summary>
        public void Explicit_ICloneable_Clone_Returns_ExceptionCollection_Copy()
        {
            Exception ex = new InvalidOperationException("Clone me");
            ExceptionCollection original = new([ex]);
            ICloneable cloneable = original;

            object clonedObject = cloneable.Clone();

            ExceptionCollection clone = Assert.IsType<ExceptionCollection>(clonedObject);
            Assert.NotSame(original, clone);
            Assert.Single(clone);
            Assert.Same(ex, clone[0]);
        }

        /// <summary>
        /// Tests that <see cref="ExceptionCollection.Dispose"/> clears all items.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Clears_All_Items.
        /// </summary>
        public void Dispose_Clears_All_Items()
        {
            ExceptionCollection collection = new([
                new InvalidOperationException("One"),
                new ApplicationException("Two")
            ]);

            collection.Dispose();

            Assert.Empty(collection);
        }

        /// <summary>
        /// Tests that <see cref="ExceptionCollection.Dispose"/> can be called repeatedly without throwing.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Can_Be_Called_Multiple_Times_Safely.
        /// </summary>
        public void Dispose_Can_Be_Called_Multiple_Times_Safely()
        {
            ExceptionCollection collection = new([
                new InvalidOperationException("One")
            ]);

            collection.Dispose();
            collection.Dispose();
            collection.Dispose();

            Assert.Empty(collection);
        }
    }
}