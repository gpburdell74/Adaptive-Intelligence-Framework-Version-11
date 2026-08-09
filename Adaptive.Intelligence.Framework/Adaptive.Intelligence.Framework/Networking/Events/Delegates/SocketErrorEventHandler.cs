namespace Adaptive.Intelligence.Networking.Events.Delegates
{
    /// <summary>
    /// Provides a delegate definition for socket error-related events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="SocketErrorEventArgs"/> instance containing the event data.</param>
    public delegate void SocketErrorEventHandler(object sender, SocketErrorEventArgs e);
}
