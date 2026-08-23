using Adaptive.Intelligence.Events.Arguments;
using Adaptive.Intelligence.Events.Delegates;
using Adaptive.Intelligence.Logging;
using Adaptive.Intelligence.Win32.UI.MessageBox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace Adaptive.Intelligence.Win32.UI.Dialogs.Base;
#pragma warning disable CS4014
#pragma warning disable VSTHRD110

/// <summary>
/// Provides the standard base dialog definition for dialogs in Adaptive Intelligence UI applications.
/// </summary>
/// <remarks>
/// It is intended that each dialog descend from this class to provide a variety of features for these dialogs:
///     * Standard processing of OnLoad to provide both a sequential way to display a wait UI, 
///       to assign event handlers for the dialogs on the dialog, to load data for the dialog sequentially OR 
///       asynchronously, and to set the state of the dialog after the data is loaded and return the dialog to a usable state.
///     * Provide a consistent and single location for removal of event handlers and object disposal when the dialog is closed.
///     * Provide a mechanism for asynchronous methods to update the UI on the UI thread.
///     * Provide overridable methods for setting the state of the dialog when busy.
///     * Provide overridable methods for setting the state of the dialog when idle.
///     * Provide overridable methods for setting the state of the dialog based on data content and UI state.
///     * Provide overridable methods for setting the state of the dialog based on security roles.
///     * Provide dialog methods for confiruming user action, and displaying error messages.
///     * Provide a convenient built-in event handler for generic dialog state changes.
///     * Provide an optional detailed debug output mechanism when the dialog executes its operations.
///     * Other common UI operations.
///     
/// When implementing a child class, these common methods assist the develper in providing a consistent user experience 
/// event when invoked from a non-UI thread:
///     SetState()                      - Sets the general state of the dialog based on its contained data.
///     SetDisplayState()               - Sets the general display state of the dialog based on its contained data.
///     SetSecurityState()              - Sets the dialogs' states and availablity based on the security roles of the user.
///     SetPreloadState()               - Sets the dialog to appear as busy, with wait cursor.
///     SetPostLoadState()              - Sets the dialog to appear as idle, with normal pointer.
///     AssignEventHandlers()           - Assigns event handlers to the dialogs on the dialog.
///     RemoveEventHandlers()           - Removes the event handlers assigned in AssignEventHandlers()
///     OnInitLoadComplete()            - Excutes when the all the dialog load operations are completed the first time.
///     ContinueInMainThread()          - Executes the provided method in the main UI thread.
///     InitializeDataContent()         - Loads the data content for the dialog.
///     InitializeDataContentAsync()    - Asynchronously loads the data content for the dialog.
///     GetUserConfirmation()           - Displays a message box to the user to confirm an action.
///     // NOTE: These may change as the implementation of a MessageBox service (allowing custom dialogs) is added.
///     ShowError()                     - Displays a message box to the user to show an error message.
///     ShowMessage()                   - Displays a message box to the user to show a general message.
///     HandleGenericControlChange()    - Used for a dialog's generic "Changed" event to invoke SetState() in th main UI thread.
///     RunAsynchronouslyAndReturnToUI() - Not yet implemented: allows a caller to invoke a simple asynchronous method and 
///                                        automatically invoke SetState() and SetPostloadState() on return in the main UI thread.
/// </remarks>
[SupportedOSPlatform("windows")]
public partial class AdaptiveDialogBase : Form
{
    #region Public Event Declarations
    /// <summary>
    /// Occurs when an <see cref="Exception"/> is caught during execution of the dialog.
    /// </summary>
    public event ExceptionEventHandler? ExecutionError;
    #endregion

    #region Private Member Declarations
    /// <summary>
    /// The message box service implementation for showing message boxes.
    /// </summary>
    private IMessageBoxService? _messageBoxService;

    /// <summary>
    /// Points to a method to be executed only once the handle for the
    /// dialog is created.
    /// </summary>
    private readonly List<Action> _futureExecutionTargetList;

