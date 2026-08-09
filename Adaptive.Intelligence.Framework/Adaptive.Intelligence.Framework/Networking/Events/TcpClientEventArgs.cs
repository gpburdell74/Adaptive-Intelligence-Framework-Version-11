using Adaptive.Intelligence.Networking.Tcp;

namespace Adaptive.Intelligence.Networking.Events
{
    /// <summary>
    /// Provides an event arguments instnace for <see cref="TcpClient"/> related events.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class TcpClientEventArgs(TcpClient clientInstance) : EventArgs
    {
        /// <summary>
        /// Gets the reference to the <see cref="TcpClient"/> instance associated with the event.
        /// </summary>
        /// <value>
        /// A <see cref="TcpClient"/> instance, or <b>null</b>.
        /// </value>
        public TcpClient? Client => clientInstance;
    }
}
