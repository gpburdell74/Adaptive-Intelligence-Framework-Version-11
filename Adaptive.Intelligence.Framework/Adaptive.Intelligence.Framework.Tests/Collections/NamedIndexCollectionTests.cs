using Adaptive.Intelligence.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Collections
{
    /// <summary>
    /// Gets the definition for NamedIndexCollectionTests.
    /// </summary>
    public class NamedIndexCollectionTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for ConstructorTest.
        /// </summary>
        public void ConstructorTest()
        {
            TestNameIndexCollection? list = [];
            Assert.NotNull(list);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor2Test.
        /// </summary>
        public void Constructor2Test()
        {
            TestNameIndexCollection? list = new(1000);
            Assert.NotNull(list);
            Assert.Equal(1000, list.Capacity);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor3Test.
        /// </summary>
        public void Constructor3Test()
        {
            List<TestItem> sourcelist =
                [
                    new TestItem
                    {
                        Name = "Item1"
                    },
                    new TestItem
                    {
                        Name = "Test2"
                    }
                ];

            string a = sourcelist[0].Name;
            string b = sourcelist[1].Name;

            TestNameIndexCollection? list = new(2);
            list.AddRange(sourcelist);
            Assert.NotNull(list);
            Assert.Equal(2, list.Count);
            Assert.IsType<TestItem>(list[0]);
            Assert.IsType<TestItem>(list[1]);

            Assert.NotNull(list[a]);
            Assert.NotNull(list[b]);
            list.Clear();
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_Item_AddsToCollection.
        /// </summary>
        public void Add_Item_AddsToCollection()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };

            // Act
            collection.Add(item);

            // Assert
            Assert.Contains(item, collection);
            Assert.True(collection.Contains(item.Name));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddRange_Items_AddsToCollection.
        /// </summary>
        public void AddRange_Items_AddsToCollection()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            List<TestItem> items =
            [
                new TestItem { Name = "Test1" },
                new TestItem { Name = "Test2" },
                new TestItem { Name = "Test3" },
                new TestItem { Name = "Test4" },
                new TestItem { Name = "Test5" }
            ];

            // Act
            collection.AddRange(items);

            // Assert
            foreach (var item in items)
            {
                Assert.Contains(item, collection);
                Assert.True(collection.Contains(item.Name));
            }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Contains_ItemExists_ReturnsTrue.
        /// </summary>
        public void Contains_ItemExists_ReturnsTrue()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };
            collection.Add(item);

            // Act
            var result = collection.Contains(item.Name);

            // Assert
            Assert.True(result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Contains_ItemDoesNotExist_ReturnsFalse.
        /// </summary>
        public void Contains_ItemDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var itemName = "Test1";

            // Act
            var result = collection.Contains(itemName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Clear_Collection_ClearsAllItems.
        /// </summary>
        public void Clear_Collection_ClearsAllItems()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            List<TestItem> items =
            [
                new TestItem { Name = "Test1" },
                new TestItem { Name = "Test2" },
                new TestItem { Name = "Test3" },
                new TestItem { Name = "Test4" },
                new TestItem { Name = "Test5" }
            ];

            collection.AddRange(items);

            // Act
            collection.Clear();

            // Assert
            Assert.Empty(collection);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Insert_Item_InsertsAtIndex.
        /// </summary>
        public void Insert_Item_InsertsAtIndex()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };

            // Act
            collection.Insert(0, item);

            // Assert
            Assert.Equal(item, collection[0]);
            Assert.True(collection.Contains(item.Name));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Remove_Item_RemovesFromCollection.
        /// </summary>
        public void Remove_Item_RemovesFromCollection()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };

            collection.Add(item);

            // Act
            collection.Remove(item);

            // Assert
            Assert.DoesNotContain(item, collection);
            Assert.False(collection.Contains(item.Name));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for RemoveByName_Item_RemovesFromCollection.
        /// </summary>
        public void RemoveByName_Item_RemovesFromCollection()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };
            collection.Add(item);

            // Act
            collection.Remove(item.Name);

            // Assert
            Assert.DoesNotContain(item, collection);
            Assert.False(collection.Contains(item.Name));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for RemoveAt_Index_RemovesFromCollection.
        /// </summary>
        public void RemoveAt_Index_RemovesFromCollection()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };
            collection.Add(item);

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.DoesNotContain(item, collection);
            Assert.False(collection.Contains(item.Name));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Indexer_Get_ReturnsCorrectItem.
        /// </summary>
        public void Indexer_Get_ReturnsCorrectItem()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };
            collection.Add(item);

            // Act
            var result = collection[item.Name];

            // Assert
            Assert.Equal(item, result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Indexer_Set_UpdatesItem.
        /// </summary>
        public void Indexer_Set_UpdatesItem()
        {
            // Arrange
            var collection = new TestNameIndexCollection();
            var item = new TestItem
            {
                Name = "Test1"
            };
            collection.Add(item);
            var newItem = new TestItem
            {
                Name = "Test1"
            };

            // Act
            collection[item.Name] = newItem;

            // Assert
            Assert.Equal(newItem, collection[item.Name]);
        }
    }

    /// <summary>
    /// Gets the definition for TestItem.
    /// </summary>
    public class TestItem
    {
        /// <summary>
        /// Gets the definition for Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Gets the definition for TestNameIndexCollection.
    /// </summary>
    public class TestNameIndexCollection : NamedIndexCollection<TestItem>
    {
        public TestNameIndexCollection() : base()
        {

        }
        public TestNameIndexCollection(int capacity) : base(capacity)
        {
        }
        /// <summary>
        /// Gets the definition for GetName.
        /// </summary>
        protected override string GetName(TestItem item)
        {
            return item.Name;
        }
    }
}