    /// <summary>
    /// Logger instance.
    /// </summary>
#pragma warning disable IDE0052 // -- This is somehow magically called by LogError(Exception).
    private readonly ILogger _logger = NullLogger.Instance;
#pragma warning restore IDE0052 // Remove unread private members
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="AdaptiveDialogBase"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public AdaptiveDialogBase()
    {
        LogStart();

        // Initialize the standard basic properties for the dialog.
        InternalInitializeComponent();

        // Set the base font usage.
        SetDefaultFont();

        // Create the list to store method calls to be executed in the future on the main thread.
        _futureExecutionTargetList = [];

        // Create the default message box service implementation for showing message boxes.
        _messageBoxService = new Win32MessageBoxService(this);

    }

    ///// <summary>
    ///// Initializes a new instance of the <see cref="AdaptiveDialogBase"/> class.
    ///// </summary>
    ///// <param name="messageBoxService">
    ///// The <see cref="IMessageBoxService"/> implementation to use.
    ///// </param>
    ///// <param name="logger">
    ///// The <see cref="ILogger"/> implementation to use.
    ///// </param>
    //public AdaptiveDialogBase(IMessageBoxService messageBoxService) : this()
    //{
    //    // Replace the default with the new reference.
    //    _messageBoxService?.Dispose();
    //    _messageBoxService = messageBoxService;
    //}

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing">
    /// <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        LogStart();

        if (!IsDisposed && disposing)
        {
            _futureExecutionTargetList.Clear();
        }

        _messageBoxService = null;
        base.Dispose(disposing);
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets a value that indicates whether the <see cref="Component"/> is currently in design
    /// mode.
    /// </summary>
    /// <value>
    /// <b>true</b> if the component is in design mode; otherwise
    /// <b>false</b>.
    /// </value>
    [Browsable(false),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected new bool DesignMode => UiConstants.InDesignMode() || base.DesignMode;
    #endregion

    #region Protected Method Overrides
    /// <summary>
    /// Raises the <see cref="UserControl.Load"/> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="EventArgs"/> instance containing the event data.
    /// </param>
    protected override void OnLoad(EventArgs e)
    {
        LogStart();

        // Set the form state variables.
        HelpButton = false;
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        // Complete the standard Windows load.
        LogBaseStart();
        base.OnLoad(e);

        LogBaseStart(nameof(SuspendLayout));
        SuspendLayout();

        // If the handle has been created, run the dialog's startup process.
        AdaptiveDebug.WriteLine(UiConstants.IsHandleCreatedText(IsHandleCreated));
        if (IsHandleCreated && !DesignMode)
        {
            // Set the UI State before load.
            SetPreLoadState();

            // Assign the event handlers for the dialogs on the dialog.
            AssignEventHandlers();

            // Initialize Data may be implemented synchronously or
            // asynchronously. Ensure execution proceeds sequentially.
            StartInitializeDataContentAsync();
        }
        else
        {
            LogBaseStart(nameof(ResumeLayout));
            ResumeLayout();
        }
    }

    /// <summary>
    /// Raises the <see cref="Control.HandleCreated"/> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="EventArgs"/> instance containing the event data.
    /// </param>
    protected override void OnHandleCreated(EventArgs e)
    {
        LogStart();
        LogBaseStart();
        base.OnHandleCreated(e);

        // If there are any methods(s) waiting to be executed, execute them
        // now that the window handle has been created.
        if (!DesignMode && _futureExecutionTargetList.Count > 0)
        {
            foreach (Action methodPointer in _futureExecutionTargetList)
            {
                try
                {
                    methodPointer();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnExecutionError(new ExceptionEventArgs(ex));
                }
            }
            _futureExecutionTargetList.Clear();
        }
    }

    /// <summary>
    /// Raises the <see cref="Form.FormClosing"/> event.
    /// </summary>
    /// <param name="e">
    /// A <see cref="CancelEventArgs"/> that contains the event data.
    /// </param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        LogStart();
        LogBaseStart();
        base.OnFormClosing(e);

        if ((!e.Cancel) && (!DesignMode))
        {
            RemoveEventHandlers();
        }
    }
    #endregion

    #region Protected Methods for Descendant Classes To Override
    /// <summary>
    /// Raises the <see cref="ExecutionError"/> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="ExceptionEventArgs"/> instance containing the related Exception.
    /// </param>
    protected virtual void OnExecutionError(ExceptionEventArgs e)
    {
        ContinueInMainThread(() =>
        {
            ExecutionError?.Invoke(this, e);
        });
    }

