using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Win32.UI.MessageBox;

/// <summary>
/// Provides the <see cref="IMessageBoxService"/> implementation for displaying standard Windows message boxes.
/// </summary>
/// <param name="parentWindowHandle">
/// A reference to the parent <see cref="IWin32Window"/>-derived instance acting the "parent" of the message box.
/// If set to <b>null</b>, the message box will be displayed without a parent control, 
/// which may result in the message box appearing behind other windows as the desktop becomes the parent window handle.
/// </param>
public sealed class Win32MessageBoxService(IWin32Window? parentWindowHandle) : DisposableObjectBase, IMessageBoxService
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
    public bool GetUserConfirmation(string caption, string message)
    {
        DialogResult result = System.Windows.Forms.MessageBox.Show(parentWindowHandle, message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return result == DialogResult.Yes;
    }

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
    public DialogResult GetUserGeneralConfirmation(string caption, string message)
    {
        return System.Windows.Forms.MessageBox.Show(parentWindowHandle, message, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Displays an error message to the user.
    /// </summary>
    /// <param name="caption">
    /// A string containing the caption or title for the mesage box.
    /// </param>
    /// <param name="message">
    /// A string containing the error message to display in the message box.
    /// </param>
    public void ShowError(string caption, string message)
    {
        System.Windows.Forms.MessageBox.Show(parentWindowHandle, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

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
    public DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return System.Windows.Forms.MessageBox.Show(parentWindowHandle, message, caption, buttons, icon);
    }
}