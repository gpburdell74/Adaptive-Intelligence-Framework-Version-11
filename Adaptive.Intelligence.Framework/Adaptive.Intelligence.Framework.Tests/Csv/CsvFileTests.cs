namespace Adaptive.Intelligence.Csv.Tests
{
    public class CsvFileTests : IDisposable
    {
        private string _testFile;
        private string _testFileWithHeader;
        private string _outputFile;
        private CsvFile _csvFile;

        public CsvFileTests()
        {
            _testFile = Path.GetTempFileName();
            _testFileWithHeader = Path.GetTempFileName();
            _outputFile = Path.GetTempFileName();
            File.WriteAllLines(_testFile, new[] { "1,Alpha", "2,Beta" });
            File.WriteAllLines(_testFileWithHeader, new[] { "Id,Name", "1,Alpha", "2,Beta" });
            _csvFile = new CsvFile(_testFile);
        }

        public void Dispose()
        {
            _csvFile.Dispose();
            if (File.Exists(_testFile)) File.Delete(_testFile);
            if (File.Exists(_testFileWithHeader)) File.Delete(_testFileWithHeader);
            if (File.Exists(_outputFile)) File.Delete(_outputFile);
        }

        [Fact]
        public void Constructor_InitializesFileName()
        {
            Assert.Equal(_testFile, _csvFile.FileName);
        }

        [Fact]
        public void LoadContent_LoadsRowsWithoutHeader()
        {
            _csvFile.LoadContent(_testFile);
            Assert.Equal(2, _csvFile.RowCount);
            Assert.Equal(2, _csvFile.ColumnCount);
        }

        [Fact]
        public void LoadContent_LoadsRowsWithHeader()
        {
            _csvFile.LoadContent(_testFileWithHeader, true);
            Assert.Equal(2, _csvFile.RowCount);
            Assert.Equal(2, _csvFile.ColumnCount);
        }

        [Fact]
        public async Task LoadContentAsync_LoadsRowsWithHeader()
        {
            await _csvFile.LoadContentAsync(_testFileWithHeader, true);
            Assert.Equal(2, _csvFile.RowCount);
            Assert.Equal(2, _csvFile.ColumnCount);
        }

        [Fact]
        public void SaveAs_SavesLoadedContent()
        {
            _csvFile.LoadContent(_testFile);
            _csvFile.SaveAs(_outputFile);
            Assert.True(File.Exists(_outputFile));
            var lines = File.ReadAllLines(_outputFile);
            Assert.Equal(new[] { "1,Alpha", "2,Beta" }, lines);
        }

        [Fact]
        public void ValidateColumnCounts_ReturnsEmptyForValidRows()
        {
            _csvFile.LoadContent(_testFile);
            var invalidRows = _csvFile.ValidateColumnCounts();
            Assert.Empty(invalidRows);
        }

        [Fact]
        public void ValidateColumnCounts_ReturnsInvalidRows()
        {
            var badFile = Path.GetTempFileName();
            File.WriteAllLines(badFile, new[] { "1,Alpha", "2" });
            var file = new CsvFile(badFile);
            file.LoadContent(badFile);
            var invalidRows = file.ValidateColumnCounts();
            Assert.Single(invalidRows);
            Assert.Equal(1, invalidRows[0]);
            file.Dispose();
            File.Delete(badFile);
        }

        [Fact]
        public void Properties_AreCorrect()
        {
            _csvFile.LoadContent(_testFile);
            Assert.Equal(_testFile, _csvFile.FileName);
            Assert.True(_csvFile.Length > 0);
            Assert.Equal(2, _csvFile.RowCount);
            Assert.Equal(2, _csvFile.ColumnCount);
            Assert.Equal(65536, _csvFile.MaximumCellSize);
        }

        [Fact]
        public void Close_ClearsData()
        {
            _csvFile.LoadContent(_testFile);
            _csvFile.Close();
            Assert.Equal(-1, _csvFile.RowCount);
            Assert.Equal(-1, _csvFile.ColumnCount);
        }

        [Fact]
        public void CompareTo_ReturnsExpected()
        {
            var fileA = new CsvFile(_testFile);
            var fileB = new CsvFile(_testFile);
            Assert.Equal(0, fileA.CompareTo(fileB));
        }

        [Fact]
        public void Can_Read_And_Write_Consistently()
        {
            string source = CreateTestFile();

            using MemoryStream sourceStream = new();
            using (StreamWriter sourceWriter = new(sourceStream, leaveOpen: true))
            {
                sourceWriter.Write(source);
                sourceWriter.Flush();
            }

            sourceStream.Seek(0, SeekOrigin.Begin);

            List<string> expectedHeader;
            List<List<string>> expectedRows;
            using (CsvReader sourceReader = new(sourceStream, true))
            {
                expectedHeader = sourceReader.ReadHeader() ?? [];
                expectedRows = sourceReader.ReadRawDataRows(true) ?? [];
            }

            using MemoryStream destinationStream = new();
            using (CsvWriter writer = new(destinationStream))
            {
                writer.WriteHeader(expectedHeader);
                writer.WriteRawDataRows(expectedRows);
                writer.Close();
            }

            destinationStream.Seek(0, SeekOrigin.Begin);

            List<string> actualHeader;
            List<List<string>> actualRows;
            using (CsvReader destinationReader = new(destinationStream, true))
            {
                actualHeader = destinationReader.ReadHeader() ?? [];
                actualRows = destinationReader.ReadRawDataRows(true) ?? [];
            }

            Assert.Equal(expectedHeader, actualHeader);
            Assert.Equal(expectedRows.Count, actualRows.Count);

            for (int i = 0; i < expectedRows.Count; i++)
            {
                Assert.Equal(expectedRows[i], actualRows[i]);
            }
        }
        

        private static string CreateTestFile()
        {
            string data = 
"""
RowId,Category,Name,Description,QuotedText,DelimitedText,UnicodeText,WhitespaceText,EmptyText,IntValue,LongValue,DecimalValue,FloatValue,BoolValue,DateIso,DateUs,GuidValue,Email,Phone,Url,JsonPayload,XmlPayload,Status,Code,Tags,Notes
0,Seed,Alpha,Baseline row,"\"He said "\""\"Hello"\""\""\","\"A,B,C"\",naïve café,"\"  padded  "\",,0,0,0.00,0.0,true,2026-01-01T00:00:00Z,01/01/2026,00000000-0000-0000-0000-000000000000,seed@example.com,+1-555-0100,https://example.test/seed,"\"{"\""\"k"\""\":"\""\"v"\""\"}"\",<r><k>v</k></r>,Active,S-000,seed|baseline,
1,Edge,"\"CommaName, Inc."\",Contains comma,"\""\""\"Quoted"\""\" value"\","\"X,Y"\",東京,"\"	trim	"\",,-1,9223372036854775807,-12345.6789,3.402823E+37,false,2026-02-28T23:59:59Z,02/28/2026,11111111-1111-1111-1111-111111111111,comma@example.com,+1-555-0101,"\"https://example.test/comma?a=1&b=2"\","\"{"\""\"arr"\""\":[1,2,3]}"\","\"<x a="\""\"1"\""\"/>"\",Paused,S-001,comma|quote,"\"quote + comma"\"
2,Edge,Emoji 😃,Unicode row,"\"He said "\""\"CSV"\""\""\","\"D,E,F"\",مرحبا,"\" leading"\",,2147483647,-9223372036854775808,9999999999.9999,-3.402823E+37,TRUE,2026-12-31T23:59:59Z,12/31/2026,22222222-2222-2222-2222-222222222222,unicode@example.com,+1-555-0102,https://example.test/unicode,"\"{"\""\"emoji"\""\":"\""\"😃"\""\"}"\",<u>東京</u>,Closed,S-002,unicode|emoji,rtl + cjk
3,Edge,EmptyFields,Mostly empty,,,,,,,,,,,,2026-03-15T12:00:00Z,03/15/2026,33333333-3333-3333-3333-333333333333,,,,{},<e/>,Unknown,S-003,,
""";
            return data;
        }
    }
}