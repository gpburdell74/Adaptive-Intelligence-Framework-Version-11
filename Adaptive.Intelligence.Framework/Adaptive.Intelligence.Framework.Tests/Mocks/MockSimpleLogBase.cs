using Adaptive.Intelligence.Common.Abstractions;

namespace Adaptive.Intelligence.Framework.Tests.Mocks;

/// <summary>
/// Provides a mock implementation of the <see cref="SimpleLogBase"/> class for testing purposes.
/// </summary>
public class MockSimpleLogBase : SimpleLogBase
{
    public const string FooterText = "Adaptive Intelligence Framework - Simple Log Footer\r\nVersion 11.0.1.0\r\n";
    public const string HeaderText = "Adaptive Intelligence Framework - Simple Log Header\r\nVersion 11.0.1.0\r\n";

    public MockSimpleLogBase() : base()
    {

    }
    public MockSimpleLogBase(string fileName) : base(fileName)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleLogBase"/> class.
    /// </summary>
    /// <param name="destinationStream">
    /// The <see cref="Stream"/> instance to which the log will be written.
    /// </param>
    public MockSimpleLogBase(Stream destinationStream) : base(destinationStream)
    {
    }



    protected override string? FormatException(Exception exception)
    {
        return $"Exception: ['{exception.Message}']\r\n";
    }

    protected override string? FormatFileFooter()
    {
        return FooterText;
    }

    protected override string? FormatFileHeader()
    {
        return HeaderText;
    }
}
