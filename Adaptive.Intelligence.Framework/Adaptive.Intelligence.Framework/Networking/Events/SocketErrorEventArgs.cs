namespace Adaptive.Intelligence.Networking.Events
{
    /// <summary>
    /// Provides an event arguments instnace for socket-error related events.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class SocketErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// An integer specifying the socket error code.
        /// </value>
        public int Error { get; set; }
    }
}