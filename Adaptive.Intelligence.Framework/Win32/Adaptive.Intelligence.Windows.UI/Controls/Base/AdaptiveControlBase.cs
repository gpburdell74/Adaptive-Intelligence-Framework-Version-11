using Adaptive.Intelligence.Events.Arguments;
using Adaptive.Intelligence.Events.Delegates;
using Adaptive.Intelligence.Logging;
using Adaptive.Intelligence.Win32.UI.MessageBox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace Adaptive.Intelligence.Win32.UI.controls.Base;
#pragma warning disable CS4014
#pragma warning disable VSTHRD110

/// <summary>
/// Provides the standard base control definition for custom controls in Adaptive Intelligence UI applications.
/// </summary>
/// <remarks>
/// It is intended that each control descending from this class to provide a variety of features for these controls:
///     * Standard processing of OnLoad to provide both a sequential way to display a wait UI, 
///       to assign event handlers for the controls on the control, to load data for the control sequentially OR 
///       asynchronously, and to set the state of the control after the data is loaded and return the control to a usable state.
///     * Provide a consistent and single location for removal of event handlers and object disposal when the control is closed.
///     * Provide a mechanism for asynchronous methods to update the UI on the UI thread.
///     * Provide overridable methods for setting the state of the control when busy.
///     * Provide overridable methods for setting the state of the control when idle.
///     * Provide overridable methods for setting the state of the control based on data content and UI state.
///     * Provide overridable methods for setting the state of the control based on security roles.
///     * Provide control methods for confirming user action, and displaying error messages.
///     * Provide a convenient built-in event handler for generic control state changes.
///     * Provide an optional detailed debug output mechanism when the control executes its operations.
///     * Other common UI operations.
///     
/// When implementing a child class, these common methods assist the develper in providing a consistent user experience 
/// event when invoked from a non-UI thread:
///     SetState()                      - Sets the general state of the control based on its contained data.
///     SetDisplayState()               - Sets the general display state of the control based on its contained data.
///     SetSecurityState()              - Sets the controls' states and availablity based on the security roles of the user.
///     SetPreloadState()               - Sets the control to appear as busy, with wait cursor.
///     SetPostLoadState()              - Sets the control to appear as idle, with normal pointer.
///     AssignEventHandlers()           - Assigns event handlers to the controls on the control.
///     RemoveEventHandlers()           - Removes the event handlers assigned in AssignEventHandlers()
///     OnInitLoadComplete()            - Excutes when the all the control load operations are completed the first time.
///     ContinueInMainThread()          - Executes the provided method in the main UI thread.
///     InitializeDataContent()         - Loads the data content for the control.
///     InitializeDataContentAsync()    - Asynchronously loads the data content for the control.
///     GetUserConfirmation()           - Displays a message box to the user to confirm an action.
///     // NOTE: These may change as the implementation of a MessageBox service (allowing custom controls) is added.
///     ShowError()                     - Displays a message box to the user to show an error message.
///     ShowMessage()                   - Displays a message box to the user to show a general message.
///     HandleGenericControlChange()    - Used for a control's generic "Changed" event to invoke SetState() in th main UI thread.
///     RunAsynchronouslyAndReturnToUI() - Not yet implemented: allows a caller to invoke a simple asynchronous method and 
///                                        automatically invoke SetState() and SetPostloadState() on return in the main UI thread.
/// </remarks>
[SupportedOSPlatform("windows")]
public partial class AdaptiveControlBase : UserControl
{
    #region Public Event Declarations
    /// <summary>
    /// Occurs when an <see cref="Exception"/> is caught during execution of the control.
    /// </summary>
    public event ExceptionEventHandler? ExecutionError;
    /// <summary>
    /// Occurs when the content of the control is changed, and a parent
    /// container needs to be notified.
    /// </summary>
    public event EventHandler? ContentChanged;
    /// <summary>
    /// Occurs when the initial load is completed.
    /// </summary>
    public event EventHandler? InitLoadComplete;
    #endregion

    #region Private Member Declarations
    /// <summary>
    /// The message box service implementation for showing message boxes.
    /// </summary>
    private IMessageBoxService? _messageBoxService;

    /// <summary>
    /// Points to a method to be executed only once the handle for the
    /// control is created.
    /// </summary>
    private readonly List<Action> _futureExecutionTargetList;

    /// <summary>
    /// List of child controls that are derived from <see cref="AdaptiveControlBase"/>. 
    /// This is used to assign and remove event handlers for the child controls.
    /// </summary>
    private readonly List<AdaptiveControlBase> _childControls;

