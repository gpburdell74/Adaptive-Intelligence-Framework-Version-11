namespace Adaptive.Intelligence.Win32.UI.MessageBox;

/// <summary>
/// Provides the signature definition for a service to provide Message Boxes to the UI.
/// </summary>
public interface IMessageBoxService : IDisposable
{
    /// <summary>
    /// Displays a message box to get a Yes or No response from the user.
    /// </summary>
    /// <param name="caption">
    /// A string containing the caption or title for the mesage box.
    /// </param>
    /// <param name="message">
    /// A string containing the text message to display in the message box.
    /// </param>
    /// <returns>
    /// <b>true</b> if the user confirms the action (Yes); <b>false</b> if the user does not confirm the action (No or Cancel).
    /// </returns>
    bool GetUserConfirmation(string caption, string message);

    /// <summary>
    /// Displays a message box to get a Yes, No, or Cancel response from the user.
    /// </summary>
    /// <param name="caption">
    /// A string containing the caption or title for the mesage box.
    /// </param>
    /// <param name="message">
    /// A string containing the text message to display in the message box.
    /// </param>
    /// <returns>
    /// A <see cref="DialogResult"/> enumerated value indicating the user's selection.
    /// </returns>
    DialogResult GetUserGeneralConfirmation(string caption, string message);

    /// <summary>
    /// Displays an error message to the user.
    /// </summary>
    /// <param name="caption">
    /// A string containing the caption or title for the mesage box.
    /// </param>
    /// <param name="message">
    /// A string containing the error message to display in the message box.
    /// </param>
    void ShowError(string caption, string message);

    /// <summary>
    /// Displays a message to the user with the specified caption, message, buttons, and icon.
    /// </summary>
    /// <param name="caption">
    /// A string containing the caption or title for the mesage box.
    /// </param>
    /// <param name="message">
    /// A string containing the text message to display in the message box.
    /// </param>
    /// <param name="buttons">
    /// A <see cref="MessageBoxButtons"/> enumerated value indicating the buttons to display in the message box.
    /// </param>
    /// <param name="icon">
    /// A <see cref="MessageBoxIcon"/> enumerated value indicating the icon to display in the message box.
    /// </param>
    /// <returns>
    /// A <see cref="DialogResult"/> enumerated value indicating the user's selection.
    /// </returns>
    DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxIcon icon);
}
