using Adaptive.Intelligence.Common.Events.Arguments;

namespace Adaptive.Intelligence.Common.Events.Delegates;

/// <summary>
/// Provides the event handler delegate definition for events that communicate string values.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="e">The <see cref="StringEventArgs"/> instance containing the event data.
/// </param>
public delegate void StringEventHandler(object? sender, StringEventArgs e);
