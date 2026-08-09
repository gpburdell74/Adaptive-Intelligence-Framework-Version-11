using System.Net.Sockets;

namespace Adaptive.Intelligence.Networking.Events.Delegates
{
    /// <summary>
    /// Provides a delegate definition for <see cref="Socket"/> related events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="SocketEventArgs"/> instance containing the event data.</param>
    public delegate void SocketEventHandler(object? sender, SocketEventArgs e);
}
