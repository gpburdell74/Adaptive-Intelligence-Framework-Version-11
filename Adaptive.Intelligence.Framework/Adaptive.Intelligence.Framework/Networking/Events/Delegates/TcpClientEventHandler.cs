namespace Adaptive.Intelligence.Networking.Events.Delegates
{
    /// <summary>
    /// Provides the event handler delegate definition for handling TCP client events.
    /// </summary>
    /// <param name="sender">
    /// The instance raising the event.
    /// </param>
    /// <param name="e">
    /// A <see cref="TcpClientEventArgs"/> instance containing the event data.
    /// </param>
    public delegate void TcpClientEventHandler(object? sender, TcpClientEventArgs e);
}
