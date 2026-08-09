using Adaptive.Intelligence.Events.Arguments;

namespace Adaptive.Intelligence.Events.Delegates
{
    /// <summary>
    /// Provides an event delegate definition for events that communicate exceptions.
    /// </summary>
    /// <param name="sender">
    /// The object raising the event.
    /// </param>
    /// <param name="e">
    /// The <see cref="ExceptionEventArgs"/> instance containing the event data.
    /// </param>
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);
}