    /// <summary>
    /// Called when the initial loading process is completed.
    /// </summary>
    protected virtual void OnInitLoadComplete()
    {
        LogStart();
    }

    /// <summary>
    /// Assigns the event handlers for the dialogs on the dialog.
    /// </summary>
    protected virtual void AssignEventHandlers()
    {
        LogStart();
    }

    /// <summary>
    /// Removes the event handlers for the dialogs on the dialog.
    /// </summary>
    protected virtual void RemoveEventHandlers()
    {
        LogStart();
    }

    /// <summary>
    /// Continues the execution of the provided lambda or method in the main
    /// UI thread.
    /// </summary>
    /// <param name="target">
    /// An <see cref="Action"/> representing the code section to invoke.
    /// </param>
    protected void ContinueInMainThread(Action target)
    {
        LogStart();

        if (!IsHandleCreated)
        {
            // We have to wait for the handle to be created before this
            // method can be invoked.
            if (!IsDisposed)
            {
                _futureExecutionTargetList.Add(target);
            }
        }
        else
        {
            if (!InvokeRequired)
            {
                try
                {
                    target();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnExecutionError(new ExceptionEventArgs(ex));
                }
            }
            else
            {
                BeginInvoke(target);
            }
        }
    }

    /// <summary>
    /// Sets the state of the UI dialogs before the data content is loaded.
    /// </summary>
    protected virtual void SetPreLoadState()
    {
        LogStart();
        Cursor = Cursors.WaitCursor;
        Enabled = false;
        SuspendLayout();
    }

    /// <summary>
    /// Sets the state of the UI dialogs after the data content is loaded.
    /// </summary>
    protected virtual void SetPostLoadState()
    {
        LogStart();
        Cursor = Cursors.Default;
        Enabled = true;
        ResumeLayout();
    }

    /// <summary>
    /// When implemented in a derived class, sets the display state for the dialogs on the dialog based on
    /// current conditions.
    /// </summary>
    /// <remarks>
    /// This is called by <see cref="SetState"/> after <see cref="SetSecurityState"/> is called.
    /// </remarks>
    protected virtual void SetDisplayState()
    {
        LogStart();
    }

    /// <summary>
    /// Sets the visual state of the dialog based on security conditions and current user status.
    /// </summary>
    /// <remarks>
    /// This is invoked by <see cref="SetState"/> in order to enforce security and role-based user permissions
    /// when needed.
    /// </remarks>
    protected virtual void SetSecurityState()
    {
        LogStart();
    }

    /// <summary>
    /// Initializes the dialog and dialog state according to the form data.
    /// </summary>
    protected virtual void InitializeDataContent()
    {
        LogStart();
    }

    /// <summary>
    /// An asynchronous method to initialize the dialog and dialog state
    /// according to the form data.
    /// </summary>
    protected virtual Task InitializeDataContentAsync()
    {
        LogStart();
        return Task.CompletedTask;
    }
    /// <summary>
    /// Provides a method for setting the window text in the descendant class constructors without the 
    /// standard warning.
    /// </summary>
    /// <param name="windowText">
    /// A string containing the new window text.
    /// </param>
    protected void InnerSetText(string windowText)
    {
        LogStart();
        Text = windowText;
    }

    /// <summary>
    /// Prompts the user to confirm an operation.
    /// </summary>
    /// <param name="caption">
    /// The caption to display on the message box.
    /// </param>
    /// <param name="message">
    /// The message to display on the message box.
    /// </param>
    /// <returns>
    /// <b>true</b> if the user clicks Yes, otherwise, returns <b>false</b>.
    /// </returns>
    protected bool GetUserConfirmation(string caption, string message)
    {
        LogStart();
        if (_messageBoxService is null)
        {
            return false;
        }
        bool result = false;
        if (!InvokeRequired)
        {
            result = _messageBoxService.GetUserConfirmation(caption, message);
        }
        else
        {
            Invoke(() =>
            {

                result = _messageBoxService.GetUserConfirmation(caption, message);
            });
        }
        return result;
    }

