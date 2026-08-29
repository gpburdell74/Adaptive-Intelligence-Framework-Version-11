namespace Adaptive.Intelligence.Csv.Tests
{
    public class CsvReaderTests : IDisposable
    {
        private MemoryStream _stream;
        private CsvReader _reader;

        public CsvReaderTests()
        {
            _stream = new MemoryStream();
            var writer = new StreamWriter(_stream);
            writer.WriteLine("Id,Name");
            writer.WriteLine("1,Alpha");
            writer.WriteLine("2,Beta");
            writer.Flush();
            _stream.Position = 0;
            _reader = new CsvReader(_stream, true);
        }

        public void Dispose()
        {
            _reader.Dispose();
            _stream.Dispose();
        }

        [Fact]
        public void Constructor_InitializesReader()
        {
            Assert.NotNull(_reader);
            Assert.True(_stream.CanRead);
        }

        [Fact]
        public void Delimiter_DefaultIsComma()
        {
            Assert.Equal(',', _reader.Delimiter);
        }

        [Fact]
        public void Delimiter_CanBeSet()
        {
            _reader.Delimiter = ';';
            Assert.Equal(';', _reader.Delimiter);
        }

        [Fact]
        public void ReadHeader_ReturnsHeaderRow()
        {
            var header = _reader.ReadHeader();
            Assert.NotNull(header);
            Assert.Equal(new[] { "Id", "Name" }, header);
        }

        [Fact]
        public async Task ReadHeaderAsync_ReturnsHeaderRow()
        {
            var header = await _reader.ReadHeaderAsync();
            Assert.NotNull(header);
            Assert.Equal(new[] { "Id", "Name" }, header);
        }

        [Fact]
        public void ReadRawDataRows_ReadsRowsWithHeader()
        {
            var rows = _reader.ReadRawDataRows(true);
            Assert.NotNull(rows);
            Assert.Equal(2, rows.Count);
            Assert.Equal(new[] { "1", "Alpha" }, rows[0]);
            Assert.Equal(new[] { "2", "Beta" }, rows[1]);
        }

        [Fact]
        public async Task ReadRawDataRowsAsync_ReadsRowsWithHeader()
        {
            var rows = await _reader.ReadRawDataRowsAsync(true);
            Assert.NotNull(rows);
            Assert.Equal(2, rows.Count);
            Assert.Equal(new[] { "1", "Alpha" }, rows[0]);
            Assert.Equal(new[] { "2", "Beta" }, rows[1]);
        }

        [Fact]
        public void ReadRawDataRows_ThrowsOnNullStream()
        {
            var reader = new CsvReader(new MemoryStream());
            reader.Dispose();
            Assert.Throws<Adaptive.Intelligence.Csv.Exceptions.NullStreamException>(() => reader.ReadRawDataRows());
        }

        public class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void ReadDataRows_ReadsObjectsWithHeader()
        {
            var objects = _reader.ReadDataRows<TestObject>(true);
            Assert.NotNull(objects);
            Assert.Equal(2, objects.Count);
            Assert.Equal(1, objects[0].Id);
            Assert.Equal("Alpha", objects[0].Name);
            Assert.Equal(2, objects[1].Id);
            Assert.Equal("Beta", objects[1].Name);
        }

        [Fact]
        public void Position_ReturnsStreamPosition()
        {
            Assert.True(_reader.Position >= 0);
        }

        [Fact]
        public void ReadRawDataRows_ParsesQuotesAndDelimiters()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine("A,B");
            writer.WriteLine("\"A,B\",\"C\"\"D\"");
            writer.Flush();
            stream.Position = 0;

            byte[] data = stream.ToArray();
            string x = System.Text.Encoding.UTF8.GetString(data);

            var reader = new CsvReader(stream, true);
            var rows = reader.ReadRawDataRows(true);
            Assert.NotNull(rows);
            Assert.Single(rows);
            Assert.Equal(new[] { "A,B", "C\"D" }, rows[0]);
        }
    }
}