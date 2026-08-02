using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Logging;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Logging
{
    /// <summary>
    /// Gets the definition for StructuredLogRecordCollectionTests.
    /// </summary>
    public class StructuredLogRecordCollectionTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_ShouldInitializeEmptyCollection.
        /// </summary>
        public void Constructor_ShouldInitializeEmptyCollection()
        {
            // Arrange & Act
            var collection = new StructuredLogRecordCollection();

            // Assert
            Assert.NotNull(collection);
            Assert.Empty(collection);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Sets_Capacity.
        /// </summary>
        public void Constructor_Sets_Capacity()
        {
            // Arrange & Act
            var collection = new StructuredLogRecordCollection(10);

            // Assert
            Assert.NotNull(collection);
            Assert.Empty(collection);
            Assert.Equal(10, collection.Capacity);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Adds_Records.
        /// </summary>
        public void Constructor_Adds_Records()
        {
            List<SimpleLogRecord> list =
            [
                new SimpleLogRecord
                {
                    InformationMessage = "Test message 1",
                    Level = LogLevel.Information
                },
                new SimpleLogRecord
                {
                    InformationMessage = "Test message 2",
                    Level = LogLevel.Information
                },
                new SimpleLogRecord
                {
                    InformationMessage = "Test message 3",
                    Level = LogLevel.Information
                },
            ];
            // Arrange & Act
            var collection = new StructuredLogRecordCollection(list);

            // Assert
            Assert.NotNull(collection);
            Assert.NotEmpty(collection);
            Assert.Equal(3, collection.Count);

            var item1 = collection[0];
            Assert.NotNull(item1);
            Assert.Equal("Test message 1", item1.InformationMessage);

            var item2 = collection[1];
            Assert.NotNull(item2);
            Assert.Equal("Test message 2", item2.InformationMessage);

            var item3 = collection[2];
            Assert.NotNull(item3);
            Assert.Equal("Test message 3", item3.InformationMessage);

        }

        [Fact]
        /// <summary>
        /// Gets the definition for Indexer_Get_Works.
        /// </summary>
        public void Indexer_Get_Works()
        {
            var newRecord = new SimpleLogRecord
            {
                InformationMessage = "Test message 3",
                Level = LogLevel.Information
            };

            var collection = new StructuredLogRecordCollection
            {
                newRecord
            };

            // Assert
            Assert.NotNull(collection);
            Assert.NotEmpty(collection);
            Assert.Single(collection);
            Assert.Equal(newRecord, collection[0]);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for Indexer_Set_Works.
        /// </summary>
        public void Indexer_Set_Works()
        {
            var oldRecord = new SimpleLogRecord
            {
                InformationMessage = "To Be Replaced",
                Level = LogLevel.Information
            };
            var newRecord = new SimpleLogRecord
            {
                InformationMessage = "Is Replaced",
                Level = LogLevel.Information
            };

            var collection = new StructuredLogRecordCollection
            {
                oldRecord
            };

            collection[0] = newRecord;

            // Assert
            Assert.NotNull(collection);
            Assert.NotEmpty(collection);
            Assert.Single(collection);
            Assert.Equal(newRecord, collection[0]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_Range_Works.
        /// </summary>
        public void Add_Range_Works()
        {
            List<SimpleLogRecord> list =
            [
                new SimpleLogRecord
                {
                    InformationMessage = "Test message 1",
                    Level = LogLevel.Information
                },
                new SimpleLogRecord
                {
                    InformationMessage = "Test message 2",
                    Level = LogLevel.Information
                },
                new SimpleLogRecord
                {
                    InformationMessage = "Test message 3",
                    Level = LogLevel.Information
                },
            ];
            // Arrange & Act
            var collection = new StructuredLogRecordCollection(3);

            collection.AddRange(list);

            // Assert
            Assert.NotNull(collection);
            Assert.NotEmpty(collection);
            Assert.Equal(3, collection.Count);

            var item1 = collection[0];
            Assert.NotNull(item1);
            Assert.Equal("Test message 1", item1.InformationMessage);

            var item2 = collection[1];
            Assert.NotNull(item2);
            Assert.Equal("Test message 2", item2.InformationMessage);

            var item3 = collection[2];
            Assert.NotNull(item3);
            Assert.Equal("Test message 3", item3.InformationMessage);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Remove_Works.
        /// </summary>
        public void Remove_Works()
        {
            var record1 = new SimpleLogRecord
            {
                InformationMessage = "Test message 1",
                Level = LogLevel.Information
            };
            var record2 = new SimpleLogRecord
            {
                InformationMessage = "Test message 2",
                Level = LogLevel.Information
            };
            var collection = new StructuredLogRecordCollection
            {
                record1,
                record2
            };
            // Act
            collection.Remove(record1);
            bool hasInstance = collection.Contains(record1);

            // Assert
            Assert.False(hasInstance);
            Assert.Single(collection);
            Assert.Equal(record2, collection[0]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for RemoveAll_Works.
        /// </summary>
        public void RemoveAll_Works()
        {
            var record1 = new SimpleLogRecord
            {
                InformationMessage = "Test message 1",
                Level = LogLevel.Information
            };
            var record2 = new SimpleLogRecord
            {
                InformationMessage = "Test message 2",
                Level = LogLevel.Information
            };
            var collection = new StructuredLogRecordCollection
            {
                record1,
                record2
            };
            // Act
            collection.RemoveAll(r => r.InformationMessage != null && r.InformationMessage.Contains("Test message"));
            // Assert
            Assert.Empty(collection);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for RemoveAt_Works.
        /// </summary>
        public void RemoveAt_Works()
        {
            var record1 = new SimpleLogRecord
            {
                InformationMessage = "Test message 1",
                Level = LogLevel.Information
            };
            var record2 = new SimpleLogRecord
            {
                InformationMessage = "Test message 2",
                Level = LogLevel.Information
            };
            var collection = new StructuredLogRecordCollection
            {
                record1,
                record2
            };
            // Act
            collection.RemoveAt(0);
            // Assert
            Assert.Single(collection);
            Assert.Equal(record2, collection[0]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Remove_Range_Works.
        /// </summary>
        public void Remove_Range_Works()
        {
            var record1 = new SimpleLogRecord
            {
                InformationMessage = "Test message 1",
                Level = LogLevel.Information
            };
            var record2 = new SimpleLogRecord
            {
                InformationMessage = "Test message 2",
                Level = LogLevel.Information
            };
            var record3 = new SimpleLogRecord
            {
                InformationMessage = "Test message 3",
                Level = LogLevel.Information
            };
            var collection = new StructuredLogRecordCollection
            {
                record1,
                record2,
                record3
            };
            // Act
            collection.RemoveRange(0, 2);
            // Assert
            Assert.Single(collection);
            Assert.Equal(record3, collection[0]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Reverse_Works.
        /// </summary>
        public void Reverse_Works()
        {
            var record1 = new SimpleLogRecord
            {
                InformationMessage = "Test message 1",
                Level = LogLevel.Information
            };
            var record2 = new SimpleLogRecord
            {
                InformationMessage = "Test message 2",
                Level = LogLevel.Information
            };
            var record3 = new SimpleLogRecord
            {
                InformationMessage = "Test message 3",
                Level = LogLevel.Information
            };
            var collection = new StructuredLogRecordCollection
            {
                record1,
                record2,
                record3
            };
            // Act
            collection.Reverse();
            // Assert
            Assert.Equal(record3, collection[0]);
            Assert.Equal(record2, collection[1]);
            Assert.Equal(record1, collection[2]);
        }
    }
}