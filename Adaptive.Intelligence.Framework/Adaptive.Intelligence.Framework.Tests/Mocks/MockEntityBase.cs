using Adaptive.Intelligence.Common.Abstractions;

namespace Adaptive.Intelligence.Framework.Tests.Mocks;

/// <summary>
/// Provides a testable wrapper for the <see cref="EntityBase{T}"/> abstract record.
/// </summary>
public record MockEntityBase : EntityBase<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MockEntityBase"/> record.
    /// </summary>
    public MockEntityBase()
    {
    }

    /// <summary>
    /// Gets a value indicating whether the instance has been disposed.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the instance has been disposed; otherwise, <see langword="false"/>.
    /// </value>
    public bool MockIsDisposed => IsDisposed;
}