using Adaptive.Intelligence.Csv;
using Adaptive.Intelligence.Csv.Exceptions;
using System.Text;

namespace Adaptive.Intelligence.Csv.Tests;

public class CsvCorrectnessFocusedTests
{
    private sealed class PersonRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void WriteRawDataRows_EscapesQuotedAndDelimitedCells()
    {
        using MemoryStream stream = new();
        using CsvWriter writer = new(stream);

        List<List<string>> rows =
        [
            ["Col1", "Col2"],
            ["A,B", "C\"D"]
        ];

        writer.WriteRawDataRows(rows, true);
        writer.Close();

        stream.Position = 0;
        using StreamReader reader = new(stream, Encoding.UTF8, leaveOpen: true);

        Assert.Equal("Col1,Col2", reader.ReadLine());
        Assert.Equal("\"A,B\",\"C\"\"D\"", reader.ReadLine());
    }

    [Fact]
    public void ReadHeader_ParsesQuotedHeaderCells()
    {
        using MemoryStream stream = new();
        using (StreamWriter sw = new(stream, Encoding.UTF8, leaveOpen: true))
        {
            sw.WriteLine("\"Last, First\",Age");
            sw.WriteLine("\"Jones, Sam\",42");
            sw.Flush();
        }

        stream.Position = 0;
        using CsvReader reader = new(stream, hasHeader: true);

        List<string>? header = reader.ReadHeader();

        Assert.NotNull(header);
        Assert.Equal(["Last, First", "Age"], header);
    }

    [Fact]
    public void ReadRawDataRows_DoesNotStopAtBlankLine()
    {
        using MemoryStream stream = new();
        using (StreamWriter sw = new(stream, Encoding.UTF8, leaveOpen: true))
        {
            sw.WriteLine("Id,Name");
            sw.WriteLine("1,Alpha");
            sw.WriteLine(string.Empty);
            sw.WriteLine("2,Beta");
            sw.Flush();
        }

        stream.Position = 0;
        using CsvReader reader = new(stream, hasHeader: true);

        List<List<string>>? rows = reader.ReadRawDataRows(true);

        Assert.NotNull(rows);
        Assert.Equal(2, rows.Count);
        Assert.Equal(["1", "Alpha"], rows[0]);
        Assert.Equal(["2", "Beta"], rows[1]);
    }

    [Fact]
    public void ReadDataRows_ThrowsOnDisposedReader()
    {
        using MemoryStream stream = new();
        using (StreamWriter sw = new(stream, Encoding.UTF8, leaveOpen: true))
        {
            sw.WriteLine("Id,Name");
            sw.WriteLine("1,Alpha");
            sw.Flush();
        }

        stream.Position = 0;
        CsvReader reader = new(stream, hasHeader: true);
        reader.Dispose();

        Assert.Throws<NullStreamException>(() => reader.ReadDataRows<PersonRow>());
    }

    [Fact]
    public void ToChar_ReturnsNullForEmptyInput()
    {
        char? value = CsvTypeConverter.ToChar(string.Empty);
        Assert.Null(value);
    }

    [Fact]
    public void ToInt32_ReturnsNullForInvalidInput()
    {
        int? value = CsvTypeConverter.ToInt32("not-an-int");
        Assert.Null(value);
    }
}