    /// <summary>
    /// Logger instance.
    /// </summary>
#pragma warning disable IDE0052 // -- This is somehow magically called by LogError(Exception).
    private readonly ILogger _logger = NullLogger.Instance;
#pragma warning restore IDE0052 // Remove unread private members
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="AdaptiveControlBase"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public AdaptiveControlBase(ILogger? logger = null) : base()
    {
        LogStart();

        // Set control styles.
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        // Initialize the standard basic properties for the control.
        InternalInitializeComponent();

        // Set the base font usage.
        SetDefaultFont();

        // Create the list to store method calls to be executed in the future on the main thread.
        _futureExecutionTargetList = [];

        // Create the list to store the list of AdaptiveControlBase child controls.
        _childControls = [];

        // Create the default message box service implementation for showing message boxes.
        _messageBoxService = new Win32MessageBoxService(this);

        // Set logger reference.
        if (logger != null)
        {
            _logger = logger;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdaptiveControlBase"/> class.
    /// </summary>
    /// <param name="messageBoxService">
    /// The <see cref="IMessageBoxService"/> implementation to use.
    /// </param>
    /// <param name="logger">
    /// The <see cref="ILogger"/> implementation to use.
    /// </param>
    public AdaptiveControlBase(IMessageBoxService messageBoxService, ILogger? logger = null) : this(logger)
    {
        // Replace the default with the new reference.
        _messageBoxService?.Dispose();
        _messageBoxService = messageBoxService;
    }

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

    #region Event Methods
    /// <summary>
    /// Raises the <see cref="ContentChanged" /> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="EventArgs"/> instance containing the event data.
    /// </param>
    protected virtual void OnContentChanged(EventArgs e)
    {
        LogStart();
        ContinueInMainThread(() => { ContentChanged?.Invoke(this, e); });
    }
    /// <summary>
    /// Raises the <see cref="InitLoadComplete" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected virtual void OnInitLoadComplete(EventArgs e)
    {
        LogStart();
        InitLoadComplete?.Invoke(this, e);
    }
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
        // Complete the standard Windows load.
        LogBaseStart();
        base.OnLoad(e);

        LogBaseStart(nameof(SuspendLayout));
        SuspendLayout();

        // If the handle has been created, run the control's startup process.
        AdaptiveDebug.WriteLine(UiConstants.IsHandleCreatedText(IsHandleCreated));
        if (IsHandleCreated && !DesignMode)
        {
            // Set the UI State before load.
            SetPreLoadState();

            // Capture the list of AdaptiveControlBase child controls.
            foreach (Control item in this.Controls)
            {
                if (item is AdaptiveControlBase adaptiveControl)
                {
                    _childControls.Add(adaptiveControl);
                }
            }

            // Assign the event handlers for the controls on the control.
            AssignChildEventHandlers();
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
    /// Raises the <see cref="Control.HandleDestroyed"/> event.
    /// </summary>
    /// <param name="e">
    /// A <see cref="EventArgs"/> that contains the event data.
    /// </param>
    protected override void OnHandleDestroyed(EventArgs e)
    {
        LogStart();
        LogBaseStart();
        base.OnHandleDestroyed(e);
        if (!DesignMode)
        {
            RemoveChildEventHandlers();
            RemoveEventHandlers();
        }
    }

    /// <summary>
    /// Raises the <see cref="Control.Resize" /> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
    }

    /// <summary>
    /// Paints the background of the control.
    /// </summary>
    /// <remarks>
    /// This override is used to ensure the highest graphics quality settings when drawing.
    /// </remarks>
    /// <param name="e">
    /// A <see cref="PaintEventArgs"/> instance containing the graphics instance.
    /// </param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        if (e.Graphics.CompositingQuality != System.Drawing.Drawing2D.CompositingQuality.HighQuality)
        {
            SetGraphicsQuality(e.Graphics);
        }
        base.OnPaintBackground(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.Paint" /> event and draws the control.
    /// </summary>
    /// <remarks>
    /// This override is used to ensure the highest graphics quality settings when drawing.
    /// </remarks>
    /// <param name="e">
    /// A <see cref="PaintEventArgs"/> instance containing the graphics instance.
    /// </param>
    protected override void OnPaint(PaintEventArgs e)
    {
        if (e.Graphics.CompositingQuality != System.Drawing.Drawing2D.CompositingQuality.HighQuality)
        {
            SetGraphicsQuality(e.Graphics);
        }
        base.OnPaint(e);
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
    /// Assigns the child controls' ContentChanged event handlers to the generic handler for this control,
    /// if present.
    /// </summary>
    /// <remarks>
    /// This only applies to any child <see cref="AdaptiveControlBase"/>-derived instances.
    /// </remarks>
    protected virtual void AssignChildEventHandlers()
    {
        LogStart();

        // Automatically bind the ContentChanged event of child controls, when present.
        foreach (AdaptiveControlBase control in _childControls)
        {
            control.ContentChanged += HandleGenericContentChange;
        }
    }

    /// <summary>
    /// Assigns the event handlers for the controls on the control.
    /// </summary>
    protected virtual void AssignEventHandlers()
    {
        LogStart();
    }

    /// <summary>
    /// Removes the child controls' ContentChanged event handler assignments.
    /// if present.
    /// </summary>
    /// <remarks>
    /// This only applies to any child <see cref="AdaptiveControlBase"/>-derived instances.
    /// </remarks>
    protected virtual void RemoveChildEventHandlers()
    {
        // Automatically remove the ContentChanged event handler of child controls, when present.
        foreach (AdaptiveControlBase control in _childControls)
        {
            control.ContentChanged -= HandleGenericContentChange;
        }
    }

    /// <summary>
    /// Removes the event handlers for the controls on the control.
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
    /// Sets the state of the UI controls before the data content is loaded.
    /// </summary>
    protected virtual void SetPreLoadState()
    {
        LogStart();
        Cursor = Cursors.WaitCursor;
        Enabled = false;
        SuspendLayout();

        // Propogate the call to child controls.
        foreach (AdaptiveControlBase control in _childControls)
        {
            control.SetPreLoadState();
        }
    }

    /// <summary>
    /// Sets the state of the UI controls after the data content is loaded.
    /// </summary>
    protected virtual void SetPostLoadState()
    {
        LogStart();
        Cursor = Cursors.Default;
        Enabled = true;
        ResumeLayout();

        // Propagate the call to child controls.
        foreach (AdaptiveControlBase control in _childControls)
        {
            control.SetPostLoadState();
        }
    }

    /// <summary>
    /// When implemented in a derived class, sets the display state for the controls on the control based on
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
    /// Sets the visual state of the control based on security conditions and current user status.
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
    /// Initializes the control and control state according to the form data.
    /// </summary>
    protected virtual void InitializeDataContent()
    {
        LogStart();
    }

    /// <summary>
    /// An asynchronous method to initialize the control and control state
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
    /// Handles the generic event when the content of a control changes.
    /// </summary>
    /// <remarks>
    /// This is used to bubble up the <see cref="ContentChanged"/> event 
    /// when <see cref="AdaptiveControlBase"/> child controls are contained.
    /// </remarks>
    /// <param name="sender">
    /// The reference to the object that raised the event.
    /// </param>
    /// <param name="e">
    /// The <see cref="EventArgs"/> instance containing the event data.
    /// </param>
    protected virtual void HandleGenericContentChange(object? sender, EventArgs e)
    {
        LogStart();
        ContinueInMainThread(SetState);
        OnContentChanged(e);
    }

    /// <summary>
    /// Sets the graphics object to draw items at high-quality value.
    /// </summary>
    /// <param name="g">
    /// The <see cref="Graphics"/> instance to use.
    /// </param>
    protected virtual void SetGraphicsQuality(Graphics g)
    {
        try
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Sets the display state for the controls on the control based on
    /// current conditions.
    /// </summary>
    public void SetState()
    {
        LogStart(nameof(SetState));

        SetSecurityState();
        SetDisplayState();

        // Propagate the call to child controls.
        foreach (Control item in this.Controls)
        {
            if (item is AdaptiveControlBase adaptiveControl)
            {
                adaptiveControl.SetState();
            }
        }
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
            // Assign the data to the controls on the control.
            InitializeDataContent();

            // Se the visual state.
            SetState();

            // Set the UI State after load.
            SetPostLoadState();
        }

        ResumeLayout();

        // Indicate the initial load process has completed.
        OnInitLoadComplete(EventArgs.Empty);
    }

    /// <summary>
    /// Performs the basic setup of the control from the constructor.
    /// </summary>
    private void InternalInitializeComponent()
    {
        LogStart();

        SuspendLayout();

        // AdaptiveControlBase
        AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(300, 300);
        Name = "AdaptiveControlBase";
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
    /// Starts the process of background asynchronous loading of any data content for the control.
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
    /// The log error delegate for the <see cref="AdaptiveControlBase"/> class.
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
