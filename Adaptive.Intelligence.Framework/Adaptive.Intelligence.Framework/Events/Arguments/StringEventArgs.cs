namespace Adaptive.Intelligence.Events.Arguments
{
    /// <summary>
    /// Provides an event arguments definition for handing string values.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class StringEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringEventArgs"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public StringEventArgs()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StringEventArgs"/> class.
        /// </summary>
        /// <param name="content">
        /// A string containing the content for the event.
        /// </param>
        public StringEventArgs(string content)
        {
            Content = content;
        }
        /// <summary>
        /// Finalizes an instance of the <see cref="StringEventArgs"/> class.
        /// </summary>
        ~StringEventArgs()
        {
            Content = null;
        }

        /// <summary>
        /// Gets or sets the string content for the event.
        /// </summary>
        /// <value>
        /// A string containing the content for the event, or <b>null</b>.
        /// </value>
        public string? Content { get; set; }
    }
}