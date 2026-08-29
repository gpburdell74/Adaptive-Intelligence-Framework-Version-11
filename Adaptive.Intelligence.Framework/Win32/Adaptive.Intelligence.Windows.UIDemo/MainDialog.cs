using Adaptive.Intelligence.Win32.UI.Dialogs.Base;

namespace Adaptive.Intelligence.Windows.UIDemo;

public partial class MainDialog : AdaptiveDialogBase
{
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
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

}
