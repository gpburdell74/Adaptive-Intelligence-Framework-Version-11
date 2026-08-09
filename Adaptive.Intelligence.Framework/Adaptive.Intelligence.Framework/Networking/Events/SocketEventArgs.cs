using System.Net.Sockets;

namespace Adaptive.Intelligence.Networking.Events
{
    /// <summary>
    /// Provides an event arguments instance for <see cref="Socket"/> related events.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class SocketEventArgs(Socket? socket) : EventArgs
    {
        /// <summary>
        /// Finalizes an instance of the <see cref="SocketEventArgs"/> class.
        /// </summary>
        ~SocketEventArgs()
        {
            Socket = null;
        }

        /// <summary>
        /// Gets or sets the reference to the socket being used by the event.
        /// </summary>
        /// <value>
        /// The <see cref="Socket"/> instance, or <b>null</b>.
        /// </value>
        public Socket? Socket { get => socket; set => socket = value; }
    }
}
