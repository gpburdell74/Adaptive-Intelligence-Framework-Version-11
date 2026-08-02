using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for CaseInsensitiveStringDictionaryTests.
    /// </summary>
    public class CaseInsensitiveStringDictionaryTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Default_InitializesEmptyDictionary.
        /// </summary>
        public void Constructor_Default_InitializesEmptyDictionary()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>();

            Assert.NotNull(dictionary);
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Values);
            Assert.False(dictionary.IsReadOnly);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_StringKeyValue_AddsItemWithOriginalKey.
        /// </summary>
        public void Add_StringKeyValue_AddsItemWithOriginalKey()
        {
            var dictionary = new CaseInsensitiveStringDictionary<string>
            {
                { "TestKey", "Value" }
            };

            Assert.Single(dictionary);
            Assert.Contains("TestKey", dictionary.Keys);
            Assert.Contains("Value", dictionary.Values);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_KeyValuePair_AddsItem.
        /// </summary>
        public void Add_KeyValuePair_AddsItem()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                new KeyValuePair<string, int>("One", 1)
            };

            Assert.Single(dictionary);
            Assert.True(dictionary.ContainsKey("one"));
            Assert.Equal(1, dictionary["ONE"]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_DuplicateKeyDifferentCasing_ThrowsArgumentException.
        /// </summary>
        public void Add_DuplicateKeyDifferentCasing_ThrowsArgumentException()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "Alpha", 1 }
            };

            Assert.Throws<ArgumentException>(() => dictionary.Add("ALPHA", 2));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IndexerGet_ExistingKeyDifferentCase_ReturnsValue.
        /// </summary>
        public void IndexerGet_ExistingKeyDifferentCase_ReturnsValue()
        {
            var dictionary = new CaseInsensitiveStringDictionary<string>
            {
                { "CustomerId", "ABC123" }
            };

            string value = dictionary["customerid"];

            Assert.Equal("ABC123", value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IndexerGet_MissingKey_ReturnsDefault.
        /// </summary>
        public void IndexerGet_MissingKey_ReturnsDefault()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>();

            int value = dictionary["Missing"];

            Assert.Equal(default, value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IndexerSet_ExistingKey_UpdatesValue.
        /// </summary>
        public void IndexerSet_ExistingKey_UpdatesValue()
        {
            var dictionary = new CaseInsensitiveStringDictionary<string>
            {
                { "Mode", "A" }
            };

            dictionary["MODE"] = "B";

            Assert.Single(dictionary);
            Assert.Equal("B", dictionary["mode"]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IndexerSet_NewKey_AddsValue.
        /// </summary>
        public void IndexerSet_NewKey_AddsValue()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                ["Count"] = 10
            };

            Assert.Single(dictionary);
            Assert.True(dictionary.ContainsKey("count"));
            Assert.Equal(10, dictionary["COUNT"]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ContainsKey_DifferentCase_ReturnsTrue.
        /// </summary>
        public void ContainsKey_DifferentCase_ReturnsTrue()
        {
            var dictionary = new CaseInsensitiveStringDictionary<bool>
            {
                { "Enabled", true }
            };

            bool contains = dictionary.ContainsKey("ENABLED");

            Assert.True(contains);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Contains_KeyValuePair_WhenKeyExists_ReturnsTrue.
        /// </summary>
        public void Contains_KeyValuePair_WhenKeyExists_ReturnsTrue()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "Item", 42 }
            };

            bool contains = dictionary.Contains(new KeyValuePair<string, int>("ITEM", 999));

            Assert.True(contains);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for TryGetValue_ExistingKeyDifferentCase_ReturnsTrueWithValue.
        /// </summary>
        public void TryGetValue_ExistingKeyDifferentCase_ReturnsTrueWithValue()
        {
            var dictionary = new CaseInsensitiveStringDictionary<string>
            {
                { "Theme", "Dark" }
            };

            bool success = dictionary.TryGetValue("THEME", out string value);

            Assert.True(success);
            Assert.Equal("Dark", value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for TryGetValue_MissingKey_ReturnsFalseAndDefaultValue.
        /// </summary>
        public void TryGetValue_MissingKey_ReturnsFalseAndDefaultValue()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>();

            bool success = dictionary.TryGetValue("Missing", out int value);

            Assert.False(success);
            Assert.Equal(default, value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Remove_ByKeyDifferentCase_RemovesItem.
        /// </summary>
        public void Remove_ByKeyDifferentCase_RemovesItem()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "OrderId", 900 }
            };

            bool removed = dictionary.Remove("ORDERID");

            Assert.True(removed);
            Assert.Empty(dictionary);
            Assert.False(dictionary.ContainsKey("orderid"));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Remove_ByKeyValuePair_RemovesItem.
        /// </summary>
        public void Remove_ByKeyValuePair_RemovesItem()
        {
            var dictionary = new CaseInsensitiveStringDictionary<string>
            {
                { "File", "a.txt" }
            };

            bool removed = dictionary.Remove(new KeyValuePair<string, string>("FILE", "ignored"));

            Assert.True(removed);
            Assert.Empty(dictionary.Values);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Clear_WithItems_RemovesAllKeysAndValues.
        /// </summary>
        public void Clear_WithItems_RemovesAllKeysAndValues()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "A", 1 },
                { "B", 2 }
            };

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Values);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyTo_WithOffset_CopiesDictionaryEntries.
        /// </summary>
        public void CopyTo_WithOffset_CopiesDictionaryEntries()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "One", 1 },
                { "Two", 2 }
            };
            KeyValuePair<string, int>[] destination = new KeyValuePair<string, int>[4];

            dictionary.CopyTo(destination, 1);

            Assert.Equal(default, destination[0]);
            Assert.NotEqual(default, destination[1]);
            Assert.NotEqual(default, destination[2]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Enumeration_WithItems_ReturnsAllItems.
        /// </summary>
        public void Enumeration_WithItems_ReturnsAllItems()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "North", 1 },
                { "South", 2 }
            };

            List<KeyValuePair<string, int>> items = [.. dictionary];

            Assert.Equal(2, items.Count);
            Assert.Contains(items, pair => pair.Key == "north" && pair.Value == 1);
            Assert.Contains(items, pair => pair.Key == "south" && pair.Value == 2);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_WithNullOrEmptyKey_DoesNotAddItem.
        /// </summary>
        public void Add_WithNullOrEmptyKey_DoesNotAddItem()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { null!, 1 },
                { string.Empty, 2 }
            };

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Count_WhenDictionaryHasItems_ReturnsItemCount.
        /// </summary>
        public void Count_WhenDictionaryHasItems_ReturnsItemCount()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "First", 1 },
                { "Second", 2 }
            };

            int count = dictionary.Count;

            Assert.Equal(2, count);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IsReadOnly_DefaultInstance_ReturnsFalse.
        /// </summary>
        public void IsReadOnly_DefaultInstance_ReturnsFalse()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>();

            bool isReadOnly = dictionary.IsReadOnly;

            Assert.False(isReadOnly);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_WhenCalled_CountReturnsZeroAndKeysAreEmpty.
        /// </summary>
        public void Dispose_WhenCalled_CountReturnsZeroAndKeysAreEmpty()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>
            {
                { "Alpha", 1 }
            };

            dictionary.Dispose();

            int count = dictionary.Count;

            Assert.True(count == 0);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_WhenCalledTwice_DoesNotThrow.
        /// </summary>
        public void Dispose_WhenCalledTwice_DoesNotThrow()
        {
            var dictionary = new CaseInsensitiveStringDictionary<int>();

            dictionary.Dispose();
            Exception? exception = Record.Exception(() => dictionary.Dispose());

            Assert.Null(exception);
        }

    }
}