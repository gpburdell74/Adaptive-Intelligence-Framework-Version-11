using System;
using System.Collections.Generic;
using System.IO;
using Adaptive.Intelligence.Csv;
using Xunit;

namespace Adaptive.Intelligence.Csv.Tests
{
    public class CsvWriterTests : IDisposable
    {
        private MemoryStream _stream;
        private CsvWriter _writer;

        public CsvWriterTests()
        {
            _stream = new MemoryStream();
            _writer = new CsvWriter(_stream);
        }

        public void Dispose()
        {
            _writer.Dispose();
            _stream.Dispose();
        }

        [Fact]
        public void Constructor_InitializesWriter()
        {
            Assert.NotNull(_writer);
            Assert.True(_stream.CanWrite);
        }

        [Fact]
        public void Delimiter_DefaultIsComma()
        {
            Assert.Equal(',', _writer.Delimiter);
        }

        [Fact]
        public void Delimiter_CanBeSet()
        {
            _writer.Delimiter = ';';
            Assert.Equal(';', _writer.Delimiter);
        }

        [Fact]
        public void WriteHeader_WritesHeaderRow()
        {
            var header = new List<string> { "A", "B", "C" };
            _writer.WriteHeader(header);
            _writer.Close();
            _stream.Position = 0;
            using var reader = new StreamReader(_stream);
            var line = reader.ReadLine();
            Assert.Equal("A,B,C", line);
        }

        [Fact]
        public void WriteDataRow_WritesSingleRow()
        {
            var row = new List<string> { "1", "2", "3" };
            _writer.WriteDataRow(row);
            _writer.Close();
            _stream.Position = 0;
            using var reader = new StreamReader(_stream);
            var line = reader.ReadLine();
            Assert.Equal("1,2,3", line);
        }

        [Fact]
        public void WriteDataRow_ThrowsOnNullStream()
        {
            var row = new List<string> { "1", "2" };
            var writer = new CsvWriter(new MemoryStream());
            writer.Dispose();
            Assert.Throws<Adaptive.Intelligence.Csv.Exceptions.NullStreamException>(() => writer.WriteDataRow(row));
        }

        [Fact]
        public void WriteRawDataRows_WritesMultipleRowsWithHeader()
        {
            var rows = new List<List<string>>
 {
 new List<string> { "A", "B" },
 new List<string> { "1", "2" },
 new List<string> { "3", "4" }
 };
            _writer.WriteRawDataRows(rows, true);
            _writer.Close();
            _stream.Position = 0;
            using var reader = new StreamReader(_stream);
            Assert.Equal("A,B", reader.ReadLine());
            Assert.Equal("1,2", reader.ReadLine());
            Assert.Equal("3,4", reader.ReadLine());
        }

        [Fact]
        public void WriteRawDataRows_ThrowsOnNullStream()
        {
            var rows = new List<List<string>> { new List<string> { "A" } };
            var writer = new CsvWriter(new MemoryStream());
            writer.Dispose();
            Assert.Throws<Adaptive.Intelligence.Csv.Exceptions.NullStreamException>(() => writer.WriteRawDataRows(rows));
        }

        public class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void WriteDataRows_WritesObjectsWithHeader()
        {
            var objects = new List<TestObject>
             {
             new TestObject { Id =1, Name = "Alpha" },
             new TestObject { Id =2, Name = "Beta" }
             };
            _writer.WriteDataRows(objects, true);
            _writer.Close();
            _stream.Position = 0;
            using var reader = new StreamReader(_stream);
            Assert.Equal("Id,Name", reader.ReadLine());
            Assert.Equal("1,Alpha", reader.ReadLine());
            Assert.Equal("2,Beta", reader.ReadLine());
        }

        [Fact]
        public void WriteDataRows_WritesObjectsWithoutHeader()
        {
            var objects = new List<TestObject>
 {
 new TestObject { Id =1, Name = "Alpha" },
 new TestObject { Id =2, Name = "Beta" }
 };
            _writer.WriteDataRows(objects, false);
            _writer.Close();
            _stream.Position = 0;
            using var reader = new StreamReader(_stream);
            Assert.Equal("1,Alpha", reader.ReadLine());
            Assert.Equal("2,Beta", reader.ReadLine());
        }

        [Fact]
        public void WriteDataRow_EscapesQuotesAndDelimiters()
        {
            var row = new List<string> { "A,B", "C\"D" };
            _writer.WriteDataRow(row);
            _writer.Close();
            _stream.Position = 0;
            using var reader = new StreamReader(_stream);
            var line = reader.ReadLine();
            Assert.Equal("\"A,B\",\"C\"\"D\"", line);
        }

        [Fact]
        public void Position_ReturnsStreamPosition()
        {
            var row = new List<string> { "1", "2" };
            _writer.WriteDataRow(row);
            Assert.True(_writer.Position > 0);
        }
    }
}