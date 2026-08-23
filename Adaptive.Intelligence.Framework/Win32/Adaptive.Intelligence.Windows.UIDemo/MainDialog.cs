
using Adaptive.Intelligence.Win32.UI.Dialogs.Base;

namespace Adaptive.Intelligence.Windows.UIDemo;

/// <summary>
/// Provides the central Dialog for the application.
/// </summary>
public partial class MainDialog : AdaptiveDialogBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainDialog"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public MainDialog()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            components?.Dispose();
        }
        components = null;
        base.Dispose(disposing);
    }
}
