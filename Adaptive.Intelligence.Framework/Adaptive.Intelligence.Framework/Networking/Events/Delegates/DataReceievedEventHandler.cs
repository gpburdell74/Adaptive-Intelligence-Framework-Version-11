namespace Adaptive.Intelligence.Networking.Events.Delegates
{
    /// <summary>
    /// Provides a delegate definition for events that handle reception of data on network connections.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
    public delegate void DataReceievedEventHandler(object? sender, DataReceivedEventArgs e);
}