    /// <summary>
    /// Displays the specified error message to the user.
    /// </summary>
    /// <param name="caption">
    /// The caption to display on the message box.
    /// </param>
    /// <param name="message">
    /// The message to display on the message box.
    /// </param>
    protected void ShowError(string caption, string message)
    {
        LogStart();
        if (_messageBoxService is null)
        {
            return;
        }
        ContinueInMainThread(() =>
        {
            _messageBoxService.ShowError(caption, message);
        });
    }

    /// <summary>
    /// Displays the specified error message to the user.
    /// </summary>
    /// <param name="caption">
    /// The caption to display on the message box.
    /// </param>
    /// <param name="message">
    /// The message to display on the message box.
    /// </param>
    protected void ShowMessage(string caption, string message)
    {
        LogStart();
        if (_messageBoxService is null)
        {
            return;
        }
        ContinueInMainThread(() =>
        {
            _messageBoxService.ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    /// <summary>
    /// Handles the generic event when the content of a dialog changes.
    /// </summary>
    /// <remarks>
    /// This is used in various locations to invoke <see cref="SetState"/>,
    /// so now this provides the standard event handler built-in.
    /// </remarks>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The <see cref="EventArgs"/> instance containing the event data.
    /// </param>
    protected virtual void HandleGenericControlChange(object? sender, EventArgs e)
    {
        LogStart();
        ContinueInMainThread(SetState);
    }
    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Sets the display state for the dialogs on the dialog based on
    /// current conditions.
    /// </summary>
    public void SetState()
    {
        LogStart(nameof(SetState));

        SetSecurityState();
        SetDisplayState();
    }
    #endregion

    #region Private Methods / Functions
    /// <summary>
    /// Completes the background data loading process in the main UI thread.
    /// </summary>
    private void FinishBackgroundLoad()
    {
        LogStart();

        if (IsHandleCreated && !DesignMode)
        {
            // Assign the data to the dialogs on the dialog.
            InitializeDataContent();

            // Se the visual state.
            SetState();

            // Set the UI State after load.
            SetPostLoadState();
        }

        ResumeLayout();

        // Indicate the initial load process has completed.
        OnInitLoadComplete();
    }

    /// <summary>
    /// Performs the basic setup of the dialog from the constructor.
    /// </summary>
    private void InternalInitializeComponent()
    {
        LogStart();

        SuspendLayout();

        // AdaptiveDialogBase
        AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 600);
        Name = "AdaptiveDialogBase";
        DoubleBuffered = true;
        ResumeLayout(false);
    }

    /// <summary>
    /// Sets the default font.
    /// </summary>
    private void SetDefaultFont()
    {
        LogStart();
        Font = UiConstants.CreateStandardFont();
    }

    /// <summary>
    /// Starts the process of background asynchronous loading of any data content for the dialog.
    /// </summary>
    /// <remarks>
    /// When completed, the <see cref="FinishBackgroundLoad"/> method is invoked on the main UI thread.
    /// </remarks>
    private async Task StartInitializeDataContentAsync()
    {
        LogStart();

        // Initialize Data may be implemented synchronously or
        // asynchronously. Ensure execution proceeds sequentially.
        try
        {
            await InitializeDataContentAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            LogError(ex);
            OnExecutionError(new ExceptionEventArgs(ex));
        }

        ContinueInMainThread(FinishBackgroundLoad);
    }

    /// <summary>
    /// Writes a log entry indicating the calling method is starting to execute.
    /// </summary>
    [Conditional("DEBUG")]
    private void LogStart([System.Runtime.CompilerServices.CallerMemberName] string? caller = null)
    {
        AdaptiveDebug.WriteLine(UiConstants.MethodStartText(Name, caller));
    }

    /// <summary>
    /// Writes a log entry indicating the calling method's base method is starting to execute.
    /// </summary>
    [Conditional("DEBUG")]
    private void LogBaseStart([System.Runtime.CompilerServices.CallerMemberName] string? caller = null)
    {
        AdaptiveDebug.WriteLine(UiConstants.MethodStartText(Name, "base." + caller));
    }

    /// <summary>
    /// The log error delegate for the <see cref="AdaptiveDialogBase"/> class.
    /// </summary>
    /// <seealso>
    /// https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1848
    /// </seealso> 
    /// <param name="ex">
    /// The <see cref="Exception"/> instance tha was caught.
    /// </param>
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 1001)]
    private partial void LogError(Exception ex);
    #endregion
}
